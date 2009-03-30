// Copyright 2009 Novell, Inc.
// This software is made available under the MIT License
// See COPYING for details

using System;
using System.Collections.Generic;
using NDesk.DBus;
using org.freedesktop.DBus;

namespace org.freedesktop.atspi
{
	public class Accessible
	{
			internal string path;
		//IAccessible proxy;
		internal Application application;
		internal List<Accessible> children;
		internal Accessible parent;
		internal string name;
		internal string description;
		internal Role role;
		internal StateSet stateSet;
		bool supportsAction;
		bool supportsComponent;
		bool supportsEditableText;
		bool supportsHypertext;
		bool supportsImage;
		bool supportsSelection;
		bool supportsStreamableContent;
		bool supportsTable;
		bool supportsText;
		bool supportsValue;

		internal Accessible (Application application, string path)
		{
			this.application = application;
			this.path = path;
			this.children = new List<Accessible> ();
			//proxy = Registry.Bus.GetObject<AccessibleInterface> (application.name, new ObjectPath (path));
		}

		internal void Dispose ()
		{
			// TODO: Fire event
			if (parent != null)
				parent.children.Remove (this);
			children.Clear ();
			stateSet.AddState (StateType.Defunct);
		}

		internal void AddInterface (string name)
		{
			SetInterface (name, true);
		}

		void SetInterface (string name, bool val)
		{
			if (name == "org.freedesktop.atspi.Accessible") {
				// All objects should support this
			} else if (name == "org.freedesktop.atspi.Action") {
				supportsAction = val;
			} else if (name == "org.freedesktop.atspi.Component") {
				supportsComponent = val;
			} else if (name == "org.freedesktop.atspi.EditableText") {
				supportsEditableText = val;
			} else if (name == "org.freedesktop.atspi.Hypertext") {
				supportsHypertext = val;
			} else if (name == "org.freedesktop.atspi.Image") {
				supportsImage = val;
			} else if (name == "org.freedesktop.atspi.Selection") {
				supportsSelection = val;
			} else if (name == "org.freedesktop.atspi.StreamableContent") {
				supportsStreamableContent = val;
			} else if (name == "org.freedesktop.atspi.Table") {
				supportsTable = val;
			} else if (name == "org.freedesktop.atspi.Text") {
				supportsText = val;
			} else if (name == "org.freedesktop.atspi.Value") {
				supportsValue = val;
			} else
				// TODO: Don't release with this exception
				throw new ArgumentException ("Invalid interface name \"" + name + "\"");
		}

		public Accessible Parent { get { return Parent; } }
		public List<Accessible> Children { get { return children; } }
		public Role Role { get { return role; } }
		public StateSet StateSet { get { return stateSet; } }
		public string Name { get { return name; } }
		public string Description { get { return description; } }

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

		public Component QueryComponent ()
		{
			if (!supportsComponent)
				return null;
			return new Component (this);
		}
	}

	public delegate bool FindPredicate (Accessible a, params object [] args);

}
