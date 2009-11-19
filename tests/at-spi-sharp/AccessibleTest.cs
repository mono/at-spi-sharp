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
	public class AccessibleTest : Base
	{
		Accessible frame = null;
		Accessible radioButton = null;

		public AccessibleTest ()
		{
			frame = GetFrame ("gtkradiobutton.py");
			radioButton = FindByRole (frame, Role.RadioButton, true);
			Assert.IsNotNull (radioButton, "Find radioButton");
		}

		#region Test
		[Test]
		public void States ()
		{
			States (radioButton,
				StateType.Armed,
				StateType.Checked,
				StateType.Enabled,
				StateType.Focusable,
				StateType.Sensitive,
				StateType.Showing,
				StateType.Visible
			);
		}

		[Test]
		public void Relations ()
		{
			Relation [] set = radioButton.RelationSet;
			Assert.AreEqual (1, set.Length, "RelationSet length");
			Assert.AreEqual (RelationType.MemberOf, set [0].Type, "RelationSet[0].Type");
			Assert.AreEqual (3, set[0].Targets.Length, "RelationSet[0].Targets.Length");
			Assert.AreEqual ("Cherry", set [0].Targets [0].Name, "RelationSet[0].Targets[0].Name");
			Assert.AreEqual ("Banana", set [0].Targets [1].Name, "RelationSet[0].Targets[1].Name");
			Assert.AreEqual ("Apple", set [0].Targets [2].Name, "RelationSet[0].Targets[2].Name");
		}

		[Test]
		public void Navigation ()
		{
			Assert.AreEqual (0, radioButton.Children.Count, "children");
			Accessible parent = radioButton.Parent;
			Assert.AreEqual (Role.Filler, parent.Role, "Parent Role");
			Assert.AreEqual (0, parent.Children [0].IndexInParent, "IndexInParent (0)");
			Assert.AreEqual (1, parent.Children [1].IndexInParent, "IndexInParent (1)");
		}

		[Test]
		public void ParentOfApplication ()
		{
			Accessible application = frame.Parent;
			Assert.AreEqual (Role.Application, application.Role, "Application Role");
			Accessible parent = application.Parent;
			Assert.IsNotNull (parent, "Parent of application should not be null");
			Assert.AreEqual (Role.DesktopFrame, parent.Role, "Parent Role");
		}
		#endregion
	}
}
