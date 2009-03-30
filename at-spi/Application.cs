// Copyright 2009 Novell, Inc.
// This software is made available under the MIT License
// See COPYING for details

using System;
using System.Collections.Generic;
using NDesk.DBus;
using org.freedesktop.DBus;

namespace org.freedesktop.atspi
{
	internal class Application
	{
			internal string name;
		ITree proxy;
		Dictionary<string, Accessible> accessibles;

		internal Application (string name)
		{
Console.WriteLine ("dbg: add application: " + name);
			this.name = name;
			proxy = Registry.Bus.GetObject<ITree> (name, new ObjectPath ("/org/freedesktop/atspi/tree"));
			accessibles = new Dictionary<string, Accessible> ();
			AccessibleProxy [] elements = proxy.getTree ();
			proxy.updateAccessible += OnUpdateAccessible;
			proxy.removeAccessible += OnRemoveAccessible;
			AddAccessibles (elements);
		}

		void AddAccessibles (AccessibleProxy [] elements)
		{
			foreach (AccessibleProxy e in elements)
				GetElement (e);
	}

		Accessible GetElement (AccessibleProxy e)
		{
Console.WriteLine ("dbg: GetElement: " + e.role);
			Accessible obj = GetElement (e.path, true);
			obj.name = e.name;
			obj.parent = GetElement (e.parent, true);
			obj.role = (Role)e.role;
Console.WriteLine ("dbg: adding object: " + obj.Role);
			obj.description = e.description;
			foreach (string iface in e.interfaces)
				obj.AddInterface (iface);
			obj.stateSet = new StateSet (e.states);
			foreach (string path in e.children)
				obj.children.Add (GetElement (path, true));
			return obj;
		}

		void OnUpdateAccessible (AccessibleProxy nodeAdded)
		{
			GetElement (nodeAdded);
			// TODO: Send event
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
			return GetElement (op.ToString (), false);
		}

		internal string Name { get { return name; } }

		internal Accessible GetRoot ()
		{
			ObjectPath o = proxy.getRoot ();
			return GetElement (o);
		}
	}

	[Interface ("org.freedesktop.atspi.Tree")]
	interface ITree : Introspectable
	{
		ObjectPath getRoot ();
		AccessibleProxy [] getTree ();
		event UpdateAccessibleHandler updateAccessible;
		event RemoveAccessibleHandler removeAccessible;
	}

	delegate void UpdateAccessibleHandler (AccessibleProxy nodeAdded);
	delegate void RemoveAccessibleHandler (ObjectPath nodeRemoved);

	struct AccessibleProxy
	{
		public string path;
		public string parent;
		public string [] children;
		public string [] interfaces;
		public string name;
		public uint role;
		public string description;
		public int [] states;	// 2 32-bit flags
	}
}
