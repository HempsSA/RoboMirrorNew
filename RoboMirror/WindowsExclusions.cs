/*
 * Copyright (c) Martin Kinkelin
 *
 * See the "License.txt" file in the root directory for infos
 * about permitted and prohibited uses of this code.
 */

using System.Collections.Generic;

namespace RoboMirror
{
	/// <summary>
	/// Provides curated default lists of Windows system folders and files
	/// that are commonly excluded from mirroring/scanning because they are
	/// large, volatile, or require elevated privileges.
	/// Paths are relative to the source folder and begin with a directory
	/// separator, matching the format used by <see cref="MirrorTask.ExcludedFolders"/>
	/// and <see cref="MirrorTask.ExcludedFiles"/>.
	/// </summary>
	public static class WindowsExclusions
	{
		/// <summary>
		/// Default folders to exclude from scanning. These are system-managed,
		/// temporary, or cache directories that should almost never be mirrored.
		/// </summary>
		public static readonly List<string> DefaultFolders = new List<string>
		{
			// ── Windows system folders ──────────────────────────────
			"\\System Volume Information",
			"\\$Recycle.Bin",
			"\\Recovery",
			"\\$WinREAgent",
			"\\$SysReset",
			"\\MSOCache",
			"\\Windows\\Installer",
			"\\Windows\\WinSxS\\ManifestCache",
			"\\Windows\\ServiceProfiles\\LocalService\\AppData\\Local\\FontCache",
			"\\Windows\\ServiceProfiles\\NetworkService\\AppData\\Local\\FontCache",
			"\\Windows\\SoftwareDistribution\\Download",

			// ── Temp and cache ──────────────────────────────────────
			"\\Temp",
			"\\Windows\\Temp",
			"\\AppData\\Local\\Temp",
			"\\AppData\\Local\\Microsoft\\Windows\\INetCache",
			"\\AppData\\Local\\Microsoft\\Windows\\INetCookies",
			"\\AppData\\Local\\Microsoft\\Windows\\Temporary Internet Files",
			"\\AppData\\Local\\Microsoft\\Windows\\WebCache",
			"\\AppData\\Local\\Microsoft\\Windows\\WER",
			"\\AppData\\Local\\Microsoft\\Windows\\AppCache",
			"\\AppData\\Local\\Microsoft\\Windows\\PRICache",
			"\\AppData\\Local\\Microsoft\\Windows\\PrivacIE",
			"\\AppData\\Local\\CrashDumps",
			"\\AppData\\Local\\D3DSCache",
			"\\AppData\\Local\\NVIDIA\\DXCache",
			"\\AppData\\Local\\NVIDIA\\GLCache",
			"\\AppData\\Local\\AMD\\DXCache",
			"\\AppData\\Local\\AMD\\GLCache",
			"\\AppData\\Local\\pip\\cache",
			"\\AppData\\Local\\nuget\\cache",
			"\\AppData\\Local\\yarn\\cache",
			"\\AppData\\Local\\npm-cache",
			"\\AppData\\Local\\pnpm\\cache",
			"\\AppData\\Local\\Microsoft\\Windows\\Explorer\\iconcache*",

			// ── Windows Update ──────────────────────────────────────
			"\\Windows\\SoftwareDistribution",
			"\\Windows\\SoftwareDistribution\\DataStore",
			"\\Windows\\SoftwareDistribution\\Download",

			// ── Delivery Optimization ───────────────────────────────
			"\\Windows\\ServiceProfiles\\NetworkService\\AppData\\Local\\Microsoft\\Windows\\DeliveryOptimization\\Cache",
			"\\Windows\\ServiceProfiles\\NetworkService\\AppData\\Local\\Microsoft\\Windows\\DeliveryOptimization\\CacheData",
			"\\Windows\\ServiceProfiles\\NetworkService\\AppData\\Local\\Microsoft\\Windows\\DeliveryOptimization\\CacheMeta",

			// ── Windows Search / Indexing ───────────────────────────
			"\\ProgramData\\Microsoft\\Search\\Data\\Applications\\Windows",
			"\\ProgramData\\Microsoft\\Search\\Data\\Temp",
			"\\ProgramData\\Microsoft\\Search\\Data\\Config",

			// ── Windows Security / Defender ─────────────────────────
			"\\ProgramData\\Microsoft\\Windows Defender\\Scans\\History",
			"\\ProgramData\\Microsoft\\Windows Defender\\Scans\\mpqueue*",
			"\\ProgramData\\Microsoft\\Windows Defender\\Support",

			// ── Windows Event / Diagnostics ─────────────────────────
			"\\Windows\\System32\\winevt\\Logs",
			"\\Windows\\System32\\SleepStudy",
			"\\Windows\\System32\\sru",

			// ── Windows Error Reporting ─────────────────────────────
			"\\ProgramData\\Microsoft\\Windows\\WER\\ReportArchive",
			"\\ProgramData\\Microsoft\\Windows\\WER\\ReportQueue",
			"\\ProgramData\\Microsoft\\Windows\\WER\\Temp",

			// ── Prefetch ────────────────────────────────────────────
			"\\Windows\\Prefetch",

			// ── Distributed Transaction Coordinator ─────────────────
			"\\Windows\\System32\\MSDtc",

			// ── Common application cache ────────────────────────────
			"\\AppData\\Local\\Microsoft\\Windows\\GameDVR",
			"\\AppData\\Local\\Microsoft\\Windows\\Notifications",
			"\\AppData\\Local\\Microsoft\\Windows\\Caches",
			"\\AppData\\Local\\Microsoft\\Windows\\History",
			"\\AppData\\Local\\Microsoft\\Windows\\SafetyNet\\Logs",
			"\\AppData\\Local\\Microsoft\\Windows\\CIECache",
			"\\AppData\\Local\\Microsoft\\Windows\\ConnectedDevicesPlatform",
			"\\AppData\\Local\\ConnectedDevicesPlatform",

			// ── OneDrive cache ──────────────────────────────────────
			"\\AppData\\Local\\OneDrive\\logs",
			"\\AppData\\Local\\OneDrive\\setup\\logs",
			"\\AppData\\Local\\OneDrive\\updates",
			"\\AppData\\Local\\OneDrive\\cache",

			// ── Browser caches ──────────────────────────────────────
			"\\AppData\\Local\\Google\\Chrome\\User Data\\Default\\Cache",
			"\\AppData\\Local\\Google\\Chrome\\User Data\\Default\\Code Cache",
			"\\AppData\\Local\\Google\\Chrome\\User Data\\Default\\Service Worker\\CacheStorage",
			"\\AppData\\Local\\Microsoft\\Edge\\User Data\\Default\\Cache",
			"\\AppData\\Local\\Microsoft\\Edge\\User Data\\Default\\Code Cache",
			"\\AppData\\Local\\Microsoft\\Edge\\User Data\\Default\\Service Worker\\CacheStorage",
			"\\AppData\\Local\\Mozilla\\Firefox\\Profiles\\*\\cache2",
			"\\AppData\\Local\\BraveSoftware\\Brave-Browser\\User Data\\Default\\Cache",

			// ── Development tool caches ─────────────────────────────
			"\\.git",
			"\\.svn",
			"\\.hg",
			"\\node_modules",
			"\\.gradle",
			"\\.m2",
			"\\.nuget",
			"\\.dotnet",
			"\\.vscode\\extensions",
			"\\__pycache__",
			"\\.idea",
			"\\.vs\\*",

			// ── Docker / WSL ────────────────────────────────────────
			"\\AppData\\Local\\Docker",
			"\\AppData\\Local\\Packages\\CanonicalGroupLimited.Ubuntu*",

			// ── Windows.old / upgrade remnants ──────────────────────
			"\\Windows.old",

			// ── Recycle Bin (alternate format) ──────────────────────
			"\\$Recycle.Bin",
		};

		/// <summary>
		/// Default files to exclude from scanning. These are system-generated,
		/// cache, or thumbnail files that should almost never be mirrored.
		/// </summary>
		public static readonly List<string> DefaultFiles = new List<string>
		{
			// ── Windows system files ────────────────────────────────
			"\\pagefile.sys",
			"\\swapfile.sys",
			"\\hiberfil.sys",
			"\\bootmgr",
			"\\bootmgr.efi",
			"\\ntldr",
			"\\NTDETECT.COM",
			"\\boot.ini",
			"\\bootsect.bak",
			"\\System Volume Information\\desktop.ini",
			"\\$Recycle.Bin\\desktop.ini",

			// ── Thumbnail / icon caches ─────────────────────────────
			"\\Thumbs.db",
			"\\ehThumbs.db",
			"\\ehThumbs.lax",
			"\\desktop.ini",
			"\\AppData\\Local\\IconCache.db",
			"\\AppData\\Local\\Microsoft\\Windows\\Explorer\\thumbcache_*.db",
			"\\AppData\\Local\\Microsoft\\Windows\\Explorer\\iconcache_*.db",

			// ── Temporary files ─────────────────────────────────────
			"\\*.tmp",
			"\\*.temp",
			"\\~$*",
			"\\*.log",
			"\\AppData\\Local\\Temp\\*",

			// ── Office temp / lock files ────────────────────────────
			"\\~*.tmp",
			"\\~*.doc",
			"\\~*.xls",
			"\\~*.ppt",
			"\\~*.docx",
			"\\~*.xlsx",
			"\\~*.pptx",

			// ── Windows Update ──────────────────────────────────────
			"\\Windows\\SoftwareDistribution\\DataStore\\edb*.log",
			"\\Windows\\SoftwareDistribution\\DataStore\\edb*.jrs",
			"\\Windows\\SoftwareDistribution\\DataStore\\edb*.chk",

			// ── Crash dumps ─────────────────────────────────────────
			"\\AppData\\Local\\CrashDumps\\*.dmp",
			"\\Windows\\Minidump\\*.dmp",
			"\\Windows\\MEMORY.DMP",

			// ── Windows Event Logs (binary) ─────────────────────────
			"\\Windows\\System32\\winevt\\Logs\\*.evtx.old",

			// ── Debug logs ──────────────────────────────────────────
			"\\*.log.txt",
			"\\debug.log",
			"\\*.etl",
			"\\*.etl.old",
		};

		/// <summary>
		/// Excluded file attributes (RASHCNETO) for common system files.
		/// </summary>
		public const string DefaultAttributes = "SH";
	}
}
