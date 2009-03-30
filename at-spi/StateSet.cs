// Copyright 2009 Novell, Inc.
// This software is made available under the MIT License
// See COPYING for details

using System;
using System.Collections.Generic;
using NDesk.DBus;
using org.freedesktop.DBus;

namespace org.freedesktop.atspi
{
	public class StateSet
	{
		int [] states;

		public StateSet (int [] states)
		{
			this.states = states;
		}

		public bool ContainsState (StateType state)
		{
			int n = (int)state;
			if (n < 0 || n >= (int)StateType.LastDefined)
				throw new ArgumentOutOfRangeException ();
			return (states [n / 32] & (1 << (n % 32))) != 0? true: false;
		}

		public void AddState (StateType state)
		{
			int n = (int)state;
			if (n < 0 || n >= (int)StateType.LastDefined)
				throw new ArgumentOutOfRangeException ();
			states [n / 32] |= (1 << (n % 32));
		}
	}
}
