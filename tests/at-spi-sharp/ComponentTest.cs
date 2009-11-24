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
	public class ComponentTest : Base
	{
		Accessible frame = null;
		Accessible pushButton = null;

		public ComponentTest ()
		{
			frame = GetFrame ("gtkbutton.py");
			pushButton = FindByRole (frame, Role.PushButton, true);
			Assert.IsNotNull (pushButton, "Find pushButton");
		}

		#region Test
		
		[Test]
		public void GetExtents ()
		{
			Component component = frame.QueryComponent ();
			BoundingBox windowExtents, screenExtents;
			windowExtents = component.GetExtents (CoordType.Window);
			Assert.IsTrue (windowExtents.Width > 0, "width > 0");
			Assert.IsTrue (windowExtents.Height > 0, "width > 0");
			screenExtents = component.GetExtents (CoordType.Screen);
			Assert.AreEqual (windowExtents.Width, screenExtents.Width, "GetExtents: CoordType shouldn't affect width");
			Assert.AreEqual (windowExtents.Height, screenExtents.Height, "GetExtents: CoordType shouldn't affect height");
			Assert.IsTrue (screenExtents.X > windowExtents.X, "GetExtents: X should be greater with CoordType.Screen");
			Assert.IsTrue (screenExtents.Y > windowExtents.Y, "GetExtents: Y should be greater with CoordType.Screen");

			BoundingBox buttonExtents;
			buttonExtents = pushButton.QueryComponent().GetExtents (CoordType.Screen);
			Assert.IsTrue (buttonExtents.X > screenExtents.X, "Button X > frame X");
			Assert.IsTrue (buttonExtents.Y > screenExtents.Y, "Button Y > frame Y");
			Assert.IsTrue (buttonExtents.Width < screenExtents.Width, "Button Width < frame Width");
			Assert.IsTrue (buttonExtents.Height < screenExtents.Height, "Button Height < frame Height");
		}

		[Test]
		public void Contains ()
		{
			Component component = frame.QueryComponent ();
			BoundingBox screenExtents;
			screenExtents = component.GetExtents (CoordType.Screen);
			Assert.IsTrue (component.Contains (screenExtents.X + 1, screenExtents.Y + 1, CoordType.Screen), "Contains");
			Assert.IsFalse (component.Contains (screenExtents.X - 1, screenExtents.Y - 1, CoordType.Screen), "Contains");
		}

		[Test]
		public void GetAccessibleAtPoint ()
		{
			Component component = frame.QueryComponent ();
			Accessible child = frame.Children [0];
			BoundingBox childExtents;
			childExtents = child.QueryComponent().GetExtents (CoordType.Screen);
			Accessible accessibleResult = component.GetAccessibleAtPoint (childExtents.X, childExtents.Y, CoordType.Screen);
			Assert.AreEqual (accessibleResult, child, "GetAccessibleAtPoint");
			accessibleResult = component.GetAccessibleAtPoint (childExtents.X + childExtents.Width + 1, childExtents.Y, CoordType.Screen);
			Assert.IsNull (accessibleResult, "GetAccessibleAtPoint OOR");
		}

		[Test]
		public void GetPositionAndSize ()
		{
			int x, y;
			Component component = pushButton.QueryComponent ();
			BoundingBox buttonExtents;
			buttonExtents = component.GetExtents (CoordType.Screen);
			component.GetPosition (CoordType.Screen, out x, out y);
			Assert.AreEqual (buttonExtents.X, x, "GetPosition X");
			Assert.AreEqual (buttonExtents.Y, y, "GetPosition Y");

			component.GetSize (out x, out y);
			Assert.AreEqual (buttonExtents.Width, x, "GetSize Width");
			Assert.AreEqual (buttonExtents.Height, y, "GetSize Height");
		}

		[Test]
		public void MiscFunctions ()
		{
			// just make sure we don't crash
			Component component = frame.QueryComponent ();
			ComponentLayer layer = component.Layer;
			Assert.AreEqual (ComponentLayer.Window, layer, "Layer");
			short MDIZOrder = component.MDIZOrder;
			Assert.AreEqual (-1, MDIZOrder, "MDIZOrder");
			double alpha = component.Alpha;
			Assert.AreEqual (1, alpha, "Alpha");
			component.GrabFocus ();
		}
#endregion
	}
}
