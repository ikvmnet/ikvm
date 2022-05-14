/*
 * Copyright (c) 2002, 2003, Oracle and/or its affiliates. All rights reserved.
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.  Oracle designates this
 * particular file as subject to the "Classpath" exception as provided
 * by Oracle in the LICENSE file that accompanied this code.
 *
 * This code is distributed in the hope that it will be useful, but WITHOUT
 * ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or
 * FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License
 * version 2 for more details (a copy is included in the LICENSE file that
 * accompanied this code).
 *
 * You should have received a copy of the GNU General Public License version
 * 2 along with this work; if not, write to the Free Software Foundation,
 * Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301 USA.
 *
 * Please contact Oracle, 500 Oracle Parkway, Redwood Shores, CA 94065 USA
 * or visit www.oracle.com if you need additional information or have any
 * questions.
 */

/*IKVM*/
/*
 * Modified for IKVM by Jeroen Frijters on May 22, 2007.
 * 
 * This is a merged version of the Windows & Solaris platform specific versions.
 * Since the IKVM class library binary can be used both on Windows and on *nix,
 * I've merged the platform specific classes into a generic class that at
 * runtime determines if it runs on Windows or not.
 * 
/*IKVM*/

package sun.misc;

import java.net.URL;
import java.io.File;
import sun.net.www.ParseUtil;

/**
 * Platform specific handling for file: URLs . In particular deals
 * with network paths mapping them to UNCs.
 *
 * @author      Michael McMahon
 * @version     1.10, 07/05/05
 */

public class FileURLMapper {
    private static final boolean runningOnWindows = cli.System.Environment.get_OSVersion().ToString().indexOf("Unix") == -1;
    URL url;
    String file;

    public FileURLMapper (URL url) {
        this.url = url;
    }

    /**
     * @returns the platform specific path corresponding to the URL, and in particular
     *  returns a UNC when the authority contains a hostname
     */

    public String getPath () {
        if (file != null) {
            return file;
        }
        if (runningOnWindows) {
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
        String path = getPath();
        if (path == null) {
            return false;
        }
        File f = new File (path);
        return f.exists();
    }
}
