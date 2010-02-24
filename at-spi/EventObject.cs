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
	public class EventObject : EventBase
	{
		private IEventObject proxy;

		public EventObject (Accessible accessible) : base (accessible)
		{
			proxy = Registry.Bus.GetObject<IEventObject> (accessible.Application.Name, new ObjectPath (accessible.path));

			// Hack so that managed-dbus can get the alternate name
			if (accessible.Application is Registry)
				proxy.Introspect ();
		}

		public event EventSV PropertyChange {
			add {
				proxy.PropertyChange += GetDelegate (value);
			}
			remove {
				proxy.PropertyChange -= GetDelegate (value);
			}
		}

		public event EventR BoundsChanged {
			add {
				proxy.BoundsChanged += GetDelegate (value);
			}
			remove {
				proxy.BoundsChanged -= GetDelegate (value);
			}
		}

		public event EventI LinkSelected {
			add {
				proxy.LinkSelected += GetDelegate (value);
			}
			remove {
				proxy.LinkSelected -= GetDelegate (value);
			}
		}

		public event EventSB StateChanged {
			add {
				proxy.StateChanged += GetDelegate (value);
			}
			remove {
				proxy.StateChanged -= GetDelegate (value);
			}
		}

		public event EventSIO ChildrenChanged {
			add {
				proxy.ChildrenChanged += GetChildrenChangedDelegate (value);
			}
			remove {
				proxy.ChildrenChanged -= GetChildrenChangedDelegate (value);
			}
		}

		public event EventSimple VisibleDataChanged {
			add {
				proxy.VisibleDataChanged += GetDelegate (value);
			}
			remove {
				proxy.VisibleDataChanged -= GetDelegate (value);
			}
		}

		public event EventSimple SelectionChanged {
			add {
				proxy.SelectionChanged += GetDelegate (value);
			}
			remove {
				proxy.SelectionChanged -= GetDelegate (value);
			}
		}

		public event EventSimple ModelChanged {
			add {
				proxy.ModelChanged += GetDelegate (value);
			}
			remove {
				proxy.ModelChanged -= GetDelegate (value);
			}
		}

		public event EventO ActiveDescendantChanged {
			add {
				proxy.ActiveDescendantChanged += GetDelegate (value);
			}
			remove {
				proxy.ActiveDescendantChanged -= GetDelegate (value);
			}
		}

		public event EventII RowInserted {
			add {
				proxy.RowInserted += GetDelegate (value);
			}
			remove {
				proxy.RowInserted -= GetDelegate (value);
			}
		}

		public event EventSimple RowReordered {
			add {
				proxy.RowReordered += GetDelegate (value);
			}
			remove {
				proxy.RowReordered -= GetDelegate (value);
			}
		}

		public event EventII RowDeleted {
			add {
				proxy.RowDeleted += GetDelegate (value);
			}
			remove {
				proxy.RowDeleted -= GetDelegate (value);
			}
		}

		public event EventII ColumnInserted {
			add {
				proxy.ColumnInserted += GetDelegate (value);
			}
			remove {
				proxy.ColumnInserted -= GetDelegate (value);
			}
		}

		public event EventSimple ColumnReordered {
			add {
				proxy.ColumnReordered += GetDelegate (value);
			}
			remove {
				proxy.ColumnReordered -= GetDelegate (value);
			}
		}

		public event EventII ColumnDeleted {
			add {
				proxy.ColumnDeleted += GetDelegate (value);
			}
			remove {
				proxy.ColumnDeleted -= GetDelegate (value);
			}
		}

		public event EventSimple TextBoundsChanged {
			add {
				proxy.TextBoundsChanged += GetDelegate (value);
			}
			remove {
				proxy.TextBoundsChanged -= GetDelegate (value);
			}
		}

		public event EventSimple TextSelectionChanged {
			add {
				proxy.TextSelectionChanged += GetDelegate (value);
			}
			remove {
				proxy.TextSelectionChanged -= GetDelegate (value);
			}
		}

		public event EventSIIS TextChanged {
			add {
				proxy.TextChanged += GetDelegate (value);
			}
			remove {
				proxy.TextChanged -= GetDelegate (value);
			}
		}

		public event EventSimple TextAttributesChanged {
			add {
				proxy.TextAttributesChanged += GetDelegate (value);
			}
			remove {
				proxy.TextAttributesChanged -= GetDelegate (value);
			}
		}

		public event EventI TextCaretMoved {
			add {
				proxy.TextCaretMoved += GetDelegate (value);
			}
			remove {
				proxy.TextCaretMoved -= GetDelegate (value);
			}
		}

		public event EventSimple AttributesChanged {
			add {
				proxy.AttributesChanged += GetDelegate (value);
			}
			remove {
				proxy.AttributesChanged -= GetDelegate (value);
			}
		}
	}

	[Interface ("org.a11y.atspi.Event.Object")]
	internal interface IEventObject : Introspectable
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
}
