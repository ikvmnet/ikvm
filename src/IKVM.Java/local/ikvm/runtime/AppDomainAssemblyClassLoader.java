package ikvm.runtime;

import cli.System.AppDomain;
import cli.System.Reflection.Assembly;

import java.io.IOException;
import java.net.URL;
import java.util.Enumeration;
import java.util.Vector;

public final class AppDomainAssemblyClassLoader extends ClassLoader {

    public AppDomainAssemblyClassLoader(Assembly assembly) {
        super(new AssemblyClassLoader(assembly));
    }

    protected native Class findClass(String name) throws ClassNotFoundException;

    protected native URL findResource(String name);

    @Override
    public Enumeration<URL> getResources(String name) throws IOException {
        return concatResources(super.getResources(name), name);
    }

    private native Enumeration<URL> concatResources(Enumeration<URL> parent, String name);

}
