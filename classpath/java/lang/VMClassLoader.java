/* VMClassLoader.java -- Reference implementation of native interface
   required by ClassLoader
   Copyright (C) 1998, 2001, 2002 Free Software Foundation

This file is part of GNU Classpath.

GNU Classpath is free software; you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation; either version 2, or (at your option)
any later version.

GNU Classpath is distributed in the hope that it will be useful, but
WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
General Public License for more details.

You should have received a copy of the GNU General Public License
along with GNU Classpath; see the file COPYING.  If not, write to the
Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA
02111-1307 USA.

Linking this library statically or dynamically with other modules is
making a combined work based on this library.  Thus, the terms and
conditions of the GNU General Public License cover the whole
combination.

As a special exception, the copyright holders of this library give you
permission to link this library with independent modules to produce an
executable, regardless of the license terms of these independent
modules, and to copy and distribute the resulting executable under
terms of your choice, provided that you also meet, for each linked
independent module, the terms and conditions of the license of that
module.  An independent module is a module which is not derived from
or based on this library.  If you modify this library, you may extend
this exception to your version of the library, but you are not
obligated to do so.  If you do not wish to do so, delete this
exception statement from your version. */

package java.lang;

import java.security.ProtectionDomain;
import java.net.URL;
import java.io.IOException;
import java.util.Enumeration;
import java.util.Map;
import java.util.HashMap;
import java.util.ArrayList;
import java.util.Collection;
import java.util.StringTokenizer;
import java.lang.reflect.Constructor;
import gnu.classpath.SystemProperties;
import gnu.java.util.DoubleEnumeration;
import cli.System.*;
import cli.System.Reflection.*;

/**
 * java.lang.VMClassLoader is a package-private helper for VMs to implement
 * on behalf of java.lang.ClassLoader.
 *
 * @author John Keiser
 * @author Mark Wielaard <mark@klomp.org>
 * @author Eric Blake <ebb9@email.byu.edu>
 * @author Jeroen Frijters
 */
final class VMClassLoader
{
    /**
     * Helper to define a class using a string of bytes. This assumes that
     * the security checks have already been performed, if necessary.
     * <strong>This method will be removed in a future version of GNU
     * Classpath</strong>.
     *
     * @param name the name to give the class, or null if unknown
     * @param data the data representing the classfile, in classfile format
     * @param offset the offset into the data where the classfile starts
     * @param len the length of the classfile data in the array
     * @return the class that was defined
     * @throws ClassFormatError if data is not in proper classfile format
     * @deprecated Implement
     * {@link #defineClass(ClassLoader, String, byte[], int, int, ProtectionDomain)}
     *   instead.
     */
    static Class defineClass(ClassLoader cl, String name, byte[] data, int offset, int len)
    {
	return defineClass(cl, name, data, offset, len, null);
    }

    /**
     * Helper to define a class using a string of bytes. This assumes that
     * the security checks have already been performed, if necessary.
     *
     * @param name the name to give the class, or null if unknown
     * @param data the data representing the classfile, in classfile format
     * @param offset the offset into the data where the classfile starts
     * @param len the length of the classfile data in the array
     * @param pd the protection domain
     * @return the class that was defined
     * @throws ClassFormatError if data is not in proper classfile format
     */
    static Class defineClass(ClassLoader cl, String name, byte[] data, int offset, int len, ProtectionDomain pd)
    {
        try
        {
            return defineClassImpl(cl, name, data, offset, len, pd);
        }
        catch(ClassNotFoundException x)
        {
            throw new NoClassDefFoundError(x.getMessage());
        }
    }

    private static native Class defineClassImpl(ClassLoader cl, String name, byte[] data, int offset, int len, ProtectionDomain pd)
        throws ClassNotFoundException;

    /**
     * Helper to resolve all references to other classes from this class.
     *
     * @param c the class to resolve
     */
    static void resolveClass(Class c)
    {
    }

    /**
     * Helper to load a class from the bootstrap class loader.
     *
     * @param name the class name to load
     * @param resolve whether to resolve it
     * @return the class, loaded by the bootstrap classloader
     */
    static native Class loadClass(String name, boolean resolve) throws ClassNotFoundException;

    private static native ClassLoader getBootstrapClassLoader();

    /**
     * Helper to load a resource from the bootstrap class loader.
     *
     * @param name the resource to find
     * @return the URL to the resource
     */
    static URL getResource(String name)
    {
	try
	{
	    Assembly assembly = findResourceAssembly(name);
	    if(assembly != null)
	    {
		return new URL("ikvmres", assembly.get_FullName(), -1, "/" + name);
	    }
	    ClassLoader bootstrap = getBootstrapClassLoader();
	    if(bootstrap != null)
	    {
		URL url = bootstrap.findResource(name);
		if(url != null)
		{
		    return url;
		}
	    }
	}
	catch(java.net.MalformedURLException x)
	{
	}
	return null;
    }
    private static native Assembly findResourceAssembly(String name);
    private static native Assembly[] findResourceAssemblies(String name);

    /**
     * Helper to get a list of resources from the bootstrap class loader.
     *
     * @param name the resource to find
     * @return an enumeration of resources
     * @throws IOException if one occurs
     */
    static Enumeration getResources(String name) throws IOException
    {
	if(__tls_nestedGetResourcesHack)
	{
	    return gnu.java.util.EmptyEnumeration.getInstance();
	}
	__tls_nestedGetResourcesHack = true;
	try
	{
	    Assembly[] assemblies = findResourceAssemblies(name);
	    java.util.Vector v = new java.util.Vector();
	    for(int i = 0; i < assemblies.length; i++)
	    {
		v.addElement(new URL("ikvmres", assemblies[i].get_FullName(), -1, "/" + name));
	    }
	    Enumeration e = v.elements();
	    ClassLoader bootstrap = getBootstrapClassLoader();
	    if(bootstrap != null)
	    {
		e = new DoubleEnumeration(e, bootstrap.getResources(name));
	    }
	    return e;
	}
	finally
	{
	    __tls_nestedGetResourcesHack = false;
	}
    }

    private static boolean __tls_nestedGetResourcesHack;

    /**
     * Helper to get a package from the bootstrap class loader.  The default
     * implementation of returning null may be adequate, or you may decide
     * that this needs some native help.
     *
     * @param name the name to find
     * @return the named package, if it exists
     */
    static Package getPackage(String name)
    {
        Package[] packages = getPackagesImpl();
        for(int i = 0; i < packages.length; i++)
        {
            if(packages[i].getName().equals(name))
            {
                return packages[i];
            }
        }
	return null;
    }

    /**
     * Helper to get all packages from the bootstrap class loader.  The default
     * implementation of returning an empty array may be adequate, or you may
     * decide that this needs some native help.
     *
     * @return all named packages, if any exist
     */
    static Package[] getPackages()
    {
        return (Package[])getPackagesImpl().clone();
    }

    private static Package[] getPackagesImpl()
    {
        Package[] packages = packageCache;
        if(packages == null)
        {
            ClassLoader boot = getBootstrapClassLoader();
            if(boot != null && __tls_nestedGetResourcesHack)
            {
                return new Package[0];
            }
            HashMap h = new HashMap();
            Assembly[] assemblies = AppDomain.get_CurrentDomain().GetAssemblies();
            for(int i = 0; i < assemblies.length; i++)
            {
                if(!(assemblies[i] instanceof cli.System.Reflection.Emit.AssemblyBuilder))
                {
                    Type[] types = assemblies[i].GetTypes();
                    for(int j = 0; j < types.length; j++)
                    {
                        String name = getPackageName(types[j]);
                        if(name != null)
                        {
                            // TODO fill out more package details
                            h.put(name, new Package(name, null, null, null, null, null, null, null));
                        }
                    }
                }
            }
            if(boot != null)
            {
                Package[] pkgboot;
                __tls_nestedGetResourcesHack = true;
                try
                {
                    pkgboot = boot.getPackages();
                }
                finally
                {
                    __tls_nestedGetResourcesHack = false;
                }
                Collection c = h.values();
                packages = new Package[c.size() + pkgboot.length];
                c.toArray(packages);
                VMSystem.arraycopy(pkgboot, 0, packages, c.size(), pkgboot.length);
                // we don't cache the result, because we don't know when the bootstrap class loader loads a new package
            }
            else
            {
                Collection c = h.values();
                packages = new Package[c.size()];
                c.toArray(packages);
                if(enablePackageCaching)
                {
                    packageCache = packages;
                }
            }
        }
        return packages;        
    }

    private static volatile Package[] packageCache;
    private static final boolean enablePackageCaching;

    private static void hookUpAssemblyLoadEvent()
        throws cli.System.Security.SecurityException
    {
        AppDomain.get_CurrentDomain().add_AssemblyLoad(new AssemblyLoadEventHandler(
            new AssemblyLoadEventHandler.Method() {
                public void Invoke(Object sender, AssemblyLoadEventArgs args) {
                    packageCache = null;
                }
            }));
    }

    static
    {
        boolean enable = false;
        try
        {
            // add_AssemblyLoad has a LinkDemand, so we need to do it in
            // a seperate method
            hookUpAssemblyLoadEvent();
            enable = true;
        }
        catch(cli.System.Security.SecurityException _)
        {
            // if we don't have the ControlAppDomain permission we can't hook
            // the event, so we don't enable package caching
        }
        enablePackageCaching = enable;
    }

    private static native String getPackageName(Type type);

    /**
     * Helper for java.lang.Integer, Byte, etc to get the TYPE class
     * at initialization time. The type code is one of the chars that
     * represents the primitive type as in JNI.
     *
     * <ul>
     * <li>'Z' - boolean</li>
     * <li>'B' - byte</li>
     * <li>'C' - char</li>
     * <li>'D' - double</li>
     * <li>'F' - float</li>
     * <li>'I' - int</li>
     * <li>'J' - long</li>
     * <li>'S' - short</li>
     * <li>'V' - void</li>
     * </ul>
     *
     * Note that this is currently a java version that converts the type code
     * to a string and calls the native <code>getPrimitiveClass(String)</code>
     * method for backwards compatibility with VMs that used old versions of
     * GNU Classpath. Please replace this method with a native method
     * <code>final static native Class getPrimitiveClass(char type);</code>
     * if your VM supports it. <strong>The java version of this method and
     * the String version of this method will disappear in a future version
     * of GNU Classpath</strong>.
     *
     * @param type the primitive type
     * @return a "bogus" class representing the primitive type
     */
    static native Class getPrimitiveClass(char type);

    /**
     * The system default for assertion status. This is used for all system
     * classes (those with a null ClassLoader), as well as the initial value for
     * every ClassLoader's default assertion status.
     *
     * @return the system-wide default assertion status
     */
    static boolean defaultAssertionStatus()
    {
	return Boolean.valueOf(SystemProperties.getProperty("ikvm.assert.default", "false")).booleanValue();
    }

    /**
     * The system default for package assertion status. This is used for all
     * ClassLoader's packageAssertionStatus defaults. It must be a map of
     * package names to Boolean.TRUE or Boolean.FALSE, with the unnamed package
     * represented as a null key.
     *
     * @return a (read-only) map for the default packageAssertionStatus
     */
    static Map packageAssertionStatus()
    {
	if(packageAssertionMap == null)
	{
	    HashMap m = new HashMap();
	    String enable = SystemProperties.getProperty("ikvm.assert.enable", null);
	    if(enable != null)
	    {
		StringTokenizer st = new StringTokenizer(enable, ":");
		while(st.hasMoreTokens())
		{
		    m.put(st.nextToken(), Boolean.TRUE);
		}
	    }
	    String disable = SystemProperties.getProperty("ikvm.assert.disable", null);
	    if(disable != null)
	    {
		StringTokenizer st = new StringTokenizer(disable, ":");
		while(st.hasMoreTokens())
		{
		    m.put(st.nextToken(), Boolean.FALSE);
		}
	    }
	    packageAssertionMap = m;
	}
	return packageAssertionMap;
    }
    private static Map packageAssertionMap;

    /**
     * The system default for class assertion status. This is used for all
     * ClassLoader's classAssertionStatus defaults. It must be a map of
     * class names to Boolean.TRUE or Boolean.FALSE
     *
     * @return a (read-only) map for the default classAssertionStatus
     */
    static Map classAssertionStatus()
    {
	// there is no distinction between the package and the class assertion status map
	// (because the command line options don't make the distinction either)
	return packageAssertionStatus();
    }

    static ClassLoader getSystemClassLoader()
    {
        if("".equals(SystemProperties.getProperty("java.class.path")) &&
            "".equals(SystemProperties.getProperty("java.ext.dirs")))
        {
            // to support running in partial trust (without file access) we special
            // case the "no class path" scenario (because the default code will assume
            // a "." class path and fail when it tries to convert the URL to a file path)
            return ClassLoader.createAuxiliarySystemClassLoader(
                ClassLoader.createSystemClassLoader(new URL[0], null));
        }
	return ClassLoader.defaultGetSystemClassLoader();
    }
}
