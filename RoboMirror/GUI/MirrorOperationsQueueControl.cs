/*
 * Copyright (c) Martin Kinkelin
 *
 * See the "License.txt" file in the root directory for infos
 * about permitted and prohibited uses of this code.
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace RoboMirror.GUI
{
	public partial class MirrorOperationsQueueControl : UserControl
	{
		private IEnumerable<MirrorOperationControl> OperationControls { get { return panel.Controls.OfType<MirrorOperationControl>(); } }

		public bool IsEmpty { get { return !OperationControls.Any(); } }

		public event EventHandler AllFinished;

		public MirrorOperationsQueueControl()
		{
			InitializeComponent();
		}

		public void Push(MirrorOperation operation)
		{
			if (operation == null)
				throw new ArgumentNullException("operation");

			var control = new MirrorOperationControl(operation)
			{
				Dock = DockStyle.Top
			};
			panel.Controls.Add(control);
			control.BringToFront();

			control.Aborted += (s, e) => { RemoveControl(control); };
			operation.Finished += (s, e) =>
			{
				RemoveControl(control);

				if (!OperationControls.Any())
				{
					if (AllFinished != null)
						AllFinished(this, EventArgs.Empty);
				}
				else
					TriggerNextOperation();
			};

			TriggerNextOperation();
		}

		private void RemoveControl(MirrorOperationControl control)
		{
			if (panel.Controls.Contains(control))
				panel.Controls.Remove(control);
		}

		/// <summary>Triggers the first pending operation if there are no other operations actively copying.
		/// Multiple operations may scan concurrently, but only one copy runs at a time.
		/// </summary>
		private void TriggerNextOperation()
		{
			var controls = OperationControls.ToArray();
			var pending = controls.Where(c => !c.IsRunning);
			if (!pending.Any())
				return;

			// allow concurrent scanning: start the next pending operation
			// even if others are running – only one operation at a time
			// should be in the final copy phase, but scanning/simulation
			// can overlap across multiple tasks
			pending.First().Start();
		}

		public void AbortAll()
		{
			var controls = OperationControls.ToArray();
			panel.Controls.Clear();

			foreach (var control in controls)
				control.Abort();
		}
	}
}
