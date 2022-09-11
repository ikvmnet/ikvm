/*
 * Copyright (c) 1998, 2017, Oracle and/or its affiliates. All rights reserved.
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

package com.sun.javatest.regtest.exec;

import java.io.File;
import java.io.PrintWriter;
import java.text.DateFormat;
import java.util.ArrayList;
import java.util.Date;
import java.util.LinkedHashMap;
import java.util.LinkedHashSet;
import java.util.List;
import java.util.Map;
import java.util.Set;
import java.util.regex.Pattern;

import com.sun.javatest.Status;
import com.sun.javatest.regtest.config.Locations;
import com.sun.javatest.regtest.config.Locations.ClassLocn;
import com.sun.javatest.regtest.config.Locations.LibLocn;
import com.sun.javatest.regtest.config.ParseException;

import static com.sun.javatest.regtest.RStatus.passed;

/**
 * This class implements the "build" action as described by the JDK tag
 * specification.
 *
 * @author Iris A Garcia
 * @see Action
 */
public class BuildAction extends Action
{
    public static final String NAME = "build";

    /**
     * {@inheritDoc}
     * @return "build"
     */
    @Override
    public String getName() {
        return NAME;
    }

    /**
     * A method used by sibling classes to run both the init() and run()
     * method of BuildAction.
     *
     * @param opts The options for the action.
     * @param reason Indication of why this action was invoked.
     * @param args The arguments for the actions.
     * @param script The script.
     * @return     The result of the action.
     * @throws TestRunException if an error occurs during the work
     * @see #init
     * @see #run
     */
    public Status build(Map<String,String> opts, List<String> args, String reason,
                        RegressionScript script) throws TestRunException
    {
        init(opts, args, reason, script);
        return run();
    } // build()

    /**
     * This method does initial processing of the options and arguments for the
     * action.  Processing is determined by the requirements of run().
     *
     * Verify that the options are of length 0 and that there is at least one
     * argument.
     *
     * @param opts The options for the action.
     * @param args The arguments for the actions.
     * @param reason Indication of why this action was invoked.
     * @param script The script.
     * @exception  ParseException If the options or arguments are not expected
     *             for the action or are improperly formated.
     */
    @Override
    public void init(Map<String,String> opts, List<String> args, String reason,
                     RegressionScript script)
        throws ParseException
    {
        super.init(opts, args, reason, script);

        for (Map.Entry<String,String> e: opts.entrySet()) {
            String optName  = e.getKey();
            String optValue = e.getValue();
            if (optName.equals("implicit") && optValue.equals("none")) {
                implicitOpt = "-implicit:none";
                continue;
            }
            throw new ParseException(BUILD_UNEXPECT_OPT);
        }

        if (args.isEmpty())
            throw new ParseException(BUILD_NO_CLASSNAME);

        for (String currArg : args) {
            if (!BUILD_PTN.matcher(currArg).matches()) {
                throw new ParseException(BUILD_BAD_CLASSNAME + currArg);
            }
        }
    } // init()

    private static final String IGNORE_CASE = "(?i)";
    private static final String OPT_MODULE = "([a-z_][.a-z0-9_$]*/)?";
    private static final String PKG_CLASS = "(([a-z_][.a-z0-9_$]*)(\\.\\*|\\.package-info)?)";
    private static final String PKG_CLASS_OR_OTHER = "(" + PKG_CLASS + "|\\*|module-info)";
    static final Pattern BUILD_PTN = Pattern.compile(IGNORE_CASE + OPT_MODULE + PKG_CLASS_OR_OTHER);

    @Override
    public Set<File> getSourceFiles() {
        Set<File> files = new LinkedHashSet<>();
        for (String arg: args) {
            // the arguments to build are classnames or package names with wildcards
            try {
                for (ClassLocn cl: script.locations.locateClasses(arg)) {
                    files.add(cl.absSrcFile);
                }
            } catch (Locations.Fault ignore) {
            }
        }
        return files;
    }

    @Override
    public Set<String> getModules() {
        Set<String> modules = new LinkedHashSet<>();
        for (String arg: args) {
            int sep = arg.indexOf("/");
            if (sep > 0)
                modules.add(arg.substring(0, sep));
        }
        return modules;
    }

    /**
     * The method that does the work of the action.  The necessary work for the
     * given action is defined by the tag specification.
     *
     * Each named class will be compiled if its corresponding class file doesn't
     * exist or is older than its source file.  The class name is fully
     * qualified as necessary and the ".java" extension is added before
     * compilation.
     *
     * Build is allowed to search anywhere in the library-list.  Compile is
     * allowed to search only in the directory containing the defining file of
     * the test.  Thus, compile will always absolutify by adding the directory
     * path of the defining file to the passed filename.  Build must pass an
     * absolute filename to handle files found in the library-list.
     *
     * @return  The result of the action.
     * @throws  TestRunException If an unexpected error occurs while running
     *          the test.
     */
    @Override
    public Status run() throws TestRunException {
        startAction(false);

        // step 1: see which files need compiling, and group them according
        // to the value of the library in which they appear, and hence
        // -d flag that will be required
        PrintWriter pw = section.getMessageWriter();
        long now = System.currentTimeMillis();
        Map<LibLocn, List<ClassLocn>> classLocnsToCompile = new LinkedHashMap<>();
        for (String arg: args) {
            try {
                for (ClassLocn cl: script.locations.locateClasses(arg)) {
                    if (cl.absSrcFile.lastModified() > now) {
                        pw.println(String.format(BUILD_FUTURE_SOURCE, cl.absSrcFile,
                                DateFormat.getDateTimeInstance().format(new Date(cl.absSrcFile.lastModified()))));
                        pw.println(BUILD_FUTURE_SOURCE_2);
                    }
                    if (!cl.isUpToDate()) {
                        List<ClassLocn> classLocnsForLib = classLocnsToCompile.get(cl.lib);
                        if (classLocnsForLib == null) {
                            classLocnsForLib = new ArrayList<>();
                            classLocnsToCompile.put(cl.lib, classLocnsForLib);
                        }
                        classLocnsForLib.add(cl);
                    }
                }
            } catch (Locations.Fault e) {
                throw new TestRunException(e.getMessage());
            }
        }

        // step 2: perform the compilations, if any
        Status status;
        if (classLocnsToCompile.isEmpty()) {
            status = passed(BUILD_UP_TO_DATE);
        } else {
            status = null;
            // ensure that all directories are created for any library classes
            for (File dir: script.locations.absLibClsList(LibLocn.Kind.PACKAGE)) {
                dir.mkdirs();
            }

            // compile libraries first
            for (Map.Entry<LibLocn,List<ClassLocn>> e: classLocnsToCompile.entrySet()) {
                if (e.getKey().name != null) {
                    Status s = compileLibrary(e.getKey(), e.getValue());
                    if (!s.isPassed()) {
                        status = s;
                        break;
                    }
                }
            }

            // compile test code
            if (status == null) {
                for (Map.Entry<LibLocn,List<ClassLocn>> e: classLocnsToCompile.entrySet()) {
                    if (e.getKey().name == null) {
                        Status s = compileLibrary(e.getKey(), e.getValue());
                        if (!s.isPassed()) {
                            status = s;
                            break;
                        }
                    }
                }
            }

            if (status == null)
                status = passed(BUILD_SUCC);
        }

        endAction(status);
        return status;
    } // run()

    private Status compileLibrary(LibLocn libLocn, List<ClassLocn> classLocns) throws TestRunException {
        showClasses(libLocn, classLocns);

        switch (libLocn.kind) {
            case PACKAGE:
                return compileFiles(libLocn, false, null, getSrcFiles(classLocns));

            case USER_MODULE:
                return compileFiles(libLocn, true, null, getSrcFiles(classLocns));

            case SYS_MODULE:
                Map<String, List<File>> filesForModule = new LinkedHashMap<>();
                for (ClassLocn cl: classLocns) {
                    List<File> files = filesForModule.get(cl.optModule);
                    if (files == null) {
                        filesForModule.put(cl.optModule, files = new ArrayList<>());
                    }
                    files.add(cl.absSrcFile);
                }
                for (Map.Entry<String, List<File>> e: filesForModule.entrySet()) {
                    Status s = compileFiles(libLocn, false, e.getKey(), e.getValue());
                    if (!s.isPassed())
                        return s;
                }
                return passed(BUILD_SUCC);

            case PRECOMPILED_JAR:
            default:
                throw new AssertionError();
        }
    }

    private Status compileFiles(LibLocn libLocn, boolean isMulti, String moduleName, List<File> files) throws TestRunException {
        Map<String,String> compOpts = new LinkedHashMap<>();
        if (isMulti) {
            compOpts.put("modules", null);
        }
        if (moduleName != null) {
            compOpts.put("module", moduleName);
        }

        List<String> compArgs = new ArrayList<>();
        if (script.getCompileJDK().hasOldSymbolFile())
            compArgs.add("-XDignore.symbol.file=true");
        if (implicitOpt != null)
            compArgs.add(implicitOpt);

        for (File file: files)
            compArgs.add(file.getPath());

        CompileAction ca = new CompileAction();
        return ca.compile(libLocn, compOpts, compArgs, SREASON_FILE_TOO_OLD, script);
    }

    private List<File> getSrcFiles(List<ClassLocn> classLocns) {
        List<File> files = new ArrayList<>();
        for (ClassLocn cl: classLocns) {
            files.add(cl.absSrcFile);
        }
        return files;
    }

    private void showClasses(LibLocn lib, List<ClassLocn> toCompile) {
        PrintWriter pw = section.getMessageWriter();

        if (lib.name == null) {
            pw.println("Test directory:");
        } else {
            pw.println("Library " + lib.name + ":");
        }

        String sep = "  compile: ";
        for (ClassLocn cl: toCompile) {
            pw.print(sep);
            if (cl.optModule != null) {
                pw.print(cl.optModule);
                pw.print("/");
            }
            pw.print(cl.className);
            sep = ", ";
        }
        pw.println();
    }

    //----------member variables------------------------------------------------

    private String implicitOpt;
}
