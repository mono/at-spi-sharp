// Copyright 2009 Novell, Inc.
// This software is made available under the MIT License
// See COPYING for details

using System;
using System.Collections.Generic;
using System.Threading;
using NDesk.DBus;
using org.freedesktop.DBus;

namespace org.freedesktop.atspi
{
	public class Registry
	{
		public static Registry Instance = null;
		private Bus bus;
		private Thread busThread;
		private RegistryInterface proxy;
		private List<Application> applications;
		private Desktop desktop;

		public static void Initialize ()
		{
			Initialize (false);
		}

		public static void Initialize (bool glib)
		{
			if (Instance != null)
				return;

			Instance = new Registry (glib);

Console.WriteLine ("dbg: getting applications");
			Instance.GetApplications ();
		}

		public static void Terminate ()
		{
			Instance.TerminateInternal ();
			Instance = null;
		}

		public static Bus Bus { get { return Instance.bus; } }

		public Registry (bool glib)
		{
			bus = Bus.Session;
			applications = new List<Application> ();
			desktop = new Desktop ();
			proxy = bus.GetObject<RegistryInterface> ("org.freedesktop.atspi.Registry", new ObjectPath ("/org/freedesktop/atspi/registry"));
			proxy.updateApplications += OnUpdateApplications;

			if (glib)
				BusG.Init ();
			else {
				busThread = new Thread (new ThreadStart (Iterate));
				busThread.IsBackground = true;
				busThread.Start ();
			}
		}

		public static Desktop Desktop { get { return Instance.desktop; } }

		private void GetApplications ()
		{
			string [] appNames = proxy.getApplications ();
			foreach (string app in appNames)
				applications.Add (new Application (app));
		}

		internal void TerminateInternal ()
		{
			if (busThread != null) {
				busThread.Abort ();
				busThread = null;
			}
		}

		private void Iterate ()
		{
Console.WriteLine ("dbg: iterate");
			while (true)
				bus.Iterate ();
		}

		void OnUpdateApplications (int added, string name)
		{
			// added is really a bool
Console.WriteLine ("dbg: app: " + added + ", " + name);
			if (added != 0) {
				Application app = new Application (name);
				applications.Add (app);
				desktop.Add (app);
			}
			else {
				foreach (Application a in applications) {
					if (a.name == name) {
						desktop.Remove (a);
						applications.Remove (a);
						break;
					}
				}
			}
		}
	}

	[Interface ("org.freedesktop.atspi.Registry")]
	interface RegistryInterface : Introspectable
	{
		string [] getApplications ();
		event updateApplicationsHandler updateApplications;
	}

	delegate void updateApplicationsHandler (int added, string name);
}
