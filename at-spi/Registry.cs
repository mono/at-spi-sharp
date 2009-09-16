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
		private List<Application> applications;
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
			applications = new List<Application> ();
			desktop = new Desktop ();
			proxy = bus.GetObject<RegistryInterface> ("org.freedesktop.atspi.Registry", new ObjectPath ("/org/freedesktop/atspi/registry"));
			proxy.updateApplications += OnUpdateApplications;

			BusG.Init ();

			string [] appNames = proxy.getApplications ();
			foreach (string app in appNames) {
				Application application = new Application (app);
				applications.Add (application);
				desktop.Add (application);
			}
		}

		public static Desktop Desktop {
			get { return Instance.desktop; }
		}

		internal void TerminateInternal ()
		{
			proxy.updateApplications -= OnUpdateApplications;
		}

		void OnUpdateApplications (int added, string name)
		{
			// added is really a bool
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
		event UpdateApplicationsHandler updateApplications;
	}

	delegate void UpdateApplicationsHandler (int added, string name);
}
