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
			return "../../../apps/" + filename;
		}
#endregion

		#region Protected Helper Methods
		
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
			for (;;) {
				Accessible ret = Desktop.Instance.FindDescendant (new FindPredicate (CheckRole), role);
				if (ret != null)
					return ret;
				if (!wait)
					return null;
				Iterate ();
			}
		}

		protected bool CheckRole (Accessible accessible, object [] args)
		{
			return (accessible.Role == (Role)args[0]);
		}

		protected void Iterate ()
		{
			Registry.Bus.Iterate ();
		}
		#endregion
		
		#region Abstract Members
	
		#endregion
	}
	
}
