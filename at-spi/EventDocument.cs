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
	public class EventDocument : EventBase
	{
		private IEventDocument proxy;

		public EventDocument (Accessible accessible) : base (accessible)
		{
			proxy = Registry.Bus.GetObject<IEventDocument> (accessible.Application.Name, new ObjectPath (accessible.path));
		}

		public event EventSimple LoadComplete {
			add {
				Registry.RegisterEventListener ("Document:LoadComplete");
				proxy.LoadComplete += GetDelegate (value);
			}
			remove {
				proxy.LoadComplete -= GetDelegate (value);
				Registry.DeregisterEventListener ("Document:LoadComplete");
			}
		}

		public event EventSimple Reload {
			add {
				Registry.RegisterEventListener ("Document:Reload");
				proxy.Reload += GetDelegate (value);
			}
			remove {
				proxy.Reload -= GetDelegate (value);
				Registry.DeregisterEventListener ("Document:Reload");
			}
		}

		public event EventSimple LoadStopped {
			add {
				Registry.RegisterEventListener ("Document:LoadStopped");
				proxy.LoadStopped += GetDelegate (value);
			}
			remove {
				proxy.LoadStopped -= GetDelegate (value);
				Registry.DeregisterEventListener ("Document:LoadStopped");
			}
		}

		public event EventSimple ContentChanged {
			add {
				Registry.RegisterEventListener ("Document:ContentChanged");
				proxy.ContentChanged += GetDelegate (value);
			}
			remove {
				proxy.ContentChanged -= GetDelegate (value);
				Registry.DeregisterEventListener ("Document:ContentChanged");
			}
		}

		public event EventSimple AttributesChanged {
			add {
				Registry.RegisterEventListener ("Document:AttributesChanged");
				proxy.AttributesChanged += GetDelegate (value);
			}
			remove {
				proxy.AttributesChanged -= GetDelegate (value);
				Registry.DeregisterEventListener ("Document:AttributesChanged");
			}
		}
	}

	[Interface ("org.a11y.atspi.Event.Document")]
	internal interface IEventDocument
	{
		event AtspiEventHandler LoadComplete;
		event AtspiEventHandler Reload;
		event AtspiEventHandler LoadStopped;
		event AtspiEventHandler ContentChanged;
		event AtspiEventHandler AttributesChanged;
	}
}
