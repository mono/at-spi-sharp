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
	public class ValueTest : Base
	{
		Accessible frame = null;
		Value value = null;

		public ValueTest ()
		{
			frame = GetFrame ("gtkhscale.py");

			Accessible slider = FindByRole (frame, Role.Slider);
			Assert.IsNotNull (slider, "Couldn't find the Slider");
			value = slider.QueryValue ();
			Assert.IsNotNull (value, "slider.QueryValue");
		}

		#region Test
		
		[Test]
		public void Values ()
		{
			Assert.IsNull (frame.QueryValue (), "Frame should not support Value");

			Assert.AreEqual (0, value.MinimumValue, "MinimumValue");
			Assert.AreEqual (119, value.MaximumValue, "MaximumValue");
		}

		[Test]
		public void SetValue ()
		{
			value.CurrentValue = 50;
			Assert.AreEqual (50, value.CurrentValue, "CurrentValue #1");
			value.CurrentValue = 60;
			Assert.AreEqual (60, value.CurrentValue, "CurrentValue #2");
		}
#endregion
	}
}
