/*
 * Copyright 2003-2006 Sun Microsystems, Inc.  All Rights Reserved.
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.  Sun designates this
 * particular file as subject to the "Classpath" exception as provided
 * by Sun in the LICENSE file that accompanied this code.
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
 * Please contact Sun Microsystems, Inc., 4150 Network Circle, Santa Clara,
 * CA 95054 USA or visit www.sun.com if you need additional information or
 * have any questions.
 */

package sun.management;

import java.lang.management.*;
import java.util.logging.LogManager;

import javax.management.DynamicMBean;
import javax.management.MBeanServer;
import javax.management.MBeanServerFactory;
import javax.management.MBeanInfo;
import javax.management.NotificationEmitter;
import javax.management.ObjectName;
import javax.management.ObjectInstance;
import javax.management.InstanceAlreadyExistsException;
import javax.management.InstanceNotFoundException;
import javax.management.MBeanRegistrationException;
import javax.management.NotCompliantMBeanException;
import javax.management.MalformedObjectNameException;
import javax.management.RuntimeOperationsException;
import javax.management.StandardEmitterMBean;
import javax.management.StandardMBean;
import java.security.AccessController;
import java.security.Permission;
import java.security.PrivilegedActionException;
import java.security.PrivilegedExceptionAction;

import java.util.ArrayList;
import java.util.List;
import java.util.HashMap;
import java.util.Map;
import java.util.Set;
import java.util.Iterator;
import java.util.ListIterator;

import static java.lang.management.ManagementFactory.*;

/**
 * ManagementFactory provides static factory methods to create
 * instances of the management interface.
 */
public class ManagementFactory {
    private ManagementFactory() {};

    public static synchronized ClassLoadingMXBean getClassLoadingMXBean() {
	throw new Error("Not implemented");
    }

    public static synchronized MemoryMXBean getMemoryMXBean() {
	throw new Error("Not implemented");
    }

    public static synchronized ThreadMXBean getThreadMXBean() {
	throw new Error("Not implemented");
    }

    public static synchronized RuntimeMXBean getRuntimeMXBean() {
	throw new Error("Not implemented");
    }

    public static synchronized CompilationMXBean getCompilationMXBean() {
	throw new Error("Not implemented");
    }

    public static synchronized OperatingSystemMXBean getOperatingSystemMXBean() {
	throw new Error("Not implemented");
    }

    public static List<MemoryPoolMXBean> getMemoryPoolMXBeans() {
	throw new Error("Not implemented");
    }

    public static List<MemoryManagerMXBean> getMemoryManagerMXBeans() {
	throw new Error("Not implemented");
    }

    public static List<GarbageCollectorMXBean> getGarbageCollectorMXBeans() {
	throw new Error("Not implemented");
    }

    public static MBeanServer createPlatformMBeanServer() {
	throw new Error("Not implemented");
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
