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
using System.IO;
using System.Globalization;

namespace RoboMirror.GUI
{
	/// <summary>
	/// Allows management of subfolders and files to be excluded
	/// from a mirror task.
	/// </summary>
	public partial class ExcludedItemsDialog : BaseDialog
	{
		private Button _addWindowsDefaultsButton;
		/// <summary>
		/// Gets the list of excluded files.
		/// </summary>
		public List<string> ExcludedFiles { get; private set; }

		/// <summary>
		/// Gets the list of excluded folders.
		/// </summary>
		public List<string> ExcludedFolders { get; private set; }

		/// <summary>
		/// Gets the string encoding the excluded attributes (RASHCNETO).
		/// </summary>
		public string ExcludedAttributes { get; private set; }


		/// <param name="task">Task whose excluded items are to be edited.</param>
		public ExcludedItemsDialog(MirrorTask task)
		{
			if (task == null)
				throw new ArgumentNullException("task");

			ExcludedFiles = new List<string>(task.ExcludedFiles);
			ExcludedFolders = new List<string>(task.ExcludedFolders);
			ExcludedAttributes = (task.ExcludedAttributes == null ? string.Empty : task.ExcludedAttributes);		InitializeComponent();

		foreach (string file in ExcludedFiles)
			excludedFilesControl.ExcludedItems.Add(file);
		foreach (string folder in ExcludedFolders)
			excludedFoldersControl.ExcludedItems.Add(folder);

		if (!string.IsNullOrEmpty(ExcludedAttributes))
		{
			foreach (CheckBox child in tableLayoutPanel1.Controls)
				child.Checked = ExcludedAttributes.Contains((string)child.Tag);
		}

		SetupAddWindowsDefaultsButton();
	}

	private void SetupAddWindowsDefaultsButton()
	{
		_addWindowsDefaultsButton = new Button();
		_addWindowsDefaultsButton.Text = "Add Windows defaults";
		_addWindowsDefaultsButton.AutoSize = true;
		_addWindowsDefaultsButton.AutoSizeMode = AutoSizeMode.GrowAndShrink;
		_addWindowsDefaultsButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
		_addWindowsDefaultsButton.Padding = new Padding(6, 0, 6, 0);
		_addWindowsDefaultsButton.Location = new Point(597 - 160, 65);
		_addWindowsDefaultsButton.Size = new Size(146, 28);
		_addWindowsDefaultsButton.TabIndex = 10;
		_addWindowsDefaultsButton.FlatStyle = FlatStyle.Flat;
		_addWindowsDefaultsButton.FlatAppearance.BorderColor = SystemColors.Highlight;
		_addWindowsDefaultsButton.ForeColor = SystemColors.HighlightText;
		_addWindowsDefaultsButton.BackColor = SystemColors.Highlight;
		toolTip1.SetToolTip(_addWindowsDefaultsButton,
			"Add common Windows system, temp, and cache folders/files to the exclusion list.");
		_addWindowsDefaultsButton.Click += AddWindowsDefaultsButton_Click;
		this.Controls.Add(_addWindowsDefaultsButton);
	}

		public DialogResult ShowDialog(IWin32Window owner, string sourceFolder)
		{
			if (!Directory.Exists(sourceFolder))
				throw new InvalidOperationException("The source folder does not exist.");

			excludedFilesControl.BaseFolder = sourceFolder;
			excludedFoldersControl.BaseFolder = sourceFolder;

			return ShowDialog(owner);
		}


		private void Control_Changed(object sender, EventArgs e)
		{
			HasChanged = true;
		}


	private void AddWindowsDefaultsButton_Click(object sender, EventArgs e)
	{
		int foldersAdded = 0;
		int filesAdded = 0;

		foreach (string folder in WindowsExclusions.DefaultFolders)
		{
			if (!excludedFoldersControl.ExcludedItems.Contains(folder))
			{
				excludedFoldersControl.ExcludedItems.Add(folder);
				foldersAdded++;
			}
		}

		foreach (string file in WindowsExclusions.DefaultFiles)
		{
			if (!excludedFilesControl.ExcludedItems.Contains(file))
			{
				excludedFilesControl.ExcludedItems.Add(file);
				filesAdded++;
			}
		}

		// also set the attribute exclusions if not already set
		if (!checkBox1.Checked) checkBox1.Checked = WindowsExclusions.DefaultAttributes.Contains("H");
		if (!checkBox2.Checked) checkBox2.Checked = WindowsExclusions.DefaultAttributes.Contains("S");

		if (foldersAdded + filesAdded > 0)
		{
			HasChanged = true;
			MessageBox.Show(this,
				string.Format("Added {0} folder(s) and {1} file(s) to the exclusion list.", foldersAdded, filesAdded),
				"Windows defaults added", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}
		else
		{
			MessageBox.Show(this,
				"All Windows default exclusions are already in the list.",
				"Nothing to add", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}
	}

	protected override bool ApplyChanges()
	{
		ExcludedFiles.Clear();
			foreach (string item in excludedFilesControl.ExcludedItems)
				ExcludedFiles.Add(item);

			ExcludedFolders.Clear();
			foreach (string item in excludedFoldersControl.ExcludedItems)
				ExcludedFolders.Add(item);

			foreach (CheckBox child in tableLayoutPanel1.Controls)
			{
				string tag = (string)child.Tag;

				if (child.Checked)
				{
					if (!ExcludedAttributes.Contains(tag))
						ExcludedAttributes += tag;
				}
				else
					ExcludedAttributes = ExcludedAttributes.Replace(tag, string.Empty);
			}

			return true;
		}
	}
}
