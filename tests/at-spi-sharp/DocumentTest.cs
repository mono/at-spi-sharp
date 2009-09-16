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
using NDesk.DBus;
using NUnit.Framework;
using Atspi;

namespace AtSpiTest
{
	[TestFixture]
	public class DocumentTest : Base
	{
		Accessible frame = null;
		Document document = null;

		public DocumentTest ()
		{
			frame = GetFrame ("DocumentTest.exe", "DocumentTest");

			document = frame.QueryDocument ();
			Assert.IsNotNull (document, "frame.QueryDocument");
		}

		#region Test

		[Test]
		public void Document ()
		{
			Assert.AreEqual ("en", document.Locale, "Document Locale");
			Assert.AreEqual ("2.0", document.GetAttributeValue ("left-margin"), "Document GetAttributeValue");
			Assert.AreEqual (string.Empty, document.GetAttributeValue ("undefined"), "Document GetAttributeValue for an undefined attribute");
			IDictionary<string, string> attributes = document.Attributes;
			Assert.AreEqual (1, attributes.Count, "Document Attributes.Count");
			Assert.AreEqual ("2.0", attributes ["left-margin"], "Document Attributes[\"left-margin\"]");
		}
#endregion
	}
}
