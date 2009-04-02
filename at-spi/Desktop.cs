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
using System.Threading;
using NDesk.DBus;
using org.freedesktop.DBus;

namespace Atspi
{
	public class Desktop : Accessible
	{
		private volatile static Desktop instance;
		private static object sync = new Object ();

		public static Desktop Instance {
			get { return instance; }
		}

		internal Desktop (): base (null, null)
		{
			lock (sync) {
				if (instance == null)
					instance = this;
				else
					throw new Exception ("Attempt to create a second desktop");
			}
		}

		internal void Add (Application app)
		{
			children.Add (app.GetRoot ());
		}

		internal void Remove (Application app)
		{
			foreach (Accessible accessible in children) {
				if (accessible.application == app) {
					children.Remove (accessible);
					return;
				}
			}
		}

		public static void Terminate ()
		{
			lock (sync) {
				instance = null;
			}
		}
	}
}
