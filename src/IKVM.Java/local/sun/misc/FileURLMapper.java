/*
 * This is a merged version of the Windows & Solaris platform specific versions.
 * Since the IKVM class library binary can be used both on Windows and on *nix,
 * I've merged the platform specific classes into a generic class that at
 * runtime determines if it runs on Windows or not.
 */

package sun.misc;

import java.net.URL;
import java.io.File;

import sun.net.www.ParseUtil;

public class FileURLMapper {

    URL url;
    String file;

    public FileURLMapper(URL url) {
        this.url = url;
    }

    /**
     * @returns the platform specific path corresponding to the URL, and in particular returns a UNC when the authority contains a hostname
     */
    public String getPath () {
        if (file != null) {
            return file;
        }

        if (cli.IKVM.Runtime.RuntimeUtil.get_IsWindows()) {
            String host = url.getHost();
            if (host != null && !host.equals("") &&
                !"localhost".equalsIgnoreCase(host)) {
                String rest = url.getFile();
                String s = host + ParseUtil.decode (url.getFile());
                file = "\\\\"+ s.replace('/', '\\');
                return file;
            }
            String path = url.getFile().replace('/', '\\');
            file = ParseUtil.decode(path);
            return file;
        } else {
            String host = url.getHost();
            if (host == null || "".equals(host) || "localhost".equalsIgnoreCase (host)) {
                file = url.getFile();
                file = ParseUtil.decode (file);
            }
            return file;
        }
    }
    
    public boolean exists() {
        String s = getPath();
        if (s == null) {
            return false;
        } else {
            File f = new File(s);
            return f.exists();
        }
    }

}
