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
        public void Torrent_RaisesOnPropertyChanged_WhenSet()
        {
            // Arrange

            var wrapper = new TorrentWrapper(new Torrent());
            var isFired = false;
            wrapper.PropertyChanged += (s, a) => isFired = a.PropertyName == nameof(TorrentWrapper.Torrent);

            // Act

            wrapper.Torrent = new Torrent();

            // Assert

            Assert.IsTrue(isFired);
        }

        
    }
}
