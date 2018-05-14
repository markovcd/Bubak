using System;
using System.Linq;
using System.Threading.Tasks;
using Bubak.Client;
using Bubak.Shared.Misc;
using Caliburn.Micro;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Bubak.ViewModel.Tests
{
    [TestClass]
    public class TorrentsViewModelTests
    {
        private Mock<ITorrentClient> _clientMock;
        private Mock<ILogger> _loggerMock;     
        private Mock<ITorrentWrapper> _torrentViewModelMock;
        private Mock<IEventAggregator> _eventAggregatorMock;

        private TorrentsViewModel _torrentsViewModel;

        [TestInitialize]
        public void Setup()
        {
            _clientMock = new Mock<ITorrentClient>();
            _loggerMock = new Mock<ILogger>();
            _torrentViewModelMock = new Mock<ITorrentWrapper>();
            _eventAggregatorMock = new Mock<IEventAggregator>();
        }

        [TestMethod]
        public async Task AddTorrentAsync_AddsTorrentInViewModel_WhenCalled()
        {
            // Arrange

            Func<Torrent, string, ITorrentWrapper> torrentVmCreator = (t, u) =>
            {
                _torrentViewModelMock.SetupGet(tvm => tvm.Url).Returns(u);
                _torrentViewModelMock.SetupGet(tvm => tvm.Torrent).Returns(t);
                return _torrentViewModelMock.Object;
            };

            _torrentsViewModel = new TorrentsViewModel(_clientMock.Object, _eventAggregatorMock.Object, _loggerMock.Object, torrentVmCreator);

            var url = "some url";

            // Act

            var torrentVm = await _torrentsViewModel.AddTorrentAsync(url);

            // Assert

            Assert.IsNull(torrentVm.Torrent);
            Assert.AreEqual(1, _torrentsViewModel.Torrents.Count);
            Assert.AreEqual(torrentVm, _torrentsViewModel.Torrents[0]);
        }

        [TestMethod]
        public async Task AddTorrentAsync_AddsTorrentInClient_WhenCalled()
        {
            // Arrange

            Func<Torrent, string, ITorrentWrapper> torrentVmCreator = (t, u) =>
            {
                _torrentViewModelMock.SetupGet(tvm => tvm.Url).Returns(u);
                _torrentViewModelMock.SetupGet(tvm => tvm.Torrent).Returns(t);
                return _torrentViewModelMock.Object;
            };
            
            _torrentsViewModel = new TorrentsViewModel(_clientMock.Object, _eventAggregatorMock.Object, _loggerMock.Object, torrentVmCreator);

            var url = "some url";

            // Act

            var torrentVm = await _torrentsViewModel.AddTorrentAsync(url);

            // Assert

            _clientMock.Verify(c => c.AddTorrentAsync(url), Times.Once);
        }

        [TestMethod]
        public async Task RemoveTorrentAsync_RemovesTorrentInViewModel_WhenCalled()
        {
            // Arrange

            _torrentsViewModel = new TorrentsViewModel(_clientMock.Object, _eventAggregatorMock.Object, _loggerMock.Object, null);
            _torrentsViewModel.Torrents.Add(_torrentViewModelMock.Object);

            // Act

            var removed = await _torrentsViewModel.RemoveTorrentAsync(_torrentViewModelMock.Object, false);

            // Assert

            Assert.IsTrue(removed);
            Assert.IsFalse(_torrentsViewModel.Torrents.Any());
        }

        [TestMethod]
        public async Task RemoveTorrentAsync_RemovesTorrentInClient_WhenCalled()
        {
            // Arrange

            _torrentsViewModel = new TorrentsViewModel(_clientMock.Object, _eventAggregatorMock.Object, _loggerMock.Object, null);
            _torrentsViewModel.Torrents.Add(_torrentViewModelMock.Object);
            _torrentViewModelMock.SetupGet(t => t.Torrent).Returns(new Torrent());

            // Act

            var removed = await _torrentsViewModel.RemoveTorrentAsync(_torrentViewModelMock.Object, true);

            // Assert

            _clientMock.Verify(c => c.RemoveTorrentAsync(_torrentViewModelMock.Object.Torrent, true), Times.Once);
            Assert.IsTrue(removed);
        }

        [TestMethod]
        public async Task RemoveTorrentAsync_ReturnsFalse_TorrentViewModelNotFound()
        {
            // Arrange

            _torrentsViewModel = new TorrentsViewModel(_clientMock.Object, _eventAggregatorMock.Object, _loggerMock.Object, null);
;           _torrentsViewModel.Torrents.Add(new Mock<ITorrentWrapper>().Object);

            //Act

            var removed = await _torrentsViewModel.RemoveTorrentAsync(_torrentViewModelMock.Object, false);

            // Assert

            Assert.IsFalse(removed);
            Assert.AreEqual(1, _torrentsViewModel.Torrents.Count);
        }

        [TestMethod]
        public void Dispose_DisposesClient_WhenCalled()
        {
            // Arragne

            _clientMock.As<IDisposable>();
            _torrentsViewModel = new TorrentsViewModel(_clientMock.Object, _eventAggregatorMock.Object, _loggerMock.Object, null);

            // Act

            _torrentsViewModel.Dispose();

            // Assert

            _clientMock.As<IDisposable>().Verify(d => d.Dispose(), Times.Once);
        }

        [TestMethod]
        public void Dispose_DisposesClientOnce_WhenCalledTwice()
        {
            // Arragne

            _clientMock.As<IDisposable>();
            _torrentsViewModel = new TorrentsViewModel(_clientMock.Object, _eventAggregatorMock.Object, _loggerMock.Object, null);

            // Act

            _torrentsViewModel.Dispose();
            _torrentsViewModel.Dispose();

            // Assert

            _clientMock.As<IDisposable>().Verify(d => d.Dispose(), Times.Once);
        }
    }
}
