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
