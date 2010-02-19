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
//	Mike Gorse <mgorse@novell.com>
// 

using System;
using System.Threading;
using NDesk.DBus;
using NUnit.Framework;
using Atspi;

namespace AtSpiTest
{
	[TestFixture]
	public class DeviceEventControllerTest : Base
	{
		Accessible frame = null;

		public DeviceEventControllerTest ()
		{
			// We need a gtk app so that our keystrokes have a
			// place to land where they will be ricocheted
			// back to our listener.
			frame = GetFrame ("gtktextview.py");
			Assert.IsNotNull (frame, "Couldn't open gtktextview.py");
		}

		#region Test
		[Test]
		public void BasicKeyListenerTest ()
		{
			TestKeyListener l = new TestKeyListener ();
			l.Register (KeyDefinition.All, ControllerEventMask.Unmodified, EventType.All, true, true, false);
			DeviceEventController.Instance.GenerateKeyboardEvent (65, "", KeySynthType.KeyPressRelease);
			Thread.Sleep (500);
			Assert.AreEqual (2, l.Count, "Event count");
			Assert.AreEqual (65, l.LastEvent.HwCode, "LastEvent HwCode");
		}
#endregion
	}

	class TestKeyListener: KeystrokeListener
	{
		public DeviceEvent LastEvent;
		public int Count = 0;

		public override bool NotifyEvent (DeviceEvent ev)
		{
			LastEvent = ev;
			Count++;
			return true;
		}
	}
}
