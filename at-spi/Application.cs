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
using NDesk.DBus;
using org.freedesktop.DBus;

namespace Atspi
{

	public class Application : IDisposable
	{
		internal const string SPI_PATH_ROOT = "/org/a11y/atspi/accessible/root";
		internal const string SPI_PATH_NULL = "/org/a11y/atspi/null";

		internal string name;
		private ICache proxy;
		private Properties properties;
		protected Dictionary<string, Accessible> accessibles;

		public bool Disposed {
			get; private set;
		}

		private const string IFACE = "org.a11y.atspi.Application";

		internal Application (string name)
		{
			this.name = name;
			accessibles = new Dictionary<string, Accessible> ();
			accessibles [SPI_PATH_NULL] = null;
		}

		internal void PostInit ()
		{
			if (Registry.SuspendDBusCalls) {
				Registry.pendingCalls.Enqueue (() =>
					PostInit ());
				return;
			}

			proxy = Registry.Bus.GetObject<ICache> (name, new ObjectPath ("/org/a11y/atspi/cache"));

			proxy.AddAccessible += OnAddAccessible;
			proxy.RemoveAccessible += OnRemoveAccessible;
			ObjectPath op = new ObjectPath (SPI_PATH_ROOT);
			properties = Registry.Bus.GetObject<Properties> (name, op);

			if (!(this is Registry)) {
				AccessibleProxy [] elements;
				try {
					elements = proxy.GetItems ();
				} catch (System.Exception) {
					return;
				}
				AddAccessibles (elements);
			}
		}

		public void Dispose ()
		{
			Dispose (true);
		}

		protected virtual void Dispose (bool disposing)
		{
			if (Disposed)
				return;
			if (proxy != null) {
				proxy.RemoveAccessible -= OnRemoveAccessible;
				proxy.AddAccessible -= OnAddAccessible;
			}
			Disposed = true;
			foreach (Accessible a in accessibles.Values)
				if (a != null)
					a.Dispose ();
		}

		void AddAccessibles (AccessibleProxy [] elements)
		{
			foreach (AccessibleProxy e in elements)
				GetElement (e);
		}

		Accessible GetElement (AccessibleProxy e)
		{
			Accessible obj = GetElement (e.path.path, true);
			obj.Update (e);
			return obj;
		}

		void OnAddAccessible (AccessibleProxy nodeAdded)
		{
			GetElement (nodeAdded);
		}

		void OnRemoveAccessible (ObjectPath nodeRemoved)
		{
			Accessible a = GetElement (nodeRemoved);
			if (a != null)
				a.Dispose ();
		}

		internal Accessible GetElement (string path, bool create)
		{
			Accessible obj;
			if (accessibles.TryGetValue (path, out obj))
				return obj;
			if (!create)
				return null;
			obj = new Accessible (this, path);
			accessibles [path] = obj;
			// Events must be initialized after insertion into
			// hash, since we need to gracefully handle reentrancy
			obj.InitEvents ();
			return obj;
		}

		internal Accessible GetElement (ObjectPath op)
		{
			return GetElement (op, false);
		}

		internal Accessible GetElement (ObjectPath op, bool create)
		{
			return GetElement (op.ToString (), create);
		}

		internal string Name {
			get { return name; }
		}

		internal Accessible Root  {
			get {
				return GetElement ("/org/a11y/atspi/accessible/root", true);
			}
		}

		public string ToolkitName {
			get {
				return (string) properties.Get (IFACE, "ToolkitName");
			}
		}

		public uint Pid {
			get {
				return Registry.BusProxy.GetConnectionUnixProcessID (name);
			}
		}
	}

	[Interface ("org.a11y.atspi.Cache")]
	interface ICache : Introspectable
	{
		AccessibleProxy [] GetItems ();
		event AddAccessibleHandler AddAccessible;
		event RemoveAccessibleHandler RemoveAccessible;
	}

	delegate void AddAccessibleHandler (AccessibleProxy nodeAdded);
	delegate void RemoveAccessibleHandler (ObjectPath nodeRemoved);

	internal struct AccessiblePath
	{
		internal string bus_name;
		internal ObjectPath path;
	}

	struct AccessibleProxy
	{
		public AccessiblePath path;
		public AccessiblePath app_root;
		public AccessiblePath parent;
		public AccessiblePath [] children;
		public string [] interfaces;
		public string name;
		public uint role;
		public string description;
		public uint [] states;	// 2 32-bit flags
	}
}
