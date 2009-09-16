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
		private Accessible accessible;
		private ITable proxy;
		private Properties properties;

		private const string IFACE = "org.freedesktop.atspi.Table";

		public Table (Accessible accessible)
		{
			this.accessible = accessible;
			proxy = Registry.Bus.GetObject<ITable> (accessible.application.name, new ObjectPath (accessible.path));

			ObjectPath op = new ObjectPath (accessible.path);
			properties = Registry.Bus.GetObject<Properties> (accessible.application.name, op);
		}

		public int NRows {
			get {
				return (int) properties.Get (IFACE, "nRows");
			}
		}

		public int NColumns {
			get {
				return (int) properties.Get (IFACE, "nColumns");
			}
		}

		public Accessible Caption {
			get {
				ObjectPath path = (ObjectPath) properties.Get (IFACE, "caption");
				return accessible.application.GetElement (path, true);
			}
		}

		public Accessible Summary {
			get {
				ObjectPath path = (ObjectPath) properties.Get (IFACE, "summary");
				return accessible.application.GetElement (path, true);
			}
		}

		public int NSelectedRows {
			get {
				return (int) properties.Get (IFACE, "nSelectedRows");
			}
		}

		public int NSelectedColumns {
			get {
				return (int) properties.Get (IFACE, "nSelectedColumns");
			}
		}

		public Accessible GetAccessibleAt (int row, int column)
		{
			ObjectPath o = proxy.getAccessibleAt (row, column);
			return accessible.application.GetElement (o);
		}

		public int GetIndexAt (int row, int column)
		{
			return proxy.getIndexAt (row, column);
		}

		public int GetRowAtIndex (int index)
		{
			return proxy.getRowAtIndex (index);
		}

		public int GetColumnAtIndex (int index)
		{
			return proxy.getColumnAtIndex (index);
		}

		public string GetRowDescription (int row)
		{
			return proxy.getRowDescription (row);
		}

		public string GetColumnDescription (int column)
		{
			return proxy.getColumnDescription (column);
		}

		public int GetRowExtentAt (int row, int column)
		{
			return proxy.getRowExtentAt (row, column);
		}

		public int GetColumnExtentAt (int row, int column)
		{
			return proxy.getColumnExtentAt (row, column);
		}

		public Accessible GetRowHeader (int row)
		{
			ObjectPath o = proxy.getRowHeader (row);
			return accessible.application.GetElement (o);
		}

		public Accessible GetColumnHeader (int column)
		{
			ObjectPath o = proxy.getColumnHeader (column);
			return accessible.application.GetElement (o);
		}

		public int [] GetSelectedRows ()
		{
			return proxy.getSelectedRows ();
		}

		public int [] GetSelectedColumns ()
		{
			return proxy.getSelectedColumns ();
		}

		public bool IsRowSelected (int row)
		{
			return proxy.isRowSelected (row);
		}

		public bool IsColumnSelected (int column)
		{
			return proxy.isColumnSelected (column);
		}

		public bool IsSelected (int row, int column)
		{
			return proxy.isSelected (row, column);
		}

		public bool AddRowSelection (int row)
		{
			return proxy.addRowSelection (row);
		}

		public bool AddColumnSelection (int column)
		{
			return proxy.addColumnSelection (column);
		}

		public bool RemoveRowSelection (int row)
		{
			return proxy.removeRowSelection (row);
		}

		public bool RemoveColumnSelection (int column)
		{
			return proxy.removeColumnSelection (column);
		}

		public bool GetRowColumnExtentsAtIndex (int index, out int row, out int col, out int row_extents, out int col_extents, out bool is_selected)
		{
			bool ret;
			proxy.getRowColumnExtentsAtIndex (index, out ret, out row, out col, out row_extents, out col_extents, out is_selected);
			return ret;
		}

	}

	[Interface ("org.freedesktop.atspi.Table")]
	interface ITable : Introspectable
	{
		ObjectPath getAccessibleAt (int row, int column);
		int getIndexAt (int row, int column);
		int getRowAtIndex (int index);
		int getColumnAtIndex (int index);
		string getRowDescription (int row);
		string getColumnDescription (int column);
		int getRowExtentAt (int row, int column);
		int getColumnExtentAt (int row, int column);
		ObjectPath getRowHeader (int row);
		ObjectPath getColumnHeader (int column);
		int [] getSelectedRows ();
		int [] getSelectedColumns ();
		bool isRowSelected (int row);
		bool isColumnSelected (int column);
		bool isSelected (int row, int column);
		bool addRowSelection (int row);
		bool addColumnSelection (int column);
		bool removeRowSelection (int row);
		bool removeColumnSelection (int column);
		void getRowColumnExtentsAtIndex (int index, out bool ret, out int row, out int col, out int row_extents, out int col_extents, out bool is_selected);
	}
}
