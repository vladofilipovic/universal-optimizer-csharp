using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace Transformation
{
	/// <summary>
	/// Summary description for Files.
	/// </summary>
	public class Transform
	{
		public static List<BitArray> FromSetCoveringToSplitingSkipWeight(List<string[]> old)
		{
			if (old.Count <= 0)
				return [];
			int index = 0;
			while ((old[index].Length == 0 || old[index][0] == "") && index < old.Count)
				index++;
			if (old[index].Length < 2)
				return [];
			int numElements = Convert.ToInt32(old[index][0]);
			int numSubsets = Convert.ToInt32(old[index][1]);
			List<BitArray> ret = new();
			for (int i = 0; i < numSubsets; i++)
			{
				ret.Add(new BitArray(numElements, false));
			}
			while (old[index].Length != 1)
				index++;
			int pos = 0;
			int offset = 0;
			while (index + offset < old.Count)
			{
				int num = Convert.ToInt32(old[index + offset][0]);
				offset++;
				int i = 0;
				int j = i;
				bool theEnd = false;
				while(!theEnd )
				{
					int subSet = Convert.ToInt32(old[index + offset][j]);
					ret[subSet-1][pos] = true;
					i++;
					j++;
					if (j >= old[index + offset].Length)
					{
						offset++;
						j = 0;
					}
					if (i >= num)
						theEnd = true;
				}
				pos++;
			}
			return ret;
		}

		public static List<BitArray> FromSetCoveringToSplitingNoSkip(List<string[]> old)
		{
			if (old.Count <= 0)
				return [];
			int index = 0;
			while ((old[index].Length == 0 || old[index][0] == "") && index < old.Count)
				index++;
			if (old[index].Length < 2)
				return [];
			int numElements = Convert.ToInt32(old[index][0]);
			int numSubsets = Convert.ToInt32(old[index][1]);
			List<BitArray> ret = new();
			for (int i = 0; i < numSubsets; i++)
			{
				ret.Add(new BitArray(numElements, false));
			}
			while (old[index].Length != 1)
				index++;
			int pos = 0;
			int offset = 0;
			while (index + offset < old.Count)
			{
				int num = Convert.ToInt32(old[index + offset][0]);
				offset++;
				int i = 0;
				int j = i;
				bool theEnd = false;
				while (!theEnd)
				{
					int subSet = Convert.ToInt32(old[index + offset][j]);
					ret[subSet - 1][pos] = true;
					i++;
					j++;
					if (j >= old[index + offset].Length)
					{
						offset++;
						j = 0;
					}
					if (i >= num)
						theEnd = true;
				}
				pos++;
			}
			return ret;
		}

	}
}