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
//      Mario Carrion <mcarrion@novell.com>
// 


using System;
using System.Reflection;
using System.Threading;
using Mono.Unix;
using NDesk.DBus;
using NUnit.Framework;
using org.freedesktop.atspi;

namespace AtSpiTest
{
	public abstract class Base
	{
	
		#region Protected Fields
		
		protected Registry registry;
		
		#endregion
		
		#region Private Fields
		
		private static System.Diagnostics.Process p = null;
		private bool threadDone = false;

		#endregion
		
		#region Setup/Teardown

		public virtual void StartApplication (string name)
		{			
			if (p != null)
				return;
			Registry.Initialize (true);
			p = new System.Diagnostics.Process ();
			p.StartInfo.FileName = "python";
			p.StartInfo.Arguments = name;
			p.StartInfo.UseShellExecute = false;
			p.StartInfo.CreateNoWindow = true;
			p.Start ();
		}
		
		[TestFixtureTearDown]
		public virtual void TearDown ()
		{
Console.WriteLine ("dbg: Tearing down.  Don't shed too many tears.");
			p.Kill ();
			Registry.Terminate ();
		}

		protected Accessible FindApplication (string name)
		{
			return Desktop.Instance.FindDescendant (new FindPredicate (CheckName), name);
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
				StartApplication ("../apps/" + filename);
			else
				Registry.Initialize (true);
			do {
				Accessible a = FindApplication (name);
				if (a != null)
					return a;
			} while (Iterate ());
			throw new Exception ("Couldn't find application: " + name);
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
			return FindByRole (accessible, role, 100);
		}

		protected Accessible FindByRole (Accessible accessible, Role role, int time)
		{
			for (;;) {
				Accessible ret = Desktop.Instance.FindDescendant (new FindPredicate (CheckRole), role);
				if (ret != null)
					return ret;
				if (time > 0 && Iterate (time))
					continue;
				else
			return null;
			}
		}

		protected bool CheckRole (Accessible accessible, object [] args)
		{
Console.WriteLine ("dbg: CheckRole: " + accessible.Role);
			return (accessible.Role == (Role)args[0]);
		}

		protected bool Iterate ()
		{
			return Iterate (500);
		}

		protected bool Iterate (int time)
		{
			// TODO: see if there's a more elegant way to do this
			Thread iterateThread = new Thread (new ThreadStart (IterateThread));
			threadDone = false;
			iterateThread.Start ();
			int tries = 0;
			while (!threadDone && tries < (time / 100)) {
				tries++;
				System.Threading.Thread.Sleep (100);
			}
			if (!threadDone)
				iterateThread.Abort ();
			return threadDone;
		}

		void IterateThread ()
		{
			Registry.Bus.Iterate ();
			threadDone = true;
		}
		#endregion
		
		#region Basic Tests
		
		#endregion

		#region Abstract Members
	
		#endregion
	
		#region Protected Helper Methods
		
		#endregion

	}
	
}
