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
using NUnit.Framework;
using Atspi;

namespace AtSpiTest
{
	[TestFixture]
	public class EventTest : Base
	{
		Accessible frame = null;
		EditableText et = null;
		int eventCount = 0;
		int addEventCount = 0;
		int removeEventCount = 0;
		string detail;
		int v1, v2;
		object any;
		string oldName, newName;
		string oldDescription, newDescription;
		Role oldRole, newRole;

		[TestFixtureSetUp]
		public void Init ()
		{
			frame = GetFrame ("DocumentTest.exe", "DocumentTest");
			et = frame.QueryEditableText ();
			Assert.IsNotNull (et, "frame.QueryEditableText");
		}

		#region Test

		// Object event tests
		[Test]
		public void BoundsChanged ()
		{
			frame.ObjectEvents.BoundsChanged += OnEventR;
			et.SetTextContents ("BoundsChanged");
			Sync ();
			AssertEvent ();
			frame.ObjectEvents.BoundsChanged -= OnEventR;
			et.SetTextContents ("BoundsChanged");
			Sync ();
			AssertNoEvent ();
		}

		[Test]
		public void LinkSelected ()
		{
			frame.ObjectEvents.LinkSelected += OnEventI;
			et.SetTextContents ("LinkSelected");
			Sync ();
			AssertEvent ();
			frame.ObjectEvents.LinkSelected -= OnEventI;
			et.SetTextContents ("LinkSelected");
			Sync ();
			AssertNoEvent ();
		}

		[Test]
		public void VisibleDataChanged ()
		{
			frame.ObjectEvents.VisibleDataChanged += OnEventSimple;
			et.SetTextContents ("VisibleDataChanged");
			Sync ();
			AssertEvent ();
			frame.ObjectEvents.VisibleDataChanged -= OnEventSimple;
			et.SetTextContents ("VisibleDataChanged");
			Sync ();
			AssertNoEvent ();
		}

		[Test]
		[Ignore ("TODO: SelectionChanged")]
		public void SelectionChanged ()
		{
			frame.ObjectEvents.SelectionChanged += OnEventSimple;
			et.SetTextContents ("SelectionChanged");
			Sync ();
			AssertEvent ();
			frame.ObjectEvents.SelectionChanged -= OnEventSimple;
			et.SetTextContents ("SelectionChanged");
			Sync ();
			AssertNoEvent ();
		}

		[Test]
		public void ModelChanged ()
		{
			frame.ObjectEvents.ModelChanged += OnEventSimple;
			et.SetTextContents ("ModelChanged");
			Sync ();
			AssertEvent ();
			frame.ObjectEvents.ModelChanged -= OnEventSimple;
			et.SetTextContents ("ModelChanged");
			Sync ();
			AssertNoEvent ();
		}

		[Test]
		public void ActiveDescendantChanged ()
		{
			frame.ObjectEvents.ActiveDescendantChanged += OnEventO;
			et.SetTextContents ("ActiveDescendantChanged");
			Sync ();
			AssertEvent ();
			frame.ObjectEvents.ActiveDescendantChanged -= OnEventO;
			et.SetTextContents ("ActiveDescendantChanged");
			Sync ();
			AssertNoEvent ();
		}

		[Test]
		public void RowInserted ()
		{
			frame.ObjectEvents.RowInserted += OnEventII;
			et.SetTextContents ("RowInserted");
			Sync ();
			AssertEvent ();
			frame.ObjectEvents.RowInserted -= OnEventII;
			et.SetTextContents ("RowInserted");
			Sync ();
			AssertNoEvent ();
		}

		[Test]
		public void RowReordered ()
		{
			frame.ObjectEvents.RowReordered += OnEventSimple;
			et.SetTextContents ("RowReordered");
			Sync ();
			AssertEvent ();
			frame.ObjectEvents.RowReordered -= OnEventSimple;
			et.SetTextContents ("RowReordered");
			Sync ();
			AssertNoEvent ();
		}

		[Test]
		public void RowDeleted ()
		{
			frame.ObjectEvents.RowDeleted += OnEventII;
			et.SetTextContents ("RowDeleted");
			Sync ();
			AssertEvent ();
			frame.ObjectEvents.RowDeleted -= OnEventII;
			et.SetTextContents ("RowDeleted");
			Sync ();
			AssertNoEvent ();
		}

		[Test]
		public void ColumnInserted ()
		{
			frame.ObjectEvents.ColumnInserted += OnEventII;
			et.SetTextContents ("ColumnInserted");
			Sync ();
			AssertEvent ();
			frame.ObjectEvents.ColumnInserted -= OnEventII;
			et.SetTextContents ("ColumnInserted");
			Sync ();
			AssertNoEvent ();
		}

		[Test]
		public void ColumnReordered ()
		{
			frame.ObjectEvents.ColumnReordered += OnEventSimple;
			et.SetTextContents ("ColumnReordered");
			Sync ();
			AssertEvent ();
			frame.ObjectEvents.ColumnReordered -= OnEventSimple;
			et.SetTextContents ("ColumnReordered");
			Sync ();
			AssertNoEvent ();
		}

		[Test]
		public void ColumnDeleted ()
		{
			frame.ObjectEvents.ColumnDeleted += OnEventII;
			et.SetTextContents ("ColumnDeleted");
			Sync ();
			AssertEvent ();
			frame.ObjectEvents.ColumnDeleted -= OnEventII;
			et.SetTextContents ("ColumnDeleted");
			Sync ();
			AssertNoEvent ();
		}

		[Test]
		public void TextSelectionChanged ()
		{
			frame.ObjectEvents.TextSelectionChanged += OnEventSimple;
			et.SetTextContents ("TextSelectionChanged");
			Sync ();
			AssertEvent ();
			frame.ObjectEvents.TextSelectionChanged -= OnEventSimple;
			et.SetTextContents ("TextSelectionChanged");
			Sync ();
			AssertNoEvent ();
		}

		[Test]
		public void TextChanged ()
		{
			frame.ObjectEvents.TextChanged += OnEventSIIS;
			et.SetTextContents ("TextChanged");
			Sync ();
			AssertEvent ();
			Assert.AreEqual (0, v1, "TextChanged v1");
			Assert.AreEqual (11, v2, "TextChanged v2");
			Assert.AreEqual ("insert", detail, "TextChanged detail");
			Assert.AreEqual ("TextChanged", any, "TextChanged any");
			frame.ObjectEvents.TextChanged -= OnEventSIIS;
			et.SetTextContents ("TextChanged");
			Sync ();
			AssertNoEvent ();
		}

		[Test]
		public void TextAttributesChanged ()
		{
			frame.ObjectEvents.TextAttributesChanged += OnEventSimple;
			et.SetTextContents ("TextAttributesChanged");
			Sync ();
			AssertEvent ();
			frame.ObjectEvents.TextAttributesChanged -= OnEventSimple;
			et.SetTextContents ("TextAttributesChanged");
			Sync ();
			AssertNoEvent ();
		}

		[Test]
		public void TextCaretMoved ()
		{
			frame.ObjectEvents.TextCaretMoved += OnEventI;
			et.SetTextContents ("TextCaretMoved");
			Sync ();
			AssertEvent ();
			frame.ObjectEvents.TextCaretMoved -= OnEventI;
			et.SetTextContents ("TextCaretMoved");
			Sync ();
			AssertNoEvent ();
		}

		// Document event tests
		[Test]
		public void LoadComplete ()
		{
			frame.DocumentEvents.LoadComplete += OnEventSimple;
			et.SetTextContents ("LoadComplete");
			Sync ();
			AssertEvent ();
			frame.DocumentEvents.LoadComplete -= OnEventSimple;
			et.SetTextContents ("LoadComplete");
			Sync ();
			AssertNoEvent ();
		}

		[Test]
		public void Reload ()
		{
			frame.DocumentEvents.Reload += OnEventSimple;
			et.SetTextContents ("Reload");
			Sync ();
			AssertEvent ();
			frame.DocumentEvents.Reload -= OnEventSimple;
			et.SetTextContents ("Reload");
			Sync ();
			AssertNoEvent ();
		}

		[Test]
		public void LoadStopped ()
		{
			frame.DocumentEvents.LoadStopped += OnEventSimple;
			et.SetTextContents ("LoadStopped");
			Sync ();
			AssertEvent ();
			frame.DocumentEvents.LoadStopped -= OnEventSimple;
			et.SetTextContents ("LoadStopped");
			Sync ();
			AssertNoEvent ();
		}

		[Test]
		public void Focus ()
		{
			frame.FocusEvents.Focus += OnEventSimple;
			et.SetTextContents ("Focus");
			Sync ();
			AssertEvent ();
			frame.FocusEvents.Focus -= OnEventSimple;
			et.SetTextContents ("Focus");
			Sync ();
			AssertNoEvent ();
		}

		[Test]
		public void NameChanged ()
		{
			Desktop.NameChanged += OnNameChanged;
			et.SetTextContents ("NameChanged");
			Sync ();
			Assert.AreEqual (string.Empty, oldName, "OldName");
			Assert.AreEqual ("xyzzy", newName, "NameChanged");
			Desktop.NameChanged -= OnNameChanged;
		}

		[Test]
		public void DescriptionChanged ()
		{
			Desktop.DescriptionChanged += OnDescriptionChanged;
			et.SetTextContents ("DescriptionChanged");
			Sync ();
			Assert.AreEqual (string.Empty, oldDescription, "OldDescription");
			Assert.AreEqual ("plugh", newDescription, "DescriptionChanged");
			Desktop.DescriptionChanged -= OnDescriptionChanged;
		}

		[Test]
		public void RoleChanged ()
		{
			Desktop.RoleChanged += OnRoleChanged;
			et.SetTextContents ("RoleChanged");
			Sync ();
			Assert.AreEqual (Role.Frame, oldRole, "RoleChanged OldRole");
			Assert.AreEqual (Role.Dialog, newRole, "RoleChanged");
			Desktop.RoleChanged -= OnRoleChanged;
		}

		[Test]
		public void StateChanged ()
		{
			eventCount = 0;
			Desktop.StateChanged += OnStateChanged;
			et.SetTextContents ("StateChanged");
			Sync ();
			Desktop.StateChanged -= OnStateChanged;
			AssertEvents (2);
		}

		[Test]
		public void ChildRemoved ()
		{
			addEventCount = removeEventCount = 0;
			Desktop.ChildAdded += OnChildAdded;
			Desktop.ChildRemoved += OnChildRemoved;
			et.SetTextContents ("RemoveChild");
			Sync ();
			Assert.AreEqual (0, addEventCount, "addEvents when removing");
			Assert.AreEqual (1, removeEventCount, "removeEvents");
			Desktop.ChildRemoved -= OnChildRemoved;
			Desktop.ChildAdded -= OnChildAdded;
			et.SetTextContents ("AddChild");
		}

		[Test]
		public void ChildAdded ()
		{
			et.SetTextContents ("RemoveChild");
			Sync ();
			addEventCount = removeEventCount = 0;
			Desktop.ChildAdded += OnChildAdded;
			Desktop.ChildRemoved += OnChildRemoved;
			et.SetTextContents ("AddChild");
			Sync ();
			Assert.AreEqual (1, addEventCount, "addEvents");
			Assert.AreEqual (0, removeEventCount, "removeEvents when adding");
			// Add a second child; ensure we don't get extra events
			addEventCount = removeEventCount = 0;
			et.SetTextContents ("AddChild");
			Sync ();
			Desktop.ChildAdded -= OnChildAdded;
			Assert.AreEqual (1, addEventCount, "addEvents #2");
			Assert.AreEqual (0, removeEventCount, "removeEvents when adding #2");
			Desktop.ChildRemoved -= OnChildRemoved;
		}

		private void OnEventI (Accessible sender, int v1)
		{
			eventCount++;
			this.v1 = v1;
		}

		private void OnEventII (Accessible sender, int v1, int v2)
		{
			eventCount++;
			this.v1 = v1;
			this.v2 = v2;
		}

		private void OnEventO (Accessible sender, Accessible obj)
		{
			eventCount++;
			this.any = obj;
		}

		private void OnEventR (Accessible sender, BoundingBox rect)
		{
			eventCount++;
			this.any = rect;
		}

		/*
		private void OnEventSB (Accessible sender, string detail, bool val)
		{
			eventCount++;
			this.detail = detail;
			this.v1 = (val? 1: 0);
		}
		*/

		/*
		private void OnEventSII (Accessible sender, string detail, int v1, int v2)
		{
			eventCount++;
			this.detail = detail;
			this.v1 = v1;
			this.v2 = v2;
		}
		*/

		private void OnEventSIIS (Accessible sender, string detail, int v1, int v2, string any)
		{
			eventCount++;
			this.detail = detail;
			this.v1 = v1;
			this.v2 = v2;
			this.any = any;
		}

		/*
		private void OnEventSIO (Accessible sender, string detail, int v1, Accessible obj)
		{
			eventCount++;
			this.detail = detail;
			this.v1 = v1;
			this.any = obj;
		}

		private void OnEventSV (Accessible sender, string detail, object any)
		{
			eventCount++;
			this.detail = detail;
			this.any = any;
		}
		*/

		private void OnEventSimple (Accessible sender)
		{
			eventCount++;
		}

		private void OnNameChanged (object sender, string oldName, string newName)
		{
			this.oldName = oldName;
			this.newName = newName;
		}

		private void OnDescriptionChanged (object sender, string oldDescription, string newDescription)
		{
			this.oldDescription = oldDescription;
			this.newDescription = newDescription;
		}

		private void OnRoleChanged (object sender, Role oldRole, Role newRole)
		{
			this.oldRole = oldRole;
			this.newRole = newRole;
		}

		private void OnStateChanged (Accessible sender, StateType state, bool set)
		{
			if (sender.Role == Role.PushButton &&
				state == StateType.Enabled &&
				set == false)
				eventCount++;
			else if (sender.Role == Role.PushButton &&
				state == StateType.Sensitive &&
				set == false)
				eventCount++;
		}

		private void OnChildAdded (Accessible parent, Accessible child)
		{
			addEventCount++;
		}

		private void OnChildRemoved (Accessible parent, Accessible child)
		{
			removeEventCount++;
		}
		#endregion

		private void Sync ()
		{
			System.Threading.Thread.Sleep (500);
		}

		private void AssertEvent ()
		{
			AssertEvents (1);
		}

		private void AssertNoEvent ()
		{
			AssertEvents (0);
		}

		private void AssertEvents (int n)
		{
			int oldEventCount = eventCount;
			eventCount = 0;
			Assert.AreEqual (n, oldEventCount, "eventCount");
		}
	}
}
