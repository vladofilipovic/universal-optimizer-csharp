using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Transformation
{
	class Program
	{
		static void Main(string[] args)
		{
			if (args.Length == 0)
			{
				Console.WriteLine("Usage: Transform <type> <input> <output>");
				return;
			}
			string type = args[0];
			string inputPath = "";
			if (args.Length < 2)
			{
				Console.WriteLine("Usage: Transform <type> <input> <output>");
				return;
			}
			inputPath = args[1];
			string outputPath = "";
			if (args.Length < 3)
				outputPath = "new_" + inputPath;
			else
				outputPath = args[2];
			if (type == "FromSetCoveringToSplitingSkipWeight")
			{
				int ret = TransformFromSetCoveringToSplitingSkipWeight(inputPath, outputPath);
				if (ret == 0)
					Console.WriteLine("Success: File '" + inputPath + "' is transformed and copied into file '" + outputPath + "'");
				else
					Console.WriteLine("Failure: File '" + inputPath + "' is not transformed and copied into file '" + outputPath + "'");
			}
			else if (type == "FromSetCoveringToSplitingNoSkip")
			{
				int ret = TransformFromSetCoveringToSplitingNoSkip(inputPath, outputPath);
				if (ret == 0)
					Console.WriteLine("Success: File '" + inputPath + "' is transformed and copied into file '" + outputPath + "'");
				else
					Console.WriteLine("Failure: File '" + inputPath + "' is not transformed and copied into file '" + outputPath + "'");
			}
			else
				Console.WriteLine("Unsupported type of conversion: " + type);
		}

		private static int TransformFromSetCoveringToSplitingSkipWeight(string inputPath, string outputPath)
		{
			try
			{
				List<string[]> contentOld = Files.Read(inputPath);
				List<BitArray> contentNew = Transform.FromSetCoveringToSplitingSkipWeight(contentOld);
				Files.Write(outputPath, contentNew);
				return 0;
			}
			finally
			{
			}
		}

		private static int TransformFromSetCoveringToSplitingNoSkip(string inputPath, string outputPath)
		{
			try
			{
				List<string[]> contentOld = Files.Read(inputPath);
				List<BitArray> contentNew = Transform.FromSetCoveringToSplitingNoSkip(contentOld);
				Files.Write(outputPath, contentNew);
				return 0;
			}
			finally
			{
			}
		}
	}
}
