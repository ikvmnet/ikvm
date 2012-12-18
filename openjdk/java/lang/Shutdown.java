/*
 * Copyright (c) 1999, 2005, Oracle and/or its affiliates. All rights reserved.
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

package java.lang;

import cli.System.AppDomain;
import cli.System.EventArgs;
import cli.System.EventHandler;
import cli.System.Threading.Monitor;

/**
 * Package-private utility class containing data structures and logic
 * governing the virtual-machine shutdown sequence.
 *
 * @author   Mark Reinhold
 * @since    1.3
 */

class Shutdown {

    /* Shutdown state */
    private static final int RUNNING = 0;
    private static final int HOOKS = 1;
    private static final int FINALIZERS = 2;
    private static int state = RUNNING;

    /* Should we run all finalizers upon exit? */
    static volatile boolean runFinalizersOnExit = false;

    // The system shutdown hooks are registered with a predefined slot.
    // The list of shutdown hooks is as follows:
    // (0) Console restore hook
    // (1) Application hooks
    // (2) DeleteOnExit hook
    private static final int MAX_SYSTEM_HOOKS = 10;
    private static final Runnable[] hooks = new Runnable[MAX_SYSTEM_HOOKS];

    // the index of the currently running shutdown hook to the hooks array
    private static int currentRunningHook = 0;

    // [IKVM] have we registered the AppDomain.ProcessExit event handler?
    private static boolean registeredProcessExit;

    /* The preceding static fields are protected by this lock */
    private static class Lock { };
    private static Object lock = new Lock();

    /* Lock object for the native halt method */
    private static Object haltLock = new Lock();

    /* Invoked by Runtime.runFinalizersOnExit */
    static void setRunFinalizersOnExit(boolean run) {
        synchronized (lock) {
            runFinalizersOnExit = run;
        }
    }
    
    private static void registerProcessExit() {
        try {
            // MONOBUG Mono doesn't support starting a new thread during ProcessExit
            // (and application shutdown hooks are based on threads)
            // see https://bugzilla.xamarin.com/show_bug.cgi?id=5650
            if (!ikvm.internal.Util.MONO) {
                // AppDomain.ProcessExit has a LinkDemand, so we have to have a separate method
                registerShutdownHook();
                if (false) throw new cli.System.Security.SecurityException();
            }
        }
        catch (cli.System.Security.SecurityException _) {
        }
    }

    private static void registerShutdownHook()
    {
        AppDomain.get_CurrentDomain().add_ProcessExit(new EventHandler(new EventHandler.Method() {
            public void Invoke(Object sender, EventArgs e) {
                shutdown();
            }
        }));
    }

    /**
     * Add a new shutdown hook.  Checks the shutdown state and the hook itself,
     * but does not do any security checks.
     *
     * The registerShutdownInProgress parameter should be false except
     * registering the DeleteOnExitHook since the first file may
     * be added to the delete on exit list by the application shutdown
     * hooks.
     *
     * @params slot  the slot in the shutdown hook array, whose element
     *               will be invoked in order during shutdown
     * @params registerShutdownInProgress true to allow the hook
     *               to be registered even if the shutdown is in progress.
     * @params hook  the hook to be registered
     *
     * @throw IllegalStateException
     *        if registerShutdownInProgress is false and shutdown is in progress; or
     *        if registerShutdownInProgress is true and the shutdown process
     *           already passes the given slot
     */
    static void add(int slot, boolean registerShutdownInProgress, Runnable hook) {
        synchronized (lock) {
            if (hooks[slot] != null)
                throw new InternalError("Shutdown hook at slot " + slot + " already registered");

            if (!registerShutdownInProgress) {
                if (state > RUNNING)
                    throw new IllegalStateException("Shutdown in progress");
            } else {
                if (state > HOOKS || (state == HOOKS && slot <= currentRunningHook))
                    throw new IllegalStateException("Shutdown in progress");
            }

            if (!registeredProcessExit) {
                registeredProcessExit = true;
                registerProcessExit();
            }

            hooks[slot] = hook;
        }
    }

    /* Run all registered shutdown hooks
     */
    private static void runHooks() {
        for (int i=0; i < MAX_SYSTEM_HOOKS; i++) {
            try {
                Runnable hook;
                synchronized (lock) {
                    // acquire the lock to make sure the hook registered during
                    // shutdown is visible here.
                    currentRunningHook = i;
                    hook = hooks[i];
                }
                if (hook != null) hook.run();
            } catch(Throwable t) {
                if (t instanceof ThreadDeath) {
                    ThreadDeath td = (ThreadDeath)t;
                    throw td;
                }
            }
        }
    }

    /* The halt method is synchronized on the halt lock
     * to avoid corruption of the delete-on-shutdown file list.
     * It invokes the true native halt method.
     */
    static void halt(int status) {
        synchronized (haltLock) {
            halt0(status);
        }
    }

    static void halt0(int status) {
        cli.System.Environment.Exit(status);
    }

    /* Wormhole for invoking java.lang.ref.Finalizer.runAllFinalizers */
    private static void runAllFinalizers() { /* [IKVM] Don't need to do anything here */ }


    /* The actual shutdown sequence is defined here.
     *
     * If it weren't for runFinalizersOnExit, this would be simple -- we'd just
     * run the hooks and then halt.  Instead we need to keep track of whether
     * we're running hooks or finalizers.  In the latter case a finalizer could
     * invoke exit(1) to cause immediate termination, while in the former case
     * any further invocations of exit(n), for any n, simply stall.  Note that
     * if on-exit finalizers are enabled they're run iff the shutdown is
     * initiated by an exit(0); they're never run on exit(n) for n != 0 or in
     * response to SIGINT, SIGTERM, etc.
     */
    private static void sequence() {
        synchronized (lock) {
            /* Guard against the possibility of a daemon thread invoking exit
             * after DestroyJavaVM initiates the shutdown sequence
             */
            if (state != HOOKS) return;
        }
        runHooks();
        boolean rfoe;
        synchronized (lock) {
            state = FINALIZERS;
            rfoe = runFinalizersOnExit;
        }
        if (rfoe) runAllFinalizers();
    }


    /* Invoked by Runtime.exit, which does all the security checks.
     * Also invoked by handlers for system-provided termination events,
     * which should pass a nonzero status code.
     */
    static void exit(int status) {
        boolean runMoreFinalizers = false;
        synchronized (lock) {
            if (status != 0) runFinalizersOnExit = false;
            switch (state) {
            case RUNNING:       /* Initiate shutdown */
                state = HOOKS;
                break;
            case HOOKS:         /* Stall and halt */
                break;
            case FINALIZERS:
                if (status != 0) {
                    /* Halt immediately on nonzero status */
                    halt(status);
                } else {
                    /* Compatibility with old behavior:
                     * Run more finalizers and then halt
                     */
                    runMoreFinalizers = runFinalizersOnExit;
                }
                break;
            }
        }
        if (runMoreFinalizers) {
            runAllFinalizers();
            halt(status);
        }
        synchronized (Shutdown.class) {
            /* Synchronize on the class object, causing any other thread
             * that attempts to initiate shutdown to stall indefinitely
             */
            sequence();
            halt(status);
        }
    }


    /* Invoked by the JNI DestroyJavaVM procedure when the last non-daemon
     * thread has finished.  Unlike the exit method, this method does not
     * actually halt the VM.
     */
    static void shutdown() {
        synchronized (lock) {
            switch (state) {
            case RUNNING:       /* Initiate shutdown */
                state = HOOKS;
                break;
            case HOOKS:         /* Stall and then return */
            case FINALIZERS:
                break;
            }
        }
        // [IKVM] We don't block here, because we're being called
        // from the AppDomain.ProcessExit event and we don't want to
        // deadlock with the thread that called exit.
        // Note that our JNI DestroyJavaVM implementation doesn't
        // call this method.
        if (Monitor.TryEnter(Shutdown.class)) {
            try {
                sequence();
            } finally {
                Monitor.Exit(Shutdown.class);
            }
        }
    }

}
