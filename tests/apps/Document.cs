using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

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
		private Gtk.Button button;

		public bool AddChild ()
		{
			if (button != null)
				return false;
			button = new Gtk.Button ("button");
			Add (button);
			GLib.Signal.Emit (Accessible,
				"children_changed::add",
				0u,
				Accessible.RefAccessibleChild (0).Handle);
			return true;
		}

		public bool RemoveChild ()
		{
			if (button == null)
				return false;
			GLib.Signal.Emit (Accessible,
				"children_changed::remove",
				(uint)0,
				Accessible.RefAccessibleChild (0).Handle);
			Remove (button);
			button = null;
			return true;
		}
	}

	class TestWindowAccessible: Atk.Object, Atk.DocumentImplementor,
		Atk.TextImplementor, Atk.HypertextImplementor,
		Atk.EditableTextImplementor, Atk.ComponentImplementor,
		Atk.TableImplementor
	{
		private TestWindow window;
		private Dictionary<string, string> attributes;
		public List<Hyperlink> links;
		private string text;

		public TestWindowAccessible (IntPtr raw): base (raw) { }

		public TestWindowAccessible (GLib.Object widget): base()
		{
			this.window = widget as TestWindow;
			text = null;
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

		// hack: Following are incomplete and only used to test events
		public string GetText (int start_offset, int end_offset)
		{
			if (end_offset == -1)
				end_offset = text.Length - start_offset;
			return text.Substring (start_offset, end_offset);
		}

		public string GetTextAfterOffset (int offset, Atk.TextBoundary boundary_type, out int start_offset, out int end_offset)
		{
			start_offset = end_offset = -1;
			return null;
		}

		public string GetTextAtOffset (int offset, Atk.TextBoundary boundary_type, out int start_offset, out int end_offset)
		{
			start_offset = end_offset = -1;
			return null;
		}

		public char GetCharacterAtOffset (int offset)
		{
			return text [offset];
		}

		public string GetTextBeforeOffset (int offset, Atk.TextBoundary boundary_type, out int start_offset, out int end_offset)
		{
			start_offset = end_offset = -1;
			return null;
		}

		public int CaretOffset {
			get {
				return -1;
			}
		}

		public Atk.Attribute[] GetRunAttributes (int offset, out int start_offset, out int end_offset)
		{
			start_offset = end_offset = -1;
			return null;
		}

		public Atk.Attribute[] DefaultAttributes {
			get {
				return null;
			}
		}

		public void GetCharacterExtents (int offset, out int x, out int y, out int width, out int height, Atk.CoordType coords)
		{
			x = y = width = height = -1;
		}

		public int CharacterCount {
			get {
				return text.Length;
			}
		}

		public int GetOffsetAtPoint (int x, int y, Atk.CoordType coords)
		{
			return -1;
		}

		public int NSelections {
			get {
				return 0;
			}
		}

		public string GetSelection (int selection_num, out int start_offset, out int end_offset)
		{
			start_offset = end_offset = -1;
			return null;
		}

		public bool AddSelection (int start_offset, int end_offset)
		{
			return false;
		}

		public bool RemoveSelection (int selection_num)
		{
			return false;
		}

		public bool SetSelection (int selection_num, int start_offset, int end_offset)
		{
			return false;
		}

		public bool SetCaretOffset (int offset)
		{
			return false;
		}

		public void GetRangeExtents (int start_offset, int end_offset, Atk.CoordType coord_type, out Atk.TextRectangle rect)
		{
			rect.X = rect.Y = rect.Height = rect.Width = -1;
		}

		public Atk.TextRange GetBoundedRanges (Atk.TextRectangle rect, Atk.CoordType coord_type, Atk.TextClipType x_clip_type, Atk.TextClipType y_clip_type)
		{
			return new Atk.TextRange ();
		}

		public bool SetRunAttributes (GLib.SList attrib_set, int start_offset, int end_offset)
		{
			return false;
		}

		public string TextContents {
			set {
				text = value;
				string glibSignal = SignalName (text);
				switch (text) {
					case "ActiveDescendantChanged": {
						Atk.Object child = RefAccessibleChild (0);
						GLib.Signal.Emit (this,
							glibSignal,
							child.Handle);
						break;
					}

					case "AddChild":
						window.AddChild ();
						break;

					case "BoundsChanged": {
						Atk.Rectangle bounds = new Atk.Rectangle ();
						bounds.X = 10;
						bounds.Y = 20;
						bounds.Height = 30;
						bounds.Width = 40;
						GLib.Signal.Emit (this,
							glibSignal,
							bounds);
						break;
					}

					case "ColumnDeleted":
						GLib.Signal.Emit (this,
							glibSignal,
							2, 1);
						break;

					case "ColumnInserted":
						GLib.Signal.Emit (this,
							glibSignal,
							3, 2);
						break;

					case "DescriptionChanged":
						window.Accessible.Description = "plugh";
						break;

					case "Focus":
						Atk.Focus.TrackerNotify (this);
						break;

					case "LinkSelected":
						GLib.Signal.Emit (this,
							glibSignal,
							1);
						break;

					case "NameChanged":
						window.Accessible.Name = "xyzzy";
						break;

					case "RemoveChild":
						window.RemoveChild ();
						break;

					case "RoleChanged":
						window.Accessible.Role = Atk.Role.Dialog;
						break;

					case "RowDeleted":
						GLib.Signal.Emit (this,
							glibSignal,
							4, 3);
						break;

					case "RowInserted":
						GLib.Signal.Emit (this,
							glibSignal,
							5, 4);
						break;

					case "StateChanged":
						window.Children [0].Sensitive = false;
						break;

					case "TextCaretMoved":
						GLib.Signal.Emit (this,
							glibSignal,
							6);
						break;

					case "TextChanged":
						GLib.Signal.Emit (this,
							"text-changed::insert",
							0, 11);
						break;

					default:
						try {
							GLib.Signal.Emit (this,
								glibSignal);
						} catch (Exception) {
							Console.WriteLine ("Warning: Attempt to send unregistered event " + text);
						}
						break;
				}
			}
		}

		public void InsertText (string str1ng, ref int position)
		{
		}

		public void CopyText (int start_pos, int end_pos)
		{
		}

		public void CutText (int start_pos, int end_pos)
		{
		}

		public void DeleteText (int start_pos, int end_pos)
		{
		}

		public void PasteText (int position)
		{
		}

		public uint AddFocusHandler (Atk.FocusHandler handler)
		{
			return 0;
		}

		public bool Contains (int x, int y, Atk.CoordType coord_type)
		{
			return false;
		}

		public Atk.Object RefAccessibleAtPoint (int x, int y, Atk.CoordType coord_type)
		{
			return null;
		}

		public void GetExtents (out int x, out int y, out int width, out int height, Atk.CoordType coord_type)
		{
			GetPosition (out x, out y, coord_type);
			GetSize (out width, out height);
		}

		public void GetPosition (out int x, out int y, Atk.CoordType coord_type)
		{
			x = y = -1;
		}

		public void GetSize (out int width, out int height)
		{
			width = height = -1;
		}

		public bool GrabFocus ()
		{
			return false;
		}

		public void RemoveFocusHandler (uint handler_id)
		{
		}

		public bool SetExtents (int x, int y, int width, int height, Atk.CoordType coord_type)
		{
			return SetPosition (x, y, coord_type) &&
				SetSize (width, height);
		}

		public bool SetPosition (int x, int y, Atk.CoordType coord_type)
		{
			return false;
		}

		public bool SetSize (int width, int height)
		{
			return false;
		}

		Atk.Layer Atk.ComponentImplementor.Layer {
			get {
				return Atk.Layer.Widget;
			}
		}

		int Atk.ComponentImplementor.MdiZorder {
			get {
				return 1;
			}
		}

		public double Alpha {
			get {
				return -1;
			}
		}

		public Atk.Object RefAt (int row, int column)
		{
			return null;
		}

		public int GetIndexAt (int row, int column)
		{
			return -1;
		}

		public int GetColumnAtIndex (int index_)
		{
			return -1;
		}

		public int GetRowAtIndex (int index_)
		{
			return -1;
		}

		public int NColumns {
			get {
				return -1;
			}
		}

		public int NRows {
			get {
				return -1;
			}
		}

		public int GetColumnExtentAt (int row, int column)
		{
			return -1;
		}

		public int GetRowExtentAt (int row, int column)
		{
			return -1;
		}

		public Atk.Object Caption {
			get {
				return null;
			}
			set {
			}
		}

		public string GetColumnDescription (int column)
		{
			return string.Empty;
		}

		public Atk.Object GetColumnHeader (int column)
		{
			return null;
		}

		public string GetRowDescription (int row)
		{
			return string.Empty;
		}

		public Atk.Object GetRowHeader (int row)
		{
			return null;
		}

		public Atk.Object Summary {
			get {
				return null;
			}
			set {
			}
		}

		public void SetColumnDescription (int column, string description)
		{
		}

		public void SetColumnHeader (int column, Atk.Object header)
		{
		}

		public void SetRowDescription (int row, string description)
		{
		}

		public void SetRowHeader (int row, Atk.Object header)
		{
		}

		public int GetSelectedColumns (out int selected)
		{
			// TODO: Fix next line when gtk-sharp is fixed
			selected = 0;
			return 0;
		}

		public int GetSelectedRows (out int selected)
		{
			// TODO: Fix next line when gtk-sharp is fixed
			selected = 0;
			return 0;
		}

		public bool IsColumnSelected (int column)
		{
			return false;
		}

		public bool IsRowSelected (int row)
		{
			return false;
		}

		public bool IsSelected (int row, int column)
		{
			return false;
		}

		public bool AddRowSelection (int row)
		{
			return false;
		}

		public bool RemoveRowSelection (int row)
		{
			return false;
		}

		public bool AddColumnSelection (int column)
		{
			return false;
		}

		public bool RemoveColumnSelection (int column)
		{
			return false;
		}

		public string SignalName (string text)
		{
			StringBuilder sb = new StringBuilder (text.Length * 2);
			for (int i = 0; i < text.Length; i++) {
				if (i > 0 && Char.IsUpper (text [i]))
					sb.Append ("-");
				sb.Append (Char.ToLower (text [i]));
			}
			return sb.ToString ();
		}

		protected override int OnGetNChildren ()
		{
			return window.Children.Length;
		}

		protected override Atk.Object OnRefChild (int index)
		{
			if (index < 0 || index >= window.Children.Length)
				return null;
			return window.Children [index].Accessible;
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
		private TestWindow window;

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
			window.AddChild ();
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
