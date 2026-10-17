using System;
using System.Diagnostics;
using System.IO;

static class Program
{

    static void Try(string label, Action a)
    {
        try { a(); Console.WriteLine($"| {label} | OK |"); }
        catch (Exception e) { Console.WriteLine($"| {label} | FAIL {e.GetType().Name}: {(e is java.lang.Throwable t ? t.getMessage() : e.Message)} |"); }
    }

    static void Main(string[] args)
    {
        var hdir = args[0];
        Console.WriteLine("| step | result |");
        Console.WriteLine("|---|---|");
        Console.WriteLine($"| host exe | {Environment.ProcessPath} |");
        Console.WriteLine($"| java.home | {java.lang.System.getProperty("java.home")} |");
        if (args.Length > 2)
        {
            // dependency dir is added to PATH only after startup, from inside the process
            Environment.SetEnvironmentVariable("PATH", Environment.GetEnvironmentVariable("PATH") + ";" + args[2]);
            Try("in-process PATH: System.load(X\\xa.dll)", () => java.lang.System.load(Path.Combine(args[1], "xa.dll")));
            Try("in-process PATH: System.load(X\\xa2.dll)", () => java.lang.System.load(Path.Combine(args[1], "xa2.dll")));
            return;
        }
        Try("System.loadLibrary(\"xa\")  xa(X on PATH) -> yb(Y on PATH)", () => java.lang.System.loadLibrary("xa"));
        Try("System.loadLibrary(\"xa2\") xa2(X) -> xm(X) -> yb(Y on PATH)", () => java.lang.System.loadLibrary("xa2"));
        Try("System.load(H\\hp.dll)  hp -> hq -> hr, all in H", () => java.lang.System.load(Path.Combine(hdir, "hp.dll")));
        Try("System.load(H\\hp2.dll) hp2(H) -> ys(Y on PATH) -> hu(only H)", () => java.lang.System.load(Path.Combine(hdir, "hp2.dll")));
        java.lang.System.setProperty("java.awt.headless", "true");
        Try("java.util.zip.CRC32", () => { var c = new java.util.zip.CRC32(); c.update(new byte[] { 1, 2, 3 }); c.getValue(); });
        Try("java.util.zip.Deflater", () => { var d = new java.util.zip.Deflater(); d.setInput(new byte[] { 1, 2, 3 }); d.finish(); d.deflate(new byte[64]); d.end(); });
        Try("java.net.InetAddress.getLocalHost", () => java.net.InetAddress.getLocalHost());
        Try("java.net.ServerSocket(0)", () => new java.net.ServerSocket(0).close());
        Try("java.nio.channels.ServerSocketChannel.open", () => java.nio.channels.ServerSocketChannel.open().close());
        Try("java.awt.image.ColorModel.getRGBdefault", () => java.awt.image.ColorModel.getRGBdefault());
        Try("javax.imageio.ImageIO.write png", () => javax.imageio.ImageIO.write(new java.awt.image.BufferedImage(4, 4, java.awt.image.BufferedImage.TYPE_INT_ARGB), "png", new java.io.ByteArrayOutputStream()));
        Try("java.awt.Font.createFont/getStringBounds", () => new java.awt.Font("Dialog", 0, 12).getStringBounds("abc", new java.awt.font.FontRenderContext(null, true, true)));
        var home = Path.GetFullPath(java.lang.System.getProperty("java.home"));
        foreach (ProcessModule m in Process.GetCurrentProcess().Modules)
            if (m.FileName.StartsWith(home, StringComparison.OrdinalIgnoreCase) || m.FileName.Contains(@"\dlltest\"))
                Console.WriteLine($"| loaded module | {m.FileName} |");
    }

}
