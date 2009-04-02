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

namespace Atspi
{
	// TODO: Derive this from the at-spi xml?
	public enum StateType
	{
		Invalid,
		Active,
		Armed,
		Busy,
		Checked,
		Defunct,
		Editable,
		Enabled,
		Expandable,
		Expanded,
		Focusable,
		Focused,
		Horizontal,
		Iconified,
		Modal,
		MultiLine,
		Multiselectable,
		Opaque,
		Pressed,
		Resizable,
		Selectable,
		Selected,
		Sensitive,
		Showing,
		SingleLine,
		Stale,
		Transient,
		Vertical,
		Visible,
		ManagesDescendants,
		Indeterminate,
		Truncated,
		Required,
		InvalidEntry,
		SupportsAutocompletion,
		SelectableText,
		Default,
		Animated,
		Visited,
		LastDefined
	}
}
