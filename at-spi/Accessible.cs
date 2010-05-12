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
		private static Dictionary<string, StateType> stateMapping;

		internal string path;
		internal IAccessible proxy;
		EventObject objectEvents;
		EventWindow windowEvents;
		EventTerminal terminalEvents;
		EventDocument documentEvents;
		EventFocus focusEvents;

		internal Application application;
		protected IList<Accessible> children;
		private Accessible parent;
		private string name;
		private string description;
		protected Role role;
		internal StateSet stateSet;
		private Interfaces interfaces;

		private Properties properties;

		private const string IFACE = "org.a11y.atspi.Accessible";

		internal Accessible (Application application, string path)
		{
			this.application = application;
			this.path = path;
			if (application != null)
				proxy = Registry.Bus.GetObject<IAccessible> (application.name, new ObjectPath (path));
			if (path != null) {
				ObjectPath op = new ObjectPath (path);
				properties = Registry.Bus.GetObject<Properties> (application.name, op);
			}
			interfaces = Interfaces.Invalid;
		}

		internal Accessible (Application application, AccessibleProxy e)
		{
			this.application = application;
			this.path = e.path.ToString ();
			proxy = Registry.Bus.GetObject<IAccessible> (application.name, e.path.path);
			Update (e);
		}

		internal void InitEvents ()
		{
			if (Registry.SuspendDBusCalls) {
				Registry.pendingCalls.Enqueue (() =>
					InitEvents ());
				return;
			}

			if (stateMapping == null)
				InitStateMapping ();
			if (objectEvents == null && ObjectEvents != null) {
				ObjectEvents.StateChanged += OnStateChanged;
				ObjectEvents.ChildrenChanged += OnChildrenChanged;
				ObjectEvents.PropertyChange += OnPropertyChange;
			}
		}

		private static void InitStateMapping ()
		{
			stateMapping = new Dictionary<string, StateType> ();
			stateMapping ["active"] = StateType.Active;
			stateMapping ["armed"] = StateType.Armed;
			stateMapping ["busy"] = StateType.Busy;
			stateMapping ["checked"] = StateType.Checked;
			stateMapping ["collapsed"] = StateType.Collapsed;
			stateMapping ["defunct"] = StateType.Defunct;
			stateMapping ["editable"] = StateType.Editable;
			stateMapping ["enabled"] = StateType.Enabled;
			stateMapping ["expandable"] = StateType.Expandable;
			stateMapping ["expanded"] = StateType.Expanded;
			stateMapping ["focusable"] = StateType.Focusable;
			stateMapping ["focused"] = StateType.Focused;
			stateMapping ["has-tooltip"] = StateType.HasToolTip;
			stateMapping ["horizontal"] = StateType.Horizontal;
			stateMapping ["iconified"] = StateType.Iconified;
			stateMapping ["modal"] = StateType.Modal;
			stateMapping ["multi-line"] = StateType.MultiLine;
			stateMapping ["multiselectable"] = StateType.Multiselectable;
			stateMapping ["opaque"] = StateType.Opaque;
			stateMapping ["pressed"] = StateType.Pressed;
			stateMapping ["resizable"] = StateType.Resizable;
			stateMapping ["selectable"] = StateType.Selectable;
			stateMapping ["selected"] = StateType.Selected;
			stateMapping ["sensitive"] = StateType.Sensitive;
			stateMapping ["showing"] = StateType.Showing;
			stateMapping ["single-line"] = StateType.SingleLine;
			stateMapping ["stale"] = StateType.Stale;
			stateMapping ["transient"] = StateType.Transient;
			stateMapping ["vertical"] = StateType.Vertical;
			stateMapping ["visible"] = StateType.Visible;
			stateMapping ["manages-descendants"] = StateType.ManagesDescendants;
			stateMapping ["indeterminate"] = StateType.Indeterminate;
			stateMapping ["required"] = StateType.Required;
			stateMapping ["truncated"] = StateType.Truncated;
			stateMapping ["animated"] = StateType.Animated;
			stateMapping ["invalid-entry"] = StateType.InvalidEntry;
			stateMapping ["supports-autocompletion"] = StateType.SupportsAutocompletion;
			stateMapping ["selectable-text"] = StateType.SelectableText;
			stateMapping ["is-default"] = StateType.IsDefault;
			stateMapping ["visited"] = StateType.Visited;
		}

		public void Dispose ()
		{
			Dispose (true);
		}

		protected virtual  void Dispose (bool disposing)
		{
			if (stateSet != null && stateSet.Contains (StateType.Defunct))
				return;
			if (parent != null) {
				Desktop.RaiseChildRemoved (parent, this);
				if (parent.children != null)
					parent.children.Remove (this);
			}
			if (children is List<Accessible>)
				children.Clear ();
			if (stateSet == null)
				stateSet = new StateSet ();
			stateSet.Add (StateType.Defunct);
			if (ObjectEvents != null) {
				ObjectEvents.PropertyChange -= OnPropertyChange;
				ObjectEvents.ChildrenChanged -= OnChildrenChanged;
				ObjectEvents.StateChanged -= OnStateChanged;
			}
		}

		private void OnStateChanged (Accessible sender, string state, bool set)
		{
			if (stateMapping.ContainsKey (state)) {
				StateType type = stateMapping [state];
				if (set)
					StateSet.Add (type);
				else
					StateSet.Remove (type);
				Desktop.RaiseStateChanged (this, type, set);
			}
		}

		private void OnChildrenChanged (Accessible sender, string detail, int n, Accessible child)
		{
			bool added = (detail == "add");
			if (added && child == null)
				return;
			if (!added && this is Desktop) {
				if (child != null)
					Registry.Instance.RemoveApplication (child.application.name);
			}
			else if (children is List<Accessible>) {
				if (added) {
					if (!(this is Desktop))
						children.Insert (n, child);
				}
				else if (child != null)
					children.Remove (child);
				else if (n >= 0 && n < children.Count)
					children.RemoveAt (n);
			}
			if (added)
				Desktop.RaiseChildAdded (this, child);
			else
				Desktop.RaiseChildRemoved (this, child);
		}

		private void OnPropertyChange (Accessible sender, string property, object value)
		{
			if (property == "accessible-name") {
				string oldName = name;
				name = value as string;
				Desktop.RaiseNameChanged (sender, oldName, name);
			}
			else if (property == "accessible-description") {
				string oldDescription = description;
				description = value as string;
				Desktop.RaiseDescriptionChanged (sender, oldDescription, description);
			}
			else if (property == "accessible-parent" && value is Accessible) {
				parent = (Accessible)value;
			}
			else if (property == "accessible-role" && value is uint) {
				Role oldRole = role;
				role = (Role) (uint) value;
				Desktop.RaiseRoleChanged (sender, oldRole, role);
			}
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

			parent = Registry.GetElement (e.parent, false);
			if (parent == null && role == Role.Application)
				parent = Desktop.Instance;
			// Assuming that old and new parents are also
			// going to send ChildrenChanged signals, so
			// not going to update their caches or send
			// add/remove notifications here.

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

			interfaces = 0;
			foreach (string iface in e.interfaces)
				AddInterface (iface);

			stateSet = new StateSet (this, e.states);
			// Using at-spi StateChanged events to broadcast
			// changes for now; needed for gail Focused handling
			UpdateChildren (e.children);
		}

		private bool PathListContains (AccessiblePath [] paths, string bus_name, string path)
		{
			foreach (AccessiblePath child in paths)
				if (child.bus_name == bus_name && child.path.ToString () == path)
					return true;
			return false;
		}

		private void UpdateChildren (AccessiblePath [] childPaths)
		{
			bool initializing = (children == null);
			if (StateSet.Contains (StateType.ManagesDescendants) ||
				(Role == Role.Filler)) {
				// Do not cache filler to work around BGO#577392
				if (!(children is UncachedChildren))
					children = new UncachedChildren (this);
			} else {
				Accessible [] oldChildren = null;
				if (children is List<Accessible>) {
					oldChildren = new Accessible [children.Count];
					children.CopyTo (oldChildren, 0);
				}
				children = new List<Accessible> ();
				children.Clear ();
				foreach (AccessiblePath path in childPaths) {
					Accessible child = Registry.GetElement (path, true);
					if (!initializing &&
						(oldChildren == null ||
						Array.IndexOf (oldChildren, child) == -1))
						Desktop.RaiseChildAdded (this, child);
					children.Add (child);
				}
				if (!initializing && oldChildren != null)
					foreach (Accessible child in oldChildren)
						if (!PathListContains (childPaths, child.application.name, child.path))
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

			if (name == "org.a11y.atspi.Accessible") {
				// All objects should support this
			} else if (name == "org.a11y.atspi.Action") {
				flag = Interfaces.Action;
			} else if (name == "org.a11y.atspi.Application") {
				flag = Interfaces.Application;
			} else if (name == "org.a11y.atspi.Collection") {
				// All objects should support this
			} else if (name == "org.a11y.atspi.Component") {
				flag = Interfaces.Component;
			} else if (name == "org.a11y.atspi.Document") {
				flag = Interfaces.Document;
			} else if (name == "org.a11y.atspi.EditableText") {
				flag = Interfaces.EditableText;
			} else if (name == "org.a11y.atspi.Hyperlink") {
				flag = Interfaces.Hyperlink;
			} else if (name == "org.a11y.atspi.Hypertext") {
				flag = Interfaces.Hypertext;
			} else if (name == "org.a11y.atspi.Image") {
				flag = Interfaces.Image;
			} else if (name == "org.a11y.atspi.Selection") {
				flag = Interfaces.Selection;
			} else if (name == "org.a11y.atspi.StreamableContent") {
				flag = Interfaces.StreamableContent;
			} else if (name == "org.a11y.atspi.Table") {
				flag = Interfaces.Table;
			} else if (name == "org.a11y.atspi.Text") {
				flag = Interfaces.Text;
			} else if (name == "org.a11y.atspi.Value") {
				flag = Interfaces.Value;
			} else
				Console.WriteLine ("at-spi-sharp: Warning: Unknown interface name \"" + name + "\"");
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
			get {
				if (parent == null && !(this is Desktop)) {
					object o = properties.Get (IFACE, "Parent");
					AccessiblePath path = (AccessiblePath) Convert.ChangeType (o, typeof (AccessiblePath));
					parent = Registry.GetElement (path, false);
					if (parent == null && Role == Role.Application)
						parent = Desktop.Instance;
				}
				return parent;
			}
		}

		public IList<Accessible> Children {
			get {
				if (children == null) {
					AccessiblePath [] childPaths;
					try {
						childPaths = proxy.GetChildren ();
					} catch (System.Exception) {
						children = new List<Accessible> ();
						return children;
					}
					UpdateChildren (childPaths);
				}
				return children;
			}
		}

		public Accessible GetChildAtIndexNoCache (int index)
		{
			AccessiblePath path = proxy.GetChildAtIndex (index);
			return Registry.GetElement (path, true);
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
			get {
				if (role == Role.Invalid) {
					try {
						role = (Role) proxy.GetRole ();
					} catch (System.Exception) { }
				}
				return role;
			}
		}

		public StateSet StateSet {
			get {
				if (stateSet == null) {
					uint [] states;
					try {
						states = proxy.GetState ();
					} catch (System.Exception) {
						stateSet = new StateSet ();
						stateSet.Add (StateType.Defunct);
						return stateSet;
					}
					stateSet = new StateSet (this, states);
				}
				return stateSet;
			}
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

		public IDictionary<string, string> GetAttributes ()
		{
			return proxy.GetAttributes ();
		}

		public string Name {
			get {
				if (name == null) {
					try {
						name = (string) properties.Get (IFACE, "Name");
					} catch (System.Exception) {
						return null;
					}
				}
				return name;
			}
		}

		public string Description {
			get {
				if (description == null)
					description = (string) properties.Get (IFACE, "Description");
				return description;
			}
		}

		internal Interfaces Interfaces {
			get {
				if ((interfaces & Interfaces.Invalid) != 0) {
					string [] ifaces = proxy.GetInterfaces ();
					interfaces = 0;
					foreach (string iface in ifaces)
						AddInterface (iface);
				}
				return interfaces;
			}
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
			if (StateSet.Contains (StateType.ManagesDescendants))
				return null;
			Accessible [] childrenCopy = new Accessible [Children.Count];
			children.CopyTo (childrenCopy, 0);
			foreach (Accessible a in childrenCopy) {
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
			if (stateSet.Contains (StateType.ManagesDescendants))
				return null;
			Accessible [] childrenCopy = new Accessible [children.Count];
			children.CopyTo (childrenCopy, 0);
			foreach (Accessible a in childrenCopy)
				if (d (a, args))
					return a;
			foreach (Accessible a in childrenCopy) {
				Accessible ret = a.FindDescendantBreadthFirst (d, args);
				if (ret != null)
					return ret;
			}
			return null;
		}

		public Action QueryAction ()
		{
			if ((Interfaces & Interfaces.Action) != 0)
				return new Action (this);
			return null;
		}

		public Component QueryComponent ()
		{
			if ((Interfaces & Interfaces.Component) != 0)
				return new Component (this);
			return null;
		}

		public Document QueryDocument ()
		{
			if ((Interfaces & Interfaces.Document) != 0)
				return new Document (this);
			return null;
		}

		public EditableText QueryEditableText ()
		{
			if ((Interfaces & Interfaces.EditableText) != 0)
				return new EditableText (this);
			return null;
		}

		public Hyperlink QueryHyperlink ()
		{
			if ((Interfaces & Interfaces.Hyperlink) != 0)
				return new Hyperlink (this, path);
			return null;
		}

		public Hypertext QueryHypertext ()
		{
			if ((Interfaces & Interfaces.Hypertext) != 0)
				return new Hypertext (this);
			return null;
		}

		public Image QueryImage ()
		{
			if ((Interfaces & Interfaces.Image) != 0)
				return new Image (this);
			return null;
		}

		public Selection QuerySelection ()
		{
			if ((Interfaces & Interfaces.Selection) != 0)
				return new Selection (this);
			return null;
		}

		public Table QueryTable ()
		{
			if ((Interfaces & Interfaces.Table) != 0)
				return new Table (this);
			return null;
		}

		public Text QueryText ()
		{
			if ((Interfaces & Interfaces.Text) != 0)
				return new Text (this);
			return null;
		}

		public Value QueryValue ()
		{
			if ((Interfaces & Interfaces.Value) != 0)
				return new Value (this);
			return null;
		}

		public EventObject ObjectEvents {
			get {
				if (objectEvents == null && application != null)
					objectEvents = new EventObject (this);
				return objectEvents;
			}
		}

		public EventWindow WindowEvents {
			get {
				if (windowEvents == null &&
					application != null &&
					(Role == Role.Window ||
					 Role == Role.Frame ||
					 Role == Role.Dialog))
					windowEvents = new EventWindow (this);
				return windowEvents;
			}
		}

		public EventTerminal TerminalEvents {
			get {
				if (terminalEvents == null &&
					application != null &&
					Role == Role.Terminal)
					terminalEvents = new EventTerminal (this);
				return terminalEvents;
			}
		}

		public EventDocument DocumentEvents {
			get {
				if (documentEvents == null &&
					application != null)
					documentEvents = new EventDocument (this);
				return documentEvents;
			}
		}

		public EventFocus FocusEvents {
			get {
				if (focusEvents == null && application != null)
					focusEvents = new EventFocus (this);
				return focusEvents;
			}
		}
	}

	public delegate bool FindPredicate (Accessible a, params object [] args);

	[System.Flags]
	public enum Interfaces : uint
	{
		Action = 0x0001,
		Application = 0x0002,
		Component = 0x0004,
		Document = 0x0008,
		EditableText = 0x0010,
		Hyperlink = 0x0020,
		Hypertext = 0x0040,
		Image = 0x0080,
		Selection = 0x0100,
		StreamableContent = 0x0120,
		Table = 0x0400,
		Text = 0x0800,
		Value = 0x1000,
		Invalid = 0x80000000
	}

	[Interface ("org.a11y.atspi.Accessible")]
	interface IAccessible : Introspectable
	{
		IDictionary<string, string> GetAttributes ();
		DBusRelation [] GetRelationSet ();
		int GetIndexInParent ();
		AccessiblePath GetChildAtIndex (int index);
		AccessiblePath [] GetChildren ();
		string [] GetInterfaces ();
		uint GetRole ();
		uint [] GetState ();
	}
}
