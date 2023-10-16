package java.lang;

import java.io.*;
import java.util.*;

final class ProcessImpl extends Process {

    cli.System.Diagnostics.Process process;
    OutputStream outputStream;
    InputStream inputStream;
    InputStream errorStream;

    static native Process start(String cmdarray[], java.util.Map<String, String> environment, String dir, ProcessBuilder.Redirect[] redirects, boolean redirectErrorStream) throws IOException;

    private ProcessImpl(cli.System.Diagnostics.Process process, OutputStream outputStream, InputStream inputStream, InputStream errorStream) {
        this.process = process;
        this.outputStream = outputStream;
        this.inputStream = inputStream;
        this.errorStream = errorStream;
    }

    @Override
    public OutputStream getOutputStream() {
        return outputStream;
    }

    @Override
    public InputStream getInputStream() {
        return inputStream;
    }
    
    @Override
    public InputStream getErrorStream() {
        return errorStream;
    }
    
    @Override
    public native int waitFor() throws InterruptedException;
    
    @Override
    public native int exitValue();
    
    @Override
    public native void destroy();

}
