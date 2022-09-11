/*
 * Copyright (c) 1998, 2019, Oracle and/or its affiliates. All rights reserved.
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

import java.io.BufferedReader;
import java.io.BufferedWriter;
import java.io.ByteArrayOutputStream;
import java.io.File;
import java.io.FileNotFoundException;
import java.io.FileReader;
import java.io.FileWriter;
import java.io.IOException;
import java.io.PrintStream;
import java.io.PrintWriter;
import java.io.StringReader;
import java.lang.reflect.Constructor;
import java.lang.reflect.InvocationTargetException;
import java.lang.reflect.Method;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Collections;
import java.util.Iterator;
import java.util.LinkedHashSet;
import java.util.List;
import java.util.Map;
import java.util.Set;
import java.util.concurrent.TimeUnit;

import com.sun.javatest.Status;
import com.sun.javatest.regtest.TimeoutHandler;
import com.sun.javatest.regtest.agent.AStatus;
import com.sun.javatest.regtest.agent.CompileActionHelper;
import com.sun.javatest.regtest.agent.JDK_Version;
import com.sun.javatest.regtest.agent.SearchPath;
import com.sun.javatest.regtest.config.ExecMode;
import com.sun.javatest.regtest.config.JDK;
import com.sun.javatest.regtest.config.JDKOpts;
import com.sun.javatest.regtest.config.Locations;
import com.sun.javatest.regtest.config.Locations.LibLocn;
import com.sun.javatest.regtest.config.Modules;
import com.sun.javatest.regtest.config.ParseException;
import com.sun.javatest.regtest.exec.RegressionScript.PathKind;
import com.sun.javatest.regtest.util.StringUtils;

import static com.sun.javatest.regtest.RStatus.createStatus;
import static com.sun.javatest.regtest.RStatus.error;
import static com.sun.javatest.regtest.RStatus.failed;
import static com.sun.javatest.regtest.RStatus.normalize;
import static com.sun.javatest.regtest.RStatus.passed;

/**
 * This class implements the "compile" action as described by the JDK tag
 * specification. It is also invoked implicitly as needed by the "build"
 * action.
 *
 * @author Iris A Garcia
 * @see Action
 * @see com.sun.javatest.regtest.agent.MainActionHelper
 */
public class CompileAction extends Action {
    public static final String NAME = "compile";

    /**
     * {@inheritDoc}
     * @return "compile"
     */
    @Override
    public String getName() {
        return NAME;
    }

    /**
     * A method used by sibling classes to run both the init() and run()
     * method of CompileAction.
     *
     * @param libLocn details for where to place the compiled classes
     * @param opts The options for the action.
     * @param args The arguments for the actions.
     * @param reason Indication of why this action was invoked.
     * @param script The script.
     * @return     The result of the action.
     * @throws TestRunException if an error occurs while executing this action
     * @see #init
     * @see #run
     */
    Status compile(LibLocn libLocn, Map<String,String> opts, List<String> args, String reason,
            RegressionScript script) throws TestRunException {
        this.libLocn = libLocn;
        init(opts, args, reason, script);
        return run();
    } // compile()

    /**
     * This method does initial processing of the options and arguments for the
     * action.  Processing is determined by the requirements of run() and
     * getSourceFiles(). If run will be called, script.hasEnv() will be true.
     * If script.hasEnv() is false, there is no context available to determine
     * any class directories.
     *
     * Verify that the options are valid for the "compile" action.
     *
     * Verify that there is at least one argument.  Find the class names to
     * compile (via presence of ".java") and modify to contain fully qualified
     * path.
     *
     * If one of the JVM options is "-classpath" or "-cp", add the test classes
     * and test sources directory to the provided path.
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
            throws ParseException {
        super.init(opts, args, reason, script);

        if (reason.startsWith(SREASON_USER_SPECIFIED))
            addDebugOpts = true;

        if (args.isEmpty())
            throw new ParseException(COMPILE_NO_CLASSNAME);

        for (Map.Entry<String,String> e: opts.entrySet()) {
            String optName  = e.getKey();
            String optValue = e.getValue();

            switch (optName) {
                case "fail":
                    reverseStatus = parseFail(optValue);
                    break;
                case "timeout":
                    timeout = parseTimeout(optValue);
                    break;
                case "ref":
                    ref = parseRef(optValue);
                    break;
                case "process":
                    process = true;
                    break;
                case "module":
                    module = parseModule(optValue);
                    modules = Collections.singleton(module);
                    break;
                case "modules":
                    if (optValue != null)
                        throw new ParseException(COMPILE_MODULES_UEXPECT + optValue);
                    multiModule = true;
                    modules = new LinkedHashSet<>();
                    break;
                default:
                    throw new ParseException(COMPILE_BAD_OPT + optName);
            }
        }

        if (module != null && multiModule) {
            throw new ParseException("Bad combination of options: /module=" + module + ", /modules");
        }

        if (module == null && !multiModule)
            modules = Collections.emptySet();

        if (timeout < 0)
            timeout = script.getActionTimeout(-1);

        // add absolute path name to all the .java files create appropriate
        // class directories
        Locations locations = script.locations;
        if (libLocn == null) {
            destDir = multiModule ? locations.absTestModulesDir() : locations.absTestClsDir(module);
        } else {
            destDir = (module == null) ? libLocn.absClsDir : new File(libLocn.absClsDir, module);
        }
        if (!script.isCheck())
            mkdirs(destDir);

        boolean foundJavaFile = false;
        boolean foundAsmFile = false;

        for (int i = 0; i < args.size(); i++) {
            // note: in the following code, some args are overrwritten in place
            String currArg = args.get(i);

            if (currArg.endsWith(".java")) {
                foundJavaFile = true;
                File sourceFile = new File(currArg.replace('/', File.separatorChar));
                if (!sourceFile.isAbsolute()) {
                    // User must have used @compile, so file must be
                    // in the same directory as the defining file.
                    if (multiModule)
                        addModule(currArg);
                    File absSourceFile = locations.absTestSrcFile(module, sourceFile);
                    if (!absSourceFile.exists())
                        throw new ParseException(CANT_FIND_SRC + currArg);
                    args.set(i, absSourceFile.getPath());
                }
            } else if (currArg.endsWith(".jasm") || currArg.endsWith("jcod")) {
                if (module != null) {
                    throw new ParseException(COMPILE_OPT_DISALLOW);
                }
                foundAsmFile = true;
                File sourceFile = new File(currArg.replace('/', File.separatorChar));
                if (!sourceFile.isAbsolute()) {
                    // User must have used @compile, so file must be
                    // in the same directory as the defining file.
                    if (multiModule)
                        addModule(currArg);
                    File absSourceFile = locations.absTestSrcFile(null, sourceFile);
                    if (!absSourceFile.exists())
                        throw new ParseException(CANT_FIND_SRC + currArg);
                    args.set(i, absSourceFile.getPath());
                }
            }

            if (currArg.equals("-classpath") || currArg.equals("-cp")
                    || currArg.equals("--class-path") || currArg.startsWith("--class-path=")) {
                if (module != null || multiModule) {
                    throw new ParseException(COMPILE_OPT_DISALLOW);
                }
                classpathp = true;
                if (!currArg.startsWith("--class-path=")) {
                    i++;
                }
            } else if (currArg.equals("-sourcepath")
                    || currArg.equals("--source-path") || currArg.startsWith("--source-path=")) {
                if (module != null || multiModule) {
                    throw new ParseException(COMPILE_OPT_DISALLOW);
                }
                sourcepathp = true;
                if (!currArg.startsWith("--source-path=")) {
                    i++;
                }
            } else if (currArg.equals("-d")) {
                throw new ParseException(COMPILE_OPT_DISALLOW);
            }
        }

        if (!foundJavaFile && !process && !foundAsmFile) {
            throw new ParseException(COMPILE_NO_DOT_JAVA);
        }
        if (foundAsmFile) {
            if (sourcepathp || classpathp || process) {
                throw new ParseException(COMPILE_OPT_DISALLOW);
            }
            if (reverseStatus || ref != null) {
                throw new ParseException(COMPILE_OPT_DISALLOW);
            }
        }
    } // init()

    @Override
    public Set<File> getSourceFiles() {
        Set<File> files = new LinkedHashSet<>();

        for (String currArg : args) {
            if (currArg.endsWith(".java")
                    || currArg.endsWith(".jasm")
                    || currArg.endsWith(".jcod")) {
                files.add(new File(currArg));
            }
        }

        return files;
    }

    @Override
    public Set<String> getModules() {
        return modules;
    }

    /**
     * The method that does the work of the action.  The necessary work for the
     * given action is defined by the tag specification.
     *
     * Invoke the compiler on the given arguments which may possibly include
     * compiler options.  Equivalent to "javac arg+".
     *
     * Each named class will be compiled if its corresponding class file doesn't
     * exist or is older than its source file.  The class name is fully
     * qualified as necessary and the ".java" extension is added before
     * compilation.
     *
     * Build is allowed to search anywhere in the library-list.  Compile is
     * allowed to search only in the directory containing the defining file of
     * the test.  Thus, compile will always make files absolute by adding the
     * directory path of the defining file to the passed filename.
     * Build must pass an absolute filename to handle files found in the
     * library-list.
     *
     * @return  The result of the action.
     * @throws  TestRunException If an unexpected error occurs while executing
     *          the action.
     */
    @Override
    public Status run() throws TestRunException {
        startAction(true);

        List<String> javacArgs = new ArrayList<>();
        List<String> jasmArgs = new ArrayList<>();
        List<String> jcodArgs = new ArrayList<>();
        boolean runJavac = process;

        for (String currArg : args) {
            if (currArg.endsWith(".java")) {
                if (!(new File(currArg)).exists())
                    throw new TestRunException(CANT_FIND_SRC + currArg);
                javacArgs.add(currArg);
                runJavac = true;
            } else if (currArg.endsWith(".jasm")) {
                jasmArgs.add(currArg);
            } else if (currArg.endsWith(".jcod")) {
                jcodArgs.add(currArg);
            } else
                javacArgs.add(currArg);
        }

        Status status;

        if (script.isCheck()) {
            status = passed(CHECK_PASS);
        } else {
            // run jasm and jcod first (if needed) in case the resulting class
            // files will be required when compiling the .java files.
            status = passed("Not yet run");
            if (status.isPassed() && !jasmArgs.isEmpty())
                status = jasm(jasmArgs);
            if (status.isPassed() && !jcodArgs.isEmpty())
                status = jcod(jcodArgs);
            if (status.isPassed() && runJavac) {
                javacArgs = getJavacCommandArgs(javacArgs);
                for (String arg: javacArgs) {
                    if (arg.startsWith("-J")) {
                        othervmOverrideReasons.add("JVM options specified for compiler");
                        break;
                    }
                }
                if (explicitAnnotationProcessingRequested(javacArgs)
                        && !getExtraModuleConfigOptions(Modules.Phase.DYNAMIC).isEmpty()) {
                    othervmOverrideReasons.add("additional runtime exports needed for annotation processing");
                }
                switch (!othervmOverrideReasons.isEmpty() ? ExecMode.OTHERVM : script.getExecMode()) {
                    case AGENTVM:
                        showMode(ExecMode.AGENTVM);
                        status = runAgentJVM(javacArgs);
                        break;
                    case OTHERVM:
                        showMode(ExecMode.OTHERVM, othervmOverrideReasons);
                        status = runOtherJVM(javacArgs);
                        break;
                    default:
                        throw new AssertionError();
                }
            }
        }

        endAction(status);
        return status;
    } // run()

    //----------internal methods------------------------------------------------

    private Status jasm(List<String> files) {
        return asmtools("jasm", files);
    }

    private Status jcod(List<String> files) {
        return asmtools("jcoder", files);
    }

    private Status asmtools(String toolName, List<String> files) {
        if (files.isEmpty())
            return Status.passed(toolName + ": no files");

        List<String> toolArgs = new ArrayList<>();
        toolArgs.add("-d");
        toolArgs.add(destDir.getPath());
        toolArgs.addAll(files);
        try {
            String toolClassName = "org.openjdk.asmtools." + toolName + ".Main";
            recorder.asmtools(toolClassName, toolArgs);
            Class<?> toolClass = Class.forName(toolClassName);
            Constructor<?> constr = toolClass.getConstructor(PrintStream.class, String.class);
            ByteArrayOutputStream baos = new ByteArrayOutputStream();
            PrintStream ps = new PrintStream(baos);
            try {
                Object tool = constr.newInstance(ps, toolName);
                Method m = toolClass.getMethod("compile", String[].class);
                Object r = m.invoke(tool, new Object[] { toolArgs.toArray(new String[0]) });
                if (r instanceof Boolean) {
                    boolean ok = (Boolean) r;
                    return ok ? Status.passed(toolName + " OK") : Status.failed(toolName + " failed");
                } else
                    return Status.error("unexpected result from " + toolName + ": " + r.toString());
            } finally {
                try (PrintWriter out = section.createOutput(toolName)) {
                    out.write(baos.toString());
                }
            }
        } catch (ClassNotFoundException e) {
            return Status.error("can't find " + toolName);
        } catch (ReflectiveOperationException t) {
            return Status.error("error invoking " + toolName + ": " + t);
        }
    }

    /**
     * Determine the arguments for the compilation.
     * Three different types of compilation are supported.
     * <ul>
     * <li>Compilation of classes in the unnamed module.
     *     This is the default "classic" compilation.
     *     The output directory should be a package-oriented directory.
     *     Sources and classes for the unnamed module are put on the
     *     sourcepath and classpath.
     * <li>Compilation of classes in a single named user module.
     *     This mode is indicated by the option /module=module-name
     *     where module-name is not the name of a system module.
     *     The output directory should be the appropriate subdirectory
     *     of a module-oriented directory.
     *     The output directory should appear on the classpath.
     *     Sources and classes for the unnamed module are <i>not</i> available.
     * <li>Compilation of classes to patch those in a system module.
     *     This mode is indicated by the option /module=module-name
     *     where module-name is the name of a system module.
     * <li>Compilation of classes in one or more named user modules.
     *     This mode is indicated by the option /modules.
     *     The output directory should be a module-oriented directory.
     *     Sources and classes for the unnamed module are put on the
     *     sourcepath and classpath.
     * </ul>
     */
    private List<String> getJavacCommandArgs(List<String> args) throws TestRunException {
        Map<PathKind, SearchPath> compilePaths = script.getCompilePaths(libLocn, multiModule, module);

        JDKOpts javacArgs = new JDKOpts();
        javacArgs.addAll(script.getTestCompilerOptions());

        if (isModuleOptionsAllowed(args)) {
            javacArgs.addAll(getExtraModuleConfigOptions(Modules.Phase.STATIC));
        }

        if (destDir != null) {
            javacArgs.add("-d");
            javacArgs.add(destDir.toString());
        }

        // modulesourcepath and sourcepath are mutually exclusive
        if (multiModule) {
            javacArgs.addPath("--module-source-path", compilePaths.get(PathKind.MODULESOURCEPATH));
        } else if (module != null) {
            // Note: any additional patches for this module will be
            // automatically merged with this one.
            javacArgs.addAll("--patch-module", module + "=" + compilePaths.get(PathKind.SOURCEPATH));
        } else {
            javacArgs.addPath("--source-path", compilePaths.get(PathKind.SOURCEPATH));
        }

        // Need to refine what it means to put absTestClsDir unconditionally on the compilePath
        SearchPath cp = compilePaths.get(PathKind.CLASSPATH);
        javacArgs.addPath("--class-path", cp);

        javacArgs.addPath("--module-path", compilePaths.get(PathKind.MODULEPATH));

        SearchPath pp = compilePaths.get(PathKind.PATCHPATH);
        javacArgs.addAllPatchModules(pp); // will merge as needed with any similar preceding options
        if (pp != null && !pp.isEmpty() && cp != null && !cp.isEmpty()) {
            // provide addReads from patch modules to unnamed module(s).
            for (String s: getModules(pp)) {
                javacArgs.add("--add-reads=" + s + "=ALL-UNNAMED");
            }
        }

        Set<String> userMods = getModules(compilePaths.get(PathKind.MODULEPATH));
        if (!userMods.isEmpty()) {
            javacArgs.add("--add-modules");
            javacArgs.add(StringUtils.join(userMods, ","));
        }

        javacArgs.addAll(args);

        return javacArgs.toList();
    }

    private boolean isModuleOptionsAllowed(List<String> args) {
        Iterator<String> iter = args.iterator();
        while (iter.hasNext()) {
            String option = iter.next();
            switch (option) {
                case "-source":
                case "-target":
                case "--release":
                    if (iter.hasNext()) {
                        JDK_Version v = JDK_Version.forName(iter.next());
                        return (v == null) ? false : v.compareTo(JDK_Version.V9) >= 0;
                    }
                    break;

                default:
                    if (option.startsWith("--release=")) {
                        JDK_Version v = JDK_Version.forName(
                                option.substring(option.indexOf("=") + 1));
                        return (v == null) ? false : v.compareTo(JDK_Version.V9) >= 0;
                    }
                    break;
            }
        }
        return true;
    }

    private Status runOtherJVM(List<String> javacArgs) throws TestRunException {
        Status status;

        // Set test.src and test.classes for the benefit of annotation processors
        Map<String, String> javacProps = script.getTestProperties();

        // CONSTRUCT THE COMMAND LINE
        Map<String, String> env = script.getEnvVars();

        String javacCmd = script.getJavacProg();

        JDKOpts javacVMOpts = new JDKOpts();
        javacVMOpts.addAll(script.getTestVMOptions());
        if (addDebugOpts && script.getCompileJDK().equals(script.getTestJDK()))
            javacVMOpts.addAll(script.getTestDebugOptions());

        if (explicitAnnotationProcessingRequested(javacArgs)) {
            javacVMOpts.addAll(getExtraModuleConfigOptions(Modules.Phase.DYNAMIC));
        }

        // WRITE ARGUMENT FILE
        List<String> fullJavacArgs = javacArgs;
        if (javacArgs.size() >= 10) {
            File argFile = getArgFile();
            try (BufferedWriter w = new BufferedWriter(new FileWriter(argFile))) {
                for (String arg: javacArgs) {
                    if (arg.startsWith("-J")) {
                        // remove -J for now; it will be added back later
                        javacVMOpts.add(arg.substring(2));
                    } else {
                        w.write(arg);
                        w.newLine();
                    }
                }
            } catch (IOException e) {
                return error(COMPILE_CANT_WRITE_ARGS);
            } catch (SecurityException e) {
                // shouldn't happen since JavaTestSecurityManager allows file ops
                return error(COMPILE_SECMGR_FILEOPS);
            }
            javacArgs = Arrays.asList("@" + argFile);
        }

        List<String> command = new ArrayList<>();
        command.add(javacCmd);
        for (String opt: javacVMOpts.toList())
            command.add("-J" + opt);
        for (Map.Entry<String,String> e: javacProps.entrySet())
            command.add("-J-D" + e.getKey() + "=" + e.getValue());
        command.addAll(javacArgs);

        if (showMode)
            showMode("compile", ExecMode.OTHERVM, section);
        if (showCmd)
            showCmd("compile", command, section);

        new ModuleConfig("Boot Layer (javac runtime environment)")
                .setFromOpts(javacVMOpts)
                .write(configWriter);
        new ModuleConfig("javac compilation environment")
                .setFromOpts(fullJavacArgs)
                .write(configWriter);
        recorder.javac(env, javacCmd, javacVMOpts.toList(), javacProps, javacArgs);

        // PASS TO PROCESSCOMMAND
        PrintStringWriter stdOut = new PrintStringWriter();
        PrintStringWriter stdErr = new PrintStringWriter();
        ProcessCommand cmd = new ProcessCommand() {
            @Override
            protected Status getStatus(int exitCode, Status logStatus) {
                // logStatus is never used by javac, so ignore it
                JDK_Version v = script.getCompileJDKVersion();
                AStatus aStatus = CompileActionHelper.getStatusForJavacExitCode(v, exitCode);
                return new Status(aStatus.getType(), aStatus.getReason());
            }
        };

        TimeoutHandler timeoutHandler =
            script.getTimeoutHandlerProvider().createHandler(this.getClass(), script, section);

        cmd.setExecDir(script.absTestScratchDir())
            .setCommand(command)
            .setEnvironment(env)
            .setStreams(stdOut, stdErr)
            .setTimeout(timeout, TimeUnit.SECONDS)
            .setTimeoutHandler(timeoutHandler);

        status = normalize(cmd.exec());

        try (PrintWriter sysOut = section.createOutput("System.out")) {
            sysOut.write(stdOut.getOutput());
        }

        try (PrintWriter sysErr = section.createOutput("System.err")) {
            sysErr.write(stdErr.getOutput());
        }

        // EVALUATE THE RESULTS
        status = checkReverse(status, reverseStatus);

        // COMPARE OUTPUT TO GOLDENFILE IF REQUIRED
        // tag-spec says that "standard error is redirected to standard out
        // so that /ref can be used."  Simulate this by concatenating streams.
        if ((ref != null) && status.isPassed()) {
            String combined = stdOut.getOutput() + stdErr.getOutput();
            status = checkGoldenFile(combined, status);
        }

        return status;
    } // runOtherJVM()

    private Status runAgentJVM(List<String> javacArgs) throws TestRunException {
        // TAG-SPEC:  "The source and class directories of a test are made
        // available to main and applet actions via the system properties
        // "test.src" and "test.classes", respectively"
        Map<String, String> javacProps = script.getTestProperties();

        if (showMode)
            showMode("compile", ExecMode.AGENTVM, section);
        if (showCmd)
            showCmd("compile", javacArgs, section);

        String javacProg = script.getJavacProg();
        List<String> javacVMOpts = script.getTestVMJavaOptions();
        recorder.javac(script.getEnvVars(), javacProg, javacVMOpts, javacProps, javacArgs);

        Agent agent;
        try {
            JDK jdk = script.getCompileJDK();
            SearchPath agentClasspath = new SearchPath(jdk.getJDKClassPath(), script.getJavaTestClassPath());
            List<String> vmOpts = addDebugOpts && jdk.equals(script.getTestJDK())
                    ? join(script.getTestVMOptions(), script.getTestDebugOptions())
                    : script.getTestVMOptions();
            agent = script.getAgent(jdk, agentClasspath, vmOpts);
            section.getMessageWriter().println("Agent id: " + agent.getId());
            new ModuleConfig("Boot Layer (javac runtime environment)")
                    .setFromOpts(agent.vmOpts)
                    .write(configWriter);
        } catch (Agent.Fault e) {
            return error(AGENTVM_CANT_GET_VM + ": " + e.getCause());
        }

        TimeoutHandler timeoutHandler =
            script.getTimeoutHandlerProvider().createHandler(this.getClass(), script, section);

        Status status;
        try {
            new ModuleConfig("javac compilation environment")
                    .setFromOpts(javacArgs)
                    .write(configWriter);
            status = agent.doCompileAction(
                    script.getTestResult().getTestName(),
                    javacProps,
                    javacArgs,
                    timeout,
                    timeoutHandler,
                    section);
        } catch (Agent.Fault e) {
            if (e.getCause() instanceof IOException)
                status = error(String.format(AGENTVM_IO_EXCEPTION, e.getCause()));
            else
                status = error(String.format(AGENTVM_EXCEPTION, e.getCause()));
        }
        if (status.isError()) {
            script.closeAgent(agent);
        }

        // EVALUATE THE RESULTS
        status = checkReverse(status, reverseStatus);

        // COMPARE OUTPUT TO GOLDENFILE IF REQUIRED
        // tag-spec says that "standard error is redirected to standard out
        // so that /ref can be used."  Simulate this by concatenating streams.
        if ((ref != null) && status.isPassed()) {
            String outString = getOutput(OutputHandler.OutputKind.DIRECT);
            String errString = getOutput(OutputHandler.OutputKind.DIRECT_LOG);
            String stdOutString = getOutput(OutputHandler.OutputKind.STDOUT);
            String stdErrString = getOutput(OutputHandler.OutputKind.STDERR);
            String combined = (outString + errString + stdOutString + stdErrString);
            status = checkGoldenFile(combined, status);
        }

        return status;
    } // runAgentJVM()

    private String getOutput(OutputHandler.OutputKind kind) {
        String s = section.getOutput(kind.name);
        return (s == null) ? "" : s;
    }

    // See JavaCompiler.explicitAnnotationProcessingRequested
    private boolean explicitAnnotationProcessingRequested(List<String> javacArgs) {
        for (String arg: javacArgs) {
            if (arg.equals("-processor")
                    || arg.equals("-processorpath")
                    || arg.equals("-processormodulepath")
                    || arg.equals("-proc:only")
                    || arg.equals("-Xprint")) {
                return true;
            }
        }
        return false;
    }

    //----------internal methods------------------------------------------------

    /**
     * This method parses the <em>ref</em> action option used by the compile
     * action. It verifies that the indicated reference file exists in the
     * directory containing the defining file of the test.
     *
     * @param value The proposed filename for the reference file.
     * @return     A string indicating the name of the reference file for the
     *             test.
     * @exception  ParseException If the passed filename is null, the empty
     *             string, or does not exist.
     */
    private String parseRef(String value) throws ParseException {
        if ((value == null) || (value.equals("")))
            throw new ParseException(COMPILE_NO_REF_NAME);
        File refFile = new File(script.absTestSrcDir(), value);
        if (!refFile.exists())
            throw new ParseException(COMPILE_CANT_FIND_REF + refFile);
        return value;
    } // parseRef()

    private Status checkReverse(Status status, boolean reverseStatus) {
        if (!status.isError()) {
            boolean ok = status.isPassed();
            int st = status.getType();
            String sr;
            if (ok && reverseStatus) {
                sr = COMPILE_PASS_UNEXPECT;
                st = Status.FAILED;
            } else if (ok && !reverseStatus) {
                sr = COMPILE_PASS;
            } else if (!ok && reverseStatus) {
                sr = COMPILE_FAIL_EXPECT;
                st = Status.PASSED;
            } else { /* !ok && !reverseStatus */
                sr = COMPILE_FAIL;
            }
            if ((st == Status.FAILED) && ! (status.getReason() == null) &&
                    !status.getReason().equals(EXEC_PASS))
                sr += ": " + status.getReason();
            status = createStatus(st, sr);
        }
        return status;
    }

    /**
     * Compare output against a reference file.
     * @param status default result if no differences found
     * @param actual the text to be compared against the reference file
     * @return a status indicating the first difference, or the default status
     *          if no differences found
     * @throws TestRunException if the reference file can't be found
     */
    private Status checkGoldenFile(String actual, Status status) throws TestRunException {
        File refFile = new File(script.absTestSrcDir(), ref);
        try (BufferedReader r1 = new BufferedReader(new StringReader(actual));
            BufferedReader r2 = new BufferedReader(new FileReader(refFile)) ) {
            int lineNum;
            if ((lineNum = compareGoldenFile(r1, r2)) != 0) {
                return failed(COMPILE_GOLD_FAIL + ref +
                        COMPILE_GOLD_LINE + lineNum);
            }
            return status;
        } catch (FileNotFoundException e) {
            throw new TestRunException(COMPILE_CANT_FIND_REF + refFile);
        } catch (IOException e) {
            throw new TestRunException(COMPILE_CANT_READ_REF + refFile);
        }
    }

    /**
     * Line by line comparison of compile output and a reference file.  If no
     * differences are found, then 0 is returned.  Otherwise, the line number
     * where differences are first detected is returned.
     *
     * @param r1   The first item for comparison.
     * @param r2   The second item for comparison.
     * @return 0   If no differences are returned.  Otherwise, the line number
     *             where differences were first detected.
     */
    private int compareGoldenFile(BufferedReader r1, BufferedReader r2)
    throws TestRunException {
        try {
            int lineNum = 0;
            for ( ; ; ) {
                String s1 = r1.readLine();
                String s2 = r2.readLine();
                lineNum++;

                if ((s1 == null) && (s2 == null))
                    return 0;
                if ((s1 == null) || (s2 == null) || !s1.equals(s2)) {
                    return lineNum;
                }
            }
        } catch (IOException e) {
            File refFile = new File(script.absTestSrcDir(), ref);
            throw new TestRunException(COMPILE_GOLD_READ_PROB + refFile);
        }
    } // compareGoldenFile()

    private void addModule(String file) {
        int sep = file.indexOf('/');
        if (sep > 0)
            modules.add(file.substring(0, sep));
    }

    //----------member variables------------------------------------------------

    private LibLocn libLocn;
    private File destDir;

    private boolean reverseStatus = false;
    private String  ref = null;
    private int     timeout = -1;
    private boolean classpathp  = false;
    private boolean sourcepathp = false;
    private boolean process = false;
    private String module = null;
    private boolean multiModule = false;
    private Set<String> modules;
    private boolean addDebugOpts = false;
    protected Set<String> othervmOverrideReasons = new LinkedHashSet<>();
}
