/*
  Copyright (C) 2002-2015 Jeroen Frijters

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
using System.Reflection;

using IKVM.CoreLib.Exceptions;
using IKVM.Runtime;
using IKVM.Runtime.Accessors.Java.Lang;

namespace IKVM.Java.Externs.ikvm.runtime
{

    static class AssemblyClassLoader
    {

#if FIRST_PASS == false

        static ClassLoaderAccessor classLoaderAccessor;

        static ClassLoaderAccessor ClassLoaderAccessor => JVM.Internal.BaseAccessors.Get(ref classLoaderAccessor);

#endif

        public static void setWrapper(global::java.lang.ClassLoader _this, Assembly assembly)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            JVM.Context.ClassLoaderFactory.SetWrapperForClassLoader(_this, JVM.Context.AssemblyClassLoaderFactory.FromAssembly(assembly));
#endif
        }

        public static global::java.lang.Class loadClass(global::java.lang.ClassLoader _this, string name, bool resolve)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            try
            {
                if (ClassLoaderAccessor.InvokeCheckName(_this, name) == false)
                    throw new ClassNotFoundException(name);

                var wrapper = (RuntimeAssemblyClassLoader)JVM.Context.ClassLoaderFactory.GetClassLoaderWrapper(_this);
                var tw = wrapper.LoadClass(name);
                if (tw == null)
                {
                    JVM.Context.Diagnostics.GenericClassLoadingInfo($"Failed to load class \"{name}\" from {_this}");
                    global::java.lang.Throwable.suppressFillInStackTrace = true;
                    throw new global::java.lang.ClassNotFoundException(name);
                }

                JVM.Context.Diagnostics.GenericClassLoadingInfo($"Loaded class \"{name}\" from {_this}");
                return tw.ClassObject;
            }
            catch (ClassNotFoundException x)
            {
                JVM.Context.Diagnostics.GenericClassLoadingInfo($"Failed to load class \"{name}\" from {_this}: {x.Message}");
                throw new global::java.lang.ClassNotFoundException(x.Message);
            }
            catch (ClassLoadingException x)
            {
                JVM.Context.Diagnostics.GenericClassLoadingInfo($"Failed to load class \"{name}\" from {_this}: {x.Message}");
                throw x.InnerException;
            }
            catch (TranslatableJavaException e)
            {
                JVM.Context.Diagnostics.GenericClassLoadingInfo($"Failed to load class \"{name}\" from {_this}: {e.Message}");
                throw JVM.Context.ExceptionHelper.MapException<Exception>(e, true, false);
            }
#endif
        }

        public static global::java.net.URL getResource(global::java.lang.ClassLoader _this, string name)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var wrapper = (RuntimeAssemblyClassLoader)JVM.Context.ClassLoaderFactory.GetClassLoaderWrapper(_this);
            foreach (global::java.net.URL url in wrapper.GetResources(name))
                return url;

            return null;
#endif
        }

        public static global::java.util.Enumeration getResources(global::java.lang.ClassLoader _this, string name)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            return new global::ikvm.runtime.EnumerationWrapper(((RuntimeAssemblyClassLoader)JVM.Context.ClassLoaderFactory.GetClassLoaderWrapper(_this)).GetResources(name));
#endif
        }

        public static global::java.net.URL findResource(global::java.lang.ClassLoader _this, string name)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var wrapper = (RuntimeAssemblyClassLoader)JVM.Context.ClassLoaderFactory.GetClassLoaderWrapper(_this);
            foreach (global::java.net.URL url in wrapper.FindResources(name))
            {
                return url;
            }

            return null;
#endif
        }

        public static global::java.util.Enumeration findResources(global::java.lang.ClassLoader _this, string name)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            return new global::ikvm.runtime.EnumerationWrapper(((RuntimeAssemblyClassLoader)JVM.Context.ClassLoaderFactory.GetClassLoaderWrapper(_this)).FindResources(name));
#endif
        }

#if !FIRST_PASS

        static global::java.net.URL GetCodeBase(Assembly assembly)
        {
            try
            {
                if (assembly.Location != null)
                    return new global::java.io.File(assembly.Location).toURI().toURL();
            }
            catch (NotSupportedException)
            {

            }
            catch (global::java.net.MalformedURLException)
            {

            }

            return null;
        }

        /// <summary>
        /// Returns the first non-null value for the attribute in either the first attribute set or the second.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        static string GetAttributeValue(global::java.util.jar.Attributes.Name name, global::java.util.jar.Attributes first, global::java.util.jar.Attributes second)
        {
            return first?.getValue(name) ?? second?.getValue(name);
        }

#endif

#if !FIRST_PASS

        /// <summary>
        /// Gets the JAR manifest for the given assembly and resource name.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="resourceName"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        static global::java.util.jar.Manifest GetManifestForAssemblyJar(Assembly assembly, string resourceName)
        {
            if (assembly is null)
                throw new ArgumentNullException(nameof(assembly));
            if (string.IsNullOrEmpty(resourceName))
                throw new ArgumentException($"'{nameof(resourceName)}' cannot be null or empty.", nameof(resourceName));

            using var resStream = assembly.GetManifestResourceStream(resourceName);
            if (resStream == null)
                throw new InternalException($"Could not find assembly resource {resourceName}.");

            using var wrpStream = new global::ikvm.io.InputStreamWrapper(resStream);
            using var jarStream = new global::java.util.jar.JarInputStream(wrpStream);
            var manifest = jarStream.getManifest();

            return manifest;
        }

#endif

        /// <summary>
        /// Defines all of the packages in the class loader that are not currently defined, using the assembly as a source.
        /// </summary>
        /// <param name="_this"></param>
        public static void lazyDefinePackages(global::java.lang.ClassLoader _this)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var wrapper = (RuntimeAssemblyClassLoader)JVM.Context.ClassLoaderFactory.GetClassLoaderWrapper(_this);
            var sealBase = GetCodeBase(wrapper.MainAssembly);

            foreach (var packages in wrapper.GetPackageInfo())
            {
                // if the package exists in a JAR within the main assembly, load its manifest
                var manifest = packages.Key != null ? GetManifestForAssemblyJar(wrapper.MainAssembly, packages.Key) : null;

                // if we loaded a manifest, get the main attributes
                var mainAttributes = manifest?.getMainAttributes();

                // for each package, check if defined, else define
                foreach (var name in packages.Value)
                {
                    if (_this.getPackage(name) != null)
                        continue;

                    // manifest might contain an overriding entry for this specific package
                    var packageAttributes = manifest?.getAttributes(name.Replace('.', '/') + '/');

                    // define package with package information from package attributes or main attributes
                    _this.definePackage(
                        name,
                        GetAttributeValue(global::java.util.jar.Attributes.Name.SPECIFICATION_TITLE, packageAttributes, mainAttributes),
                        GetAttributeValue(global::java.util.jar.Attributes.Name.SPECIFICATION_VERSION, packageAttributes, mainAttributes),
                        GetAttributeValue(global::java.util.jar.Attributes.Name.SPECIFICATION_VENDOR, packageAttributes, mainAttributes),
                        GetAttributeValue(global::java.util.jar.Attributes.Name.IMPLEMENTATION_TITLE, packageAttributes, mainAttributes),
                        GetAttributeValue(global::java.util.jar.Attributes.Name.IMPLEMENTATION_VERSION, packageAttributes, mainAttributes),
                        GetAttributeValue(global::java.util.jar.Attributes.Name.IMPLEMENTATION_VENDOR, packageAttributes, mainAttributes),
                        string.Equals(GetAttributeValue(global::java.util.jar.Attributes.Name.SEALED, packageAttributes, mainAttributes), "true", StringComparison.OrdinalIgnoreCase) ? sealBase : null);
                }
            }
#endif
        }

        public static string toString(global::java.lang.ClassLoader _this)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            return ((RuntimeAssemblyClassLoader)JVM.Context.ClassLoaderFactory.GetClassLoaderWrapper(_this)).MainAssembly.FullName;
#endif
        }

        public static global::java.lang.ClassLoader getAssemblyClassLoader(Assembly asm)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            // note that we don't do a security check here, because if you have the Assembly object,
            // you can already get at all the types in it.
            return JVM.Context.AssemblyClassLoaderFactory.FromAssembly(asm).GetJavaClassLoader();
#endif
        }

    }

}
