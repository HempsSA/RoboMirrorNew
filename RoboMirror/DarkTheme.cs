/*
 * Copyright (c) Martin Kinkelin
 *
 * See the "License.txt" file in the root directory for infos
 * about permitted and prohibited uses of this code.
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace RoboMirror
{
	/// <summary>
	/// Provides color palettes for light and dark themes and applies
	/// them recursively to a control tree.
	/// </summary>
	public static class ThemeManager
	{
		private static bool _darkMode;

		/// <summary>
		/// Fired when the dark mode setting changes so open forms
		/// can re-apply the theme.
		/// </summary>
		public static event EventHandler ThemeChanged;

		public static bool DarkMode
		{
			get { return _darkMode; }
			set
			{
				if (_darkMode == value)
					return;
				_darkMode = value;
				var handler = ThemeChanged;
				if (handler != null)
					handler(null, EventArgs.Empty);
			}
		}

		#region Color palettes

		public static class Colors
		{
			// Light theme (system defaults)
			public static readonly Color Window = SystemColors.Window;
			public static readonly Color WindowText = SystemColors.WindowText;
			public static readonly Color Control = SystemColors.Control;
			public static readonly Color ControlText = SystemColors.ControlText;
			public static readonly Color ControlDark = SystemColors.ControlDark;
			public static readonly Color ControlDarkDark = SystemColors.ControlDarkDark;
			public static readonly Color ControlLight = SystemColors.ControlLight;
			public static readonly Color Highlight = SystemColors.Highlight;
			public static readonly Color HighlightText = SystemColors.HighlightText;
			public static readonly Color ButtonFace = SystemColors.Control;
			public static readonly Color ButtonHighlight = SystemColors.ControlLightLight;
			public static readonly Color ButtonShadow = SystemColors.ControlDark;
		}

		public static class DarkColors
		{
			public static readonly Color Background = Color.FromArgb(30, 30, 30);
			public static readonly Color Surface = Color.FromArgb(37, 37, 38);
			public static readonly Color SurfaceRaised = Color.FromArgb(45, 45, 48);
			public static readonly Color Foreground = Color.FromArgb(204, 204, 204);
			public static readonly Color ForegroundDim = Color.FromArgb(140, 140, 140);
			public static readonly Color Border = Color.FromArgb(62, 62, 66);
			public static readonly Color Highlight = Color.FromArgb(0, 120, 212);
			public static readonly Color HighlightText = Color.White;
			public static readonly Color HeaderBackground = Color.FromArgb(37, 37, 38);
			public static readonly Color ListViewBackground = Color.FromArgb(30, 30, 30);
			public static readonly Color ListViewRow = Color.FromArgb(30, 30, 30);
			public static readonly Color ListViewAlternate = Color.FromArgb(37, 37, 38);
			public static readonly Color ListViewSelection = Color.FromArgb(51, 51, 51);
			public static readonly Color TextBoxBackground = Color.FromArgb(37, 37, 38);
			public static readonly Color TextBoxBorder = Color.FromArgb(62, 62, 66);
			public static readonly Color RichTextBoxBackground = Color.FromArgb(30, 30, 30);
			public static readonly Color ProgressBarBackground = Color.FromArgb(51, 51, 51);
			public static readonly Color ProgressBarForeground = Color.FromArgb(0, 120, 212);
		}

		#endregion

		/// <summary>
		/// Returns the current background color for the given control type.
		/// </summary>
		public static Color GetBackColor(Control control)
		{
			if (!_darkMode)
				return control.BackColor;

			if (control is TextBox || control is RichTextBox)
				return DarkColors.TextBoxBackground;
			if (control is ListView)
				return DarkColors.ListViewBackground;
			if (control is Panel || control is UserControl)
				return DarkColors.Background;
			if (control is CheckBox)
				return DarkColors.Background;
			if (control is Label)
				return Color.Transparent;

			return DarkColors.Background;
		}

		/// <summary>
		/// Returns the current foreground color for the given control type.
		/// </summary>
		public static Color GetForeColor(Control control)
		{
			if (!_darkMode)
				return control.ForeColor;

			if (control is Label && control.ForeColor == SystemColors.ControlText)
				return DarkColors.Foreground;

			return DarkColors.Foreground;
		}

		/// <summary>
		/// Applies the current theme to the given form and all its child controls.
		/// </summary>
		public static void Apply(Form form)
		{
			if (_darkMode)
				ApplyDark(form);
			else
				ApplyLight(form);
		}

		/// <summary>
		/// Applies the current theme to a user control and its children.
		/// </summary>
		public static void Apply(UserControl control)
		{
			if (_darkMode)
				ApplyDarkControl(control);
			else
				ApplyLightControl(control);
		}

		#region Dark theme

		private static void ApplyDark(Form form)
		{
			form.BackColor = DarkColors.Background;
			form.ForeColor = DarkColors.Foreground;

			foreach (Control c in form.Controls)
				ApplyDarkControl(c);
		}

		private static void ApplyDarkControl(Control control)
		{
			if (control == null)
				return;

			// Recurse into child controls first
			foreach (Control child in control.Controls)
				ApplyDarkControl(child);

			Type t = control.GetType();

			if (control is Form)
			{
				control.BackColor = DarkColors.Background;
				control.ForeColor = DarkColors.Foreground;
			}
			else if (control is Panel panel)
			{
				if (panel.Name == "label1" || panel.BackColor == SystemColors.ControlDarkDark)
				{
					// Header panel — keep dark accent
					panel.BackColor = DarkColors.HeaderBackground;
					panel.ForeColor = DarkColors.Foreground;
				}
				else
				{
					panel.BackColor = DarkColors.Background;
					panel.ForeColor = DarkColors.Foreground;
				}
			}
			else if (control is Label label)
			{
				// Header labels with white fore color stay readable
				if (label.ForeColor == Color.White)
				{
					label.BackColor = DarkColors.HeaderBackground;
					label.ForeColor = Color.White;
				}
				else
				{
					label.BackColor = Color.Transparent;
					label.ForeColor = DarkColors.Foreground;
				}
			}
			else if (control is Button button)
			{
				button.FlatStyle = FlatStyle.Flat;
				button.BackColor = DarkColors.SurfaceRaised;
				button.ForeColor = DarkColors.Foreground;
				button.FlatAppearance.BorderColor = DarkColors.Border;
				button.FlatAppearance.MouseOverBackColor = DarkColors.ListViewSelection;
				button.FlatAppearance.MouseDownBackColor = DarkColors.Highlight;
			}
			else if (control is TextBox textBox)
			{
				textBox.BackColor = DarkColors.TextBoxBackground;
				textBox.ForeColor = DarkColors.Foreground;
				textBox.BorderStyle = BorderStyle.FixedSingle;
			}
			else if (control is RichTextBox richTextBox)
			{
				richTextBox.BackColor = DarkColors.RichTextBoxBackground;
				richTextBox.ForeColor = DarkColors.Foreground;
				richTextBox.BorderStyle = BorderStyle.FixedSingle;
			}
			else if (control is ListView listView)
			{
				listView.BackColor = DarkColors.ListViewBackground;
				listView.ForeColor = DarkColors.Foreground;
				listView.OwnerDraw = true;
				listView.DrawColumnHeader += ListView_DrawColumnHeader;
				listView.DrawSubItem += ListView_DrawSubItem;
				listView.GridLines = true;
			}
			else if (control is CheckBox checkBox)
			{
				checkBox.BackColor = Color.Transparent;
				checkBox.ForeColor = DarkColors.Foreground;
			}
			else if (control is ProgressBar progressBar)
			{
				progressBar.BackColor = DarkColors.ProgressBarBackground;
				progressBar.ForeColor = DarkColors.ProgressBarForeground;
			}
			else if (control is ToolStrip toolStrip)
			{
				toolStrip.BackColor = DarkColors.Surface;
				toolStrip.ForeColor = DarkColors.Foreground;
				toolStrip.Renderer = new DarkToolStripRenderer();
			}
			else if (control is UserControl userControl)
			{
				userControl.BackColor = DarkColors.Background;
				userControl.ForeColor = DarkColors.Foreground;
			}
			else
			{
				// Generic fallback
				control.BackColor = DarkColors.Background;
				control.ForeColor = DarkColors.Foreground;
			}
		}

		#endregion

		#region Light theme

		private static void ApplyLight(Form form)
		{
			form.BackColor = SystemColors.Control;
			form.ForeColor = SystemColors.ControlText;

			foreach (Control c in form.Controls)
				ApplyLightControl(c);
		}

		private static void ApplyLightControl(Control control)
		{
			if (control == null)
				return;

			foreach (Control child in control.Controls)
				ApplyLightControl(child);

			if (control is Form)
			{
				control.BackColor = SystemColors.Control;
				control.ForeColor = SystemColors.ControlText;
			}
			else if (control is Panel)
			{
				control.BackColor = SystemColors.Control;
				control.ForeColor = SystemColors.ControlText;
			}
			else if (control is Label label)
			{
				label.BackColor = Color.Transparent;
				label.ForeColor = SystemColors.ControlText;
				// Preserve header label styling
				if (label.ForeColor == Color.White && label.Name == "label1")
				{
					label.BackColor = SystemColors.ControlDarkDark;
					label.ForeColor = Color.White;
				}
			}
			else if (control is Button button)
			{
				button.FlatStyle = FlatStyle.Standard;
				button.BackColor = SystemColors.Control;
				button.ForeColor = SystemColors.ControlText;
				button.FlatAppearance.BorderColor = SystemColors.ControlDark;
			}
			else if (control is TextBox textBox2)
			{
				textBox2.BackColor = SystemColors.Window;
				textBox2.ForeColor = SystemColors.WindowText;
				textBox2.BorderStyle = BorderStyle.Fixed3D;
			}
			else if (control is RichTextBox rtb2)
			{
				rtb2.BackColor = SystemColors.Window;
				rtb2.ForeColor = SystemColors.WindowText;
				rtb2.BorderStyle = BorderStyle.Fixed3D;
			}
			else if (control is ListView listView)
			{
				listView.BackColor = SystemColors.Window;
				listView.ForeColor = SystemColors.WindowText;
				listView.OwnerDraw = false;
				listView.GridLines = false;
			}
			else if (control is CheckBox)
			{
				control.BackColor = Color.Transparent;
				control.ForeColor = SystemColors.ControlText;
			}
			else if (control is ProgressBar)
			{
				control.BackColor = SystemColors.Control;
				control.ForeColor = SystemColors.Highlight;
			}
			else if (control is ToolStrip toolStrip)
			{
				toolStrip.BackColor = SystemColors.Control;
				toolStrip.ForeColor = SystemColors.ControlText;
				toolStrip.Renderer = new ToolStripProfessionalRenderer();
			}
			else if (control is UserControl)
			{
				control.BackColor = SystemColors.Control;
				control.ForeColor = SystemColors.ControlText;
			}
		}

		#endregion

		#region ListView owner-draw

		private static void ListView_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
		{
			using (var brush = new SolidBrush(DarkColors.SurfaceRaised))
				e.Graphics.FillRectangle(brush, e.Bounds);

			using (var pen = new Pen(DarkColors.Border))
				e.Graphics.DrawRectangle(pen, e.Bounds.X, e.Bounds.Y, e.Bounds.Width - 1, e.Bounds.Height - 1);

			using (var textBrush = new SolidBrush(DarkColors.ForegroundDim))
			{
				var sf = new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center };
				e.Graphics.DrawString(e.Header.Text, e.Font ?? Control.DefaultFont, textBrush, e.Bounds, sf);
			}
		}

		private static void ListView_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
		{
			Color bgColor;
			if (e.Item.Selected)
				bgColor = DarkColors.Highlight;
			else if (e.ItemIndex % 2 == 0)
				bgColor = DarkColors.ListViewRow;
			else
				bgColor = DarkColors.ListViewAlternate;

			using (var brush = new SolidBrush(bgColor))
				e.Graphics.FillRectangle(brush, e.Bounds);

			Color fgColor = e.Item.Selected ? DarkColors.HighlightText : DarkColors.Foreground;

			using (var textBrush = new SolidBrush(fgColor))
			{
				var sf = new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center };
				var textBounds = new Rectangle(e.Bounds.X + 4, e.Bounds.Y, e.Bounds.Width - 8, e.Bounds.Height);
				e.Graphics.DrawString(e.SubItem.Text, e.Item.Font ?? Control.DefaultFont, textBrush, textBounds, sf);
			}
		}

		#endregion
	}

	/// <summary>
	/// Custom ToolStrip renderer for dark mode.
	/// </summary>
	internal class DarkToolStripRenderer : ToolStripProfessionalRenderer
	{
		protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
		{
			using (var brush = new SolidBrush(ThemeManager.DarkColors.Surface))
				e.Graphics.FillRectangle(brush, e.AffectedBounds);
		}

		protected override void OnRenderButtonBackground(ToolStripItemRenderEventArgs e)
		{
			if (e.Item.Selected || e.Item.Pressed)
			{
				using (var brush = new SolidBrush(ThemeManager.DarkColors.ListViewSelection))
					e.Graphics.FillRectangle(brush, new Rectangle(Point.Empty, e.Item.Size));
			}
		}

		protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
		{
			e.TextColor = ThemeManager.DarkColors.Foreground;
			base.OnRenderItemText(e);
		}

		protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
		{
			using (var pen = new Pen(ThemeManager.DarkColors.Border))
				e.Graphics.DrawLine(pen, e.AffectedBounds.X, e.AffectedBounds.Bottom - 1,
					e.AffectedBounds.Right, e.AffectedBounds.Bottom - 1);
		}
	}
}
