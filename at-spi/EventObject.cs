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
				Registry.RegisterEventListener ("Object:PropertyChange");
				proxy.PropertyChange += GetDelegate (value);
			}
			remove {
				proxy.PropertyChange -= GetDelegate (value);
				Registry.DeregisterEventListener ("Object:PropertyChange");
			}
		}

		public event EventR BoundsChanged {
			add {
				Registry.RegisterEventListener ("Object:BoundsChanged");
				proxy.BoundsChanged += GetDelegate (value);
			}
			remove {
				proxy.BoundsChanged -= GetDelegate (value);
				Registry.DeregisterEventListener ("Object:BoundsChanged");
			}
		}

		public event EventI LinkSelected {
			add {
				Registry.RegisterEventListener ("Object:LinkSelected");
				proxy.LinkSelected += GetDelegate (value);
			}
			remove {
				proxy.LinkSelected -= GetDelegate (value);
				Registry.DeregisterEventListener ("Object:LinkSelected");
			}
		}

		public event EventSB StateChanged {
			add {
				Registry.RegisterEventListener ("Object:StateChanged");
				proxy.StateChanged += GetDelegate (value);
			}
			remove {
				proxy.StateChanged -= GetDelegate (value);
				Registry.DeregisterEventListener ("Object:StateChanged");
			}
		}

		public event EventSIO ChildrenChanged {
			add {
				Registry.RegisterEventListener ("Object:ChildrenChanged");
				proxy.ChildrenChanged += GetChildrenChangedDelegate (value);
			}
			remove {
				proxy.ChildrenChanged -= GetChildrenChangedDelegate (value);
				Registry.DeregisterEventListener ("Object:ChildrenChanged");
			}
		}

		public event EventSimple VisibleDataChanged {
			add {
				Registry.RegisterEventListener ("Object:VisibleDataChanged");
				proxy.VisibleDataChanged += GetDelegate (value);
			}
			remove {
				proxy.VisibleDataChanged -= GetDelegate (value);
				Registry.DeregisterEventListener ("Object:VisibleDataChanged");
			}
		}

		public event EventSimple SelectionChanged {
			add {
				Registry.RegisterEventListener ("Object:SelectionChanged");
				proxy.SelectionChanged += GetDelegate (value);
			}
			remove {
				proxy.SelectionChanged -= GetDelegate (value);
				Registry.DeregisterEventListener ("Object:SelectionChanged");
			}
		}

		public event EventSimple ModelChanged {
			add {
				Registry.RegisterEventListener ("Object:ModelChanged");
				proxy.ModelChanged += GetDelegate (value);
			}
			remove {
				proxy.ModelChanged -= GetDelegate (value);
				Registry.DeregisterEventListener ("Object:ModelChanged");
			}
		}

		public event EventO ActiveDescendantChanged {
			add {
				Registry.RegisterEventListener ("Object:ActiveDescendantChanged");
				proxy.ActiveDescendantChanged += GetDelegate (value);
			}
			remove {
				proxy.ActiveDescendantChanged -= GetDelegate (value);
				Registry.DeregisterEventListener ("Object:ActiveDescendantChanged");
			}
		}

		public event EventII RowInserted {
			add {
				Registry.RegisterEventListener ("Object:RowInserted");
				proxy.RowInserted += GetDelegate (value);
			}
			remove {
				proxy.RowInserted -= GetDelegate (value);
				Registry.DeregisterEventListener ("Object:RowInserted");
			}
		}

		public event EventSimple RowReordered {
			add {
				Registry.RegisterEventListener ("Object:RowReordered");
				proxy.RowReordered += GetDelegate (value);
			}
			remove {
				proxy.RowReordered -= GetDelegate (value);
				Registry.DeregisterEventListener ("Object:RowReordered");
			}
		}

		public event EventII RowDeleted {
			add {
				Registry.RegisterEventListener ("Object:RowDeleted");
				proxy.RowDeleted += GetDelegate (value);
			}
			remove {
				proxy.RowDeleted -= GetDelegate (value);
				Registry.DeregisterEventListener ("Object:RowDeleted");
			}
		}

		public event EventII ColumnInserted {
			add {
				Registry.RegisterEventListener ("Object:ColumnInserted");
				proxy.ColumnInserted += GetDelegate (value);
			}
			remove {
				proxy.ColumnInserted -= GetDelegate (value);
				Registry.DeregisterEventListener ("Object:ColumnInserted");
			}
		}

		public event EventSimple ColumnReordered {
			add {
				Registry.RegisterEventListener ("Object:ColumnReordered");
				proxy.ColumnReordered += GetDelegate (value);
			}
			remove {
				proxy.ColumnReordered -= GetDelegate (value);
				Registry.DeregisterEventListener ("Object:ColumnReordered");
			}
		}

		public event EventII ColumnDeleted {
			add {
				Registry.RegisterEventListener ("Object:ColumnDeleted");
				proxy.ColumnDeleted += GetDelegate (value);
			}
			remove {
				proxy.ColumnDeleted -= GetDelegate (value);
				Registry.DeregisterEventListener ("Object:ColumnDeleted");
			}
		}

		public event EventSimple TextBoundsChanged {
			add {
				Registry.RegisterEventListener ("Object:TextBoundsChanged");
				proxy.TextBoundsChanged += GetDelegate (value);
			}
			remove {
				proxy.TextBoundsChanged -= GetDelegate (value);
				Registry.DeregisterEventListener ("Object:TextBoundsChanged");
			}
		}

		public event EventSimple TextSelectionChanged {
			add {
				Registry.RegisterEventListener ("Object:TextSelectionChanged");
				proxy.TextSelectionChanged += GetDelegate (value);
			}
			remove {
				proxy.TextSelectionChanged -= GetDelegate (value);
				Registry.DeregisterEventListener ("Object:TextSelectionChanged");
			}
		}

		public event EventSIIS TextChanged {
			add {
				Registry.RegisterEventListener ("Object:TextChanged");
				proxy.TextChanged += GetDelegate (value);
			}
			remove {
				proxy.TextChanged -= GetDelegate (value);
				Registry.DeregisterEventListener ("Object:TextChanged");
			}
		}

		public event EventSimple TextAttributesChanged {
			add {
				Registry.RegisterEventListener ("Object:TextAttributesChanged");
				proxy.TextAttributesChanged += GetDelegate (value);
			}
			remove {
				proxy.TextAttributesChanged -= GetDelegate (value);
				Registry.DeregisterEventListener ("Object:TextAttributesChanged");
			}
		}

		public event EventI TextCaretMoved {
			add {
				Registry.RegisterEventListener ("Object:TextCaretMoved");
				proxy.TextCaretMoved += GetDelegate (value);
			}
			remove {
				proxy.TextCaretMoved -= GetDelegate (value);
				Registry.DeregisterEventListener ("Object:TextCaretMoved");
			}
		}

		public event EventSimple AttributesChanged {
			add {
				Registry.RegisterEventListener ("Object:AttributesChanged");
				proxy.AttributesChanged += GetDelegate (value);
			}
			remove {
				proxy.AttributesChanged -= GetDelegate (value);
				Registry.DeregisterEventListener ("Object:AttributesChanged");
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
