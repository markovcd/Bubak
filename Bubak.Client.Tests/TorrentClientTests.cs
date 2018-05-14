using System;
using System.Linq;
using System.Reflection;
using Bubak.Shared.Misc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Ragnar;

namespace Bubak.Client.Tests
{
    [TestClass]
    public class TorrentClientTests
    {
        Mock<ISession> _sessionMock;
        Mock<IDisposable> _sessionDisposableMock;
        Mock<ILogger> _loggerMock;
        TorrentClient _client;

        

        [TestInitialize]
        public void Setup()
        {
            _sessionMock = new Mock<ISession>();
            _sessionDisposableMock = _sessionMock.As<IDisposable>();
            _loggerMock = new Mock<ILogger>();
            _client = new TorrentClient(_loggerMock.Object, _sessionMock.Object);
        }

        [TestMethod]
        public void Dispose_DisposesSessionOnce_WhenCalledTwice()
        {
            _client.Dispose();
            _client.Dispose();
            _sessionDisposableMock.Verify(d => d.Dispose(), Times.Once);
        }

        [TestMethod]
        public void Dispose_DisposesSession_WhenCalled()
        {
            _client.Dispose();
            _sessionDisposableMock.Verify(d => d.Dispose(), Times.Once);
        }
    }
}
