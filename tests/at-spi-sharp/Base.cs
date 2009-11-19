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
using NUnit.Framework;
using Atspi;

namespace AtSpiTest
{
	public abstract class Base
	{
	
		#region Protected Fields
		
		protected Registry registry;
		
		#endregion
		
		#region Private Fields
		
		private static System.Diagnostics.Process p = null;

		#endregion
		
		#region Setup/Teardown

		public virtual void StartApplication (string name)
		{
			if (p != null) {
				Console.WriteLine ("WARNING: StartApplication called when we already have a process: killing and moving on");
				TearDown ();
			}
			Registry.Initialize (true);
			p = new System.Diagnostics.Process ();
			// TODO: It would be better to look at the first line
			// of the file, rather than infer from the name.
			// How to do this?
			p.StartInfo.FileName = (name.Contains (".exe")? "mono": "python");
			p.StartInfo.Arguments = name;
			p.StartInfo.UseShellExecute = false;
			p.StartInfo.CreateNoWindow = true;
			p.Start ();
		}
		
		[TestFixtureTearDown]
		public virtual void TearDown ()
		{
			if (p != null && !p.HasExited)
				p.Kill ();
			p = null;
			Registry.Terminate ();
			// give the registry daemon time to process the signal
			System.Threading.Thread.Sleep (100);
		}

#endregion

		#region Private Methods

		public static string GetAppPath (string filename)
		{
			return "../../apps/" + filename;
		}
#endregion

		#region Protected Helper Methods
		
		protected Accessible FindApplication (string name)
		{
			return Desktop.Instance.FindDescendant (FindApplicationPredicate, name);
		}

		protected bool FindApplicationPredicate (Accessible a, object [] args)
		{
			return (a.Name == (string)args[0] &&
				a.Role == Role.Application &&
				a.Children.Count > 0);
		}

		protected bool CheckName (Accessible a, object [] args)
		{
			return (a.Name == (string)args[0]);
		}

		protected Accessible GetApplication (string filename)
		{
			return GetApplication (filename, filename);
		}

		protected Accessible GetApplication (string filename, string name)
		{
			if (filename != null)
				StartApplication (GetAppPath (filename));
			else
				Registry.Initialize (true);
			for (;;) {
				Accessible a = FindApplication (name);
				if (a != null)
					return a;
				Iterate ();
			}
		}

		protected Accessible GetFrame (string filename)
		{
			return GetFrame (filename, filename);
		}

		protected Accessible GetFrame (string filename, string name)
		{
			Accessible app = GetApplication (filename, name);
			if (app == null)
				return null;
			Accessible frame = app.Children [0];
			Assert.AreEqual (Role.Frame, frame.Role, "First child should be a window");
			return frame;
		}

		protected Accessible FindByRole (Accessible accessible, Role role)
		{
			return FindByRole (accessible, role, false);
		}

		protected Accessible FindByRole (Accessible accessible, Role role, bool wait)
		{
			return Find (accessible, role, null, wait);
		}

		protected Accessible Find (Accessible accessible, Role role, string name, bool wait)
		{
			for (;;) {
				Accessible ret = accessible.FindDescendant (Check, role, name);
				if (ret != null)
					return ret;
				if (!wait)
					return null;
				Iterate ();
			}
		}

		protected bool Check (Accessible accessible, object [] args)
		{
			return (accessible.Role == (Role)args[0] &&
				(args [1] == null || accessible.Name == (string)args [1]));
		}

		protected void Iterate ()
		{
			System.Threading.Thread.Sleep (100);
		}
		
		public static void States (Accessible accessible, params StateType [] expected)
		{
			List <StateType> expectedStates = new List <StateType> (expected);
			List <StateType> missingStates = new List <StateType> ();
			List <StateType> superfluousStates = new List <StateType> ();
			
			StateSet stateSet = accessible.StateSet;
			foreach (StateType state in Enum.GetValues (typeof (StateType))) {
				if (expectedStates.Contains (state) && 
				    (!(stateSet.Contains (state))))
					missingStates.Add (state);
				else if ((!expectedStates.Contains (state)) && 
					     (stateSet.Contains (state)))
					superfluousStates.Add (state);
			}

			string missingStatesMsg = string.Empty;
			string superfluousStatesMsg = string.Empty;

			if (missingStates.Count != 0) {
				missingStatesMsg = "Missing states: ";
				foreach (StateType state in missingStates)
					missingStatesMsg += state.ToString () + ",";
			}
			if (superfluousStates.Count != 0) {
				superfluousStatesMsg = "Superfluous states: ";
				foreach (StateType state in superfluousStates)
					superfluousStatesMsg += state.ToString () + ",";
			}
			Assert.IsTrue ((missingStates.Count == 0) && (superfluousStates.Count == 0),
				missingStatesMsg + " .. " + superfluousStatesMsg);
		}
		#endregion

		#region Abstract Members
	
		#endregion
	}
	
}
