// Copyright 2009 Novell, Inc.
// This software is made available under the MIT License
// See COPYING for details

using System;
using System.Collections.Generic;
using System.Threading;
using NDesk.DBus;
using org.freedesktop.DBus;

namespace org.freedesktop.atspi
{
	public class Desktop : Accessible
	{
			public static Desktop Instance;

		internal Desktop (): base (null, null)
		{
			if (Instance == null)
				Instance = this;
			else
				throw new Exception ("Attempt to create a second desktop");
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
	}
}
