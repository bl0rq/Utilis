using System;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Utilis.Test
{
    [TestFixture]
    public class RunnerTests
    {
        [Test]
        public void RunReadLocked_ExecutesAndReleasesLock()
        {
            var rw = new System.Threading.ReaderWriterLockSlim();
            var wasHeldInside = false;

            Runner.RunReadLocked(() => { wasHeldInside = rw.IsReadLockHeld; }, rw);

            Assert.IsTrue(wasHeldInside);
            Assert.IsFalse(rw.IsReadLockHeld);
        }

        [Test]
        public void RunWriteLocked_ReturnsValueAndReleasesLock()
        {
            var rw = new System.Threading.ReaderWriterLockSlim();
            var wasHeldInside = false;

            var result = Runner.RunWriteLocked(() =>
            {
                wasHeldInside = rw.IsWriteLockHeld;
                return 42;
            }, rw);

            Assert.IsTrue(wasHeldInside);
            Assert.AreEqual(42, result);
            Assert.IsFalse(rw.IsWriteLockHeld);
        }

        [Test]
        public void RunWrapped_ReportsError_WhenErrorHandlerSubscribed()
        {
            var called = false;
            Action<Exception, string> handler = (ex, context) =>
            {
                if (context == "Runner.RunWrapped" && ex is InvalidOperationException)
                    called = true;
            };

            try
            {
                Runner.Error += handler;
                Runner.RunWrapped(() => throw new InvalidOperationException("boom"));
            }
            finally
            {
                Runner.Error -= handler;
            }

            Assert.IsTrue(called);
        }

        [Test]
        public void RunOnDispatcherThread_UsesDispatcherWhenNoAccess()
        {
            var originalDispatcher = Runner.Dispatcher;
            var dispatcher = new CountingDispatcher();
            var wasInvoked = false;

            try
            {
                Runner.Dispatcher = dispatcher;
                Runner.RunOnDispatcherThread(() => wasInvoked = true);
            }
            finally
            {
                Runner.Dispatcher = originalDispatcher;
            }

            Assert.AreEqual(1, dispatcher.RunAsyncCalls);
            Assert.IsTrue(wasInvoked);
        }

        [Test]
        public void RunOnDispatcherThreadBlocking_CallsCancellationCallback_WhenTaskCanceled()
        {
            var originalDispatcher = Runner.Dispatcher;
            var callbackCalled = false;

            try
            {
                Runner.Dispatcher = new CancelingDispatcher();
                Runner.RunOnDispatcherThreadBlocking(() => { }, () => callbackCalled = true);
            }
            finally
            {
                Runner.Dispatcher = originalDispatcher;
            }

            Assert.IsTrue(callbackCalled);
        }

        private sealed class CountingDispatcher : IDispatcher
        {
            public int RunAsyncCalls { get; private set; }

            public bool CheckAccess()
            {
                return false;
            }

            public Task RunAsync(Action act)
            {
                RunAsyncCalls++;
                act();
                return Task.CompletedTask;
            }
        }

        private sealed class CancelingDispatcher : IDispatcher
        {
            public bool CheckAccess()
            {
                return false;
            }

            public Task RunAsync(Action act)
            {
                return Task.FromCanceled(new CancellationToken(true));
            }
        }
    }
}
