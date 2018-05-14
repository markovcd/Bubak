using System;
using Bubak.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Bubak.ViewModel.Tests
{
    [TestClass]
    public class TorrentWrapperTests
    {
        [TestInitialize]
        public void Setup()
        {
 
        }

        [TestMethod]
        public void RaisesOnPropertyChanged_WhenTorrentUpdated()
        {
            // Arrange

            var url = "some url";
            var wrapper = new TorrentWrapper(url, new Torrent());
            var isFired = false;
            wrapper.PropertyChanged += (s, a) => isFired = a.PropertyName == nameof(TorrentWrapper.Torrent);

            // Act

            //_torrentMock.Raise(t => t.Updated += null, _torrentMock.Object);

            // Assert

            Assert.IsTrue(isFired);
        }
    }
}
