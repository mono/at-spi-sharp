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
	public class EventTerminal : EventBase
	{
		private IEventTerminal proxy;

		public EventTerminal (Accessible accessible) : base (accessible)
		{
			proxy = Registry.Bus.GetObject<IEventTerminal> (accessible.Application.Name, new ObjectPath (accessible.path));
		}

		public event EventSimple LineChanged {
			add {
				proxy.LineChanged += GetDelegate (value);
			}
			remove {
				proxy.LineChanged -= GetDelegate (value);
			}
		}

		public event EventSimple ColumncountChanged {
			add {
				proxy.ColumncountChanged += GetDelegate (value);
			}
			remove {
				proxy.ColumncountChanged -= GetDelegate (value);
			}
		}

		public event EventSimple LinecountChanged {
			add {
				proxy.LinecountChanged += GetDelegate (value);
			}
			remove {
				proxy.LinecountChanged -= GetDelegate (value);
			}
		}

		public event EventSimple ApplicationChanged {
			add {
				proxy.ApplicationChanged += GetDelegate (value);
			}
			remove {
				proxy.ApplicationChanged -= GetDelegate (value);
			}
		}

		public event EventSimple CharwidthChanged {
			add {
				proxy.CharwidthChanged += GetDelegate (value);
			}
			remove {
				proxy.CharwidthChanged -= GetDelegate (value);
			}
		}
	}

	[Interface ("org.a11y.atspi.Event.Terminal")]
	internal interface IEventTerminal
	{
		event AtspiEventHandler LineChanged;
		event AtspiEventHandler ColumncountChanged;
		event AtspiEventHandler LinecountChanged;
		event AtspiEventHandler ApplicationChanged;
		event AtspiEventHandler CharwidthChanged;
	}
}
