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
//	Mike Gorse <mgorse@novell.com>
// 

using System;
using NDesk.DBus;
using NUnit.Framework;
using Atspi;

namespace AtSpiTest
{
	[TestFixture]
	public class TableTest : Base
	{
		Accessible frame = null;
			Accessible treeTable = null;
		Table table = null;

		public TableTest ()
		{
			frame = GetFrame ("gtktreeview.py");

			treeTable = FindByRole (frame, Role.TreeTable, true);
			Assert.IsNotNull (treeTable, "Couldn't find the tree table");
			table = treeTable.QueryTable ();
			Assert.IsNotNull (table, "menuBar.QueryTable");
		}

		#region Test

		[Test]
		public void NRows ()
		{
			Assert.AreEqual (4, table.NRows, "Table NRows");
		}

		[Test]
		public void NColumns ()
		{
			Assert.AreEqual (1, table.NColumns, "Table NColumns");
		}

		[Test]
		public void Caption ()
		{
			Assert.IsNull (table.Caption, "Caption");
		}

		[Test]
		public void Summary ()
		{
			Assert.IsNull (table.Summary, "Summary");
		}

		[Test]
		public void GetAccessibleAt ()
		{
			Accessible accessible = table.GetAccessibleAt (0, 0);
			Assert.AreEqual ("parent 0", accessible.Name, "GetAccessibleAt (0, 0).Name");
			accessible = table.GetAccessibleAt (1, 0);
			Assert.AreEqual ("parent 1", accessible.Name, "GetAccessibleAt (0, 0).Name");
			accessible = table.GetAccessibleAt (1, 1);
			Assert.IsNull (accessible, "GetAccessibleAt (1, 1)");
		}

		[Test]
		public void GetIndexAt ()
		{
			Assert.AreEqual (2, table.GetIndexAt (1, 0), "GetIndexAt");
		}

		[Test]
		public void GetRowAtIndex ()
		{
			Assert.AreEqual (1, table.GetRowAtIndex (2), "GetRowAtIndex");
		}

		[Test]
		public void GetColumnAtIndex ()
		{
			Assert.AreEqual (0, table.GetColumnAtIndex (2), "GetColumnAtIndex");
		}

		[Test]
		public void GetRowDescription ()
		{
			Assert.AreEqual (string.Empty, table.GetRowDescription (0), "GetRowDescription");
		}

		[Test]
		public void GetColumnDescription ()
		{
			Assert.AreEqual ("Column 0", table.GetColumnDescription (0), "GetColumnDescription");
		}

		[Test]
		public void GetRowExtentAt ()
		{
			// Why are these 0?
			Assert.AreEqual (0, table.GetRowExtentAt (1, 0), "GetRowExtentAt");
		}

		[Test]
		public void GetColumnExtentAt ()
		{
			Assert.AreEqual (0, table.GetColumnExtentAt (1, 0), "GetColumnExtentAt");
		}

		[Test]
		public void GetRowHeader ()
		{
			Assert.IsNull (table.GetRowHeader (0), "GetRowHeader");
		}

		[Test]
		public void GetColumnHeader ()
		{
			Accessible accessible = table.GetColumnHeader (0);
			Assert.IsNotNull (accessible, "GetColumnHeader (0)");
			Assert.AreEqual (Role.TableColumnHeader, accessible.Role, "GetColumnHeader (0).Role");
			Assert.IsNull (table.GetColumnHeader (1), "GetColumnHeader (1)");
		}

		[Test]
		public void GetSelectedRows ()
		{
			int [] selectedRows = table.GetSelectedRows ();
			Assert.AreEqual (0, selectedRows.Length, "GetSelectedRows");
		}

		[Test]
		public void GetSelectedColumns ()
		{
			int [] selectedColumns = table.GetSelectedColumns ();
			Assert.AreEqual (0, selectedColumns.Length, "GetSelectedColumns");
		}

		[Test]
		public void IsRowSelected ()
		{
			Assert.IsFalse (table.IsRowSelected (0), "IsRowSelected");
		}

		[Test]
		public void IsColumnSelected ()
		{
			Assert.IsFalse (table.IsColumnSelected (0), "IsColumnSelected");
		}

		[Test]
		public void IsSelected ()
		{
			Assert.IsFalse (table.IsSelected (0, 0), "IsSelected");
		}

		[Test]
		public void RowSelection ()
		{
			Assert.IsTrue (table.AddRowSelection (0), "AddRowSelection");
			Assert.IsTrue (table.RemoveRowSelection (0), "RemoveRowSelection");
		}

		[Test]
		public void ColumnSelection ()
		{
			Assert.IsFalse (table.AddColumnSelection (0), "AddColumnSelection");
			Assert.IsFalse (table.RemoveColumnSelection (0), "RemoveColumnSelection");
		}

		[Test]
		public void GetRowColumnExtentsAtIndex ()
		{
			int row, col, row_extents, col_extents;
			bool is_selected;
			Assert.IsFalse (table.GetRowColumnExtentsAtIndex (0, out row, out col, out row_extents, out col_extents, out is_selected), "GetRowColumnExtentsAtIndex");
		}
#endregion
	}
}
