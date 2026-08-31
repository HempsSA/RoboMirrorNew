/*
 * Copyright (c) Martin Kinkelin
 *
 * See the "License.txt" file in the root directory for infos
 * about permitted and prohibited uses of this code.
 */

using System;
using System.IO;
using System.Threading;

namespace RoboMirror
{
	/// <summary>
	/// Holds aggregate statistics for a scanned directory tree.
	/// Thread-safe: all fields may be updated from multiple threads.
	/// </summary>
	public class ScanResult
	{
		internal long _fileCountBacking;
		internal long _directoryCountBacking;
		internal long _totalSizeBacking;

		public long FileCount { get { return Interlocked.Read(ref _fileCountBacking); } }
		public long DirectoryCount { get { return Interlocked.Read(ref _directoryCountBacking); } }
		public long TotalSize { get { return Interlocked.Read(ref _totalSizeBacking); } }

		private int _errorCount;
		public int ErrorCount { get { return Interlocked.CompareExchange(ref _errorCount, 0, 0); } }
		internal void IncrementErrorCount() { Interlocked.Increment(ref _errorCount); }
	}

	/// <summary>
	/// Recursively scans a directory tree and reports file/directory
	/// counts and total size. Supports cancellation via a
	/// <see cref="CancellationToken"/>.
	/// </summary>
	public class DirectoryScanner
	{
		private readonly string _path;
		private readonly ScanResult _result = new ScanResult();

		/// <summary>
		/// Fired periodically (at most every 200 ms) with the current
		/// file count so the UI can show scanning progress.
		/// </summary>
		public event EventHandler<ScanProgressEventArgs> ProgressChanged;

		/// <summary>
		/// Fired when an individual file or directory cannot be
		/// accessed. The handler should not throw.
		/// </summary>
		public event EventHandler<ScanErrorEventArgs> Error;

		private DateTime _lastProgressFire = DateTime.MinValue;
		private const int ProgressIntervalMs = 200;


		public DirectoryScanner(string path)
		{
			if (string.IsNullOrEmpty(path))
				throw new ArgumentNullException("path");
			_path = path;
		}

		/// <summary>
		/// Gets the path being scanned.
		/// </summary>
		public string Path { get { return _path; } }

		/// <summary>
		/// Gets the aggregate result. Only valid after <see cref="Scan"/>
		/// has been called and completed.
		/// </summary>
		public ScanResult Result { get { return _result; } }


		/// <summary>
		/// Scans the directory tree. Thread-safe; may be called from
		/// a background thread.
		/// </summary>
		/// <param name="cancellationToken">
		/// Optional token to abort the scan early.
		/// </param>
		public void Scan(CancellationToken cancellationToken)
		{
			ScanDirectory(_path, cancellationToken);
		}


		private void ScanDirectory(string dirPath, CancellationToken ct)
		{
			if (ct.IsCancellationRequested)
				return;

			FileInfo[] files;
			DirectoryInfo[] subDirs;

			try
			{
				var dir = new DirectoryInfo(dirPath);
				files = dir.GetFiles();
				subDirs = dir.GetDirectories();
			}
			catch (UnauthorizedAccessException ex)
			{
				_result.IncrementErrorCount();
				FireError(ex.Message);
				return;
			}
			catch (IOException ex)
			{
				_result.IncrementErrorCount();
				FireError(ex.Message);
				return;
			}
			catch (Exception ex)
			{
				_result.IncrementErrorCount();
				FireError(ex.Message);
				return;
			}

			Interlocked.Increment(ref _result._directoryCountBacking);

			foreach (var file in files)
			{
				if (ct.IsCancellationRequested)
					return;

				try
				{
					Interlocked.Add(ref _result._totalSizeBacking, file.Length);
					Interlocked.Increment(ref _result._fileCountBacking);
				}
				catch (Exception ex)
				{
					_result.IncrementErrorCount();
					FireError(ex.Message);
				}

				FireProgressIfNeeded();
			}

			foreach (var subDir in subDirs)
			{
				if (ct.IsCancellationRequested)
					return;

				ScanDirectory(subDir.FullName, ct);
			}
		}


		private void FireProgressIfNeeded()
		{
			DateTime now = DateTime.UtcNow;
			if ((now - _lastProgressFire).TotalMilliseconds < ProgressIntervalMs)
				return;
			_lastProgressFire = now;

			var handler = ProgressChanged;
			if (handler != null)
				handler(this, new ScanProgressEventArgs(_result.FileCount, _result.DirectoryCount, _result.TotalSize));
		}

		private void FireError(string message)
		{
			var handler = Error;
			if (handler != null)
				handler(this, new ScanErrorEventArgs(message));
		}
	}


	#region EventArgs

	public class ScanProgressEventArgs : EventArgs
	{
		public long FileCount { get; private set; }
		public long DirectoryCount { get; private set; }
		public long TotalSize { get; private set; }

		public ScanProgressEventArgs(long fileCount, long directoryCount, long totalSize)
		{
			FileCount = fileCount;
			DirectoryCount = directoryCount;
			TotalSize = totalSize;
		}
	}

	public class ScanErrorEventArgs : EventArgs
	{
		public string Message { get; private set; }

		public ScanErrorEventArgs(string message)
		{
			Message = message;
		}
	}

	#endregion
}
