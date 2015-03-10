﻿//
// Calendar.cs
//
// Author:
//       Claudio Rodrigo Pereyra Diaz <claudiorodrigo@pereyradiaz.com.ar>
//
// Copyright (c) 2015 Hamekoz
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using Xwt.Backends;
using System.ComponentModel;

namespace Xwt
{
	[BackendType (typeof(ICalendarBackend))]
	public class Calendar : Widget
	{
		EventHandler valueChanged, doubleClick;

		static Calendar ()
		{
			MapEvent (CalendarEvent.ValueChanged, typeof(Calendar), "OnValueChanged");
			MapEvent (CalendarEvent.DoubleClick, typeof(Calendar), "OnDoubleClick");
		}

		protected new class WidgetBackendHost: Widget.WidgetBackendHost, ICalendarEventSink
		{
			public void OnValueChanged ()
			{
				((Calendar)Parent).OnValueChanged (EventArgs.Empty);
			}

			public void OnDoubleClick ()
			{
				((Calendar)Parent).OnDoubleClick (EventArgs.Empty);
			}
		}

		public Calendar ()
		{
			MinDate = DateTime.MinValue;
			MaxDate = DateTime.MaxValue;
			Date = DateTime.Now;
		}

		ICalendarBackend Backend {
			get { return (ICalendarBackend)BackendHost.Backend; }
		}

		protected override BackendHost CreateBackendHost ()
		{
			return new WidgetBackendHost ();
		}

		public DateTime Date {
			get {
				return Backend.Date;
			}
			set {
				Backend.Date = value;
			}
		}

		public DateTime MinDate {
			get {
				return Backend.MinDate;
			}
			set {
				Backend.MinDate = value;
				if (MinDate > MaxDate)
					MaxDate = MinDate;
				if (Date < MinDate)
					Date = MinDate;
			}
		}

		public DateTime MaxDate {
			get {
				return Backend.MaxDate;
			}
			set {
				Backend.MaxDate = value;
				if (MaxDate < MinDate)
					MinDate = MaxDate;
				if (Date > MaxDate)
					Date = MaxDate;
			}
		}

		[DefaultValue (false)]
		public bool NoMonthChange {
			get {
				return Backend.NoMonthChange;
			}
			set {
				Backend.NoMonthChange = value;
			}
		}

		protected virtual void OnValueChanged (EventArgs e)
		{
			if (valueChanged != null)
				valueChanged (this, e);
		}

		public event EventHandler ValueChanged {
			add {
				BackendHost.OnBeforeEventAdd (CalendarEvent.ValueChanged, valueChanged);
				valueChanged += value;
			}
			remove {
				valueChanged -= value;
				BackendHost.OnAfterEventRemove (CalendarEvent.ValueChanged, valueChanged);
			}
		}

		protected virtual void OnDoubleClick (EventArgs e)
		{
			if (doubleClick != null)
				doubleClick (this, e);
		}

		public event EventHandler DoubleClick {
			add {
				BackendHost.OnBeforeEventAdd (CalendarEvent.DoubleClick, doubleClick);
				doubleClick += value;
			}
			remove {
				doubleClick -= value;
				BackendHost.OnAfterEventRemove (CalendarEvent.DoubleClick, doubleClick);
			}
		}
	}
}

