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
	public class EventBase
	{
		private static Dictionary<object, AtspiEventHandler> delegates;

		protected Accessible accessible;

		static EventBase ()
		{
			delegates = new Dictionary<object, AtspiEventHandler> ();
		}

		public EventBase (Accessible accessible)
		{
			this.accessible = accessible;
		}

		internal AtspiEventHandler GetDelegate (EventI value)
		// TODO: Remove unused delegates
		{
			if (!delegates.ContainsKey (value))
				delegates [value] = (detail, v1, v2, any, app_root) => value (accessible, v1);
			return delegates [value];
		}

		internal AtspiEventHandler GetDelegate (EventII value)
		{
			if (!delegates.ContainsKey (value))
				delegates [value] = (detail, v1, v2, any, app_root) => value (accessible, v1, v2);
			return delegates [value];
		}

		internal AtspiEventHandler GetDelegate (EventO value)
		{
			if (!delegates.ContainsKey (value))
				delegates [value] = (detail, v1, v2, any, app_root) => value (accessible, MarshallAccessible (any));
			return delegates [value];
		}

		internal AtspiEventHandler GetDelegate (EventR value)
		{
			if (!delegates.ContainsKey (value))
				delegates [value] = (detail, v1, v2, any, app_root) => value (accessible, (BoundingBox)Convert.ChangeType (any, typeof(BoundingBox)));
			return delegates [value];
		}

		internal AtspiEventHandler GetDelegate (EventSB value)
		{
			if (!delegates.ContainsKey (value))
				delegates [value] = (detail, v1, v2, any, app_root) => value (accessible, detail, v1 == 1);
			return delegates [value];
		}

		internal AtspiEventHandler GetDelegate (EventSII value)
		{
			if (!delegates.ContainsKey (value))
				delegates [value] = (detail, v1, v2, any, app_root) => value (accessible, detail, v1, v2);
			return delegates [value];
		}

		internal AtspiEventHandler GetDelegate (EventSIIS value)
		{
			if (!delegates.ContainsKey (value))
				delegates [value] = (detail, v1, v2, any, app_root) => value (accessible, detail, v1, v2, (string)any);
			return delegates [value];
		}

		internal AtspiEventHandler GetDelegate (EventSIO value)
		{
			if (!delegates.ContainsKey (value))
				delegates [value] = (detail, v1, v2, any, app_root) => value (accessible, detail, v1, MarshallAccessible (any, detail == "remove", true));
			return delegates [value];
		}

		internal AtspiEventHandler GetDelegate (EventSV value)
		{
			if (!delegates.ContainsKey (value))
				delegates [value] = (detail, v1, v2, any, app_root) => value (accessible, detail, MarshallVariant (any));
			return delegates [value];
		}

		internal AtspiEventHandler GetDelegate (EventSimple value)
		{
			if (!delegates.ContainsKey (value))
				delegates [value] = (detail, v1, v2, any, app_root) => value (accessible);
			return delegates [value];
		}

		private static object MarshallVariant (object any)
		{
			Accessible obj = MarshallAccessible (any, false, false);
			if (obj != null)
				return obj;
			return any;
		}

		private static Accessible MarshallAccessible (object any)
		{
			return MarshallAccessible (any, false, true);
		}

		private static Accessible MarshallAccessible (object any, bool remove, bool warn)
		{
			AccessiblePath path;
			try {
				path = (AccessiblePath)Convert.ChangeType (any, typeof(AccessiblePath));
			} catch (System.InvalidCastException) {
				if (warn)
					Console.WriteLine ("at-spi-sharp: Warning: Could not convert object to AccessiblePath");
				return null;
			}
			if (path.path.ToString () == Application.SPI_PATH_NULL && remove)
				path.path = new ObjectPath (Application.SPI_PATH_ROOT);
			if (path.path.ToString () == Application.SPI_PATH_NULL)
				return null;
			return Registry.GetElement (path, !remove);
		}
	}

	internal delegate void AtspiEventHandler (string detail, int v1, int v2, object any, AccessiblePath app_root);

	public delegate void EventI (Accessible sender, int v);
	public delegate void EventII (Accessible sender, int v1, int v2);
	public delegate void EventO (Accessible sender, Accessible o);
	public delegate void EventR (Accessible sender, BoundingBox v);
	public delegate void EventSB (Accessible sender, string s, bool b);
	public delegate void EventSII (Accessible sender, string s1, int v1, int v2);
	public delegate void EventSIIS (Accessible sender, string s1, int v1, int v2, string s2);
	public delegate void EventSIO (Accessible sender, string s, int i, Accessible a);
	public delegate void EventSV (Accessible sender, string s1, object any);
	public delegate void EventSimple (Accessible sender);
}
