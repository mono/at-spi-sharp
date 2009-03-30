// Copyright 2009 Novell, Inc.
// This software is made available under the MIT License
// See COPYING for details

using System;
using System.Collections.Generic;
using NDesk.DBus;
using org.freedesktop.DBus;

namespace org.freedesktop.atspi
{
	public class Component
	{
			Accessible accessible;
		IComponent proxy;

		public Component (Accessible accessible)
		{
			this.accessible = accessible;
			proxy =Registry.Bus.GetObject<IComponent> (accessible.application.name, new ObjectPath (accessible.path));
		}

		public bool Contains (int x, int y, CoordType coordType)
		{
			return proxy.contains (x, y, coordType);
		}

		public Accessible GetAccessibleAtPoint (int x, int y, CoordType coordType)
		{
			ObjectPath o = proxy.getAccessibleAtPoint (x, y, coordType);
			return accessible.application.GetElement (o);
		}

		public BoundingBox GetExtents (CoordType coordType)
		{
			return proxy.getExtents (coordType);
		}

		public void GetPosition (CoordType coordType, out int x, out int y)
		{
			proxy.getPosition (coordType, out x, out y);
		}

		public void GetSize (out int x, out int y)
		{
			proxy.getSize (out x, out y);
		}

		public ComponentLayer Layer {
			get { return proxy.getLayer (); }
		}

		public short MDIZOrder {
			get { return proxy.getMDIZOrder (); }
		}

		public bool GrabFocus ()
		{
			return proxy.grabFocus ();
		}

		// TODO: register/deregister focus handler

		public double Alpha {
			get { return proxy.getAlpha (); }
		}
	}

	[Interface ("org.freedesktop.atspi.Component")]
	interface IComponent : Introspectable
		{
			bool contains (int x, int y, CoordType coord_type);
			ObjectPath getAccessibleAtPoint (int x, int y, CoordType coord_type);
			BoundingBox getExtents (CoordType coord_type);
			void getPosition (CoordType coord_type, out int x, out int y);
			void getSize (out int x, out int y);
			ComponentLayer getLayer ();
			short getMDIZOrder ();
			bool grabFocus ();
			void registerFocusHandler (ObjectPath handler);
			void deregisterFocusHandler (ObjectPath handler);
			double getAlpha ();
	}

	public enum ComponentLayer: uint
	{
		Invalid,
		Background,
		Canvas,
		Widget,
		MDI,
		PopUp,
		Overlay,
		Window,
		LastDefined
	}

	public enum CoordType : short
	{
		Screen,
		Window
	}

	public struct BoundingBox
	{
		public int X;
		public int Y;
		public int Width;
		public int Height;
	}
}
