// Permission is hereby granted, free of charge, to any person obtaining 
// a copy of this software and associated documentation files (the 
// "Software"), to deal in the Software without restriction, including 
// without limitation the rights to use, copy, modify, merge, publish, 
// distribute, sublicense, and/or sell copies of the Software, and to 
// permit persons to whom the Software is furnished to do so, subject to 
// the following conditions: 
//  
// The above copyright notice and this permission notice shall be 
// included in all copies or substantial portions of the Software. 
//  
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, 
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF 
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND 
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE 
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION 
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION 
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. 
// 
// Copyright (c) 2009 Novell, Inc. (http://www.novell.com) 
// 
// Authors:
//      Mike Gorse <mgorse@novell.com>
// 

using System;
using System.Collections.Generic;
using System.Threading;
using NDesk.DBus;
using org.freedesktop.DBus;

namespace Atspi
{
	public class Registry
	{
		public static Registry Instance {
			get {
				return instance;
			}
		}

		private volatile static Registry instance;
		private Bus bus;
		private RegistryInterface proxy;
		private Dictionary<string, Application> applications;
		private Desktop desktop;
		private static object sync = new object ();

		public static void Initialize ()
		{
			Initialize (false);
		}

		public static void Initialize (bool startLoop)
		{
			lock (sync) {
				if (instance != null)
					return;

				new Registry (startLoop);
			}
		}

		public static void Terminate ()
		{
			instance.TerminateInternal ();
			Desktop.Terminate ();
			instance = null;
		}

		public static Bus Bus { get { return Instance.bus; } }

		internal Registry (bool startLoop)
		{
			lock (sync) {
				if (instance != null)
					throw new Exception ("Attempt to create a second registry");
				instance = this;
			}

			bus = Bus.Session;
			applications = new Dictionary<string, Application> ();
			desktop = new Desktop ();
			proxy = bus.GetObject<RegistryInterface> ("org.freedesktop.atspi.Registry", new ObjectPath ("/org/freedesktop/atspi/registry"));
			proxy.Introspect ();
			proxy.UpdateApplications += OnUpdateApplications;

			string [] appNames = proxy.GetApplications ();
			foreach (string name in appNames) {
				Application application = new Application (name);
				applications [name] = application;
				desktop.Add (application);
			}
		}

		public static Desktop Desktop {
			get { return Instance.desktop; }
		}

		internal static Accessible GetElement (AccessiblePath ap, Accessible reference, bool create)
		{
			Application application = (reference != null?
					reference.application: null);
			return GetElement (ap, application, create);
		}

		internal static Accessible GetElement (AccessiblePath ap, Application reference, bool create)
		{
			Application application;
			application = (Instance.applications.ContainsKey (ap.bus_name)
				? Instance.applications [ap.bus_name]
				: reference);
			if (application == null)
				return null;
			return application.GetElement (ap.path, create);
		}
		internal void TerminateInternal ()
		{
			proxy.UpdateApplications -= OnUpdateApplications;
			foreach (string bus_name in applications.Keys)
				applications [bus_name].Dispose ();
			applications = null;
		}

		void OnUpdateApplications (int added, string name)
		{
			// added is really a bool
			if (added != 0) {
				Application app = new Application (name);
				applications [name] = app;
				desktop.Add (app);
			} else if (applications.ContainsKey (name)) {
				Application application = applications [name];
				desktop.Remove (application);
				applications.Remove (name);
				application.Dispose ();
			}
		}
	}

	[Interface ("org.freedesktop.atspi.Registry")]
	interface RegistryInterface : Introspectable
	{
		string [] GetApplications ();
		event UpdateApplicationsHandler UpdateApplications;
	}

	delegate void UpdateApplicationsHandler (int added, string name);
}
