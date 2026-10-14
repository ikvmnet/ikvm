using System;
using System.IO;

using FluentAssertions;

using Microsoft.Build.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace IKVM.MSBuild.Tasks.Tests
{

    [TestClass]
    public class IkvmCleanDirTests
    {

        static readonly TimeSpan MaxAge = TimeSpan.FromDays(7);

        string root;
        string cache;

        [TestInitialize]
        public void Setup()
        {
            root = Path.Combine(Path.GetTempPath(), "IkvmCleanDirTests", Guid.NewGuid().ToString("N"));
            cache = Path.Combine(root, "cache", "1");
            Directory.CreateDirectory(cache);
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (Directory.Exists(root))
                Directory.Delete(root, true);
        }

        /// <summary>
        /// Creates a cache entry with a single output file, everything within it last written at the given age.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="age"></param>
        /// <returns></returns>
        string CreateEntry(string name, TimeSpan age)
        {
            var entry = Path.Combine(cache, name);
            Directory.CreateDirectory(entry);
            var file = Path.Combine(entry, "output.dll");
            File.WriteAllBytes(file, [1, 2, 3]);
            File.SetLastWriteTimeUtc(file, DateTime.UtcNow - age);
            Directory.SetLastWriteTimeUtc(entry, DateTime.UtcNow - age);
            return entry;
        }

        bool Execute()
        {
            var task = new IkvmCleanDir();
            task.BuildEngine = new Mock<IBuildEngine>().Object;
            task.Directory = cache;
            task.MaxAge = (int)MaxAge.TotalMinutes;
            return task.Execute();
        }

        [TestMethod]
        public void Should_remove_expired_entry_as_a_whole()
        {
            var entry = CreateEntry("expired", TimeSpan.FromDays(8));
            File.WriteAllBytes(Path.Combine(entry, "output.pdb"), [1]);
            File.SetLastWriteTimeUtc(Path.Combine(entry, "output.pdb"), DateTime.UtcNow - TimeSpan.FromDays(8));
            Directory.SetLastWriteTimeUtc(entry, DateTime.UtcNow - TimeSpan.FromDays(8));

            Execute().Should().BeTrue();
            Directory.Exists(entry).Should().BeFalse();
        }

        [TestMethod]
        public void Should_keep_recent_entry()
        {
            var entry = CreateEntry("recent", TimeSpan.FromDays(1));
            Execute().Should().BeTrue();
            Directory.Exists(entry).Should().BeTrue();
        }

        [TestMethod]
        public void Should_keep_old_entry_marked_as_used()
        {
            var entry = CreateEntry("used", TimeSpan.FromDays(30));
            IkvmTaskUtil.MarkCacheEntryUsed(entry);
            Execute().Should().BeTrue();
            File.Exists(Path.Combine(entry, "output.dll")).Should().BeTrue();
        }

        [TestMethod]
        public void Should_keep_new_empty_entry()
        {
            // another build has created the entry, but not yet written its output
            var entry = Path.Combine(cache, "creating");
            Directory.CreateDirectory(entry);
            Execute().Should().BeTrue();
            Directory.Exists(entry).Should().BeTrue();
        }

        [TestMethod]
        public void Should_record_last_clean()
        {
            Execute().Should().BeTrue();
            var lastClean = Path.Combine(cache, IkvmCleanDir.LastCleanFileName);
            File.Exists(lastClean).Should().BeTrue();
            File.GetLastWriteTimeUtc(lastClean).Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));
        }

        [TestMethod]
        public void Should_only_refresh_used_marker_once_a_day()
        {
            var entry = CreateEntry("used", TimeSpan.FromDays(30));
            var used = Path.Combine(entry, IkvmTaskUtil.CacheEntryUsedFileName);

            IkvmTaskUtil.MarkCacheEntryUsed(entry);
            var marked = DateTime.UtcNow - TimeSpan.FromHours(2);
            File.SetLastWriteTimeUtc(used, marked);
            IkvmTaskUtil.MarkCacheEntryUsed(entry);
            File.GetLastWriteTimeUtc(used).Should().Be(marked);

            File.SetLastWriteTimeUtc(used, DateTime.UtcNow - TimeSpan.FromDays(2));
            IkvmTaskUtil.MarkCacheEntryUsed(entry);
            File.GetLastWriteTimeUtc(used).Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));
        }

        [TestMethod]
        public void Should_not_mark_missing_entry()
        {
            var entry = Path.Combine(cache, "missing");
            IkvmTaskUtil.MarkCacheEntryUsed(entry);
            Directory.Exists(entry).Should().BeFalse();
        }

    }

}
