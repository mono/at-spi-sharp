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
	public enum RelationType : uint
	{
		Null,
		LabelFor,
		LabelledBy,
		ControllerFor,
		ControlledBy,
		MemberOf,
		TooltipFor,
		NodeChildOf,
		Extended,
		FlowsTo,
		FlowsFrom,
		SubWindowOf,
		Embeds,
		EmbeddedBy,
		PopupFor,
		ParentWindowOf,
		DescriptionFor,
		DescribedBy
	}

	public class Relation
	{
		private RelationType type;
		private Accessible [] targets;

		public RelationType Type {
			get {
				return type;
			}
		}

		public Accessible [] Targets {
			get {
				return targets;
			}
		}

		internal Relation (Application application, DBusRelation rel)
		{
			type = rel.type;
			targets = new Accessible [rel.targets.Length];
			for (int i = 0; i < rel.targets.Length; i++)
				targets [i] = Registry.GetElement (rel.targets [i], application, false);
		}
	}

	struct DBusRelation
	{
		public RelationType type;
		public AccessiblePath [] targets;
	}
}
