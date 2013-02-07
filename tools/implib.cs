/*
  Copyright (C) 2013 Jeroen Frijters

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
using System.Text.RegularExpressions;
using IKVM.Reflection;
using IKVM.Reflection.Emit;
using Type = IKVM.Reflection.Type;

static class ImpLib
{
    static readonly Regex definition = new Regex(@"^\s*(.+)=\[([^\]]+)\](.+)::([^\s]+)\s+@(\d+)$");
    static readonly Universe universe = new Universe();

    static int Main(string[] args)
    {
        Options options = new Options();
        List<Export> exports = new List<Export>();
        if (!ParseArgs(args, options) || !ParseDefFile(options.deffile, exports))
        {
            return 1;
        }
        AssemblyName name = new AssemblyName(Path.GetFileNameWithoutExtension(options.outputFile));
        name.Version = options.version;
        name.KeyPair = options.key;
        AssemblyBuilder ab = universe.DefineDynamicAssembly(name, AssemblyBuilderAccess.Save);
        ModuleBuilder modb = ab.DefineDynamicModule(name.Name, options.outputFile);
        foreach (Export exp in exports)
        {
            ExportMethod(modb, exp);
        }
        modb.CreateGlobalFunctions();
        if (options.win32res != null)
        {
            ab.DefineUnmanagedResource(options.win32res);
        }
        else
        {
            if (options.description != null)
            {
                ab.SetCustomAttribute(new CustomAttributeBuilder(universe.Import(typeof(System.Reflection.AssemblyTitleAttribute)).GetConstructor(new Type[] { universe.Import(typeof(string)) }), new object[] { options.description }));
            }
            ab.DefineVersionInfoResource(options.product, options.version.ToString(), options.company, options.copyright, null);
        }
        ab.Save(options.outputFile, options.peKind, options.machine);
        return 0;
    }

    static bool ParseArgs(string[] args, Options options)
    {
        options.peKind = PortableExecutableKinds.Required32Bit;
        options.machine = ImageFileMachine.I386;
        foreach (string arg in args)
        {
            if (arg.StartsWith("-r:", StringComparison.Ordinal) || arg.StartsWith("-reference:", StringComparison.Ordinal))
            {
                universe.LoadFile(arg.Substring(arg.IndexOf(':') + 1));
            }
            else if (arg.StartsWith("-out:", StringComparison.Ordinal))
            {
                options.outputFile = arg.Substring(5);
            }
            else if (arg == "-platform:x86")
            {
                options.peKind = PortableExecutableKinds.Required32Bit;
                options.machine = ImageFileMachine.I386;
            }
            else if (arg == "-platform:x64")
            {
                options.peKind = PortableExecutableKinds.PE32Plus;
                options.machine = ImageFileMachine.AMD64;
            }
            else if (arg == "-platform:arm")
            {
                options.peKind = PortableExecutableKinds.Unmanaged32Bit;
                options.machine = ImageFileMachine.ARM;
            }
            else if (arg.StartsWith("-win32res:", StringComparison.Ordinal))
            {
                options.win32res = arg.Substring(10);
            }
            else if (arg.StartsWith("-key:", StringComparison.Ordinal))
            {
                options.key = new StrongNameKeyPair(arg.Substring(5));
            }
            else if (arg.StartsWith("-version:", StringComparison.Ordinal))
            {
                options.version = new Version(arg.Substring(9));
            }
            else if (arg.StartsWith("-product:", StringComparison.Ordinal))
            {
                options.product = arg.Substring(9);
            }
            else if (arg.StartsWith("-company:", StringComparison.Ordinal))
            {
                options.company = arg.Substring(9);
            }
            else if (arg.StartsWith("-copyright:", StringComparison.Ordinal))
            {
                options.copyright = arg.Substring(11);
            }
            else if (arg.StartsWith("-description:", StringComparison.Ordinal))
            {
                options.description = arg.Substring(13);
            }
            else if (options.deffile == null)
            {
                options.deffile = arg;
            }
            else
            {
                Console.WriteLine("Unknown option: {0}", arg);
                return false;
            }
        }

        if (options.deffile == null || options.outputFile == null)
        {
            Console.WriteLine("Usage: implib <exports.def> -out:<outputAssembly.dll> -r:<inputAssembly.dll> [-platform:<x86|x64|arm>] [-win32res:<file>] [-key:<keycontainer>] [-version:<M.m.b.r>]");
            return false;
        }

        return true;
    }

    static bool ParseDefFile(string fileName, List<Export> exports)
    {
        using (StreamReader sr = new StreamReader(fileName))
        {
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                Match m = definition.Match(line);
                if (m.Groups.Count == 6)
                {
                    Export exp;
                    exp.name = m.Groups[1].Value;
                    exp.ordinal = Int32.Parse(m.Groups[5].Value);
                    exp.method = GetMethod(m.Groups[2].Value, m.Groups[3].Value, m.Groups[4].Value);
                    if (exp.method == null)
                    {
                        Console.WriteLine("Unable to find {0}", exp.name);
                        return false;
                    }
                    exports.Add(exp);
                }
            }
        }
        return true;
    }

    static MethodInfo GetMethod(string assembly, string typeName, string method)
    {
        foreach (Assembly asm in universe.GetAssemblies())
        {
            if (asm.GetName().Name.Equals(assembly, StringComparison.OrdinalIgnoreCase))
            {
                Type type = asm.GetType(typeName);
                if (type != null)
                {
                    return type.GetMethod(method, BindingFlags.Public | BindingFlags.Static);
                }
            }
        }
        return null;
    }

    static void ExportMethod(ModuleBuilder modb, Export exp)
    {
        ParameterInfo[] parameters = exp.method.GetParameters();
        Type[] types = new Type[parameters.Length];
        for (int i = 0; i < types.Length; i++)
        {
            types[i] = parameters[i].ParameterType;
        }
        MethodBuilder mb = modb.DefineGlobalMethod(exp.name, MethodAttributes.Public | MethodAttributes.Static, exp.method.ReturnType, types);
        ILGenerator ilgen = mb.GetILGenerator();
        for (int i = 0; i < types.Length; i++)
        {
            ilgen.Emit(OpCodes.Ldarg_S, (byte)i);
        }
        ilgen.Emit(OpCodes.Call, exp.method);
        ilgen.Emit(OpCodes.Ret);
        mb.__AddUnmanagedExport(mb.Name, exp.ordinal);
    }

    sealed class Options
    {
        internal PortableExecutableKinds peKind;
        internal ImageFileMachine machine;
        internal string deffile;
        internal string outputFile;
        internal string win32res;
        internal StrongNameKeyPair key;
        internal Version version;
        internal string product;
        internal string company;
        internal string copyright;
        internal string description;
    }

    struct Export
    {
        internal string name;
        internal int ordinal;
        internal MethodInfo method;
    }
}
