/*
  Copyright (C) 2002 Jeroen Frijters

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
package ikvm.lang;

import java.io.InputStream;
import java.io.OutputStream;
import java.io.File;
import cli.System.Text.StringBuilder;
import cli.System.Diagnostics.ProcessStartInfo;

public class DotNetProcess extends Process
{
	private cli.System.Diagnostics.Process proc;

	private DotNetProcess(cli.System.Diagnostics.Process proc)
	{
		this.proc = proc;
	}

	public OutputStream getOutputStream()
	{
		return new java.io.FileOutputStream(new java.io.FileDescriptor(proc.get_StandardInput().get_BaseStream()));
	}

	public InputStream getInputStream()
	{
		return new java.io.FileInputStream(new java.io.FileDescriptor(proc.get_StandardOutput().get_BaseStream()));
	}

	public InputStream getErrorStream()
	{
		return new java.io.FileInputStream(new java.io.FileDescriptor(proc.get_StandardError().get_BaseStream()));
	}

	public int waitFor() throws InterruptedException
	{
		proc.WaitForExit();
		return proc.get_ExitCode();
	}

	public int exitValue()
	{
		if(proc.get_HasExited())
		{
			return proc.get_ExitCode();
		}
		throw new IllegalThreadStateException();
	}

	public void destroy()
	{
		try
		{
			if(false) throw new cli.System.InvalidOperationException();
			proc.Kill();
		}
		catch(cli.System.InvalidOperationException x)
		{
		}
	}

	public static Process execInternal(Runtime runtime, String[] cmd, String[] env, File dir)
	{
		StringBuilder sb = new StringBuilder();
		for(int i = 1; i < cmd.length; i++)
		{
			if(i > 1)
			{
				sb.Append(' ');
			}
			// HACK if the arg contains a space, we surround it with quotes
			// this isn't nearly good enough, but for now it'll have to do
			if(cmd[i].indexOf(' ') >= 0)
			{
				sb.Append('"').Append(cmd[i]).Append('"');
			}
			else
			{
				sb.Append(cmd[i]);
			}
		}
		ProcessStartInfo si = new ProcessStartInfo(cmd[0], sb.ToString());
		si.set_UseShellExecute(false);
		si.set_RedirectStandardError(true);
		si.set_RedirectStandardOutput(true);
		si.set_RedirectStandardInput(true);
		si.set_CreateNoWindow(true);
		if(dir != null)
		{
			si.set_WorkingDirectory(dir.toString());
		}
		if(env != null)
		{
			for(int i = 0; i < env.length; i++)
			{
				int pos = env[i].indexOf('=');
				si.get_EnvironmentVariables().Add(env[i].substring(0, pos), env[i].substring(pos + 1));
			}
		}
		// TODO map the exceptions
		return new DotNetProcess(cli.System.Diagnostics.Process.Start(si));
	}
}
