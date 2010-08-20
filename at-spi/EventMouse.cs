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
	public class EventMouse : EventBase
	{
		private IEventMouse proxy;

		public EventMouse (Accessible accessible) : base (accessible)
		{
			proxy = Registry.Bus.GetObject<IEventMouse> (accessible.Application.Name, new ObjectPath (accessible.path));
		}

		public event EventII Abs {
			add {
				Registry.RegisterEventListener ("Mouse:Abs");
				proxy.Abs += GetDelegate (value);
			}
			remove {
				proxy.Abs -= GetDelegate (value);
				Registry.DeregisterEventListener ("Mouse:Abs");
			}
		}

		public event EventSimple Rel {
			add {
				Registry.RegisterEventListener ("Mouse:Rel");
				proxy.Rel += GetDelegate (value);
			}
			remove {
				proxy.Rel -= GetDelegate (value);
				Registry.DeregisterEventListener ("Mouse:Rel");
			}
		}

		public event EventSII Button {
			add {
				Registry.RegisterEventListener ("Mouse:Button");
				proxy.Button += GetDelegate (value);
			}
			remove {
				proxy.Button -= GetDelegate (value);
				Registry.DeregisterEventListener ("Mouse:Button");
			}
		}
	}

	[Interface ("org.a11y.atspi.Event.Mouse")]
	internal interface IEventMouse
	{
		event AtspiEventHandler Abs;
		event AtspiEventHandler Rel;
		event AtspiEventHandler Button;
	}
}
