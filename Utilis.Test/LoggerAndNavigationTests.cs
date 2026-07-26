using System;
using System.Linq;
using System.Threading;
using NUnit.Framework;
using Utilis.Messaging;
using Utilis.UI.Navigation;

namespace Utilis.Test
{
    [TestFixture]
    public class LoggerAndNavigationTests
    {
        [Test]
        public void LogInfo_SendsStatusMessage_WithExpectedFields()
        {
            var listener = new CapturingStatusListener();
            using (Bus.Instance.ListenFor(listener))
            {
                Logger.LogInfo("hello", "ctx");
                Assert.IsTrue(listener.WaitForMessage());
            }

            Assert.IsTrue(listener.Messages.Any(m =>
                m.Data.Type == StatusMessage.Types.Information
                && m.Data.Context == "ctx"
                && m.Data.Message == "hello"));
        }

        [Test]
        public void LogError_SendsStatusMessage_WithExpectedFields()
        {
            var listener = new CapturingStatusListener();
            using (Bus.Instance.ListenFor(listener))
            {
                Logger.LogError("err", "detail", "ctx2");
                Assert.IsTrue(listener.WaitForMessage());
            }

            Assert.IsTrue(listener.Messages.Any(m =>
                m.Data.Type == StatusMessage.Types.Error
                && m.Data.Context == "ctx2"
                && m.Data.Message == "err"
                && m.Data.TechnicalDetail == "detail"));
        }

        [Test]
        public void LogException_SendsErrorMessage()
        {
            var listener = new CapturingStatusListener();
            using (Bus.Instance.ListenFor(listener))
            {
                Logger.Log(new InvalidOperationException("bad op"), "RunContext");
                Assert.IsTrue(listener.WaitForMessage());
            }

            Assert.IsTrue(listener.Messages.Any(m =>
                m.Data.Type == StatusMessage.Types.Error
                && m.Data.Context == "RunContext"
                && ( m.Data.Message?.Contains("Error in RunContext") ?? false )
                && ( m.Data.TechnicalDetail?.Contains("bad op") ?? false )));
        }

        [Test]
        public void LogTimeSpan_SendsDebugMessageWithDuration()
        {
            var listener = new CapturingStatusListener();
            using (Bus.Instance.ListenFor(listener))
            {
                Logger.Log(TimeSpan.FromSeconds(2), "Save", "Ctx");
                Assert.IsTrue(listener.WaitForMessage());
            }

            Assert.IsTrue(listener.Messages.Any(m =>
                m.Data.Type == StatusMessage.Types.Debug
                && m.Data.Context == "Ctx"
                && ( m.Data.Message?.Contains("Save took") ?? false )));
        }

        [Test]
        public void FakeNavigationService_Navigate_SetsCurrentViewModel()
        {
            var nav = new FakeNavigationService();
            var vm = new TestViewModel();

            var result = nav.Navigate(vm);

            Assert.IsTrue(result);
            Assert.AreSame(vm, nav.CurrentViewModel);
            Assert.IsTrue(nav.CanGoBack());
        }

        [Test]
        public void FakeNavigationService_NavigateAsync_SetsCurrentViewModel()
        {
            var nav = new FakeNavigationService();
            var vm = new TestViewModel();

            var result = nav.NavigateAsync(vm).GetAwaiter().GetResult();

            Assert.IsTrue(result);
            Assert.AreSame(vm, nav.CurrentViewModel);
        }

        private sealed class CapturingStatusListener : IListener<StatusMessage>
        {
            private readonly ManualResetEventSlim _signal = new ManualResetEventSlim(false);
            private readonly System.Collections.Concurrent.ConcurrentQueue<StatusMessage> _messages = new System.Collections.Concurrent.ConcurrentQueue<StatusMessage>();

            public System.Collections.Generic.IReadOnlyCollection<StatusMessage> Messages => _messages.ToArray();

            public void Receive(StatusMessage message)
            {
                _messages.Enqueue(message);
                _signal.Set();
            }

            public bool WaitForMessage()
            {
                return _signal.Wait(TimeSpan.FromSeconds(2));
            }
        }

        private class TestViewModel : Utilis.UI.ViewModel.Base
        {
        }
    }
}
