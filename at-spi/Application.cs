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
	internal class Application
	{
		internal string name;
		private ITree proxy;
		private Dictionary<string, Accessible> accessibles;

		internal Application (string name)
		{
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
			Accessible obj = GetElement (e.path, false);
			if (obj == null) {
				obj = new Accessible (this, e);
				accessibles [e.path] = obj;
			} else
				obj.Update (e);
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

		internal string Name {
			get { return name; }
		}

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
