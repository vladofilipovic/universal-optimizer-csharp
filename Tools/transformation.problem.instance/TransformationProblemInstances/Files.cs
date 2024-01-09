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
	public class Files
	{
					
		public static void CopyFile( string iz, string u )
		{
			FileStream? fsIz = null;
			FileStream? fsU = null;
			try
			{
				fsIz = new FileStream( iz, System.IO.FileMode.Open, System.IO.FileAccess.Read );
				fsU = new FileStream( u, System.IO.FileMode.Create, System.IO.FileAccess.ReadWrite );
				int a;
				while( (a = fsIz.ReadByte()) != -1 )
					fsU.WriteByte( (byte)a );   
			}
			finally
			{
				if( fsU != null )
				{
					fsU.Flush();
					fsU.Close();
				}
				if( fsIz != null )
					fsIz.Close();
			}
		}

		public static List<string[]> Read(string fajl)
		{
			List<string[]> ret = [];
			StreamReader reader = new((System.IO.Stream)File.OpenRead(fajl), Encoding.ASCII);
			string? ln = "";
			while (ln != null)
			{
				if (ln != null)
				{
					ln = ln.Trim();
					while (ln.Contains("  ", StringComparison.CurrentCulture))
						ln = ln.Replace("  ", " ");
					string[] elements = ln.Split(" ".ToCharArray());
					ret.Add(elements);
				}
				ln = reader.ReadLine();
			}
			reader.Close();
			return ret;
		}

		public static void Write(string fajl, List<BitArray> values)
		{
			StreamWriter writer = new((System.IO.Stream)File.OpenWrite(fajl), Encoding.ASCII);
			foreach (BitArray ba in values)
			{
				foreach (bool b in ba)
					writer.Write((b)?"1":"0");
				writer.WriteLine();
			}
			writer.Close();
		}
	}
}
