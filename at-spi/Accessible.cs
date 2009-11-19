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
	public class Accessible : IDisposable
	{
		internal string path;
		IAccessible proxy;
		IEventObject objectEvents;
		IEventWindow windowEvents;
		IEventTerminal terminalEvents;
		IEventDocument documentEvents;
		IEventFocus focusEvents;

		internal Application application;
		protected IList<Accessible> children;
		private Accessible parent;
		private string name;
		private string description;
		protected Role role;
		private StateSet stateSet;
		private Interfaces interfaces;

		private Properties properties;

		private const string IFACE = "org.freedesktop.atspi.Accessible";

		internal Accessible (Application application, string path)
		{
			this.application = application;
			this.path = path;
			this.children = new List<Accessible> ();
			if (application != null)
				proxy = Registry.Bus.GetObject<IAccessible> (application.name, new ObjectPath (path));
			if (path != null) {
				ObjectPath op = new ObjectPath (path);
				properties = Registry.Bus.GetObject<Properties> (application.name, op);
			}
		}

		internal Accessible (Application application, AccessibleProxy e)
		{
			this.application = application;
			this.path = e.path.ToString ();
			proxy = Registry.Bus.GetObject<IAccessible> (application.name, e.path);
			Update (e);
		}

		public void Dispose ()
		{
			Dispose (true);
		}

		protected virtual  void Dispose (bool disposing)
		{
			if (parent != null) {
				Desktop.RaiseChildRemoved (parent, this);
				parent.children.Remove (this);
			}
			children.Clear ();
			stateSet.Add (StateType.Defunct);
		}

		internal void Update (AccessibleProxy e)
		{
			bool initializing = (stateSet == null);
			if (e.name != name) {
				string oldName = name;
				name = e.name;
				if (!initializing)
					Desktop.RaiseNameChanged (this, oldName, e.name);
			}

			Accessible newParent = Registry.GetElement (e.parent, this, true);
			if (newParent != parent) {
				Accessible oldParent = parent;
				parent = newParent;
				if (!initializing) {
					Desktop.RaiseChildRemoved (oldParent, this);
					Desktop.RaiseChildAdded (parent, this);
				}
			}

			Role newRole = (Role)e.role;
			if (newRole != role) {
				Role oldRole = role;
				role = newRole;
				if (!initializing)
					Desktop.RaiseRoleChanged (this, oldRole, newRole);
			}

			if (e.description != description) {
				string oldDescription = description;
				description = e.description;
				if (!initializing)
					Desktop.RaiseDescriptionChanged (this, oldDescription, e.description);
			}

			foreach (string iface in e.interfaces)
				AddInterface (iface);

			StateSet newStateSet = new StateSet (e.states);
			if (newStateSet != stateSet) {
				StateSet oldStateSet = stateSet;
				stateSet = newStateSet;
				if (!initializing)
					foreach (StateType type in Enum.GetValues (typeof (StateType)))
						if (oldStateSet.Contains (type) != newStateSet.Contains (type))
							Desktop.RaiseStateChanged (this, type, newStateSet.Contains (type));
			}

			if (stateSet.Contains (StateType.ManagesDescendants)) {
				if (!(children is UncachedChildren))
					children = new UncachedChildren (this);
			} else {
				List<Accessible> oldChildren = children as List<Accessible>;
				if (!(children is List<Accessible>))
					children = new List<Accessible> ();
				children.Clear ();
				foreach (AccessiblePath path in e.children) {
					Accessible child = Registry.GetElement (path, this, true);
					if (!initializing &&
						(oldChildren == null ||
						oldChildren.IndexOf (child) == -1))
						Desktop.RaiseChildAdded (this, child);
					children.Add (child);
				}
				if (!initializing && oldChildren != null)
					foreach (Accessible child in oldChildren)
						if (!e.ContainsChild (child.path))
							Desktop.RaiseChildRemoved (this, child);
			}
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
			} else if (name == "org.freedesktop.atspi.Collection") {
				// All objects should support this
			} else if (name == "org.freedesktop.atspi.Component") {
				flag = Interfaces.Component;
			} else if (name == "org.freedesktop.atspi.Document") {
				flag = Interfaces.Document;
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

		public Application Application {
			get {
				return application;
			}
		}

		public Accessible Parent {
			get { return parent; }
		}

		public IList<Accessible> Children {
			get { return children; }
		}

		public Accessible GetChildAtIndexNoCache (int index)
		{
			AccessiblePath path = proxy.GetChildAtIndex (index);
			return Registry.GetElement (path, this, true);
		}

		public int IndexInParent {
			get {
				if (parent == null)
					return -1;
				return parent.children.IndexOf (this);
			}
		}

		public int IndexInParentNoCache {
			get {
				return proxy.GetIndexInParent ();
			}
		}

		public int ChildCountNoCache {
			get {
				if (path == null)
					throw new NotSupportedException ();
				return (int) properties.Get (IFACE, "ChildCount");
			}
		}

		public Role Role {
			get { return role; }
		}

		public StateSet StateSet {
			get { return stateSet; }
		}

		public Relation [] RelationSet {
			get {
				if (proxy == null)
					return new Relation [0];
				DBusRelation [] dRels = proxy.GetRelationSet ();
				Relation [] set = new Relation [dRels.Length];
				for (int i = 0; i < dRels.Length; i++)
					set [i] = new Relation (application, dRels [i]);
				return set;
			}
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

		public Document QueryDocument ()
		{
			if ((interfaces & Interfaces.Document) != 0)
				return new Document (this);
			return null;
		}

		public EditableText QueryEditableText ()
		{
			if ((interfaces & Interfaces.EditableText) != 0)
				return new EditableText (this);
			return null;
		}

		public Hypertext QueryHypertext ()
		{
			if ((interfaces & Interfaces.Hypertext) != 0)
				return new Hypertext (this);
			return null;
		}

		public Image QueryImage ()
		{
			if ((interfaces & Interfaces.Image) != 0)
				return new Image (this);
			return null;
		}

		public Selection QuerySelection ()
		{
			if ((interfaces & Interfaces.Selection) != 0)
				return new Selection (this);
			return null;
		}

		public Table QueryTable ()
		{
			if ((interfaces & Interfaces.Table) != 0)
				return new Table (this);
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

		public IEventObject ObjectEvents {
			get {
				if (objectEvents == null)
					objectEvents = Registry.Bus.GetObject<IEventObject> (application.name, new ObjectPath (path));
				return objectEvents;
			}
		}

		public IEventWindow WindowEvents {
			get {
				if (windowEvents == null &&
					(Role == Role.Window ||
					 Role == Role.Frame ||
					 Role == Role.Dialog))
					windowEvents = Registry.Bus.GetObject<IEventWindow> (application.name, new ObjectPath (path));
				return windowEvents;
			}
		}

		public IEventTerminal TerminalEvents {
			get {
				if (terminalEvents == null && Role == Role.Terminal)
					terminalEvents = Registry.Bus.GetObject<IEventTerminal> (application.name, new ObjectPath (path));
				return terminalEvents;
			}
		}

		public IEventDocument DocumentEvents {
			get {
				if (documentEvents == null)
					documentEvents = Registry.Bus.GetObject<IEventDocument> (application.name, new ObjectPath (path));
				return documentEvents;
			}
		}

		public IEventFocus FocusEvents {
			get {
				if (focusEvents == null)
					focusEvents = Registry.Bus.GetObject<IEventFocus> (application.name, new ObjectPath (path));
				return focusEvents;
			}
		}
	}

	public delegate bool FindPredicate (Accessible a, params object [] args);

	[System.Flags]
	public enum Interfaces
	{
		Action = 0x0001,
		Component = 0x0002,
		Document = 0x0004,
		EditableText = 0x0008,
		Hypertext = 0x0010,
		Image = 0x0020,
		Selection = 0x0040,
		StreamableContent = 0x0080,
		Table = 0x0100,
		Text = 0x0200,
		Value = 0x0400
	}

	[Interface ("org.freedesktop.atspi.Accessible")]
	interface IAccessible : Introspectable
	{
		DBusRelation [] GetRelationSet ();
		int GetIndexInParent ();
		AccessiblePath GetChildAtIndex (int index);
	}
}
