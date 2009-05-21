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
	public class Accessible
	{
		internal string path;
		//IAccessible proxy;
		internal Application application;
		internal List<Accessible> children;
		private Accessible parent;
		private string name;
		private string description;
		private Role role;
		private StateSet stateSet;
		private Interfaces interfaces;

		internal Accessible (Application application, string path)
		{
			this.application = application;
			this.path = path;
			this.children = new List<Accessible> ();
			//proxy = Registry.Bus.GetObject<AccessibleInterface> (application.name, new ObjectPath (path));
		}

		internal Accessible (Application application, AccessibleProxy e)
		{
			this.application = application;
			this.path = e.path;
			this.children = new List<Accessible> ();
			//proxy = Registry.Bus.GetObject<AccessibleInterface> (application.name, new ObjectPath (path));
			Update (e);
		}

		internal void Dispose ()
		{
			// TODO: Fire event
			if (parent != null)
				parent.children.Remove (this);
			children.Clear ();
			stateSet.AddState (StateType.Defunct);
		}

		internal void Update (AccessibleProxy e)
		{
			name = e.name;
			parent = application.GetElement (e.parent, true);
			role = (Role)e.role;
			description = e.description;
			foreach (string iface in e.interfaces)
				AddInterface (iface);
			stateSet = new StateSet (e.states);
			foreach (string path in e.children)
				children.Add (application.GetElement (path, true));
		}

		internal void AddInterface (string name)
		{
			SetInterface (name, true);
		}

		void SetInterface (string name, bool val)
		{
			Interfaces flag = 0;

			if (name == "org.freedesktop.atspi.Accessible") {
				// All objects should support this
			} else if (name == "org.freedesktop.atspi.Action") {
				flag = Interfaces.Action;
			} else if (name == "org.freedesktop.atspi.Component") {
				flag = Interfaces.Component;
			} else if (name == "org.freedesktop.atspi.EditableText") {
				flag = Interfaces.EditableText;
			} else if (name == "org.freedesktop.atspi.Hypertext") {
				flag = Interfaces.Hypertext;
			} else if (name == "org.freedesktop.atspi.Image") {
				flag = Interfaces.Image;
			} else if (name == "org.freedesktop.atspi.Selection") {
				flag = Interfaces.Selection;
			} else if (name == "org.freedesktop.atspi.StreamableContent") {
				flag = Interfaces.StreamableContent;
			} else if (name == "org.freedesktop.atspi.Table") {
				flag = Interfaces.Table;
			} else if (name == "org.freedesktop.atspi.Text") {
				flag = Interfaces.Text;
			} else if (name == "org.freedesktop.atspi.Value") {
				flag = Interfaces.Value;
			} else
				// TODO: Don't release with this exception
				throw new ArgumentException ("Invalid interface name \"" + name + "\"");
			if (val)
				interfaces |= flag;
			else
				interfaces &= ~flag;
		}

		public Accessible Parent {
			get { return parent; }
		}

		public IList<Accessible> Children {
			get { return children; }
		}

		public int IndexInParent {
			get {
				if (parent == null)
					return -1;
				return parent.children.IndexOf (this);
			}
		}

		public Role Role {
			get { return role; }
		}

		public StateSet StateSet {
			get { return stateSet; }
		}

		public string Name {
			get { return name; }
		}

		public string Description {
			get { return description; }
		}

		public Accessible FindDescendant (FindPredicate d, params object [] args)
		{
			return FindDescendantDepthFirst (d, args);
		}

		public Accessible FindDescendant (FindPredicate d, bool breadthFirst, params object [] args)
		{
			if (breadthFirst)
				return FindDescendantBreadthFirst (d, args);
			else
				return FindDescendantDepthFirst (d, args);
		}

		private Accessible FindDescendantDepthFirst (FindPredicate d, object [] args)
		{
			foreach (Accessible a in children) {
				if (d (a, args))
					return a;
				Accessible ret = a.FindDescendantDepthFirst (d, args);
				if (ret != null)
					return ret;
			}
			return null;
		}

		private Accessible FindDescendantBreadthFirst (FindPredicate d, object [] args)
		{
			foreach (Accessible a in children)
				if (d (a, args))
					return a;
			foreach (Accessible a in children) {
				Accessible ret = a.FindDescendantBreadthFirst (d, args);
				if (ret != null)
					return ret;
			}
			return null;
		}

		public Action QueryAction ()
		{
			if ((interfaces & Interfaces.Action) != 0)
				return new Action (this);
			return null;
		}

		public Component QueryComponent ()
		{
			if ((interfaces & Interfaces.Component) != 0)
				return new Component (this);
			return null;
		}

		public EditableText QueryEditableText ()
		{
			if ((interfaces & Interfaces.EditableText) != 0)
				return new EditableText (this);
			return null;
		}

		public Image QueryImage ()
		{
			if ((interfaces & Interfaces.Image) != 0)
				return new Image (this);
			return null;
		}

		public Text QueryText ()
		{
			if ((interfaces & Interfaces.Text) != 0)
				return new Text (this);
			return null;
		}

		public Value QueryValue ()
		{
			if ((interfaces & Interfaces.Value) != 0)
				return new Value (this);
			return null;
		}
	}

	public delegate bool FindPredicate (Accessible a, params object [] args);

	[System.Flags]
	public enum Interfaces
	{
		Action = 0x0001,
		Component = 0x0002,
		EditableText = 0x0004,
		Hypertext = 0x0008,
		Image = 0x0010,
		Selection = 0x0020,
		StreamableContent = 0x0040,
		Table = 0x0080,
		Text = 0x0100,
		Value = 0x0200
	}
}
