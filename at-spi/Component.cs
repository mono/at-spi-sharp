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
	public class Component
	{
		private IComponent proxy;

		public Component (Accessible accessible)
		{
			proxy = Registry.Bus.GetObject<IComponent> (accessible.application.name, new ObjectPath (accessible.path));
		}

		public bool Contains (int x, int y, CoordType coordType)
		{
			return proxy.contains (x, y, coordType);
		}

		public Accessible GetAccessibleAtPoint (int x, int y, CoordType coordType)
		{
			AccessiblePath path = proxy.GetAccessibleAtPoint (x, y, coordType);
			return Registry.GetElement (path, true);
		}

		public BoundingBox GetExtents (CoordType coordType)
		{
			return proxy.GetExtents (coordType);
		}

		public void GetPosition (CoordType coordType, out int x, out int y)
		{
			proxy.GetPosition (coordType, out x, out y);
		}

		public void GetSize (out int x, out int y)
		{
			proxy.GetSize (out x, out y);
		}

		public ComponentLayer Layer {
			get { return proxy.GetLayer (); }
		}

		public short MDIZOrder {
			get { return proxy.GetMDIZOrder (); }
		}

		public bool GrabFocus ()
		{
			return proxy.GrabFocus ();
		}

		// TODO: register/deregister Focus handler

		public double Alpha {
			get { return proxy.GetAlpha (); }
		}
	}

	[Interface ("org.a11y.atspi.Component")]
	interface IComponent : Introspectable
	{
		bool contains (int x, int y, CoordType coord_type);
		AccessiblePath GetAccessibleAtPoint (int x, int y, CoordType coord_type);
		BoundingBox GetExtents (CoordType coord_type);
		void GetPosition (CoordType coord_type, out int x, out int y);
		void GetSize (out int x, out int y);
		ComponentLayer GetLayer ();
		short GetMDIZOrder ();
		bool GrabFocus ();
		void registerFocusHandler (ObjectPath handler);
		void deregisterFocusHandler (ObjectPath handler);
		double GetAlpha ();
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
		Window
	}

	public enum CoordType : uint
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
