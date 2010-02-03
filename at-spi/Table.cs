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
	public class Table
	{
		private ITable proxy;
		private Properties properties;

		private const string IFACE = "org.freedesktop.atspi.Table";

		public Table (Accessible accessible)
		{
			proxy = Registry.Bus.GetObject<ITable> (accessible.application.name, new ObjectPath (accessible.path));

			ObjectPath op = new ObjectPath (accessible.path);
			properties = Registry.Bus.GetObject<Properties> (accessible.application.name, op);
		}

		public int NRows {
			get {
				return (int) properties.Get (IFACE, "NRows");
			}
		}

		public int NColumns {
			get {
				return (int) properties.Get (IFACE, "NColumns");
			}
		}

		public Accessible Caption {
			get {
				object o = properties.Get (IFACE, "Caption");
				AccessiblePath path = (AccessiblePath) Convert.ChangeType (o, typeof (AccessiblePath));
				return Registry.GetElement (path, true);
			}
		}

		public Accessible Summary {
			get {
				object o = properties.Get (IFACE, "Summary");
				AccessiblePath path = (AccessiblePath) Convert.ChangeType (o, typeof (AccessiblePath));
				return Registry.GetElement (path, true);
			}
		}

		public int NSelectedRows {
			get {
				return (int) properties.Get (IFACE, "NSelectedRows");
			}
		}

		public int NSelectedColumns {
			get {
				return (int) properties.Get (IFACE, "NSelectedColumns");
			}
		}

		public Accessible GetAccessibleAt (int row, int column)
		{
			AccessiblePath path = proxy.GetAccessibleAt (row, column);
			return Registry.GetElement (path, true);
		}

		public int GetIndexAt (int row, int column)
		{
			return proxy.GetIndexAt (row, column);
		}

		public int GetRowAtIndex (int index)
		{
			return proxy.GetRowAtIndex (index);
		}

		public int GetColumnAtIndex (int index)
		{
			return proxy.GetColumnAtIndex (index);
		}

		public string GetRowDescription (int row)
		{
			return proxy.GetRowDescription (row);
		}

		public string GetColumnDescription (int column)
		{
			return proxy.GetColumnDescription (column);
		}

		public int GetRowExtentAt (int row, int column)
		{
			return proxy.GetRowExtentAt (row, column);
		}

		public int GetColumnExtentAt (int row, int column)
		{
			return proxy.GetColumnExtentAt (row, column);
		}

		public Accessible GetRowHeader (int row)
		{
			AccessiblePath path = proxy.GetRowHeader (row);
			return Registry.GetElement (path, true);
		}

		public Accessible GetColumnHeader (int column)
		{
			AccessiblePath path = proxy.GetColumnHeader (column);
			return Registry.GetElement (path, true);
		}

		public int [] GetSelectedRows ()
		{
			return proxy.GetSelectedRows ();
		}

		public int [] GetSelectedColumns ()
		{
			return proxy.GetSelectedColumns ();
		}

		public bool IsRowSelected (int row)
		{
			return proxy.IsRowSelected (row);
		}

		public bool IsColumnSelected (int column)
		{
			return proxy.IsColumnSelected (column);
		}

		public bool IsSelected (int row, int column)
		{
			return proxy.IsSelected (row, column);
		}

		public bool AddRowSelection (int row)
		{
			return proxy.AddRowSelection (row);
		}

		public bool AddColumnSelection (int column)
		{
			return proxy.AddColumnSelection (column);
		}

		public bool RemoveRowSelection (int row)
		{
			return proxy.RemoveRowSelection (row);
		}

		public bool RemoveColumnSelection (int column)
		{
			return proxy.RemoveColumnSelection (column);
		}

		public bool GetRowColumnExtentsAtIndex (int index, out int row, out int col, out int row_extents, out int col_extents, out bool is_selected)
		{
			bool ret;
			proxy.GetRowColumnExtentsAtIndex (index, out ret, out row, out col, out row_extents, out col_extents, out is_selected);
			return ret;
		}

	}

	[Interface ("org.freedesktop.atspi.Table")]
	interface ITable : Introspectable
	{
		AccessiblePath GetAccessibleAt (int row, int column);
		int GetIndexAt (int row, int column);
		int GetRowAtIndex (int index);
		int GetColumnAtIndex (int index);
		string GetRowDescription (int row);
		string GetColumnDescription (int column);
		int GetRowExtentAt (int row, int column);
		int GetColumnExtentAt (int row, int column);
		AccessiblePath GetRowHeader (int row);
		AccessiblePath GetColumnHeader (int column);
		int [] GetSelectedRows ();
		int [] GetSelectedColumns ();
		bool IsRowSelected (int row);
		bool IsColumnSelected (int column);
		bool IsSelected (int row, int column);
		bool AddRowSelection (int row);
		bool AddColumnSelection (int column);
		bool RemoveRowSelection (int row);
		bool RemoveColumnSelection (int column);
		void GetRowColumnExtentsAtIndex (int index, out bool ret, out int row, out int col, out int row_extents, out int col_extents, out bool is_selected);
	}
}
