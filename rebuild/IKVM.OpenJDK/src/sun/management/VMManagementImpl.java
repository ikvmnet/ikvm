/*
 * Copyright (c) 2003, 2013, Oracle and/or its affiliates. All rights reserved.
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

package sun.management;

import sun.misc.Perf;
import sun.management.counter.*;
import java.nio.ByteBuffer;
import java.io.IOException;
import java.net.InetAddress;
import java.net.UnknownHostException;
import java.util.List;
import java.util.Arrays;
import java.util.Collections;
import java.security.AccessController;
import java.security.PrivilegedAction;
import sun.security.action.GetPropertyAction;

/**
 * Implementation of VMManagement interface that accesses the management
 * attributes and operations locally within the same Java virtual
 * machine.
 */
class VMManagementImpl implements VMManagement {

    private static String version = "1.2";

    private static boolean compTimeMonitoringSupport;
    private static boolean threadContentionMonitoringSupport;
    private static boolean currentThreadCpuTimeSupport = true;
    private static boolean otherThreadCpuTimeSupport;
    private static boolean bootClassPathSupport;
    private static boolean objectMonitorUsageSupport;
    private static boolean synchronizerUsageSupport;
    private static boolean threadAllocatedMemorySupport;
    private static boolean gcNotificationSupport;
    private static boolean remoteDiagnosticCommandsSupport;

    // Optional supports
    public boolean isCompilationTimeMonitoringSupported() {
        return compTimeMonitoringSupport;
    }

    public boolean isThreadContentionMonitoringSupported() {
        return threadContentionMonitoringSupport;
    }

    public boolean isCurrentThreadCpuTimeSupported() {
        return currentThreadCpuTimeSupport;
    }

    public boolean isOtherThreadCpuTimeSupported() {
        return otherThreadCpuTimeSupport;
    }

    public boolean isBootClassPathSupported() {
        return bootClassPathSupport;
    }

    public boolean isObjectMonitorUsageSupported() {
        return objectMonitorUsageSupport;
    }

    public boolean isSynchronizerUsageSupported() {
        return synchronizerUsageSupport;
    }

    public boolean isThreadAllocatedMemorySupported() {
        return threadAllocatedMemorySupport;
    }

    public boolean isGcNotificationSupported() {
        return gcNotificationSupport;
    }

    public boolean isRemoteDiagnosticCommandsSupported() {
        return remoteDiagnosticCommandsSupport;
    }

    public boolean isThreadContentionMonitoringEnabled() {
        return false;
    }

    public boolean isThreadCpuTimeEnabled() {
        return true;
    }

    public boolean isThreadAllocatedMemoryEnabled() {
        return false;
    }

    // Class Loading Subsystem
    public int    getLoadedClassCount() {
        long count = getTotalClassCount() - getUnloadedClassCount();
        return (int) count;
    }
    public long getTotalClassCount() {
        throw new Error("Not implemented");
    }
    public long getUnloadedClassCount() {
        throw new Error("Not implemented");
    }

    public boolean getVerboseClass() {
        return false;
    }

    // Memory Subsystem
    public boolean getVerboseGC() {
        return false;
    }

    // Runtime Subsystem
    public String   getManagementVersion() {
        return version;
    }

    public String getVmId() {
        int pid = getProcessId();
        String hostname = "localhost";
        try {
            hostname = InetAddress.getLocalHost().getHostName();
        } catch (UnknownHostException e) {
            // ignore
        }

        return pid + "@" + hostname;
    }
    private int getProcessId() {
        return cli.System.Diagnostics.Process.GetCurrentProcess().get_Id();
    }

    public String   getVmName() {
        return System.getProperty("java.vm.name");
    }

    public String   getVmVendor() {
        return System.getProperty("java.vm.vendor");
    }
    public String   getVmVersion() {
        return System.getProperty("java.vm.version");
    }
    public String   getVmSpecName()  {
        return System.getProperty("java.vm.specification.name");
    }
    public String   getVmSpecVendor() {
        return System.getProperty("java.vm.specification.vendor");
    }
    public String   getVmSpecVersion() {
        return System.getProperty("java.vm.specification.version");
    }
    public String   getClassPath() {
        return System.getProperty("java.class.path");
    }
    public String   getLibraryPath()  {
        return System.getProperty("java.library.path");
    }

    public String   getBootClassPath( ) {
        PrivilegedAction<String> pa
            = new GetPropertyAction("sun.boot.class.path");
        String result =  AccessController.doPrivileged(pa);
        return result;
    }

    public long getUptime() {
        return cli.System.DateTime.get_Now().Subtract(cli.System.Diagnostics.Process.GetCurrentProcess().get_StartTime()).get_Ticks() / 10000L;
    }

    private List<String> vmArgs = null;
    public synchronized List<String> getVmArguments() {
        if (vmArgs == null) {
            String[] args = getVmArguments0();
            List<String> l = ((args != null && args.length != 0) ? Arrays.asList(args) :
                                        Collections.<String>emptyList());
            vmArgs = Collections.unmodifiableList(l);
        }
        return vmArgs;
    }
    public String[] getVmArguments0() {
        return new String[0];
    }

    public long getStartupTime() {
        return (long)(cli.System.Diagnostics.Process.GetCurrentProcess().get_StartTime().ToUniversalTime().Subtract(new cli.System.DateTime(1970, 1, 1))).get_TotalMilliseconds();
    }
    public int getAvailableProcessors() {
        return cli.System.Environment.get_ProcessorCount();
    }

    // Compilation Subsystem
    public String   getCompilerName() {
        String name =  AccessController.doPrivileged(
            new PrivilegedAction<String>() {
                public String run() {
                    return System.getProperty("sun.management.compiler");
                }
            });
        return name;
    }
    public long getTotalCompileTime() {
        throw new Error("Not implemented");
    }

    // Thread Subsystem
    public long getTotalThreadCount() {
        throw new Error("Not implemented");
    }
    public int  getLiveThreadCount() {
        throw new Error("Not implemented");
    }
    public int  getPeakThreadCount() {
        throw new Error("Not implemented");
    }
    public int  getDaemonThreadCount() {
        throw new Error("Not implemented");
    }

    // Operating System
    public String getOsName() {
        return System.getProperty("os.name");
    }
    public String getOsArch() {
        return System.getProperty("os.arch");
    }
    public String getOsVersion() {
        return System.getProperty("os.version");
    }

    // Hotspot-specific runtime support
    public long getSafepointCount() {
        throw new Error("Not implemented");
    }
    public long getTotalSafepointTime() {
        throw new Error("Not implemented");
    }
    public long getSafepointSyncTime() {
        throw new Error("Not implemented");
    }
    public long getTotalApplicationNonStoppedTime() {
        throw new Error("Not implemented");
    }

    public long getLoadedClassSize() {
        throw new Error("Not implemented");
    }
    public long getUnloadedClassSize() {
        throw new Error("Not implemented");
    }
    public long getClassLoadingTime() {
        throw new Error("Not implemented");
    }
    public long getMethodDataSize() {
        throw new Error("Not implemented");
    }
    public long getInitializedClassCount() {
        throw new Error("Not implemented");
    }
    public long getClassInitializationTime() {
        throw new Error("Not implemented");
    }
    public long getClassVerificationTime() {
        throw new Error("Not implemented");
    }

    public List<Counter> getInternalCounters(String pattern) {
        return Collections.emptyList();
    }
}
