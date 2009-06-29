/*
  Copyright (C) 2009 Jeroen Frijters

  This software is provided 'as-is', without any express or implied
  warranty.  In no event will the authors be held liable for any damages
  arising from the use of this software.

  Permission is granted to anyone to use this software for any purpose,
  including commercial applications, and to alter it and redistribute it
  freely, subject to the following restrictions:

  1. The origin of this software must not be misrepresented; you must not
     claim that you wrote the original software. If you use this software
     in a product, an acknowledgment in the product documentation would be
     appreciated but is not required.
  2. Altered source versions must be plainly marked as such, and must not be
     misrepresented as being the original software.
  3. This notice may not be removed or altered from any source distribution.

  Jeroen Frijters
  jeroen@frijters.net
  
*/

using System;
using System.Collections.Generic;
using System.IO;

/*
 * This tool updates the -baseaddress options in response.txt based on the current file sizes.
 *
 * Usage: updbaseaddresses \ikvm\openjdk\response.txt
 */

class UpdateBaseAddresses
{
	static void Main(string[] args)
	{
		string[] input = File.ReadAllLines(args[0]);
		List<string> output = new List<string>();
		int baseAddress = 0x56000000;
		string dir = Path.GetDirectoryName(args[0]);
		bool dirty = false;
		for (int i = 0; i < input.Length; i++)
		{
			string line = input[i];
			if (!line.Contains("-baseaddress:"))
			{
				output.Add(line);
			}
			if (line.Trim().StartsWith("-out:"))
			{
				string str = String.Format("    -baseaddress:0x{0:X}", baseAddress);
				output.Add(str);
				if (str != input[i + 1])
				{
					dirty = true;
				}
				string file = line.Trim().Substring(5);
				FileInfo fileInfo = new FileInfo(Path.Combine(dir, file));
				baseAddress += 3 * (((int)fileInfo.Length + 65535) / 65536) * 65536;
			}
		}
		if (dirty)
		{
			File.WriteAllLines(args[0], output.ToArray());
		}
	}
}
