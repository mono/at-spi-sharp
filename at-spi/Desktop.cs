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

		public delegate void StructureEventHandler (Accessible sender, Accessible child);
		public delegate void PropertyChangedEventHandler<T> (object sender, T oldValue,  T newValue);
		public delegate void StateChangedEventHandler (Accessible sender, StateType state, bool set);

		public static event StructureEventHandler OnChildAdded;
		public static event StructureEventHandler OnChildRemoved;
		public static event PropertyChangedEventHandler<string> OnNameChanged;
		public static event PropertyChangedEventHandler<string> OnDescriptionChanged;
		public static event PropertyChangedEventHandler<Role> OnRoleChanged;
		public static event StateChangedEventHandler OnStateChanged;

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
			role = Role.DesktopFrame;
		}

		internal void Add (Application app)
		{
			lock (sync) {
				children.Add (app.GetRoot ());
			}
		}

		internal void Remove (Application app)
		{
			lock (sync) {
				foreach (Accessible accessible in children) {
					if (accessible.application == app) {
						children.Remove (accessible);
						return;
					}
				}
			}
		}

		public static void Terminate ()
		{
			lock (sync) {
				instance = null;
			}
		}

		internal static void RaiseChildAdded (Accessible sender, Accessible child)
		{
			if (OnChildAdded != null)
				OnChildAdded (sender, child);
		}

		internal static void RaiseChildRemoved (Accessible sender, Accessible child)
		{
			if (OnChildRemoved != null)
				OnChildRemoved (sender, child);
		}

		internal static void RaiseNameChanged (Accessible sender, string oldName, string newName)
		{
			if (OnNameChanged != null)
				OnNameChanged (sender, oldName, newName);
		}

		internal static void RaiseDescriptionChanged (Accessible sender, string oldDescription, string newDescription)
		{
			if (OnDescriptionChanged != null)
				OnDescriptionChanged (sender, oldDescription, newDescription);
		}

		internal static void RaiseRoleChanged (Accessible sender, Role oldRole, Role newRole)
		{
			if (OnRoleChanged != null)
				OnRoleChanged (sender, oldRole, newRole);
		}

		internal static void RaiseStateChanged (Accessible sender, StateType type, bool set)
		{
			if (OnStateChanged != null)
				OnStateChanged (sender, type, set);
		}
	}
}
