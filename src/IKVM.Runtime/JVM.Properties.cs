using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;

using IKVM.Runtime.Vfs;

namespace IKVM.Runtime
{

	static partial class JVM
	{

		/// <summary>
		/// Property values loaded into the JVM from various sources.
		/// </summary>
		public static class Properties
		{

			/// <summary>
			/// Represents an entry in the IKVM properties.
			/// </summary>
			internal record struct IkvmPropEntry(string BasePath, string Value);

			static readonly IDictionary<string, string> user = new Dictionary<string, string>();
			static readonly Lazy<Dictionary<string, IkvmPropEntry>> ikvm = new Lazy<Dictionary<string, IkvmPropEntry>>(GetIkvmProperties);
			static readonly Lazy<Dictionary<string, string>> init = new Lazy<Dictionary<string, string>>(GetInitProperties);
			static readonly Lazy<string> homePath = new Lazy<string>(GetHomePath);

			/// <summary>
			/// Gets the set of properties that are set by the user before initialization. Modification of values in this
			/// dictionary must happen early in the program's initialization before any Java code has been accessed or
			/// run.
			/// </summary>
			public static IDictionary<string, string> User => user;

			/// <summary>
			/// Gets the set of properties that are set in the 'ikvm.properties' file before initialization.
			/// </summary>
			internal static IReadOnlyDictionary<string, IkvmPropEntry> Ikvm => ikvm.Value;

			/// <summary>
			/// Gets the set of properties that are initialized with the JVM and provided to the JDK.
			/// </summary>
			internal static IReadOnlyDictionary<string, string> Init => init.Value;

			/// <summary>
			/// Gets the home path.
			/// </summary>
			internal static string HomePath => homePath.Value;

			/// <summary>
			/// Gets the raw search paths to examine for ikvm.properties. 
			/// </summary>
			/// <returns></returns>
			static IEnumerable<string> GetIkvmPropertiesSearchPathsIter()
			{
				if (AppContext.BaseDirectory is string basePath && !string.IsNullOrEmpty(basePath))
					yield return basePath;

				if (AppDomain.CurrentDomain.BaseDirectory is string appBasePath && !string.IsNullOrEmpty(appBasePath))
					yield return appBasePath;

				// search upwards from the location of IKVM.Runtime
				// we do this because IKVM.Runtime may be in runtimes/{rid}/lib
				if (typeof(Properties).Assembly.Location is string runtimeAssemblyPath && !string.IsNullOrEmpty(runtimeAssemblyPath))
					foreach (var parent in GetParentDirs(runtimeAssemblyPath))
						yield return parent;
			}

			/// <summary>
			/// Returns an iteration of each parent path of the given path until the root.
			/// </summary>
			/// <param name="path"></param>
			/// <returns></returns>
			static IEnumerable<string> GetParentDirs(string path)
			{
				while (string.IsNullOrWhiteSpace(path = Path.GetDirectoryName(path)) == false)
					yield return path;
			}

			/// <summary>
			/// Gets the unique search paths to examine for ikvm.properties. 
			/// </summary>
			/// <returns></returns>
			static IEnumerable<string> GetIkvmPropertiesSearchPaths()
			{
				return GetIkvmPropertiesSearchPathsIter().Distinct();
			}

			/// <summary>
			/// Gets the set of properties loaded from any companion 'ikvm.properties' file.
			/// </summary>
			/// <returns></returns>
			static Dictionary<string, IkvmPropEntry> GetIkvmProperties()
			{
				var props = new Dictionary<string, IkvmPropEntry>();

				foreach (var basePath in GetIkvmPropertiesSearchPaths())
				{
					var ikvmPropertiesPath = Path.Combine(basePath, "ikvm.properties");
					if (File.Exists(ikvmPropertiesPath))
					{
						LoadProperties(basePath, File.ReadAllLines(ikvmPropertiesPath), props);
						break;
					}
				}

				return props;
			}

			/// <summary>
			/// Gets the home path for IKVM.
			/// </summary>
			/// <returns></returns>
			static string GetHomePath()
			{
				// user value takes priority
				if (User.TryGetValue("ikvm.home", out var userHomePath) && !string.IsNullOrWhiteSpace(userHomePath))
					if (Directory.Exists(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, userHomePath))))
						return Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, userHomePath));

				// ikvm properties value comes next
				if (Ikvm.TryGetValue("ikvm.home", out var ikvmHomeEntry) && !string.IsNullOrWhiteSpace(ikvmHomeEntry.Value))
					if (Directory.Exists(Path.GetFullPath(Path.Combine(ikvmHomeEntry.BasePath, ikvmHomeEntry.Value))))
						return Path.GetFullPath(Path.Combine(ikvmHomeEntry.BasePath, ikvmHomeEntry.Value));

#if NET
                // specified home directory in runtime.json, where path is relative to application
                if (AppContext.GetData("IKVM.Home") is string confHome && !string.IsNullOrWhiteSpace(confHome))
                    if (Directory.Exists(Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, confHome))))
                        return Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, confHome));
#endif

#if NETFRAMEWORK
                // attempt to find settings in legacy app.config
                try
                {
                    // specified home directory in app.config relative to base directory
                    if (ConfigurationManager.AppSettings["ikvm:ikvm.home"] is string confHome && !string.IsNullOrWhiteSpace(confHome))
                        if (Directory.Exists(Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, confHome))))
                            return Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, confHome));
                }
                catch (ConfigurationException)
                {
                    // app.config is invalid, ignore
                }
#endif

				// find first occurance of home root
				if (User.TryGetValue("ikvm.home.root", out var userHomeRoot) && !string.IsNullOrWhiteSpace(userHomeRoot))
					if (ResolveHomePathFromRoot(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, userHomeRoot))) is string userHomeRootPath)
						return userHomeRootPath;

				// ikvm properties value comes next
				if (Ikvm.TryGetValue("ikvm.home.root", out var ikvmHomeRootEntry) && !string.IsNullOrWhiteSpace(ikvmHomeRootEntry.Value))
					if (ResolveHomePathFromRoot(Path.GetFullPath(Path.Combine(ikvmHomeRootEntry.BasePath, ikvmHomeRootEntry.Value))) is string ikvmHomeRootPath)
						return ikvmHomeRootPath;

#if NET
                // specified home root directory in runtime.json, where path is relative to application
                if (AppContext.GetData("IKVM.Home.Root") is string confHomeRoot && !string.IsNullOrWhiteSpace(confHomeRoot))
                    if (ResolveHomePathFromRoot(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, confHomeRoot)) is string confHomeRootPath)
                        return confHomeRootPath;
#endif

#if NETFRAMEWORK
                // attempt to find settings in legacy app.config
                try
                {
                    // specified home root directory in app.config relative to base directory
                    if (ConfigurationManager.AppSettings["ikvm:ikvm.home.root"] is string confHomeRoot && !string.IsNullOrWhiteSpace(confHomeRoot))
                        if (ResolveHomePathFromRoot(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, confHomeRoot)) is string confHomeRootPath)
                            return confHomeRootPath;
                }
                catch (ConfigurationException)
                {
                    // app.config is invalid, ignore
                }
#endif

				// fallback to directory in base dir
				if (string.IsNullOrWhiteSpace(AppContext.BaseDirectory) == false)
					if (ResolveHomePathFromRoot(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "ikvm"))) is string appHomeRootPath)
						return appHomeRootPath;

				// fallback to directory in base dir
				if (string.IsNullOrWhiteSpace(AppDomain.CurrentDomain.BaseDirectory) == false)
					if (ResolveHomePathFromRoot(Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ikvm"))) is string domainHomeRootPath)
						return domainHomeRootPath;

				throw new InternalException("Could not locate ikvm home path.");
			}

			/// <summary>
			/// Scans the given root path for the home for the currently executing runtime.
			/// </summary>
			/// <param name="homePathRoot"></param>
			/// <returns></returns>
			static string ResolveHomePathFromRoot(string homePathRoot)
			{
				// calculate ikvm.home from ikvm.home.root
				if (Directory.Exists(homePathRoot))
				{
					foreach (var rid in RuntimeUtil.SupportedRuntimeIdentifiers)
					{
						var ikvmHomePath = Path.GetFullPath(Path.Combine(homePathRoot, rid));
						if (Directory.Exists(ikvmHomePath))
							return ikvmHomePath;
					}
				}

				return null;
			}

			/// <summary>
			/// Reads the property lines from the specified file into the dictionary.
			/// </summary>
			/// <param name="lines"></param>
			/// <param name="props"></param>
			static void LoadProperties(string basePath, IEnumerable<string> lines, Dictionary<string, IkvmPropEntry> props)
			{
				foreach (var l in lines)
				{
					var a = l.Split(new[] { '=' }, 2, StringSplitOptions.RemoveEmptyEntries);
					if (a.Length >= 2)
						props[a[0].Trim()] = new IkvmPropEntry(basePath, a[1]?.Trim() ?? "");
				}
			}

			/// <summary>
			/// Gets the set of init properties.
			/// </summary>
			/// <returns></returns>
			static Dictionary<string, string> GetInitProperties()
			{
#if FIRST_PASS || IMPORTER || EXPORTER
                throw new NotImplementedException();
#else
				var p = new Dictionary<string, string>();
				InitSystemProperties(p);
				return p;
#endif
			}

			/// <summary>
			/// Initialize system properties key and value.
			/// </summary>
			/// <param name="p"></param>
			static void InitSystemProperties(Dictionary<string, string> p)
			{
#if FIRST_PASS || IMPORTER || EXPORTER
        throw new NotImplementedException();
#else
        p["openjdk.version"] = Constants.openjdk_version;
        p["java.vm.name"] = Constants.java_vm_name;
        p["java.vm.version"] = Constants.java_vm_version;
        p["java.vm.vendor"] = Constants.java_vm_vendor;
        p["java.vm.specification.name"] = "Java Virtual Machine Specification";
        p["java.vm.specification.version"] = Constants.java_vm_specification_version;
        p["java.vm.specification.vendor"] = Constants.java_vm_specification_vendor;
        p["java.vm.info"] = "compiled mode";
        p["java.runtime.name"] = Constants.java_runtime_name;
        p["java.runtime.version"] = Constants.java_runtime_version;

        // various directory paths
        p["ikvm.home"] = HomePath;
        p["java.home"] = HomePath;
        p["java.library.path"] = GetLibraryPath();
        p["java.ext.dirs"] = Path.Combine(HomePath, "lib", "ext");
        p["java.endorsed.dirs"] = Path.Combine(HomePath, "lib", "endorsed");
        p["sun.boot.library.path"] = GetBootLibraryPath();
        p["sun.boot.class.path"] = VfsTable.GetAssemblyClassesPath(Vfs.Context, Context.Resolver.ResolveBaseAssembly().AsReflection(), HomePath);
        p["sun.cds.enableSharedLookupCache"] = "false";

        // unlimited direct memory
        p["sun.nio.MaxDirectMemorySize"] = "-1";

        // default to FORK on OSX, instead of posix_spawn with jspawnhelper
        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            p["jdk.lang.Process.launchMechanism"] = "FORK";

#if NETFRAMEWORK
                // read properties from app.config
                try
                {
                    foreach (string key in ConfigurationManager.AppSettings)
                        if (key.StartsWith("ikvm:"))
                            p[key.Substring(5)] = ConfigurationManager.AppSettings[key];
                }
                catch (ConfigurationException)
                {
                    // app.config is invalid, ignore
                }
#endif

#if NET
                if (AppContext.GetData("IKVM.Properties") is string ikvmPropertiesContext && !string.IsNullOrWhiteSpace(ikvmPropertiesContext))
                {
                    foreach (var ikvmSystemProperty in ikvmPropertiesContext.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                    {
                        if (AppContext.GetData(ikvmSystemProperty) is string ikvmPropertyValue)
                        {
                            p[ikvmSystemProperty] = ikvmPropertyValue;
                        }
                    }
                }
#endif

				// set the properties that were specfied
				if (user != null)
					foreach (var kvp in user)
						p[kvp.Key] = kvp.Value;
#endif
			}

			/// <summary>
			/// Gets the path string for loading native libraries.
			/// </summary>
			/// <returns></returns>
			static string GetLibraryPath()
			{
#if FIRST_PASS || IMPORTER || EXPORTER
                throw new NotImplementedException();
#else
				var libraryPath = new List<string>();

				if (RuntimeUtil.IsWindows)
				{
					// see /hotspot/src/os/windows/vm/os_windows.cpp for the comment that describes how we build the path
					var windir = SafeGetEnvironmentVariable("SystemRoot");
					if (windir != null)
						libraryPath.Add(Path.Combine(windir, "Sun", "Java", "bin"));

					try
					{
						libraryPath.Add(Environment.SystemDirectory);
					}
					catch (SecurityException)
					{

					}

					if (windir != null)
						libraryPath.Add(windir);

					var path = SafeGetEnvironmentVariable("PATH");
					if (path != null)
						foreach (var i in path.Split(Path.PathSeparator))
							libraryPath.Add(i);
				}

				if (RuntimeUtil.IsLinux)
				{
					// on Linux we have some hardcoded paths (from /hotspot/src/os/linux/vm/os_linux.cpp)
					// and we can only guess the cpu arch based on bitness (that means only x86 and x64)
					libraryPath.Add(Path.Combine("/usr/java/packages/lib/", IntPtr.Size == 4 ? "i386" : "amd64"));
					libraryPath.Add("/lib");
					libraryPath.Add("/usr/lib");

					// prefix with LD_LIBRARY_PATH
					var ld_library_path = SafeGetEnvironmentVariable("LD_LIBRARY_PATH");
					if (ld_library_path != null)
					{
						var l = ld_library_path.Split(Path.PathSeparator);
						l.Reverse();
                        foreach (var i in l)
							libraryPath.Insert(0, i);
					}
				}

				if (RuntimeUtil.IsOSX)
				{
					var home = SafeGetEnvironmentVariable("HOME");
					if (home != null)
						libraryPath.Add(Path.Combine(home, "Library/Java/Extensions"));

					libraryPath.Add("/Library/Java/Extensions");
					libraryPath.Add("/Network/Library/Java/Extensions");
					libraryPath.Add("/System/Library/Java/Extensions");
					libraryPath.Add("/usr/lib/java");

					// prefix with JAVA_LIBRARY_PATH
					var javaLibraryPath = SafeGetEnvironmentVariable("JAVA_LIBRARY_PATH");
					if (javaLibraryPath != null)
						foreach (var i in javaLibraryPath.Split(Path.PathSeparator))
							libraryPath.Add(i);

					// prefix with DYLD_LIBRARY_PATH
					var dyldLibraryPath = SafeGetEnvironmentVariable("DYLD_LIBRARY_PATH");
					if (dyldLibraryPath != null)
						foreach (var i in dyldLibraryPath.Split(Path.PathSeparator))
							libraryPath.Add(i);

					if (home != null)
						libraryPath.Add(home);

					libraryPath.Add(".");
				}

				try
				{
					var l = new List<string>();

					foreach (var d in GetIkvmPropertiesSearchPaths())
					{
						l.Add(d);
						foreach (var rid in RuntimeUtil.SupportedRuntimeIdentifiers)
							l.Add(Path.Combine(d, "runtimes", rid, "native"));
					}

					libraryPath.InsertRange(0, l);
				}
				catch (Exception)
				{
					// ignore
				}

				if (RuntimeUtil.IsWindows)
					libraryPath.Add(".");

				return string.Join(Path.PathSeparator.ToString(), libraryPath.Distinct());
#endif
			}

			/// <summary>
			/// Gets the boot library paths.
			/// </summary>
			/// <returns></returns>
			static IEnumerable<string> GetBootLibraryPathsIter()
			{
				yield return Path.Combine(HomePath, "bin");
			}

			/// <summary>
			/// Gets the boot library paths 
			/// </summary>
			/// <returns></returns>
			static string GetBootLibraryPath()
			{
				return string.Join(Path.PathSeparator.ToString(), GetBootLibraryPathsIter());
			}

		}

	}

}
