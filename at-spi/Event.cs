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
	[Interface ("org.freedesktop.atspi.Event.Object")]
	public interface IEventObject
	{
		event AtspiEventHandler PropertyChange;
		event AtspiEventHandler BoundsChanged;
		event AtspiEventHandler LinkSelected;
		event AtspiEventHandler StateChanged;
		event AtspiEventHandler ChildrenChanged;
		event AtspiEventHandler VisibleDataChanged;
		event AtspiEventHandler SelectionChanged;
		event AtspiEventHandler ModelChanged;
		event AtspiEventHandler ActiveDescendantChanged;
		event AtspiEventHandler RowInserted;
		event AtspiEventHandler RowReordered;
		event AtspiEventHandler RowDeleted;
		event AtspiEventHandler ColumnInserted;
		event AtspiEventHandler ColumnReordered;
		event AtspiEventHandler ColumnDeleted;
		event AtspiEventHandler TextBoundsChanged;
		event AtspiEventHandler TextSelectionChanged;
		event AtspiEventHandler TextChanged;
		event AtspiEventHandler TextAttributesChanged;
		event AtspiEventHandler TextCaretMoved;
		event AtspiEventHandler AttributesChanged;
	}

	[Interface ("org.freedesktop.atspi.Event.Window")]
	public interface IEventWindow
	{
		event AtspiEventHandler PropertyChange;
		event AtspiEventHandler Minimize;
		event AtspiEventHandler Maximize;
		event AtspiEventHandler Restore;
		event AtspiEventHandler Close;
		event AtspiEventHandler Create;
		event AtspiEventHandler Reparent;
		event AtspiEventHandler DesktopCreate;
		event AtspiEventHandler DesktopDestroy;
		event AtspiEventHandler Destroy;
		event AtspiEventHandler Activate;
		event AtspiEventHandler Deactivate;
		event AtspiEventHandler Raise;
		event AtspiEventHandler Lower;
		event AtspiEventHandler Move;
		event AtspiEventHandler Resize;
		event AtspiEventHandler Shade;
		event AtspiEventHandler Unshade;
		event AtspiEventHandler Restyle;
	}

	[Interface ("org.freedesktop.atspi.Event.Mouse")]
	public interface IEventMouse
	{
		event AtspiEventHandler Abs;
		event AtspiEventHandler Rel;
		event AtspiEventHandler Button;
	}

	[Interface ("org.freedesktop.atspi.Event.Keyboard")]
	public interface IEventKeyboard
	{
		event AtspiEventHandler Modifiers;
	}

	[Interface ("org.freedesktop.atspi.Event.Terminal")]
	public interface IEventTerminal
	{
		event AtspiEventHandler LineChanged;
		event AtspiEventHandler ColumncountChanged;
		event AtspiEventHandler LinecountChanged;
		event AtspiEventHandler ApplicationChanged;
		event AtspiEventHandler CharwidthChanged;
	}

	[Interface ("org.freedesktop.atspi.Event.Document")]
	public interface IEventDocument
	{
		event AtspiEventHandler LoadComplete;
		event AtspiEventHandler Reload;
		event AtspiEventHandler LoadStopped;
		event AtspiEventHandler ContentChanged;
		event AtspiEventHandler AttributesChanged;
	}

	[Interface ("org.freedesktop.atspi.Event.Focus")]
	public interface IEventFocus
	{
		event AtspiEventHandler Focus;
	}

	public delegate void AtspiEventHandler (string detail, int v1, int v2, object any);
}
