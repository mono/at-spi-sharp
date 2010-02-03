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
		private Dictionary<string, Application> applications;
		private Desktop desktop;
		private static object sync = new object ();
		private static Thread loopThread;

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

		internal Registry (bool startLoop)
				: base ("org.freedesktop.atspi.Registry")
		{
			lock (sync) {
				if (instance != null)
					throw new Exception ("Attempt to create a second registry");
				instance = this;
			}

			bus = GetAtspiBus ();
			if (bus == null)
				bus = Bus.Session;

			if (startLoop && loopThread == null) {
				loopThread = new Thread (new ThreadStart (Iterate));
				loopThread.IsBackground = true;
				loopThread.Start ();
			}

			applications = new Dictionary<string, Application> ();
			applications [name] = this;
			desktop = new Desktop (this);
			accessibles [SPI_PATH_ROOT] = desktop;

			PostInit ();
			desktop.PostInit ();
		}

		[DllImport("libX11.so.6")]
		static extern IntPtr XOpenDisplay (string name);
		[DllImport("libX11.so.6")]
		static extern IntPtr XDefaultRootWindow (IntPtr display);
		[DllImport("libX11.so.6")]
		static extern IntPtr XInternAtom (IntPtr display, string atom_name, int only_if_eists);
		[DllImport("libX11.so.6")]
		static extern int XGetWindowProperty (IntPtr display, IntPtr w, IntPtr property, IntPtr long_offset, IntPtr long_length, int delete, IntPtr req_type, out IntPtr actual_type_return, out int actual_format_return, out IntPtr nitems_return, out IntPtr bytes_after_return, out string prop_return);

		public static Bus GetAtspiBus ()
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
			if (!applications.ContainsKey (name)) {
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
			foreach (string bus_name in applications.Keys)
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

		private void Iterate ()
		{
			// TODO: Make try optional; could affect performance
			for (;;)
			{
				try {
					bus.Iterate ();
				} catch (Exception e) {
					Console.WriteLine ("at-spi-sharp: Exception in Iterate: " + e);
				}
			}
		}
	}
}
