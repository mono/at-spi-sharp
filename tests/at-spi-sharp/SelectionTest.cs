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
	public class SelectionTest : Base
	{
		Accessible frame = null;
			Accessible menuBar = null;
		Selection selection = null;

		public SelectionTest ()
		{
			frame = GetFrame ("gtkmenubar.py");

			menuBar = FindByRole (frame, Role.MenuBar);
			Assert.IsNotNull (menuBar, "Couldn't find the menu bar");
			selection = menuBar.QuerySelection ();
			Assert.IsNotNull (selection, "menuBar.QuerySelection");
		}

		#region Test
		
		[Test]
		public void Selection ()
		{
			Assert.IsNull (frame.QuerySelection (), "Frame should not support Selection");

			Assert.AreEqual (0, selection.NSelectedChildren, "NSelectedChildren initial");
			Assert.IsNull (selection.GetSelectedChild (0), "GetSelectedChild initial");
			Assert.IsTrue (selection.SelectChild (0), "SelectChild");
			Accessible a = selection.GetSelectedChild (0);
			Assert.AreEqual (menuBar.Children [0], a, "GetSelectedChild[0]");
			Assert.AreEqual ("File", a.Name, "GetSelectedChild[0].Name");
			Assert.IsNull (selection.GetSelectedChild (1), "GetSelectedChild (1)");
			Assert.IsTrue (selection.IsChildSelected (0), "IsChildSelected");
			Assert.IsTrue (selection.DeselectSelectedChild (0), "DeselectSelectedChild");
			Assert.IsFalse (selection.IsChildSelected (0), "IsChildSelected after dselect");
			Assert.IsFalse (selection.SelectAll (), "SelectAll");
			Assert.IsTrue (selection.ClearSelection(), "Clear Selection");
			Assert.AreEqual (0, selection.NSelectedChildren, "NSelectedChildren after SelectAll");
			Assert.IsFalse (selection.DeselectChild (0), "DeselectChild with nothing selected");
		}
#endregion
	}
}
