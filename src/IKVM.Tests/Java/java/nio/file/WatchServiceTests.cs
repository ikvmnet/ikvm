using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

using com.sun.nio.file;

using FluentAssertions;

using java.lang;
using java.nio.file;
using java.util.concurrent;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Java.java.nio.file
{

    [TestClass]
    public class WatchServiceTests
    {

        [TestMethod]
        public void CanWatchDirectoryForBasicOperations()
        {
            // though it works on OS X, events come out in a bit of an incorrect order, and we aren't going to fix this today
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                return;

            var cts = new CancellationTokenSource();

            using var watcher = FileSystems.getDefault().newWatchService();

            var dir = Paths.get(System.IO.Path.GetTempPath(), Guid.NewGuid().ToString());
            System.IO.Directory.CreateDirectory(dir.ToString());

            var key = dir.register(watcher, new[] { StandardWatchEventKinds.ENTRY_CREATE, StandardWatchEventKinds.ENTRY_DELETE }, SensitivityWatchEventModifier.HIGH);
            var lst = new List<(WatchEvent.Kind, string)>();

            var tsk = Task.Run(() =>
            {
                while (cts.IsCancellationRequested == false)
                {
                    var key = watcher.poll(1, TimeUnit.SECONDS);
                    if (key == null)
                        continue;

                    foreach (var evt in key.pollEvents().AsEnumerable<WatchEvent>())
                    {
                        var kind = evt.kind();
                        if (kind == StandardWatchEventKinds.OVERFLOW)
                            continue;

                        var name = (Path)evt.context();
                        var item = dir.resolve(name).ToString();
                        lst.Add((kind, item));
                    }

                    key.reset();
                }
            });

            System.Threading.Thread.Sleep(3000);

            // create a file and directory
            System.IO.File.WriteAllText(dir.resolve("test.txt").ToString(), "HELLO");
            System.Threading.Thread.Sleep(3000);
            System.IO.Directory.CreateDirectory(dir.resolve("test.dir").ToString());
            System.Threading.Thread.Sleep(3000);

            // remove a file and directory
            System.IO.File.Delete(dir.resolve("test.txt").ToString());
            System.Threading.Thread.Sleep(3000);
            System.IO.Directory.Delete(dir.resolve("test.dir").ToString());
            System.Threading.Thread.Sleep(3000);

            // wait for event loop to exit
            cts.Cancel();
            tsk.Wait();
            key.cancel();

            lst[0].Item1.Should().Be(StandardWatchEventKinds.ENTRY_CREATE);
            lst[0].Item2.Should().Be(System.IO.Path.Combine(dir.ToString(), "test.txt"));

            lst[1].Item1.Should().Be(StandardWatchEventKinds.ENTRY_CREATE);
            lst[1].Item2.Should().Be(System.IO.Path.Combine(dir.ToString(), "test.dir"));

            lst[2].Item1.Should().Be(StandardWatchEventKinds.ENTRY_DELETE);
            lst[2].Item2.Should().Be(System.IO.Path.Combine(dir.ToString(), "test.txt"));

            lst[3].Item1.Should().Be(StandardWatchEventKinds.ENTRY_DELETE);
            lst[3].Item2.Should().Be(System.IO.Path.Combine(dir.ToString(), "test.dir"));
        }

    }

}
