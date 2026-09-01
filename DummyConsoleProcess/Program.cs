using System;
using System.Collections.Generic;
using System.Text;

namespace DummyConsoleProcess
{
	class Program
	{
		static void Main(string[] args)
		{
			Environment.ExitCode = -1;

			// Output 20 lines over ~2 seconds to simulate robocopy behavior
			for (int i = 1; i <= 20; ++i)
			{
				Console.WriteLine("{0,8}	{1,8}	{2,8}", i, i * 100, i * 1024);
				System.Threading.Thread.Sleep(100);
			}

			// Output a fake robocopy summary
			Console.WriteLine();
			Console.WriteLine("---------------");
			Console.WriteLine("  Total	  Copied  Skipped  Mismatch    Failed    Extras");
			Console.WriteLine(" Dirs:        0        0        0         0         0       0");
			Console.WriteLine(" Files:       0        0        0         0         0       0");
			Console.WriteLine(" Bytes:       0        0        0         0         0       0");
			Console.WriteLine(" Times:       0:00:00  0:00:00  0:00:00             0:00:00");
			Console.WriteLine("---------------");
		}
	}
}
