using System;
using System.Linq;
using System.Threading.Tasks;
using Bubak.Client;
using Bubak.Shared.Misc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Bubak.ViewModel.Tests
{
    [TestClass]
    public class TorrentsViewModelTests
    {
        private Mock<ITorrentClient> _clientMock;
        private Mock<ILogger> _loggerMock;     
        private Mock<ITorrent> _torrentMock;
        private Mock<ITorrentViewModel> _torrentViewModelMock;
        private TorrentsViewModel _torrentsViewModel;

        [TestInitialize]
        public void Setup()
        {
            _clientMock = new Mock<ITorrentClient>();
            _loggerMock = new Mock<ILogger>();
            _torrentMock = new Mock<ITorrent>();
            _torrentViewModelMock = new Mock<ITorrentViewModel>();
        }

        [TestMethod]
        public async Task AddTorrentAsync_AddsTorrentInViewModel_WhenCalled()
        {
            // Arrange

            Func<ITorrent, string, ITorrentViewModel> torrentVmCreator = (t, u) =>
            {
                _torrentViewModelMock.SetupGet(tvm => tvm.Url).Returns(u);
                _torrentViewModelMock.SetupGet(tvm => tvm.Torrent).Returns(t);
                return _torrentViewModelMock.Object;
            };

            _torrentsViewModel = new TorrentsViewModel(_clientMock.Object, null, _loggerMock.Object, torrentVmCreator);

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

            Func<ITorrent, string, ITorrentViewModel> torrentVmCreator = (t, u) =>
            {
                _torrentViewModelMock.SetupGet(tvm => tvm.Url).Returns(u);
                _torrentViewModelMock.SetupGet(tvm => tvm.Torrent).Returns(t);
                return _torrentViewModelMock.Object;
            };
            
            _torrentsViewModel = new TorrentsViewModel(_clientMock.Object, null, _loggerMock.Object, torrentVmCreator);

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

            _torrentsViewModel = new TorrentsViewModel(_clientMock.Object, null, _loggerMock.Object, null);

            _torrentsViewModel.Torrents.Add(_torrentViewModelMock.Object);
            var removed = await _torrentsViewModel.RemoveTorrentAsync(_torrentViewModelMock.Object, false);

            Assert.IsTrue(removed);
            Assert.IsFalse(_torrentsViewModel.Torrents.Any());
        }

        [TestMethod]
        public async Task RemoveTorrentAsync_ReturnsFalse_TorrentViewModelNotFound()
        {
            // Arrange

            _torrentsViewModel = new TorrentsViewModel(_clientMock.Object, null, _loggerMock.Object, null);
;
            var removed = await _torrentsViewModel.RemoveTorrentAsync(_torrentViewModelMock.Object, false);

            Assert.IsFalse(removed);
            Assert.IsFalse(_torrentsViewModel.Torrents.Any());
        }
    }
}
