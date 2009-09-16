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
	public class Hypertext
	{
		private Accessible accessible;
		private IHypertext proxy;

		private const string IFACE = "org.freedesktop.atspi.Hypertext";

		public Hypertext (Accessible accessible)
		{
			this.accessible = accessible;
			ObjectPath op = new ObjectPath (accessible.path);
			proxy = Registry.Bus.GetObject<IHypertext> (accessible.application.name, op);
		}

		public int NLinks {
			get {
				return proxy.getNLinks ();
			}
		}

		public Hyperlink GetLink (int linkIndex)
		{
			string path = proxy.getLink (linkIndex);
			if (path == "/org/freedesktop/atspi/accessible/null")
				return null;
			return new Hyperlink (accessible, path);
		}

		public int GetLinkIndex (int characterIndex)
		{
			return proxy.getLinkIndex (characterIndex);
		}
	}

	[Interface ("org.freedesktop.atspi.Hypertext")]
	interface IHypertext : Introspectable
	{
		int getNLinks ();
		string getLink (int linkIndex);
		int getLinkIndex (int characterIndex);
	}
}
