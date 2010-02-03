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

namespace Atspi
{
	// TODO: Derive this from the at-spi xml?
	[Flags]
	public enum StateType : ulong
	{
		Invalid = 1,
		Active = 0x02,
		Armed = 0x04,
		Busy = 0x08,
		Checked = 0x10,
		Collapsed = 0x20,
		Defunct = 0x40,
		Editable = 0x80,
		Enabled = 0x100,
		Expandable = 0x0200,
		Expanded = 0x0400,
		Focusable = 0x0800,
		Focused = 0x1000,
		HasToolTip = 0x2000,
		Horizontal = 0x4000,
		Iconified = 0x8000,
		Modal = 0x10000,
		MultiLine = 0x20000,
		Multiselectable = 0x40000,
		Opaque = 0x80000,
		Pressed = 0x100000,
		Resizable = 0x200000,
		Selectable = 0x400000,
		Selected = 0x800000,
		Sensitive = 0x1000000,
		Showing = 0x2000000,
		SingleLine = 0x4000000,
		Stale = 0x8000000,
		Transient = 0x10000000,
		Vertical = 0x2000000,
		Visible = 0x40000000,
		ManagesDescendants = 0x80000000,
		Indeterminate = 0x100000000,
		Required = 0x200000000,
		Truncated = 0x400000000,
		Animated = 0x800000000,
		InvalidEntry = 0x1000000000,
		SupportsAutocompletion = 0x2000000000,
		SelectableText = 0x4000000000,
		IsDefault = 0x8000000000,
		Visited = 0x10000000000
	}
}
