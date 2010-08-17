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
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using NDesk.DBus;
using org.freedesktop.DBus;

namespace Atspi
{
	public class Registry : Application
	{
		public static Registry Instance {
			get {
				return instance;
			}
		}

		private volatile static Registry instance;
		private Bus bus;
		private IBus busProxy;
		private IRegistry proxy;
		private Dictionary<string, Application> applications;
		private Desktop desktop;
		private bool suspendDBusCalls;
		private static object sync = new object ();
		private static Thread loopThread;
		internal static Queue<System.Action> pendingCalls = new Queue<System.Action> ();
		private Dictionary<string, int> eventListeners;

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

		internal static Bus Bus { get { return Instance.bus; } }

		internal static IBus BusProxy {
			get {
				return instance.busProxy;
			}
		}

		internal Registry (bool startLoop)
				: base ("org.a11y.atspi.Registry")
		{
			lock (sync) {
				if (instance != null)
					throw new Exception ("Attempt to create a second registry");
				instance = this;
			}

			bus = GetAtspiBus ();
			if (bus == null)
				bus = Bus.Session;

			ObjectPath op = new ObjectPath ("/org/a11y/atspi/registry");
			proxy = Registry.Bus.GetObject<IRegistry> ("org.a11y.atspi.Registry", op);
			eventListeners = new Dictionary<string, int> ();
			RegisterEventListener ("Object:ChildrenChanged");
			RegisterEventListener ("Object:StateChanged");
			RegisterEventListener ("Object:PropertyChange");

			applications = new Dictionary<string, Application> ();
			applications [name] = this;
			desktop = new Desktop (this);
			accessibles [SPI_PATH_ROOT] = desktop;

			busProxy = Bus.GetObject<IBus> ("org.freedesktop.DBus", new ObjectPath ("/org/freedesktop/DBus"));

			PostInit ();
			desktop.PostInit ();

			if (DeviceEventController.Instance == null)
				new DeviceEventController ();

			if (startLoop && loopThread == null) {
				loopThread = new Thread (new ThreadStart (Iterate));
				loopThread.IsBackground = true;
				loopThread.Start ();
			}
		}

		[DllImport("libX11.so.6")]
		static extern IntPtr XOpenDisplay (string name);
		[DllImport("libX11.so.6")]
		static extern IntPtr XDefaultRootWindow (IntPtr display);
		[DllImport("libX11.so.6")]
		static extern IntPtr XInternAtom (IntPtr display, string atom_name, int only_if_eists);
		[DllImport("libX11.so.6")]
		static extern int XGetWindowProperty (IntPtr display, IntPtr w, IntPtr property, IntPtr long_offset, IntPtr long_length, int delete, IntPtr req_type, out IntPtr actual_type_return, out int actual_format_return, out IntPtr nitems_return, out IntPtr bytes_after_return, out string prop_return);

		internal static Bus GetAtspiBus ()
		{
			string displayName = Environment.GetEnvironmentVariable ("AT_SPI_DISPLAY");
			if (displayName == null || displayName == String.Empty)
				displayName = Environment.GetEnvironmentVariable ("DISPLAY");
			if (displayName == null || displayName == String.Empty)
				displayName = ":0";
			for (int i = displayName.Length - 1; i >= 0; i--) {
				if (displayName [i] == '.') {
					displayName = displayName.Substring (0, i);
					break;
				} else if (displayName [i] == ':')
					break;
			}
			IntPtr display = XOpenDisplay (displayName);
			if (display == IntPtr.Zero)
				return null;
			IntPtr atSpiBus = XInternAtom (display, "AT_SPI_BUS", 0);
			IntPtr actualType, nItems, leftOver;
			int actualFormat;
			string data;
			XGetWindowProperty (display,
				XDefaultRootWindow (display),
					atSpiBus, (IntPtr) 0,
					(IntPtr) 2048, 0,
				(IntPtr) 31, out actualType, out actualFormat,
				out nItems, out leftOver, out data);
			if (data == null)
				return null;
			return Bus.Open (data);
		}

		internal Application GetApplication (string name)
		{
			return GetApplication (name, true);
		}

		internal Application GetApplication (string name, bool create)
		{
			if (string.IsNullOrEmpty (name))
				return null;
			if (!applications.ContainsKey (name) && create) {
				// TODO: Perhaps allow reentering GLib main
				// loop to support inspecting our own window
				if (BusProxy.GetConnectionUnixProcessID (name) == Process.GetCurrentProcess ().Id)
					return null;
				applications [name] = new Application (name);
				desktop.Add (applications [name]);
				applications [name].PostInit ();
			}
			if (applications.ContainsKey (name))
				return applications [name];
			return null;
		}

		public static Desktop Desktop {
			get { return Instance.desktop; }
		}

		internal static Accessible GetElement (AccessiblePath ap, bool create)
		{
			Application application;
			application = Instance.GetApplication (ap.bus_name, create);
			if (application == null)
				return null;
			return application.GetElement (ap.path, create);
		}

		internal void TerminateInternal ()
		{
			List<string> dispose = new List<string> ();
			foreach (string bus_name in applications.Keys)
				dispose.Add (bus_name);
			// TODO: Thread safety
			foreach (string bus_name in dispose)
				if (applications.ContainsKey (bus_name))
					applications [bus_name].Dispose ();
			applications = null;
		}

		internal void RemoveApplication (string name)
		{
			if (applications.ContainsKey (name)) {
				Application application = applications [name];
				Desktop.Remove (application);
				applications.Remove (name);
				application.Dispose ();
			}
		}

		internal static bool SuspendDBusCalls {
			get {
				return instance.suspendDBusCalls;
			}
			set {
				if (instance.suspendDBusCalls && !value) {
					instance.suspendDBusCalls = value;
					instance.ProcessPendingCalls ();
				} else
					instance.suspendDBusCalls = value;
			}
		}

		private void ProcessPendingCalls ()
		{
			while (pendingCalls.Count > 0)
				pendingCalls.Dequeue ()();
		}

		private void Iterate ()
		{
			// TODO: Make try optional; could affect performance
			for (;;)
			{
				try {
					bus.Iterate ();
				} catch (System.Threading.ThreadAbortException) {
					return;
				} catch (Exception e) {
					Console.WriteLine ("at-spi-sharp: Exception in Iterate: " + e);
				}
			}
		}

		internal static void RegisterEventListener (string name)
		{
			if (!instance.eventListeners.ContainsKey (name))
				instance.eventListeners [name] = 1;
			else
				instance.eventListeners [name]++;
			if (instance.eventListeners [name] == 1)
				instance.proxy.RegisterEvent (name);
		}

		internal static void DeregisterEventListener (string name)
		{
			if (!instance.eventListeners.ContainsKey (name) ||
				instance.eventListeners [name] == 0)
				return;
			instance.eventListeners [name]--;
			if (instance.eventListeners [name] == 0)
				instance.proxy.DeregisterEvent (name);
		}
	}

	[Interface ("org.a11y.atspi.Registry")]
	interface IRegistry
	{
		void RegisterEvent (string name);
		void DeregisterEvent (string name);
	}
}
