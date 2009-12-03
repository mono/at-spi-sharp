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
			frame.ObjectEvents.BoundsChanged += OnEvent;
			et.SetTextContents ("BoundsChanged");
			Sync ();
			AssertEvent ();
			frame.ObjectEvents.BoundsChanged -= OnEvent;
		}

		[Test]
		public void LinkSelected ()
		{
			frame.ObjectEvents.LinkSelected += OnEvent;
			et.SetTextContents ("LinkSelected");
			Sync ();
			AssertEvent ();
			frame.ObjectEvents.LinkSelected -= OnEvent;
		}

		[Test]
		public void VisibleDataChanged ()
		{
			frame.ObjectEvents.VisibleDataChanged += OnEvent;
			et.SetTextContents ("VisibleDataChanged");
			Sync ();
			AssertEvent ();
			frame.ObjectEvents.VisibleDataChanged -= OnEvent;
		}

		[Test]
		[Ignore ("TODO: SelectionChanged")]
		public void SelectionChanged ()
		{
			frame.ObjectEvents.SelectionChanged += OnEvent;
			et.SetTextContents ("SelectionChanged");
			Sync ();
			AssertEvent ();
			frame.ObjectEvents.SelectionChanged -= OnEvent;
		}

		[Test]
		public void ModelChanged ()
		{
			frame.ObjectEvents.ModelChanged += OnEvent;
			et.SetTextContents ("ModelChanged");
			Sync ();
			AssertEvent ();
			frame.ObjectEvents.ModelChanged -= OnEvent;
		}

		[Test]
		public void ActiveDescendantChanged ()
		{
			frame.ObjectEvents.ActiveDescendantChanged += OnEvent;
			et.SetTextContents ("ActiveDescendantChanged");
			Sync ();
			AssertEvent ();
			frame.ObjectEvents.ActiveDescendantChanged -= OnEvent;
		}

		[Test]
		public void RowInserted ()
		{
			frame.ObjectEvents.RowInserted += OnEvent;
			et.SetTextContents ("RowInserted");
			Sync ();
			AssertEvent ();
			frame.ObjectEvents.RowInserted -= OnEvent;
		}

		[Test]
		public void RowReordered ()
		{
			frame.ObjectEvents.RowReordered += OnEvent;
			et.SetTextContents ("RowReordered");
			Sync ();
			AssertEvent ();
			frame.ObjectEvents.RowReordered -= OnEvent;
		}

		[Test]
		public void RowDeleted ()
		{
			frame.ObjectEvents.RowDeleted += OnEvent;
			et.SetTextContents ("RowDeleted");
			Sync ();
			AssertEvent ();
			frame.ObjectEvents.RowDeleted -= OnEvent;
		}

		[Test]
		public void ColumnInserted ()
		{
			frame.ObjectEvents.ColumnInserted += OnEvent;
			et.SetTextContents ("ColumnInserted");
			Sync ();
			AssertEvent ();
			frame.ObjectEvents.ColumnInserted -= OnEvent;
		}

		[Test]
		public void ColumnReordered ()
		{
			frame.ObjectEvents.ColumnReordered += OnEvent;
			et.SetTextContents ("ColumnReordered");
			Sync ();
			AssertEvent ();
			frame.ObjectEvents.ColumnReordered -= OnEvent;
		}

		[Test]
		public void ColumnDeleted ()
		{
			frame.ObjectEvents.ColumnDeleted += OnEvent;
			et.SetTextContents ("ColumnDeleted");
			Sync ();
			AssertEvent ();
			frame.ObjectEvents.ColumnDeleted -= OnEvent;
		}

		[Test]
		public void TextSelectionChanged ()
		{
			frame.ObjectEvents.TextSelectionChanged += OnEvent;
			et.SetTextContents ("TextSelectionChanged");
			Sync ();
			AssertEvent ();
			frame.ObjectEvents.TextSelectionChanged -= OnEvent;
		}

		[Test]
		public void TextChanged ()
		{
			frame.ObjectEvents.TextChanged += OnEvent;
			et.SetTextContents ("TextChanged");
			Sync ();
			AssertEvent ();
			Assert.AreEqual (0, v1, "TextChanged v1");
			Assert.AreEqual (11, v2, "TextChanged v2");
			Assert.AreEqual ("insert", detail, "TextChanged detail");
			Assert.AreEqual ("TextChanged", any, "TextChanged any");
			frame.ObjectEvents.TextChanged -= OnEvent;
		}

		[Test]
		public void TextAttributesChanged ()
		{
			frame.ObjectEvents.TextAttributesChanged += OnEvent;
			et.SetTextContents ("TextAttributesChanged");
			Sync ();
			AssertEvent ();
			frame.ObjectEvents.TextAttributesChanged -= OnEvent;
		}

		[Test]
		public void TextCaretMoved ()
		{
			frame.ObjectEvents.TextCaretMoved += OnEvent;
			et.SetTextContents ("TextCaretMoved");
			Sync ();
			AssertEvent ();
			frame.ObjectEvents.TextCaretMoved -= OnEvent;
		}

		// Document event tests
		[Test]
		public void LoadComplete ()
		{
			frame.DocumentEvents.LoadComplete += OnEvent;
			et.SetTextContents ("LoadComplete");
			Sync ();
			AssertEvent ();
			frame.DocumentEvents.LoadComplete -= OnEvent;
		}

		[Test]
		public void Reload ()
		{
			frame.DocumentEvents.Reload += OnEvent;
			et.SetTextContents ("Reload");
			Sync ();
			AssertEvent ();
			frame.DocumentEvents.Reload -= OnEvent;
		}

		[Test]
		public void LoadStopped ()
		{
			frame.DocumentEvents.LoadStopped += OnEvent;
			et.SetTextContents ("LoadStopped");
			Sync ();
			AssertEvent ();
			frame.DocumentEvents.LoadStopped -= OnEvent;
		}

		[Test]
		public void Focus ()
		{
			frame.FocusEvents.Focus += OnEvent;
			et.SetTextContents ("Focus");
			Sync ();
			AssertEvent ();
			frame.FocusEvents.Focus -= OnEvent;
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
			Desktop.ChildAdded -= OnChildAdded;
			Sync ();
			Assert.AreEqual (1, addEventCount, "addEvents #2");
			Assert.AreEqual (0, removeEventCount, "removeEvents when adding #2");
			Desktop.ChildRemoved -= OnChildRemoved;
			Desktop.ChildAdded -= OnChildAdded;
		}

		private void OnEvent (string detail, int v1, int v2, object any)
		{
			eventCount++;
			this.detail = detail;
			this.v1 = v1;
			this.v2 = v2;
			this.any = any;
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

		private void AssertEvents (int n)
		{
			int oldEventCount = eventCount;
			eventCount = 0;
			Assert.AreEqual (n, oldEventCount, "eventCount");
			eventCount = 0;
		}
	}
}
