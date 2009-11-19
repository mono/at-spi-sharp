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
using System.Collections.Generic;
using NUnit.Framework;
using Atspi;

namespace AtSpiTest
{
	[TestFixture]
	public class HypertextTest : Base
	{
		Accessible frame = null;
		Hypertext hypertext = null;

		public HypertextTest ()
		{
			frame = GetFrame ("DocumentTest.exe", "DocumentTest");

			hypertext = frame.QueryHypertext ();
			Assert.IsNotNull (hypertext, "frame.QueryHypertext");
		}

		#region Test
		
		[Test]
		public void Hypertext ()
		{
			Assert.AreEqual (2, hypertext.NLinks, "NLinks");
			Assert.AreEqual (1, hypertext.GetLinkIndex (60), "GetLinkIndex");
		}

		[Test]
		public void Hyperlink ()
		{
			Assert.IsNull (hypertext.GetLink (-1), "GetLink (-1)");
			Assert.IsNull (hypertext.GetLink (2), "GetLink (2)");

			Hyperlink link = hypertext.GetLink (0);
			Assert.IsNotNull (link, "GetLink");
			Assert.AreEqual ( 5, link.StartIndex, "StartIndex");
			Assert.AreEqual (26, link.EndIndex, "EndIndex");
			Assert.AreEqual ("http://www.novell.com", link.GetURI (0), "Uri");
			Assert.AreEqual (string.Empty, link.GetURI (1), "Uri (1)");
			Assert.AreEqual (string.Empty, link.GetURI (-1), "Uri (-1)");
			Accessible obj = link.GetObject (-1);
			Assert.IsNull (obj, "GetObject (-1)");
			obj = link.GetObject (2);
			Assert.IsNull (obj, "GetObject (2)");

			obj = link.GetObject (0);
			Assert.IsNotNull (obj, "GetObject");
			Action action = obj.QueryAction ();
			Assert.IsNotNull (action, "GetObject.QueryAction");
			Assert.IsTrue (action.DoAction (0), "DoAction");

			link = hypertext.GetLink (1);
			Assert.IsNotNull (link, "GetLink (1)");
			Assert.AreEqual (55, link.StartIndex, "StartIndex");
			Assert.AreEqual (76, link.EndIndex, "EndIndex");
			Assert.AreEqual ("http://www.google.com", link.GetURI (0), "Uri");
			Accessible obj2 = link.GetObject (0);
			Assert.IsNotNull (obj, "GetObject");
			Assert.AreNotEqual (obj, obj2, "GetObject for second link should not be the same as for the first link");
		}
#endregion
	}
}
