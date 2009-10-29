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
		internal string name;
		private ITree proxy;
		private Properties properties;
		private Dictionary<string, Accessible> accessibles;

		private const string IFACE = "org.freedesktop.atspi.Application";

		internal Application (string name)
		{
			this.name = name;
			proxy = Registry.Bus.GetObject<ITree> (name, new ObjectPath ("/org/freedesktop/atspi/tree"));
			accessibles = new Dictionary<string, Accessible> ();
			accessibles ["/org/freedesktop/atspi/accessible/null"] = null;
			proxy.UpdateAccessible += OnUpdateAccessible;
			proxy.RemoveAccessible += OnRemoveAccessible;
			properties = Registry.Bus.GetObject<Properties> (name, proxy.GetRoot ());
			AccessibleProxy [] elements = proxy.GetTree ();
			AddAccessibles (elements);
		}

		public void Dispose ()
		{
			Dispose (true);
		}

		protected virtual void Dispose (bool disposing)
		{
			proxy.UpdateAccessible -= OnUpdateAccessible;
		}

		void AddAccessibles (AccessibleProxy [] elements)
		{
			foreach (AccessibleProxy e in elements)
				GetElement (e);
		}

		Accessible GetElement (AccessibleProxy e)
		{
			Accessible obj = GetElement (e.path, false);
			if (obj == null) {
				obj = new Accessible (this, e);
				accessibles [e.path.ToString ()] = obj;
			} else
				obj.Update (e);
			return obj;
		}

		void OnUpdateAccessible (AccessibleProxy nodeAdded)
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

		internal Accessible GetRoot ()
		{
			ObjectPath o = proxy.GetRoot ();
			return GetElement (o);
		}

		public string ToolkitName {
			get {
				return (string) properties.Get (IFACE, "ToolkitName");
			}
		}
	}

	[Interface ("org.freedesktop.atspi.Tree")]
	interface ITree : Introspectable
	{
		ObjectPath GetRoot ();
		AccessibleProxy [] GetTree ();
		event UpdateAccessibleHandler UpdateAccessible;
		event RemoveAccessibleHandler RemoveAccessible;
	}

	delegate void UpdateAccessibleHandler (AccessibleProxy nodeAdded);
	delegate void RemoveAccessibleHandler (ObjectPath nodeRemoved);

	struct AccessibleProxy
	{
		public ObjectPath path;
		public ObjectPath parent;
		public ObjectPath [] children;
		public string [] interfaces;
		public string name;
		public uint role;
		public string description;
		public uint [] states;	// 2 32-bit flags

		public bool ContainsChild (string path)
		{
			foreach (ObjectPath child in children)
				if (child.ToString () == path)
					return true;
			return false;
		}
	}
}
