/*
  Copyright (C) 2004 Jeroen Frijters

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
using System.IO;
using System.Reflection;
using System.Text;

class asmref
{
  static void Main(string[] args)
  {
    foreach(string s in args)
    {
      AssemblyName asm;
      if(File.Exists(s))
      {
        asm = Assembly.LoadFile(new FileInfo(s).FullName).GetName();
      }
      else
      {
        asm = Assembly.LoadWithPartialName(s).GetName();
      }

      Console.WriteLine(".assembly extern {0}", asm.Name);
      Console.WriteLine("{");
      if(asm.GetPublicKeyToken() != null)
      {
        StringBuilder sb = new StringBuilder();
        foreach(byte b in asm.GetPublicKeyToken())
        {
          sb.AppendFormat("{0:X2} ", b);
        }
        Console.WriteLine("  .publickeytoken = ({0})", sb.ToString());
      }
      Version v = asm.Version;
      Console.WriteLine("  .ver {0}:{1}:{2}:{3}", v.Major, v.Minor, v.Build, v.Revision);
      Console.WriteLine("}");
    }
  }
}
