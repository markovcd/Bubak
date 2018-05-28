using System;
using Bubak.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bubak.ViewModel.Tests
{
    [TestClass]
    public class TorrentWrapperFactoryTests
    {
        [TestMethod]
        public void Create_WrapsTorrent_WhenCalled()
        {
            // Arrange

            var factory = new TorrentWrapperFactory();
            var torrent = new Torrent();

            // Act

            var wrapper = factory.Create(torrent);

            // Assert

            Assert.IsNotNull(wrapper);
            Assert.AreEqual(torrent, wrapper.Torrent);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))] // Assert
        public void Create_ThrowsArgumentNullException_WhenTorrentNull()
        {
            // Arrange

            var factory = new TorrentWrapperFactory();

            // Act

            var wrapper = factory.Create(null);
        }
    }
}
