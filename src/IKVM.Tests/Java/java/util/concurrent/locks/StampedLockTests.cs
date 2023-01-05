using java.lang;
using java.util.concurrent;
using java.util.concurrent.locks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Java.java.util.concurrent.locks
{

    [TestClass]
    public class StampedLockTests
    {

        class ReadersUnlockAfterWriterUnlockContext
        {

            const int RNUM = 2;

            readonly StampedLock sl = new StampedLock();
            volatile bool isDone;

            readonly CyclicBarrier iterationStart = new CyclicBarrier(RNUM + 1);
            readonly CyclicBarrier readersHaveLocks = new CyclicBarrier(RNUM);
            readonly CyclicBarrier writerHasLock = new CyclicBarrier(RNUM + 1);

            class Reader : Thread
            {

                readonly ReadersUnlockAfterWriterUnlockContext owner;

                /// <summary>
                /// Initializes a new instance.
                /// </summary>
                /// <param name="owner"></param>
                /// <param name="name"></param>
                public Reader(ReadersUnlockAfterWriterUnlockContext owner, string name) : base(name)
                {
                    this.owner = owner;
                }

                public override void run()
                {
                    while (!owner.isDone && !isInterrupted())
                    {
                        try
                        {
                            owner.iterationStart.await();
                            owner.writerHasLock.await();
                            var rs = owner.sl.readLock();
                            owner.readersHaveLocks.await();
                            owner.sl.unlockRead(rs);
                        }
                        catch (System.Exception e)
                        {
                            throw new IllegalStateException(e);
                        }
                    }
                }

            }

            public void Run()
            {
                for (int r = 0; r < RNUM; ++r)
                    new Reader(this, "r" + r).start();

                int i;
                for (i = 0; i < 1024; ++i)
                {
                    try
                    {
                        iterationStart.await();
                        var ws = sl.writeLock();
                        writerHasLock.await();
                        Thread.sleep(10);
                        sl.unlockWrite(ws);
                    }
                    catch (System.Exception e)
                    {
                        throw new IllegalStateException(e);
                    }
                }

                isDone = true;
            }

        }

        [TestMethod]
        public void ReadersUnlockAfterWriterUnlock()
        {
            new ReadersUnlockAfterWriterUnlockContext().Run();
        }

    }

}
