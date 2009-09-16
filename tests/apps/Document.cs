using System;
using System.Collections.Generic;

using Gtk;

namespace TestDocument
{
	class HyperlinkActor: Atk.Object, Atk.ActionImplementor
	{
		private string description;

		public int NActions {
			get {
				return 1;
			}
		}

		public bool DoAction (int i)
		{
			return true;
		}

		public string GetName (int i)
		{
			return (i == 0? "click": null);
		}

		public string GetKeybinding (int i)
		{
			return null;
		}

		public string GetLocalizedName (int i)
		{
			return GetName (i);
		}

		public bool SetDescription (int i, string desc)
		{
			if (i != 0)
				return false;
			description = desc;
			return true;
		}

		public string GetDescription (int i)
		{
			return (i == 0? description: null);
		}
	}

	class Hyperlink: Atk.Hyperlink
	{
		public int StartOffset;
		public int EndOffset;
		public string Uri;
		private HyperlinkActor actor;

		public Hyperlink (int startOffset, int endOffset, string uri)
		{
			StartOffset = startOffset;
			EndOffset = endOffset;
			Uri = uri;
			actor = new HyperlinkActor ();
		}

		protected override Atk.Object OnGetObject (int i)
		{
			return (i == 0? actor: null);
		}

		protected override int OnGetStartIndex ()
		{
			return StartOffset;
		}

		protected override int OnGetEndIndex ()
		{
			return EndOffset;
		}

		protected override string OnGetUri (int i)
		{
			return (i == 0? Uri: null);
		}
	}

	class TestWindow: Gtk.Window
	{
		public TestWindow (string name) : base (name) { }
	}

	class TestWindowAccessible: Atk.Object, Atk.DocumentImplementor, Atk.HypertextImplementor
	{
		private Dictionary<string, string> attributes;
		public List<Hyperlink> links;

		public TestWindowAccessible (IntPtr raw): base (raw) { }

		public TestWindowAccessible (GLib.Object widget): base()
		{
			Role = Atk.Role.Frame;
			attributes = new Dictionary<string, string> ();
			attributes ["left-margin"] = "2.0";
			links = new List<Hyperlink> ();
			links.Add (new Hyperlink ( 5, 26, "http://www.novell.com"));
			links.Add (new Hyperlink (55, 76, "http://www.google.com"));
		}

		public string Locale {
			get {
				return "en";
			}
		}

		bool Atk.DocumentImplementor.SetAttributeValue (string attributeName, string attributeValue)
		{
			attributes [attributeName] = attributeValue;
			return true;
		}

		public string DocumentType {
			get {
				return "test";
			}
		}

		public IntPtr TheDocument {
			get {
				return IntPtr.Zero;
			}
		}

		Atk.Attribute [] Atk.DocumentImplementor.Attributes {
			get {
				Atk.Attribute [] ret = new Atk.Attribute [attributes.Count];
				int i = 0;
				foreach (string s in attributes.Keys) {
					ret [i] = new Atk.Attribute ();
					ret [i].Name = s;
					ret [i].Value = attributes [s];
					i++;
				}
				return ret;
			}
		}

		public string GetAttributeValue (string name)
		{
			if (attributes.ContainsKey (name))
				return attributes [name];
			return null;
		}

		public Atk.Hyperlink GetLink (int link_index)
		{
			if (link_index >= 0 && link_index < links.Count)
				return links [link_index];
			return null;
		}

		public int NLinks {
			get {
				return links.Count;
			}
		}

		public int GetLinkIndex (int char_index)
		{
			for (int i = 0; i < links.Count; i++)
				if (char_index >= links [i].StartOffset && char_index <= links [i].EndOffset)
					return i;
			return -1;
		}
	}

	class TestWindowAccessibleFactory: Atk.ObjectFactory
	{
		public static void Init ()
		{
			new TestWindowAccessibleFactory ();
			Atk.Global.DefaultRegistry.SetFactoryType ((GLib.GType)typeof(TestWindow), (GLib.GType)typeof (TestWindowAccessibleFactory));
		}

		protected override Atk.Object OnCreateAccessible (GLib.Object obj)
		{
			return new TestWindowAccessible (obj);
		}
		protected override GLib.GType OnGetAccessibleType ()
		{
			return TestWindowAccessible.GType;
		}
	}

	public class TestMain
	{
		private Gtk.Window window;

		public static void Main (string[] args)
		{
			Application.Init ();
			new TestMain ();
			Application.Run ();
		}

		public TestMain ()
		{
			TestWindowAccessibleFactory.Init ();
			window = new TestWindow ("At-spi-sharp test application");
			window.SetDefaultSize (600, 400);
			window.DeleteEvent += new DeleteEventHandler (WindowDelete);

			window.ShowAll ();
		}

		private void WindowDelete (object o, DeleteEventArgs args)
		{
			Application.Quit ();
			args.RetVal = true;
		}
	}
}
