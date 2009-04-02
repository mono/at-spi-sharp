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
	public class StateSet
	{
		private ulong states;

		public StateSet (int [] states)
		{
			if (states.Length != 2)
				throw new ArgumentException ("Expecting int [2]");
			this.states = (ulong)(states [1] << ((sizeof (int) * 8) | states [0]));
		}

		public bool ContainsState (StateType state)
		{
			int n = (int)state;
			if (n < 0 || n >= (int)StateType.LastDefined)
				throw new ArgumentOutOfRangeException ();
			return (states & ((ulong)1 << n)) != 0? true: false;
		}

		public void AddState (StateType state)
		{
			int n = (int)state;
			if (n < 0 || n >= (int)StateType.LastDefined)
				throw new ArgumentOutOfRangeException ();
			states |= ((ulong)1 << n);
		}
	}
}
