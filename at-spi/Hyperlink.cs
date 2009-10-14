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
	public class Hyperlink
	{
		private IHyperlink proxy;
		private Application application;
		private Properties properties;

		private const string IFACE = "org.freedesktop.atspi.Hyperlink";

		public Hyperlink (Accessible accessible, string path)
		{
			application = accessible.application;
			ObjectPath op = new ObjectPath (path);
			proxy = Registry.Bus.GetObject<IHyperlink> (application.name, op);
			properties = Registry.Bus.GetObject<Properties> (application.name, op);
		}

		public int NAnchors {
			get {
				return (int) properties.Get (IFACE, "NAnchors");
			}
		}

		public int StartIndex {
			get {
				return (int) properties.Get (IFACE, "StartIndex");
			}
		}

		public int EndIndex {
			get {
				return (int) properties.Get (IFACE, "EndIndex");
			}
		}

		public Accessible GetObject (int index)
		{
			ObjectPath path = proxy.GetObject (index);
			Accessible ret = application.GetElement (path, true);
			// hack -- we get an object which we may not have
			// received an event for.
			if (ret != null)
				ret.AddInterface ("org.freedesktop.atspi.Action");
			return ret;
		}

		public string GetURI (int i)
		{
			return proxy.GetURI (i);
		}
	}

	[Interface ("org.freedesktop.atspi.Hyperlink")]
	interface IHyperlink : Introspectable
	{
			ObjectPath GetObject (int index);
			string GetURI (int i);
	}
}
