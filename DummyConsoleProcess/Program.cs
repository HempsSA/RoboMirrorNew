using System;
using System.Collections.Generic;
using System.Text;

namespace DummyConsoleProcess
{
	class Program
	{
		static void Main(string[] args)
		{
			Environment.ExitCode = 1; // exit code 1 = copies pending (robocopy convention)

			// Output some lines to simulate robocopy scanning
			for (int i = 1; i <= 20; ++i)
			{
				Console.WriteLine("{0,8}	{1,8}	{2,8}", i, i * 100, i * 1024);
				System.Threading.Thread.Sleep(100);
			}

			// Output robocopy summary in exact fixed-width format
			// Each column is exactly 10 chars. GetSummary parses using Substring(colIndex * 10, 10)
			Console.WriteLine();
			Console.WriteLine("----------");
			Console.WriteLine("{0,-10}{1,10}{2,10}{3,10}{4,10}{5,10}{6,10}",
				"           ", "Total", "Copied", "Skipped", "Mismatch", "Failed", "Extras");
			Console.WriteLine("{0,-10}{1,10}{2,10}{3,10}{4,10}{5,10}{6,10}",
				" Dirs:", 5, 3, 2, 0, 0, 0);
			Console.WriteLine("{0,-10}{1,10}{2,10}{3,10}{4,10}{5,10}{6,10}",
				" Files:", 20, 12, 8, 0, 0, 0);
			Console.WriteLine("{0,-10}{1,10}{2,10}{3,10}{4,10}{5,10}{6,10}",
				" Bytes:", "50k", "30k", "20k", 0, 0, 0);
			Console.WriteLine("{0,-10}{1,10}{2,10}{3,10}{4,10}{5,10}{6,10}",
				" Times:", "0:00:02", "0:00:01", "0:00:01", "", "", "0:00:00");
			Console.WriteLine("---------------");
		}
	}
}
