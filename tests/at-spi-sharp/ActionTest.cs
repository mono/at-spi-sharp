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
	public class ActionTest : Base
	{
		Accessible frame = null;
		Action action = null;

		public ActionTest ()
		{
			frame = GetFrame ("gtkbutton.py");

			Accessible button = FindByRole (frame, Role.PushButton);
			Assert.IsNotNull (button, "Couldn't find the PushButton");
			action = button.QueryAction ();
			Assert.IsNotNull (action, "button.QueryAction");
		}

		#region Test
		
		[Test]
		public void Actions ()
		{
			Assert.IsNull (frame.QueryAction (), "Frame should not support Action");

			Assert.AreEqual (3, action.NActions, "NActions");
			Assert.AreEqual ("click", action.GetName (0), "GetName (0)");
			Assert.AreEqual ("press", action.GetName (1), "GetName (1)");
			Assert.AreEqual ("release", action.GetName (2), "GetName (2)");
			Assert.AreEqual (String.Empty, action.GetName (3), "GetName (3)");

			Assert.AreEqual (String.Empty, action.GetDescription (0), "GetDescription (0)");

			Assert.AreEqual (String.Empty, action.GetKeyBinding (0), "GetKeyBinding (0)");
		}

		[Test]
		public void ActionsProperty ()
		{
			ActionDescription [] actions = action.Actions;
			Assert.AreEqual (3, actions.Length, "GetActions Length");
			Assert.AreEqual ("click", actions [0].Name, "Name (0)");
			Assert.AreEqual (String.Empty, actions [0].Description, "Description (0)");
			Assert.AreEqual (String.Empty, actions [0].KeyBinding, "keyBinding (0)");

			Assert.AreEqual ("press", actions [1].Name, "Name (1)");
			Assert.AreEqual (String.Empty, actions [1].Description, "Description (1)");
			Assert.AreEqual (String.Empty, actions [1].KeyBinding, "keyBinding (1)");
		}

		[Test]
		public void DoAction ()
		{
			Assert.IsTrue (action.DoAction (0), "DoAction");
			// TODO: Test events?
		}
#endregion
	}
}
