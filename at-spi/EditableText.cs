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
	public class EditableText
	{
		private IEditableText proxy;

		private const string IFACE = "org.freedesktop.atspi.EditableText";

		public EditableText (Accessible accessible)
		{
			ObjectPath op = new ObjectPath (accessible.path);
			proxy = Registry.Bus.GetObject<IEditableText> (accessible.application.name, op);
		}

		// Should this be a property instead?  We would then lose
		// the return value, but atk does not return a value anyway,
		// although it ought to.
		public bool SetTextContents (string newContents)
		{
			return proxy.setTextContents (newContents);
		}

		public bool InsertText (int position, string text, int length)
		{
			return proxy.insertText (position, text, length);
		}

		public bool SetAttributes (string attributes, int startPos, int endPos)
		{
			return proxy.setAttributes (attributes, startPos, endPos);
		}

		public void CopyText (int startPos, int endPos)
		{
			proxy.copyText (startPos, endPos);
		}

		public bool CutText (int startPos, int endPos)
		{
			return proxy.cutText (startPos, endPos);
		}

		public bool DeleteText (int startPos, int endPos)
		{
			return proxy.deleteText (startPos, endPos);
		}

		public bool PasteText (int position)
		{
			return proxy.pasteText (position);
		}
	}

	[Interface ("org.freedesktop.atspi.EditableText")]
	interface IEditableText : Introspectable
	{
		bool setTextContents (string newContents);
		bool insertText (int position, string text, int length);
		bool setAttributes (string attributes, int startPos, int endPos);
		void copyText (int startPos, int endPos);
		bool cutText (int startPos, int endPos);
		bool deleteText (int startPos, int endPos);
		bool pasteText (int position);
	}
}
