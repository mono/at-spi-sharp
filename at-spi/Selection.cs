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
	public class Selection
	{
		private Accessible accessible;
		private ISelection proxy;
		private Properties properties;

		private const string IFACE = "org.freedesktop.atspi.Selection";

		public Selection (Accessible accessible)
		{
			this.accessible = accessible;
			ObjectPath op = new ObjectPath (accessible.path);
			proxy = Registry.Bus.GetObject<ISelection> (accessible.application.name, op);
			properties = Registry.Bus.GetObject<Properties> (accessible.application.name, op);
		}

		public int NSelectedChildren {
			get { return (int) properties.Get (IFACE, "nSelectedChildren"); }
		}

		public Accessible GetSelectedChild (int selectedChildIndex)
		{
			ObjectPath o = proxy.getSelectedChild (selectedChildIndex);
			return accessible.application.GetElement (o);
		}

		public bool SelectChild (int childIndex)
		{
			return proxy.selectChild (childIndex);
		}

		public bool DeselectSelectedChild (int selectedChildIndex)
		{
			return proxy.deselectSelectedChild (selectedChildIndex);
		}

		public bool IsChildSelected (int childIndex)
		{
			return proxy.isChildSelected (childIndex);
		}

		public bool SelectAll ()
		{
			return proxy.selectAll ();
		}

		public bool ClearSelection ()
		{
			return proxy.clearSelection ();
		}

		public bool DeselectChild (int childIndex)
		{
			return proxy.deselectChild (childIndex);
		}
	}

	[Interface ("org.freedesktop.atspi.Selection")]
	interface ISelection : Introspectable
	{
		ObjectPath getSelectedChild (int selectedChildIndex);
		bool selectChild (int childIndex);
		bool deselectSelectedChild (int selectedChildIndex);
		bool isChildSelected (int childIndex);
		bool selectAll ();
		bool clearSelection ();
		bool deselectChild (int childIndex);
	}
}
