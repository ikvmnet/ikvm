/*
  Copyright (C) 2012 Volker Berlin (i-net software)
  Copyright (C) 2012 Jeroen Frijters

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
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Versioning;
using System.Text;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Javac = com.sun.tools.javac.Main;
using PrintWriter = java.io.PrintWriter;

namespace IKVM.MSBuild
{
    /// <summary>
    /// Java compiler task.
    /// </summary>
    public sealed class JavaTask : ToolTask
    {
        private ITaskItem[] sources;
        private string[] references;
        private ITaskItem[] resources;
        private string configuration = "Debug";
        private string targetType;
        private string outputPath;
        private string mainFile;
        private string outputAssembly;
        private bool emitDebugInformation;
        private string platform = "x86";

        /// <summary>
        /// Gets or sets the source files that will be compiled. Is called from script.
        /// </summary>
        public ITaskItem[] Sources
        {
            get { return sources; }
            set { sources = value; }
        }

        /// <summary>
        /// Gets or sets the resources to be compiled. Is called from script.
        /// </summary>
        public ITaskItem[] Resources
        {
            get { return resources; }
            set { resources = value; }
        }

        /// <summary>
        /// Gets or sets the output assembly type. Is called from script.
        /// </summary>
        public string Configuration
        {
            get { return configuration; }
            set { configuration = value; }
        }

        /// <summary>
        /// Gets or sets the output assembly type. Is called from script.
        /// </summary>
        public string TargetType
        {
            get { return targetType; }
            set { targetType = value; }
        }

        /// <summary>
        /// Gets or sets the output path. Is called from script.
        /// </summary>
        public string OutputPath
        {
            get { return outputPath; }
            set { outputPath = value; }
        }

        /// <summary>
        /// Gets or sets the output assembly filename. Is called from script.
        /// </summary>
        public string OutputAssembly
        {
            get { return outputAssembly; }
            set { outputAssembly = value; }
        }

        /// <summary>
        /// Gets or sets the class with the main method. Is called from script.
        /// </summary>
        public string MainFile
        {
            get { return mainFile; }
            set { mainFile = value; }
        }

        /// <summary>
        /// Gets or sets the platform that will be targeted by the compiler (e.g. x86). Is called from script.
        /// </summary>
        public string Platform
        {
            get { return platform; }
            set { platform = value; }
        }

        /// <summary>
        /// Gets or sets whether the compiler should include debug. Is called from script.
        /// information in the created assembly.
        /// </summary>
        public bool EmitDebugInformation
        {
            get { return emitDebugInformation; }
            set { emitDebugInformation = value; }
        }

        /// <summary>
        /// Gets or sets the assembly references. Is called from script.
        /// </summary>
        public string[] References
        {
            get { return references; }
            set { references = value; }
        }

        protected override string ToolName
        {
			get { return "ikvmc.exe"; }
        }

        protected override string GenerateFullPathToTool()
        {
            return Path.Combine(GetAssemblyPath(), ToolName);
        }

		private static string GetAssemblyPath()
		{
			return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
		}

		private string GetIntermediatePath()
		{
			return Path.GetFullPath(Path.Combine(Path.Combine("obj", platform), configuration));
		}

        protected override string GenerateCommandLineCommands()
        {
            CommandLineBuilder commandLine = new CommandLineBuilder();

			commandLine.AppendSwitch("-nologo");

			if (EmitDebugInformation)
            {
                commandLine.AppendSwitch("-debug");
            }

            commandLine.AppendSwitch("-nostdlib");

            if (((outputAssembly == null) && (sources != null)) && ((sources.Length > 0)))
            {
                outputAssembly = Path.GetFileNameWithoutExtension(sources[0].ItemSpec);
            }
            if (string.Equals(this.TargetType, "library", StringComparison.OrdinalIgnoreCase))
            {
                outputAssembly += ".dll";
            }
            else
            {
                outputAssembly += ".exe";
            }

            if (references != null)
            {
                foreach (string reference in references)
                {
                    if (IsIkvmStandardLibrary(reference))
                    {
                        continue;
                    }
                    commandLine.AppendSwitchIfNotNull("-reference:", reference);
                }
            }

			if (resources != null)
			{
				foreach (ITaskItem item in resources)
				{
					commandLine.AppendSwitch("-resource:" + item.ItemSpec + "=" + Path.GetFullPath(item.ItemSpec));
				}
			}

            commandLine.AppendSwitchIfNotNull("-out:", Path.Combine(GetIntermediatePath(), OutputAssembly));

            if (TargetType != null)
            {
				commandLine.AppendSwitch("-target:" + TargetType.ToLower());
            }

            commandLine.AppendSwitch("-recurse:" + Path.Combine(GetIntermediatePath(), "*.*"));
            //Log.LogWarning(commandLine.ToString(), null);
            return commandLine.ToString();
        }

        /// <summary>
        /// Executes the compiler.
        /// </summary>
        public override bool Execute()
        {
            if (!RunJavac())
            {
                return false;
            }

            CopyIkvm();

            return base.Execute(); // run IKVMC
        }

        private bool RunJavac()
        {
            List<string> paramList = new List<string>();

            paramList.Add("-d");
            paramList.Add(GetIntermediatePath());

            if (sources != null)
            {
                for (int i = 0; i < sources.Length; i++)
                {
					string sourceFile = Path.GetFullPath(sources[i].ItemSpec);
                    RemoveBOM(sourceFile);
                    paramList.Add(sourceFile);
                }
            }

			using (PrintWriter pw = new PrintWriter(new LogWriter(Log), true))
			{
				return Javac.compile(paramList.ToArray(), pw) == 0;
			}
        }

        /// <summary>
        /// Copy the DLLs of IKVM in the output
        /// </summary>
        private void CopyIkvm()
        {
            foreach (FileInfo file in new DirectoryInfo(GetAssemblyPath()).GetFiles("*.dll"))
            {
                string name = file.Name;
                if (IsIkvmStandardLibrary(name))
                {
                    FileInfo target = new FileInfo(Path.Combine(outputPath, name));
                    if (!target.Exists || file.Length != target.Length || file.CreationTime != target.CreationTime)
                    {
                        try
                        {
                            File.Copy(file.FullName, target.FullName, true);
                            target.CreationTime = file.CreationTime;
                        }
                        catch (Exception ex)
                        {
                            Log.LogWarningFromException(ex, true);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Check if the name the name of a library is a standard IKVM library which should not add as reference.
        /// </summary>
        /// <param name="fileName">The name of an assambly library</param>
        /// <returns></returns>
        private bool IsIkvmStandardLibrary(string fileName)
        {
			string name = Path.GetFileNameWithoutExtension(fileName);
            if (name == "IKVM.Runtime")
            {
                return true;
            }
            if (name == "IKVM.OpenJDK.Tools")
            {
                return false;
            }
            if (name.StartsWith("IKVM.OpenJDK."))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Java does not like a BOM at start of UTF8 coded files that we remove it
        /// </summary>
        /// <param name="fileName">The name of a Java source file</param>
        private void RemoveBOM(string fileName)
        {
			using (FileStream original = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Delete | FileShare.Read))
			{
				if (original.ReadByte() == 0xef && original.ReadByte() == 0xbb && original.ReadByte() == 0xbf)
				{
					//BOM detected
					string copyName = fileName + ".nobom";
					using (FileStream copy = File.OpenWrite(copyName))
					{
						byte[] buffer = new byte[4096];
						int count;
						while ((count = original.Read(buffer, 0, buffer.Length)) > 0)
						{
							copy.Write(buffer, 0, count);
						}
					}
					File.Delete(fileName + ".withbom");
					File.Move(fileName, fileName + ".withbom");
					File.Move(copyName, fileName);
					File.Delete(fileName + ".withbom");
				}
			}
        }

        /// <summary>
        /// Redirect the output of the Java Compiler to the MSBUILD output
        /// </summary>
        private sealed class LogWriter : java.io.Writer
        {
            private readonly StringBuilder builder = new StringBuilder();
            private readonly TaskLoggingHelper log;

            internal LogWriter(TaskLoggingHelper log)
            {
                this.log = log;
            }

            public override void write(char[] buf, int off, int len)
            {
                builder.Append(buf, off, len);
            }

            public override void flush()
            {
                if (builder.Length > 0)
                {
                    string msg = builder.ToString();
                    if (msg.EndsWith("\r\n"))
                    {
                        msg = msg.Substring(0, msg.Length - 2);
                    }
                    // parsing the Java error line
                    if (msg.Length > 2 && msg[1] == ':')
                    {
                        int idx = msg.IndexOf(':', 2);
                        if (idx > 0)
                        {
                            string fileName = msg.Substring(0, idx);
                            idx++;
                            int idx2 = msg.IndexOf(':', idx);
                            if (idx2 > 0)
                            {
								int lineNr;
                                if (Int32.TryParse(msg.Substring(idx, idx2 - idx), out lineNr))
                                {
                                    msg = msg.Substring(idx2 + 1);
                                    idx = msg.IndexOf("error:");
                                    if (idx >= 0)
                                    {
                                        msg = msg.Substring(idx + 6).Trim();
										log.LogError(null, null, null, fileName, lineNr, 0, lineNr, 0, msg);
									}
                                    else
                                    {
                                        idx = msg.IndexOf("warning:");
                                        if (idx >= 0)
                                        {
                                            msg = msg.Substring(idx + 8).Trim();
											log.LogWarning(null, null, null, fileName, lineNr, 0, lineNr, 0, msg);
										}
                                    }
                                }
                            }
                        }
                    }
					builder.Length = 0;
                }
            }

            public override void close()
            {
                flush();
            }
        }
    }
}
