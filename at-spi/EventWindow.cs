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
	public class EventWindow : EventBase
	{
		private IEventWindow proxy;

		public EventWindow (Accessible accessible) : base (accessible)
		{
			proxy = Registry.Bus.GetObject<IEventWindow> (accessible.Application.Name, new ObjectPath (accessible.path));
		}

		public event EventSimple PropertyChange {
			add {
				proxy.PropertyChange += GetDelegate (value);
			}
			remove {
				proxy.PropertyChange -= GetDelegate (value);
			}
		}

		public event EventSimple Minimize {
			add {
				proxy.Minimize += GetDelegate (value);
			}
			remove {
				proxy.Minimize -= GetDelegate (value);
			}
		}

		public event EventSimple Maximize {
			add {
				proxy.Maximize += GetDelegate (value);
			}
			remove {
				proxy.Maximize -= GetDelegate (value);
			}
		}

		public event EventSimple Restore {
			add {
				proxy.Restore += GetDelegate (value);
			}
			remove {
				proxy.Restore -= GetDelegate (value);
			}
		}

		public event EventSimple Close {
			add {
				proxy.Close += GetDelegate (value);
			}
			remove {
				proxy.Close -= GetDelegate (value);
			}
		}

		public event EventSimple Create {
			add {
				proxy.Create += GetDelegate (value);
			}
			remove {
				proxy.Create -= GetDelegate (value);
			}
		}

		public event EventSimple Reparent {
			add {
				proxy.Reparent += GetDelegate (value);
			}
			remove {
				proxy.Reparent -= GetDelegate (value);
			}
		}

		public event EventSimple DesktopCreate {
			add {
				proxy.DesktopCreate += GetDelegate (value);
			}
			remove {
				proxy.DesktopCreate -= GetDelegate (value);
			}
		}

		public event EventSimple DesktopDestroy {
			add {
				proxy.DesktopDestroy += GetDelegate (value);
			}
			remove {
				proxy.DesktopDestroy -= GetDelegate (value);
			}
		}

		public event EventSimple Destroy {
			add {
				proxy.Destroy += GetDelegate (value);
			}
			remove {
				proxy.Destroy -= GetDelegate (value);
			}
		}

		public event EventSimple Activate {
			add {
				proxy.Activate += GetDelegate (value);
			}
			remove {
				proxy.Activate -= GetDelegate (value);
			}
		}

		public event EventSimple Deactivate {
			add {
				proxy.Deactivate += GetDelegate (value);
			}
			remove {
				proxy.Deactivate -= GetDelegate (value);
			}
		}

		public event EventSimple Raise {
			add {
				proxy.Raise += GetDelegate (value);
			}
			remove {
				proxy.Raise -= GetDelegate (value);
			}
		}

		public event EventSimple Lower {
			add {
				proxy.Lower += GetDelegate (value);
			}
			remove {
				proxy.Lower -= GetDelegate (value);
			}
		}

		public event EventSimple Move {
			add {
				proxy.Move += GetDelegate (value);
			}
			remove {
				proxy.Move -= GetDelegate (value);
			}
		}

		public event EventSimple Resize {
			add {
				proxy.Resize += GetDelegate (value);
			}
			remove {
				proxy.Resize -= GetDelegate (value);
			}
		}

		public event EventSimple Shade {
			add {
				proxy.Shade += GetDelegate (value);
			}
			remove {
				proxy.Shade -= GetDelegate (value);
			}
		}

		public event EventSimple Unshade {
			add {
				proxy.Unshade += GetDelegate (value);
			}
			remove {
				proxy.Unshade -= GetDelegate (value);
			}
		}

		public event EventSimple Restyle {
			add {
				proxy.Restyle += GetDelegate (value);
			}
			remove {
				proxy.Restyle -= GetDelegate (value);
			}
		}
	}

	[Interface ("org.freedesktop.atspi.Event.Window")]
	internal interface IEventWindow
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
}
