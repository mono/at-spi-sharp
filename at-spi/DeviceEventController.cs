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
	public class DeviceEventController
	{
		private volatile static DeviceEventController instance;
		private static object sync = new Object ();
		private IDeviceEventController proxy;

		public static DeviceEventController Instance {
			get { return instance; }
		}

		public DeviceEventController ()
		{
			lock (sync) {
				if (instance == null)
					instance = this;
				else
					throw new Exception ("Attempt to create a second device event controller");
			}

			ObjectPath op = new ObjectPath ("/org/a11y/atspi/registry/deviceeventcontroller");
			proxy = Registry.Bus.GetObject<IDeviceEventController> ("org.a11y.atspi.Registry", op);
		}

		public static void Terminate ()
		{
			lock (sync) {
				instance = null;
			}
		}

		public bool Register (KeystrokeListener listener)
		{
			ObjectPath op = new ObjectPath (listener.Path);
			EventListenerMode mode = new EventListenerMode (listener.Synchronous, listener.Preemptive, listener.Global);
			return proxy.RegisterKeystrokeListener (op, listener.Keys, listener.Mask, listener.Types, mode);
		}

		public bool Register (MouseListener listener)
		{
			ObjectPath op = new ObjectPath (listener.Path);
			return proxy.RegisterDeviceEventListener (op, listener.Types);
		}

		public void Deregister (KeystrokeListener listener)
		{
			ObjectPath op = new ObjectPath (listener.Path);
			proxy.DeregisterKeystrokeListener (op, listener.Keys, listener.Mask, listener.Types);
		}

		public void Deregister (MouseListener listener)
		{
			ObjectPath op = new ObjectPath (listener.Path);
			proxy.DeregisterDeviceEventListener (op, listener.Types);
		}

		public bool NotifyListenersSync (DeviceEvent ev)
		{
			return proxy.NotifyListenersSync (ev);
		}

		public void NotifyListenersAsync (DeviceEvent ev)
		{
			proxy.NotifyListenersAsync (ev);
		}

		public void GenerateKeyboardEvent (int keycode, string keystring, KeySynthType type)
		{
			proxy.GenerateKeyboardEvent (keycode, keystring, type);
		}

		public void GenerateMouseEvent (int x, int y, string eventName)
		{
			proxy.GenerateMouseEvent (x, y, eventName);
		}
	}

	public abstract class DeviceEventListener : IDeviceEventListener
	{
		private static int lastId = 0;
		private static Dictionary<int, DeviceEventListener> mappings = new Dictionary<int, DeviceEventListener> ();

		private int id;

		public DeviceEventListener () : base ()
		{
			DeviceEventListener l;
			while (mappings.TryGetValue (++lastId, out l));

			id = lastId;
			Registry.Bus.Register (new ObjectPath (Path), this);
			mappings [id] = this;
		}

		internal string Path {
			get { return "/org/a11y/atspi/Listeners/" + id; }
		}

		public abstract bool NotifyEvent (DeviceEvent ev);
	}

	public abstract class KeystrokeListener: DeviceEventListener
	{
		private KeyDefinition [] keys;
		private ControllerEventMask mask;
		private EventType types;
		private bool synchronous;
		private bool preemptive;
		private bool global;
		private bool registered;

		public bool Register (KeyDefinition [] keys, ControllerEventMask mask, EventType types, bool synchronous, bool preemptive, bool global)
		{
			Deregister ();
			this.keys = keys;
			this.mask = mask;
			this.types = types;
			this.synchronous = synchronous;
			this.preemptive = preemptive;
			this.global = global;

			registered = DeviceEventController.Instance.Register (this);
			return registered;
		}

		public void Deregister ()
		{
			if (!registered)
				return;
			DeviceEventController.Instance.Deregister (this);
		}

		public KeyDefinition [] Keys {
			get { return keys; }
		}

		public ControllerEventMask Mask {
			get { return mask; }
		}

		public EventType Types {
			get { return types; }
		}

		public bool Synchronous {
			get { return synchronous; }
		}

		public bool Preemptive {
			get { return preemptive; }
		}

		public bool Global {
			get { return global; }
		}

		public bool Registered {
			get { return registered; }
		}
	}

	public abstract class MouseListener: DeviceEventListener
	{
		private EventType types;
		private bool registered;

		public bool Register (EventType types)
		{
			Deregister ();
		this.types = types;

			registered = DeviceEventController.Instance.Register (this);
			return registered;
		}

		public void Deregister ()
		{
			if (!registered)
				return;
			DeviceEventController.Instance.Deregister (this);
		}

		public EventType Types {
			get { return types; }
		}

		public bool Registered {
			get { return registered; }
		}
	}

	public struct EventListenerMode
	{
		public bool Synchronous;
		public bool Preemptive;
		public bool Global;

		public EventListenerMode (bool synchronous, bool preemptive, bool global)
		{
			this.Synchronous = synchronous;
			this.Preemptive = preemptive;
			this.Global = global;
		}
	}

	public struct KeyDefinition
	{
		public int keycode;
		public int keysym;
		public string keystring;
		public int unused;

		// An empty key set is a special case; means all keys
		public static KeyDefinition [] All = new KeyDefinition [0];
	}

	[System.Flags]
	public enum ControllerEventMask : uint
	{
		Alt = 0x0008,
		Mod1 = 0x0008,
		Mod2 = 0x0010,
		Mod3 = 0x0020,
		Mod4 = 0x0040,
		Mod5 = 0x0080,
		Button1 = 0x0100,
		Button2 = 0x0200,
		Button3 = 0x0400,
		Button4 = 0x0800,
		Button5 = 0x0100,
		Control = 0x0004,
		Shift = 0x0001,
		ShiftLock = 0x0002,
		NumLock = 0x4000,
		Unmodified = 0
	}

	[System.Flags]
	public enum EventType : uint
	{
		All = 0,
		KeyPressed = 0x01,
		KeyReleased = 0x02,
		ButtonPressed = 0x04,
		ButtonReleased = 0x08
	}

	[System.Flags]
	public enum KeyModifier : short
	{
		Shift = 1,
		ShiftLock = 2,
		Control = 4,
		Alt = 8,
		Meta = 16,
		Meta2 = 32,
		Meta3 = 64
	}

	public struct DeviceEvent
	{
		public uint Type;
		public int Id;
		public short HwCode;
		public KeyModifier Modifiers;
		public int Timestamp;
		public string EventString;
		public bool IsText;
	}

	public enum KeySynthType : uint
	{
		KeyPress,
		KeyRelease,
		KeyPressRelease,
		KeySym,
		KeyString
	}

	[Interface ("org.a11y.atspi.DeviceEventController")]
	interface IDeviceEventController : Introspectable
	{
		bool RegisterKeystrokeListener (ObjectPath listener, KeyDefinition [] keys, ControllerEventMask mask, EventType types, EventListenerMode mode);
		void DeregisterKeystrokeListener (ObjectPath listener, KeyDefinition [] keys, ControllerEventMask mask, EventType types);
		bool RegisterDeviceEventListener (ObjectPath listener, EventType types);
		void DeregisterDeviceEventListener (ObjectPath listener, EventType types);
		bool NotifyListenersSync (DeviceEvent ev);
		void NotifyListenersAsync (DeviceEvent ev);
		void GenerateKeyboardEvent (int keycode, string keystring, KeySynthType type);
		void GenerateMouseEvent (int x, int y, string eventName);
	}

	[Interface ("org.a11y.atspi.DeviceEventListener")]
	interface IDeviceEventListener
	{
		bool NotifyEvent (DeviceEvent ev);
	}
}
