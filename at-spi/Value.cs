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
	public class Value
	{
		private Properties properties;

		private const string IFACE = "org.a11y.atspi.Value";

		public Value (Accessible accessible)
		{
			ObjectPath op = new ObjectPath (accessible.path);
			properties = Registry.Bus.GetObject<Properties> (accessible.application.name, op);
		}

		public double MinimumValue {
			get {
				return (double) properties.Get (IFACE, "MinimumValue");
			}
		}

		public double MaximumValue {
			get {
				return (double) properties.Get (IFACE, "MaximumValue");
			}
		}

		public double CurrentValue {
			get {
				return (double) properties.Get (IFACE, "CurrentValue");
			}
			set {
				properties.Set (IFACE, "CurrentValue", value);
			}
		}

		public double MinimumIncrement {
			get {
				return (double) properties.Get (IFACE, "MinimumIncrement");
			}
		}
	}
}
