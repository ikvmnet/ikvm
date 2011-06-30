/*
 * Copyright (c) 2003, 2008, Oracle and/or its affiliates. All rights reserved.
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

import java.lang.management.*;

import javax.management.InstanceAlreadyExistsException;
import javax.management.InstanceNotFoundException;
import javax.management.MBeanServer;
import javax.management.MBeanRegistrationException;
import javax.management.NotCompliantMBeanException;
import javax.management.ObjectName;
import javax.management.RuntimeOperationsException;
import java.security.AccessController;
import java.security.PrivilegedActionException;
import java.security.PrivilegedExceptionAction;
import sun.security.action.LoadLibraryAction;

import sun.util.logging.LoggingSupport;

import java.util.ArrayList;
import java.util.Collections;
import java.util.List;
import com.sun.management.OSMBeanFactory;

import static java.lang.management.ManagementFactory.*;

/**
 * ManagementFactoryHelper provides static factory methods to create
 * instances of the management interface.
 */
public class ManagementFactoryHelper {
    private ManagementFactoryHelper() {};

    private static VMManagement jvm;

    public static synchronized ClassLoadingMXBean getClassLoadingMXBean() {
        throw new Error();
    }

    public static synchronized MemoryMXBean getMemoryMXBean() {
        throw new Error();
    }

    public static synchronized ThreadMXBean getThreadMXBean() {
        throw new Error();
    }

    public static synchronized RuntimeMXBean getRuntimeMXBean() {
        throw new Error();
    }

    public static synchronized CompilationMXBean getCompilationMXBean() {
        throw new Error();
    }

    public static synchronized OperatingSystemMXBean getOperatingSystemMXBean() {
        throw new Error();
    }

    public static List<MemoryPoolMXBean> getMemoryPoolMXBeans() {
        throw new Error();
    }

    public static List<MemoryManagerMXBean> getMemoryManagerMXBeans() {
        throw new Error();
    }

    public static List<GarbageCollectorMXBean> getGarbageCollectorMXBeans() {
        throw new Error();
    }

    public static PlatformLoggingMXBean getPlatformLoggingMXBean() {
        throw new Error();
    }

    // The logging MXBean object is an instance of
    // PlatformLoggingMXBean and java.util.logging.LoggingMXBean
    // but it can't directly implement two MXBean interfaces
    // as a compliant MXBean implements exactly one MXBean interface,
    // or if it implements one interface that is a subinterface of
    // all the others; otherwise, it is a non-compliant MXBean
    // and MBeanServer will throw NotCompliantMBeanException.
    // See the Definition of an MXBean section in javax.management.MXBean spec.
    //
    // To create a compliant logging MXBean, define a LoggingMXBean interface
    // that extend PlatformLoggingMXBean and j.u.l.LoggingMXBean
    interface LoggingMXBean
        extends PlatformLoggingMXBean, java.util.logging.LoggingMXBean {
    }

    static class PlatformLoggingImpl implements LoggingMXBean
    {
        final static PlatformLoggingMXBean instance = new PlatformLoggingImpl();
        final static String LOGGING_MXBEAN_NAME = "java.util.logging:type=Logging";

        private volatile ObjectName objname;  // created lazily
        @Override
        public ObjectName getObjectName() {
            ObjectName result = objname;
            if (result == null) {
                synchronized (this) {
                    if (objname == null) {
                        result = Util.newObjectName(LOGGING_MXBEAN_NAME);
                        objname = result;
                    }
                }
            }
            return result;
        }

        @Override
        public java.util.List<String> getLoggerNames() {
            return LoggingSupport.getLoggerNames();
        }

        @Override
        public String getLoggerLevel(String loggerName) {
            return LoggingSupport.getLoggerLevel(loggerName);
        }

        @Override
        public void setLoggerLevel(String loggerName, String levelName) {
            LoggingSupport.setLoggerLevel(loggerName, levelName);
        }

        @Override
        public String getParentLoggerName(String loggerName) {
            return LoggingSupport.getParentLoggerName(loggerName);
        }
    }

    private static List<BufferPoolMXBean> bufferPools = null;
    public static synchronized List<BufferPoolMXBean> getBufferPoolMXBeans() {
        throw new Error();
    }

    private final static String BUFFER_POOL_MXBEAN_NAME = "java.nio:type=BufferPool";

    /**
     * Registers a given MBean if not registered in the MBeanServer;
     * otherwise, just return.
     */
    private static void addMBean(MBeanServer mbs, Object mbean, String mbeanName) {
        try {
            final ObjectName objName = Util.newObjectName(mbeanName);

            // inner class requires these fields to be final
            final MBeanServer mbs0 = mbs;
            final Object mbean0 = mbean;
            AccessController.doPrivileged(new PrivilegedExceptionAction<Void>() {
                public Void run() throws MBeanRegistrationException,
                                         NotCompliantMBeanException {
                    try {
                        mbs0.registerMBean(mbean0, objName);
                        return null;
                    } catch (InstanceAlreadyExistsException e) {
                        // if an instance with the object name exists in
                        // the MBeanServer ignore the exception
                    }
                    return null;
                }
            });
        } catch (PrivilegedActionException e) {
            throw Util.newException(e.getException());
        }
    }

    static void registerInternalMBeans(MBeanServer mbs) {
    }

    static void unregisterInternalMBeans(MBeanServer mbs) {
    }

    static {
        jvm = new VMManagementImpl();
    }

    public static boolean isThreadSuspended(int state) {
        return ((state & JMM_THREAD_STATE_FLAG_SUSPENDED) != 0);
    }

    public static boolean isThreadRunningNative(int state) {
        return ((state & JMM_THREAD_STATE_FLAG_NATIVE) != 0);
    }

    public static Thread.State toThreadState(int state) {
        // suspended and native bits may be set in state
        int threadStatus = state & ~JMM_THREAD_STATE_FLAG_MASK;
        return sun.misc.VM.toThreadState(threadStatus);
    }

    // These values are defined in jmm.h
    private static final int JMM_THREAD_STATE_FLAG_MASK = 0xFFF00000;
    private static final int JMM_THREAD_STATE_FLAG_SUSPENDED = 0x00100000;
    private static final int JMM_THREAD_STATE_FLAG_NATIVE = 0x00400000;

}
