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

		public event EventSimple Minimize {
			add {
				Registry.RegisterEventListener ("Window:Minimize");
				proxy.Minimize += GetDelegate (value);
			}
			remove {
				proxy.Minimize -= GetDelegate (value);
				Registry.DeregisterEventListener ("Window:Minimize");
			}
		}

		public event EventSimple Maximize {
			add {
				Registry.RegisterEventListener ("Window:Maximize");
				proxy.Maximize += GetDelegate (value);
			}
			remove {
				proxy.Maximize -= GetDelegate (value);
				Registry.DeregisterEventListener ("Window:Maximize");
			}
		}

		public event EventSimple Restore {
			add {
				Registry.RegisterEventListener ("Window:Restore");
				proxy.Restore += GetDelegate (value);
			}
			remove {
				proxy.Restore -= GetDelegate (value);
				Registry.DeregisterEventListener ("Window:Restore");
			}
		}

		public event EventSimple Close {
			add {
				Registry.RegisterEventListener ("Window:Close");
				proxy.Close += GetDelegate (value);
			}
			remove {
				proxy.Close -= GetDelegate (value);
				Registry.DeregisterEventListener ("Window:Close");
			}
		}

		public event EventSimple Create {
			add {
				Registry.RegisterEventListener ("Window:Create");
				proxy.Create += GetDelegate (value);
			}
			remove {
				proxy.Create -= GetDelegate (value);
				Registry.DeregisterEventListener ("Window:Create");
			}
		}

		public event EventSimple Reparent {
			add {
				Registry.RegisterEventListener ("Window:Reparent");
				proxy.Reparent += GetDelegate (value);
			}
			remove {
				proxy.Reparent -= GetDelegate (value);
				Registry.DeregisterEventListener ("Window:Reparent");
			}
		}

		public event EventSimple DesktopCreate {
			add {
				Registry.RegisterEventListener ("Window:DesktopCreate");
				proxy.DesktopCreate += GetDelegate (value);
			}
			remove {
				proxy.DesktopCreate -= GetDelegate (value);
				Registry.DeregisterEventListener ("Window:DesktopCreate");
			}
		}

		public event EventSimple DesktopDestroy {
			add {
				Registry.RegisterEventListener ("Window:DesktopDestroy");
				proxy.DesktopDestroy += GetDelegate (value);
			}
			remove {
				proxy.DesktopDestroy -= GetDelegate (value);
				Registry.DeregisterEventListener ("Window:DesktopDestroy");
			}
		}

		public event EventSimple Destroy {
			add {
				Registry.RegisterEventListener ("Window:Destroy");
				proxy.Destroy += GetDelegate (value);
			}
			remove {
				proxy.Destroy -= GetDelegate (value);
				Registry.DeregisterEventListener ("Window:Destroy");
			}
		}

		public event EventSimple Activate {
			add {
				Registry.RegisterEventListener ("Window:Activate");
				proxy.Activate += GetDelegate (value);
			}
			remove {
				proxy.Activate -= GetDelegate (value);
				Registry.DeregisterEventListener ("Window:Activate");
			}
		}

		public event EventSimple Deactivate {
			add {
				Registry.RegisterEventListener ("Window:Deactivate");
				proxy.Deactivate += GetDelegate (value);
			}
			remove {
				proxy.Deactivate -= GetDelegate (value);
				Registry.DeregisterEventListener ("Window:Deactivate");
			}
		}

		public event EventSimple Raise {
			add {
				Registry.RegisterEventListener ("Window:Raise");
				proxy.Raise += GetDelegate (value);
			}
			remove {
				proxy.Raise -= GetDelegate (value);
				Registry.DeregisterEventListener ("Window:Raise");
			}
		}

		public event EventSimple Lower {
			add {
				Registry.RegisterEventListener ("Window:Lower");
				proxy.Lower += GetDelegate (value);
			}
			remove {
				proxy.Lower -= GetDelegate (value);
				Registry.DeregisterEventListener ("Window:Lower");
			}
		}

		public event EventSimple Move {
			add {
				Registry.RegisterEventListener ("Window:Move");
				proxy.Move += GetDelegate (value);
			}
			remove {
				proxy.Move -= GetDelegate (value);
				Registry.DeregisterEventListener ("Window:Move");
			}
		}

		public event EventSimple Resize {
			add {
				Registry.RegisterEventListener ("Window:Resize");
				proxy.Resize += GetDelegate (value);
			}
			remove {
				Registry.DeregisterEventListener ("Window:Resize");
				proxy.Resize -= GetDelegate (value);
			}
		}

		public event EventSimple Shade {
			add {
				Registry.RegisterEventListener ("Window:Shade");
				proxy.Shade += GetDelegate (value);
			}
			remove {
				proxy.Shade -= GetDelegate (value);
				Registry.DeregisterEventListener ("Window:Shade");
			}
		}

		public event EventSimple Unshade {
			add {
				Registry.RegisterEventListener ("Window:Unshade");
				proxy.Unshade += GetDelegate (value);
			}
			remove {
				proxy.Unshade -= GetDelegate (value);
				Registry.DeregisterEventListener ("Window:Unshade");
			}
		}

		public event EventSimple Restyle {
			add {
				Registry.RegisterEventListener ("Window:Restyle");
				proxy.Restyle += GetDelegate (value);
			}
			remove {
				proxy.Restyle -= GetDelegate (value);
				Registry.DeregisterEventListener ("Window:Restyle");
			}
		}
	}

	[Interface ("org.a11y.atspi.Event.Window")]
	internal interface IEventWindow
	{
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
