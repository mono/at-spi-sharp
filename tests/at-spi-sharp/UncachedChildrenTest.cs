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
	public class UncachedChildrenTest : Base
	{
		Accessible frame = null;
		Accessible treeTable = null;

		[TestFixtureSetUp]
		public void Init ()
		{
			frame = GetFrame ("gtktreeview.py");

			treeTable = FindByRole (frame, Role.TreeTable, true);
			Assert.IsNotNull (treeTable, "Couldn't find the tree table");
		}

		#region Test

		[Test]
		public void NavigationTest ()
		{
			Assert.AreEqual (17, treeTable.Children.Count, "treeTable child count");
			Accessible a1 = treeTable.Children [0];
			Accessible a2 = treeTable.Children [1];
			Assert.AreNotEqual (a1, a2, "Children [0] != Children [1]");
			Assert.AreEqual (Role.TableColumnHeader, a1.Role, "Children [0].Role");
			Assert.AreEqual (Role.TableCell, a2.Role, "Children [1].Role");
		}

		[Test]
		public void ParentTest ()
		{
			Assert.IsNotNull (treeTable.Children [0], "TreeTable should have a child");
			Assert.AreEqual (treeTable, treeTable.Children [0].Parent, "treeTable Children[0].Parent");
		}
#endregion
	}
}
