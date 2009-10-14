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
using System.Collections;
using System.Collections.Generic;
using NDesk.DBus;
using org.freedesktop.DBus;

namespace Atspi
{
	public class UncachedChildren : IList<Accessible>
	{
		internal Accessible accessible;

		public UncachedChildren (Accessible accessible)
		{
			this.accessible = accessible;
		}

		public int Count {
			get {
				return accessible.ChildCountNoCache;
			}
		}

		public bool IsReadOnly {
			get {
				return true;
			}
		}

		public void Add (Accessible item)
		{
			throw new NotSupportedException ();
		}

		public void Clear ()
		{
			throw new NotSupportedException ();
		}

		public bool Contains (Accessible item)
		{
			return (accessible.Parent == accessible);
		}

		public void CopyTo (Accessible [] array, int arrayIndex)
		{
			int count = Count;
			for (int i = 0; i < count; i++)
				array [arrayIndex + i] = this [i];
		}

		public bool Remove (Accessible item)
		{
			return false;
		}

		public int IndexOf (Accessible item)
		{
			return item.IndexInParentNoCache;
		}

		public void Insert (int index, Accessible item)
		{
			throw new NotSupportedException ();
		}

		public void RemoveAt (int index)
		{
			throw new NotSupportedException ();
		}

		public Accessible this [int index] {
			get {
				return accessible.GetChildAtIndexNoCache (index);
			}
			set {
				throw new NotSupportedException ();
			}
		}

		IEnumerator IEnumerable.GetEnumerator ()
		{
			return new Enumerator (this);
		}

		IEnumerator<Accessible> IEnumerable<Accessible>.GetEnumerator ()
		{
			return new Enumerator (this);
		}

		public struct Enumerator : IEnumerator <Accessible>
		{
			private UncachedChildren list;
			private Accessible current;
			private bool started;

			public Enumerator (UncachedChildren list)
			{
				this.list = list;
				this.started = false;
				this.current = null;
			}

			public bool MoveNext ()
			{
				// TODO: Have a "get next" in at-spi?
				// This is not safe; the collection could be
				// modified in between our calls
				if (!started) {
					current = list.accessible.Children [0];
					if (current != null)
						started = true;
					return started;
				}
				if (current == null)
					return false;
				int index = current.IndexInParent;
				if (index < 0)
					return false;
				current = current.Parent.Children [index + 1];
				return (current != null);
			}

			public void Reset ()
			{
				started = false;
				current = null;
			}

			public void Dispose ()
			{
				((IEnumerator)this).Reset ();
			}

			Accessible IEnumerator<Accessible>.Current {
				get {
					return current;
				}
			}

			object IEnumerator.Current {
				get {
					return current;
				}
			}
		}
	}
}
