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
	public class ImageTest : Base
	{
		Accessible frame = null;
		Image image = null;

		public ImageTest ()
		{
			frame = GetFrame ("gtkbutton.py");

			Accessible button = Find (frame, Role.PushButton, "openSUSE", true);
			Assert.IsNotNull (button, "Couldn't find the PushButton");
			// It seems that the image needs time to load?
			System.Threading.Thread.Sleep (500);
			image = button.QueryImage ();
			Assert.IsNotNull (image, "button.QueryImage");
		}

		#region Test
		
		[Test]
		public void Image ()
		{
			Assert.IsNull (frame.QueryImage (), "Frame should not support Image");

			BoundingBox extents = image.GetImageExtents (CoordType.Window);
			Assert.AreEqual (208, extents.X, "Extents.X");
			Assert.AreEqual (86, extents.Y, "Extents.Y");
			int x, y, width, height;
			image.GetPosition (out x, out y, CoordType.Window);
			Assert.AreEqual (extents.X, x, "GetPosition X");
			Assert.AreEqual (extents.Y, y, "GetPosition Y");
		image.GetSize (out width, out height);
			Assert.AreEqual (extents.Width, width, "GetSize Width");
			Assert.AreEqual (extents.Height, height, "GetSize Height");

			Assert.AreEqual (string.Empty, image.Description, "Image Description");
			Assert.AreEqual (string.Empty, image.Locale, "Image Locale");
		}
		#endregion
	}
}
