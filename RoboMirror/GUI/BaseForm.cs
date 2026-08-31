/*
 * Copyright (c) Martin Kinkelin
 *
 * See the "License.txt" file in the root directory for infos
 * about permitted and prohibited uses of this code.
 */

using System;
using System.Windows.Forms;

namespace RoboMirror.GUI
{
	/// <summary>
	/// Base of all forms.
	/// Mainly used for the default font and theme support.
	/// </summary>
	public partial class BaseForm : Form
	{
		public BaseForm()
		{
			Font = System.Drawing.SystemFonts.MessageBoxFont;

			InitializeComponent();
		}

		protected override void OnLoad(EventArgs e)
		{
			ThemeManager.ThemeChanged += OnThemeChanged;
			ThemeManager.Apply(this);
			base.OnLoad(e);
		}

		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			ThemeManager.ThemeChanged -= OnThemeChanged;
			base.OnFormClosing(e);
		}

		private void OnThemeChanged(object sender, EventArgs e)
		{
			ThemeManager.Apply(this);
			Invalidate(true);
		}
	}
}
