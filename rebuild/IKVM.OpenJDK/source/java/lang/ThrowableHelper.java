/*
 * Copyright (c) 1994, 2013, Oracle and/or its affiliates. All rights reserved.
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
import  java.io.*;
import  java.util.*;

final class ThrowableHelper {
    /**
     * Holder class to defer initializing sentinel objects only used
     * for serialization.
     */
    private static class SentinelHolder {
        /**
         * {@linkplain #setStackTrace(StackTraceElement[]) Setting the
         * stack trace} to a one-element array containing this sentinel
         * value indicates future attempts to set the stack trace will be
         * ignored.  The sentinal is equal to the result of calling:<br>
         * {@code new StackTraceElement("", "", null, Integer.MIN_VALUE)}
         */
        public static final StackTraceElement STACK_TRACE_ELEMENT_SENTINEL =
            new StackTraceElement("", "", null, Integer.MIN_VALUE);

        /**
         * Sentinel value used in the serial form to indicate an immutable
         * stack trace.
         */
        public static final StackTraceElement[] STACK_TRACE_SENTINEL =
            new StackTraceElement[] {STACK_TRACE_ELEMENT_SENTINEL};
    }

    /** Caption  for labeling causative exception stack traces */
    private static final String CAUSE_CAPTION = "Caused by: ";

    /** Caption for labeling suppressed exception stack traces */
    private static final String SUPPRESSED_CAPTION = "Suppressed: ";

    /**
     * Prints this throwable and its backtrace to the
     * standard error stream. This method prints a stack trace for this
     * {@code Throwable} object on the error output stream that is
     * the value of the field {@code System.err}. The first line of
     * output contains the result of the {@link #toString()} method for
     * this object.  Remaining lines represent data previously recorded by
     * the method {@link #fillInStackTrace()}. The format of this
     * information depends on the implementation, but the following
     * example may be regarded as typical:
     * <blockquote><pre>
     * java.lang.NullPointerException
     *         at MyClass.mash(MyClass.java:9)
     *         at MyClass.crunch(MyClass.java:6)
     *         at MyClass.main(MyClass.java:3)
     * </pre></blockquote>
     * This example was produced by running the program:
     * <pre>
     * class MyClass {
     *     public static void main(String[] args) {
     *         crunch(null);
     *     }
     *     static void crunch(int[] a) {
     *         mash(a);
     *     }
     *     static void mash(int[] b) {
     *         System.out.println(b[0]);
     *     }
     * }
     * </pre>
     * The backtrace for a throwable with an initialized, non-null cause
     * should generally include the backtrace for the cause.  The format
     * of this information depends on the implementation, but the following
     * example may be regarded as typical:
     * <pre>
     * HighLevelException: MidLevelException: LowLevelException
     *         at Junk.a(Junk.java:13)
     *         at Junk.main(Junk.java:4)
     * Caused by: MidLevelException: LowLevelException
     *         at Junk.c(Junk.java:23)
     *         at Junk.b(Junk.java:17)
     *         at Junk.a(Junk.java:11)
     *         ... 1 more
     * Caused by: LowLevelException
     *         at Junk.e(Junk.java:30)
     *         at Junk.d(Junk.java:27)
     *         at Junk.c(Junk.java:21)
     *         ... 3 more
     * </pre>
     * Note the presence of lines containing the characters {@code "..."}.
     * These lines indicate that the remainder of the stack trace for this
     * exception matches the indicated number of frames from the bottom of the
     * stack trace of the exception that was caused by this exception (the
     * "enclosing" exception).  This shorthand can greatly reduce the length
     * of the output in the common case where a wrapped exception is thrown
     * from same method as the "causative exception" is caught.  The above
     * example was produced by running the program:
     * <pre>
     * public class Junk {
     *     public static void main(String args[]) {
     *         try {
     *             a();
     *         } catch(HighLevelException e) {
     *             e.printStackTrace();
     *         }
     *     }
     *     static void a() throws HighLevelException {
     *         try {
     *             b();
     *         } catch(MidLevelException e) {
     *             throw new HighLevelException(e);
     *         }
     *     }
     *     static void b() throws MidLevelException {
     *         c();
     *     }
     *     static void c() throws MidLevelException {
     *         try {
     *             d();
     *         } catch(LowLevelException e) {
     *             throw new MidLevelException(e);
     *         }
     *     }
     *     static void d() throws LowLevelException {
     *        e();
     *     }
     *     static void e() throws LowLevelException {
     *         throw new LowLevelException();
     *     }
     * }
     *
     * class HighLevelException extends Exception {
     *     HighLevelException(Throwable cause) { super(cause); }
     * }
     *
     * class MidLevelException extends Exception {
     *     MidLevelException(Throwable cause)  { super(cause); }
     * }
     *
     * class LowLevelException extends Exception {
     * }
     * </pre>
     * As of release 7, the platform supports the notion of
     * <i>suppressed exceptions</i> (in conjunction with the {@code
     * try}-with-resources statement). Any exceptions that were
     * suppressed in order to deliver an exception are printed out
     * beneath the stack trace.  The format of this information
     * depends on the implementation, but the following example may be
     * regarded as typical:
     *
     * <pre>
     * Exception in thread "main" java.lang.Exception: Something happened
     *  at Foo.bar(Foo.java:10)
     *  at Foo.main(Foo.java:5)
     *  Suppressed: Resource$CloseFailException: Resource ID = 0
     *          at Resource.close(Resource.java:26)
     *          at Foo.bar(Foo.java:9)
     *          ... 1 more
     * </pre>
     * Note that the "... n more" notation is used on suppressed exceptions
     * just at it is used on causes. Unlike causes, suppressed exceptions are
     * indented beyond their "containing exceptions."
     *
     * <p>An exception can have both a cause and one or more suppressed
     * exceptions:
     * <pre>
     * Exception in thread "main" java.lang.Exception: Main block
     *  at Foo3.main(Foo3.java:7)
     *  Suppressed: Resource$CloseFailException: Resource ID = 2
     *          at Resource.close(Resource.java:26)
     *          at Foo3.main(Foo3.java:5)
     *  Suppressed: Resource$CloseFailException: Resource ID = 1
     *          at Resource.close(Resource.java:26)
     *          at Foo3.main(Foo3.java:5)
     * Caused by: java.lang.Exception: I did it
     *  at Foo3.main(Foo3.java:8)
     * </pre>
     * Likewise, a suppressed exception can have a cause:
     * <pre>
     * Exception in thread "main" java.lang.Exception: Main block
     *  at Foo4.main(Foo4.java:6)
     *  Suppressed: Resource2$CloseFailException: Resource ID = 1
     *          at Resource2.close(Resource2.java:20)
     *          at Foo4.main(Foo4.java:5)
     *  Caused by: java.lang.Exception: Rats, you caught me
     *          at Resource2$CloseFailException.&lt;init&gt;(Resource2.java:45)
     *          ... 2 more
     * </pre>
     */
    public static void printStackTrace(Throwable _this) {
        _this.printStackTrace(System.err);
    }

    /**
     * Prints this throwable and its backtrace to the specified print stream.
     *
     * @param s {@code PrintStream} to use for output
     */
    public static void printStackTrace(Throwable _this, PrintStream s) {
        printStackTrace(_this, new WrappedPrintStream(s));
    }

    private static void printStackTrace(Throwable _this, PrintStreamOrWriter s) {
        // Guard against malicious overrides of Throwable.equals by
        // using a Set with identity equality semantics.
        Set<Throwable> dejaVu =
            Collections.newSetFromMap(new IdentityHashMap<Throwable, Boolean>());
        dejaVu.add(_this);

        synchronized (s.lock()) {
            // Print our stack trace
            s.println(_this);
            StackTraceElement[] trace = getOurStackTrace(_this);
            for (StackTraceElement traceElement : trace)
                s.println("\tat " + traceElement);

            // Print suppressed exceptions, if any
            for (Throwable se : _this.getSuppressed())
                printEnclosedStackTrace(se, s, trace, SUPPRESSED_CAPTION, "\t", dejaVu);

            // Print cause, if any
            Throwable ourCause = _this.getCause();
            if (ourCause != null)
                printEnclosedStackTrace(ourCause, s, trace, CAUSE_CAPTION, "", dejaVu);
        }
    }

    /**
     * Print our stack trace as an enclosed exception for the specified
     * stack trace.
     */
    private static void printEnclosedStackTrace(Throwable _this, PrintStreamOrWriter s,
                                         StackTraceElement[] enclosingTrace,
                                         String caption,
                                         String prefix,
                                         Set<Throwable> dejaVu) {
        assert Thread.holdsLock(s.lock());
        if (dejaVu.contains(_this)) {
            s.println("\t[CIRCULAR REFERENCE:" + _this + "]");
        } else {
            dejaVu.add(_this);
            // Compute number of frames in common between this and enclosing trace
            StackTraceElement[] trace = getOurStackTrace(_this);
            int m = trace.length - 1;
            int n = enclosingTrace.length - 1;
            while (m >= 0 && n >=0 && trace[m].equals(enclosingTrace[n])) {
                m--; n--;
            }
            int framesInCommon = trace.length - 1 - m;

            // Print our stack trace
            s.println(prefix + caption + _this);
            for (int i = 0; i <= m; i++)
                s.println(prefix + "\tat " + trace[i]);
            if (framesInCommon != 0)
                s.println(prefix + "\t... " + framesInCommon + " more");

            // Print suppressed exceptions, if any
            for (Throwable se : _this.getSuppressed())
                printEnclosedStackTrace(se, s, trace, SUPPRESSED_CAPTION,
                                           prefix +"\t", dejaVu);

            // Print cause, if any
            Throwable ourCause = _this.getCause();
            if (ourCause != null)
                printEnclosedStackTrace(ourCause, s, trace, CAUSE_CAPTION, prefix, dejaVu);
        }
    }

    private static native StackTraceElement[] getOurStackTrace(Throwable _this);

    /**
     * Prints this throwable and its backtrace to the specified
     * print writer.
     *
     * @param s {@code PrintWriter} to use for output
     * @since   JDK1.1
     */
    public static void printStackTrace(Throwable _this, PrintWriter s) {
        printStackTrace(_this, new WrappedPrintWriter(s));
    }

    /**
     * Wrapper class for PrintStream and PrintWriter to enable a single
     * implementation of printStackTrace.
     */
    private abstract static class PrintStreamOrWriter {
        /** Returns the object to be locked when using this StreamOrWriter */
        abstract Object lock();

        /** Prints the specified string as a line on this StreamOrWriter */
        abstract void println(Object o);
    }

    private static class WrappedPrintStream extends PrintStreamOrWriter {
        private final PrintStream printStream;

        WrappedPrintStream(PrintStream printStream) {
            this.printStream = printStream;
        }

        Object lock() {
            return printStream;
        }

        void println(Object o) {
            printStream.println(o);
        }
    }

    private static class WrappedPrintWriter extends PrintStreamOrWriter {
        private final PrintWriter printWriter;

        WrappedPrintWriter(PrintWriter printWriter) {
            this.printWriter = printWriter;
        }

        Object lock() {
            return printWriter;
        }

        void println(Object o) {
            printWriter.println(o);
        }
    }
}
