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
//      Mike Gorse <mgorse@novell.com>
// 

using System;
using System.Collections.Generic;
using NDesk.DBus;
using org.freedesktop.DBus;

namespace Atspi
{
	public class Text
	{
		private IText proxy;
		private Properties properties;

		private const string IFACE = "org.freedesktop.atspi.Text";

		public Text (Accessible accessible)
		{
			ObjectPath op = new ObjectPath (accessible.path);
			proxy = Registry.Bus.GetObject<IText> (accessible.application.name, op);
			properties = Registry.Bus.GetObject<Properties> (accessible.application.name, op);
		}

		public string GetText ()
		{
			return proxy.getText (0, -1);
		}

		public string GetText (int startOffset, int endOffset)
		{
			return proxy.getText (startOffset, endOffset);
		}

		public bool SetCaretOffset (int offset)
		{
			return proxy.setCaretOffset (offset);
		}

		public string GetTextBeforeOffset (int offset, BoundaryType type, out int startOffset, out int endOffset)
		{
			string text;
			proxy.getTextBeforeOffset (offset, type, out text, out startOffset, out endOffset);
			return text;
		}

		public string GetTextAtOffset (int offset, BoundaryType type, out int startOffset, out int endOffset)
		{
			string text;
			proxy.getTextAtOffset (offset, type, out text, out startOffset, out endOffset);
			return text;
		}

		public string GetTextAfterOffset (int offset, BoundaryType type, out int startOffset, out int endOffset)
		{
			string text;
			proxy.getTextAfterOffset (offset, type, out text, out startOffset, out endOffset);
			return text;
		}

		public int GetCharacterAtOffset (int offset)
		{
			return proxy.getCharacterAtOffset (offset);
		}

		public string GetAttributeValue (int offset, string attributeName, out int startOffset, out int endOffset, out bool defined)
		{
			string val;
			proxy.getAttributeValue (offset, attributeName, out val, out startOffset, out endOffset, out defined);
			return val;
		}

		public void GetCharacterExtents (int offset, out int x, out int y, out int width, out int height, CoordType coordType)
		{
			proxy.getCharacterExtents (offset, out x, out y, out width, out height, coordType);
		}

		public int GetOffsetAtPoint (int x, int y, CoordType coordType)
		{
			return proxy.getOffsetAtPoint (x, y, coordType);
		}

		public int NSelections {
			get { return proxy.getNSelections (); }
		}

		public void GetSelection (int selectionNum, out int startOffset, out int endOffset)
		{
			proxy.getSelection (selectionNum, out startOffset, out endOffset);
		}

		public bool AddSelection (int startOffset, int endOffset)
		{
			return proxy.addSelection (startOffset, endOffset);
		}

		public bool RemoveSelection (int selectionNum)
		{
			return proxy.removeSelection (selectionNum);
		}

		public bool SetSelection (int selectionNum, int startOffset, int endOffset)
		{
			return proxy.setSelection (selectionNum, startOffset, endOffset);
		}

		public void GetRangeExtents (int startOffset, int endOffset, out int x, out int y, out int width, out int height, CoordType coordType)
		{
			proxy.getRangeExtents (startOffset, endOffset, out x, out y, out width, out height, coordType);
		}

		public RangeList [] GetBoundedRanges (int x, int y, int width, int height, CoordType coordType, ClipType xClipType, ClipType yClipType)
		{
			return proxy.getBoundedRanges (x, y, width, height, coordType, xClipType, yClipType);
		}

		public IDictionary<string, string> GetAttributeRun (int offset, out int startOffset, out int endOffset, bool includeDefaults)
		{
			IDictionary<string, string> attributes;
			proxy.getAttributeRun (offset, out attributes, out startOffset, out endOffset, includeDefaults);
			return attributes;
		}

		public IDictionary<string, string> GetDefaultAttributeSet ()
		{
			return proxy.getDefaultAttributeSet ();
		}

		public int CharacterCount {
			get {
				return (int) properties.Get (IFACE, "characterCount");
			}
		}

		public int CaretOffset {
			get {
				return (int) properties.Get (IFACE, "caretOffset");
			}
		}
	}

	public struct TextDescription
	{
		public string Name;
		public string Description;
		public string KeyBinding;
	}

	public enum BoundaryType : uint
	{
		Char,
		WordStart,
		WordEnd,
		SentenceStart,
		SentenceEnd,
		LineStart,
		LineEnd
	}

	public enum ClipType : uint
	{
		None,
		Min,
		Max,
		Both
	}

	public struct RangeList
	{
		public int StartOffset;
		public int EndOffset;
		public string Comment;
		// TODO: How to map a variant?  Is the below line correct?
		public object unused;
	}

	[Interface ("org.freedesktop.atspi.Text")]
	interface IText : Introspectable
	{
		string getText (int startOffset, int endOffset);
		bool setCaretOffset (int offset);
		void getTextBeforeOffset (int offset, BoundaryType type, out string text, out int startOffset, out int endOffset);
		void getTextAtOffset (int offset, BoundaryType type, out string text, out int startOffset, out int endOffset);
		void getTextAfterOffset (int offset, BoundaryType type, out string text, out int startOffset, out int endOffset);
		int getCharacterAtOffset (int offset);
		void getAttributeValue (int offset, string attributeName, out string val, out int startOffset, out int endOffset, out bool defined);
		void getCharacterExtents (int offset, out int x, out int y, out int width, out int height, CoordType coordType);
		int getOffsetAtPoint (int x, int y, CoordType coordType);
		int getNSelections ();
		void getSelection (int selectionNum, out int startOffset, out int endOffset);
		bool addSelection (int startOffset, int endOffset);
		bool removeSelection (int selectionNum);
		bool setSelection (int selectionNum, int startOffset, int endOffset);
		void getRangeExtents (int startOffset, int endOffset, out int x, out int y, out int width, out int height, CoordType coordType);
		RangeList [] getBoundedRanges (int x, int y, int width, int height, CoordType coordType, ClipType xClipType, ClipType yClipType);
		void getAttributeRun (int offset, out IDictionary<string, string> attributes, out int startOffset, out int endOffset, bool includeDefaults);
		IDictionary<string, string> getDefaultAttributeSet ();
	}
}
