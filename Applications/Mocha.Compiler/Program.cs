using System;
using MBS.Framework.CLI;

namespace Mocha.Compiler
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			Application.Switches.Add("out");
			Application.Start();

			Application.PrintUsage();

			string outputFileName = null;
			CommandLineSwitch swOutputFileName = Application.Switches["out"];
			if (swOutputFileName != null)
			{
				outputFileName = swOutputFileName.Value;
			}
		}
	}
}
