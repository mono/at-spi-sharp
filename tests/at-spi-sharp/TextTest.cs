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
	public class TextTest : Base
	{
		Accessible frame = null;
		Text text = null;
		EditableText editableText = null;

		public TextTest ()
		{
			frame = GetFrame ("gtktextview.py");

			Accessible textView = FindByRole (frame, Role.Text);
			Assert.IsNotNull (textView, "Couldn't find the TextView");
			text = textView.QueryText ();
			Assert.IsNotNull (text, "textView.QueryText");
			editableText = textView.QueryEditableText ();
			Assert.IsNotNull (editableText, "textView.QueryEditableText");
		}

		#region Test
		
		[Test]
		public void Frame ()
		{
			Assert.IsNull (frame.QueryText (), "Frame should not support Text");
			Assert.IsNull (frame.QueryEditableText (), "Frame should not support EditableText");
		}

		[Test]
		public void Text ()
		{
			string str = "first line\nsecond line\nthird line";
			editableText.SetTextContents (str);
			Assert.AreEqual (str, text.GetText (), "GetText #1");
			Assert.AreEqual (str.Length, text.CharacterCount, "CharacterCount");
			Assert.AreEqual ("first", text.GetText (0, 5), "GetText #2");
			Assert.IsTrue (text.SetCaretOffset (2), "SetCaretOffset");
			Assert.AreEqual (2, text.CaretOffset, "CaretOffset");

			int startOffset, endOffset;
			Assert.AreEqual ("r", text.GetTextBeforeOffset (3, BoundaryType.Char, out startOffset, out endOffset), "GetTextBeforeOffset (char)");
			Assert.AreEqual (2, startOffset, "startOffset (char)");
			Assert.AreEqual (3, endOffset, "EndOffset (char)");
			Assert.AreEqual ("second ", text.GetTextBeforeOffset (18, BoundaryType.WordStart, out startOffset, out endOffset), "GetTextBeforeOffset (WordStart)");
			Assert.AreEqual (11, startOffset, "StartOffset (WordStart)");
			Assert.AreEqual (18, endOffset, "EndOffset (WordStart)");
			Assert.AreEqual ("\nsecond", text.GetTextBeforeOffset (17, BoundaryType.WordEnd, out startOffset, out endOffset), "GetTextBeforeOffset (WordEnd)");
			Assert.AreEqual (10, startOffset, "StartOffset");
			Assert.AreEqual (17, endOffset, "EndOffset");
			Assert.AreEqual ("second line\n", text.GetTextBeforeOffset (23, BoundaryType.SentenceStart, out startOffset, out endOffset), "GetTextBoreOffset (SentenceStart)");
			Assert.AreEqual (11, startOffset, "StartOffset (SentenceStart)");
			Assert.AreEqual (23, endOffset, "EndOffset (SentenceStart)");
			Assert.AreEqual ("\nsecond line", text.GetTextBeforeOffset (22, BoundaryType.SentenceEnd, out startOffset, out endOffset), "GetTextBoreOffset (SentenceEnd)");
			Assert.AreEqual (10, startOffset, "EndOffset (SentenceEnd)");
			Assert.AreEqual (22, endOffset, "EndOffset (SentenceEnd)");
			Assert.AreEqual ("\n", text.GetTextBeforeOffset (23, BoundaryType.LineStart, out startOffset, out endOffset), "GetTextBoreOffset (LineStart)");
			Assert.AreEqual (22, startOffset, "StartOffset (LineStart)");
			Assert.AreEqual (23, endOffset, "EndOffset (LineStart)");
			Assert.AreEqual ("\nsecond line", text.GetTextBeforeOffset (23, BoundaryType.LineEnd, out startOffset, out endOffset), "GetTextBoreOffset (LineEnd)");
			Assert.AreEqual (10, startOffset, "EndOffset (LineEnd)");
			Assert.AreEqual (22, endOffset, "EndOffset (LineEnd)");

			Assert.AreEqual ("s", text.GetTextAtOffset (3, BoundaryType.Char, out startOffset, out endOffset), "GetTextAtOffset (char)");
			Assert.AreEqual (3, startOffset, "startOffset (char)");
			Assert.AreEqual (4, endOffset, "EndOffset (char)");

			Assert.AreEqual ("t", text.GetTextAfterOffset (3, BoundaryType.Char, out startOffset, out endOffset), "GetTextAfterOffset (char)");
			Assert.AreEqual (4, startOffset, "startOffset (char)");
			Assert.AreEqual (5, endOffset, "EndOffset (char)");

			Assert.AreEqual (str [6], text.GetCharacterAtOffset (6), "GetCharacterAtOffset");
		}

		[Test]
		public void Attributes ()
		{
			int startOffset, endOffset;
			bool defined;
			Assert.AreEqual (String.Empty, text.GetAttributeValue (5, "none", out startOffset, out endOffset, out defined), "GetAttributeValue");
			Assert.IsFalse (defined, "defined");

			IDictionary<string, string> attributes;
			attributes = text.GetAttributeRun (1, out startOffset, out endOffset, true);
			Assert.AreEqual ("0", attributes ["left-margin"], "GetAttributeRun left-margin");
			attributes = text.GetDefaultAttributeSet ();
			Assert.AreEqual ("0", attributes ["left-margin"], "GetDefaultAttributeSet left-margin");
			// We should theoretically test
			// editableText.SetAttributes, but it is unimplemented
		}

		[Test]
		public void Screen ()
		{
			editableText.SetTextContents ("test");
			int x, y, width, height;
			text.GetCharacterExtents (0, out x, out y, out width, out height, CoordType.Screen);
			Assert.IsTrue (x > 0, "x > 0");
			Assert.IsTrue (y > 0, "y > 0");
			Assert.IsTrue (width > 0, "width > 0");
			Assert.IsTrue (height > 0, "height > 0");

			Assert.AreEqual (0, text.GetOffsetAtPoint (0, 0, CoordType.Window), "GetOffsetAtPoint");

			text.GetRangeExtents (0, 4, out x, out y, out width, out height, CoordType.Screen);
			RangeList [] ranges;
			ranges = text.GetBoundedRanges (x, y, width, height, CoordType.Screen, ClipType.None, ClipType.None);
			Assert.AreEqual (1, ranges.Length, "range length");
			Assert.AreEqual (0, ranges [0].StartOffset, "range StartOffset");
			Assert.AreEqual (3, ranges [0].EndOffset, "range EndOffset");
		}

		[Test]
		public void Selections ()
		{
			editableText.SetTextContents ("This is a test");

			Assert.AreEqual (0, text.NSelections, "NSelections");
			Assert.IsTrue (text.AddSelection (1, 2), "AddSelection");
			Assert.AreEqual (1, text.NSelections, "NSelections after add");
			Assert.IsTrue (text.SetSelection (0, 4, 7), "SetSelection");
			int startOffset, endOffset;
			text.GetSelection (0, out startOffset, out endOffset);
			Assert.AreEqual (4, startOffset, "GetSelection StartOffset");
			Assert.AreEqual (7, endOffset, "GetSelection EndOffset");

			Assert.IsTrue (text.RemoveSelection (0), "RemoveSelection");
			Assert.IsFalse (text.RemoveSelection (0), "RemoveSelection when nothing selected");
			Assert.AreEqual (0, text.NSelections, "NSelections after remove");
		}

		[Test]
		public void EditText ()
		{
			string str = "test";

			editableText.SetTextContents (str);
			Assert.AreEqual (str, text.GetText (), "GetText");
			editableText.InsertText (2, "abcd", 4);
			Assert.AreEqual ("teabcdst", text.GetText (), "GetText");
			editableText.DeleteText (2, 6);
			Assert.AreEqual (str, text.GetText (), "GetText");
		}

		[Test]
		public void ClipBoard ()
		{
			editableText.SetTextContents ("This is a test");
			Assert.IsTrue (editableText.CutText (2, 5), "CutText");
			Assert.AreEqual ("This a test", text.GetText (), "GetText after cut");
			Assert.IsTrue (editableText.PasteText (0), "PasteText");
			Assert.AreEqual ("is This a test", text.GetText (), "GetText after paste");
			editableText.CopyText (8, 10);
			Assert.IsTrue (editableText.PasteText (10), "PasteText");
			Assert.AreEqual ("is This a a test", text.GetText (), "GetText after copy/paste");
		}
#endregion
	}
}
