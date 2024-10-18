using System;
using System.Threading;

namespace IKVM.CoreLib.Threading
{

    static class ReaderWriterLockSlimExtensions
    {

        public readonly struct ReaderWriterLockSlimReadLock : IDisposable
        {

            readonly ReaderWriterLockSlim @lock;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="lock"></param>
            /// <exception cref="ArgumentNullException"></exception>
            public ReaderWriterLockSlimReadLock(ReaderWriterLockSlim @lock)
            {
                this.@lock = @lock ?? throw new ArgumentNullException(nameof(@lock));
                @lock.EnterReadLock();
            }

            /// <inheritdoc />
            public readonly void Dispose()
            {
                @lock.ExitReadLock();
            }

        }

        public readonly struct ReaderWriterLockSlimUpradeableReadLock : IDisposable
        {

            readonly ReaderWriterLockSlim @lock;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="lock"></param>
            /// <exception cref="ArgumentNullException"></exception>
            public ReaderWriterLockSlimUpradeableReadLock(ReaderWriterLockSlim @lock)
            {
                this.@lock = @lock ?? throw new ArgumentNullException(nameof(@lock));
                @lock.EnterUpgradeableReadLock();
            }

            /// <inheritdoc />
            public readonly void Dispose()
            {
                @lock.ExitUpgradeableReadLock();
            }

        }

        public readonly struct ReaderWriterLockSlimWriteLock : IDisposable
        {

            readonly ReaderWriterLockSlim @lock;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="lock"></param>
            /// <exception cref="ArgumentNullException"></exception>
            public ReaderWriterLockSlimWriteLock(ReaderWriterLockSlim @lock)
            {
                this.@lock = @lock ?? throw new ArgumentNullException(nameof(@lock));
                @lock.EnterWriteLock();
            }

            /// <inheritdoc />
            public readonly void Dispose()
            {
                @lock.ExitWriteLock();
            }

        }

        public static ReaderWriterLockSlimReadLock CreateReadLock(this ReaderWriterLockSlim @lock)
        {
            return new ReaderWriterLockSlimReadLock(@lock);
        }

        public static ReaderWriterLockSlimUpradeableReadLock CreateUpgradeableReadLock(this ReaderWriterLockSlim @lock)
        {
            return new ReaderWriterLockSlimUpradeableReadLock(@lock);
        }

        public static ReaderWriterLockSlimWriteLock CreateWriteLock(this ReaderWriterLockSlim @lock)
        {
            return new ReaderWriterLockSlimWriteLock(@lock);
        }

    }

}
