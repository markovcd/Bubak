using System;
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
        //private Mock<ILogger>
        private TorrentsViewModel _torrentsViewModel;

        [TestInitialize]
        public void Setup()
        {
            _clientMock = new Mock<ITorrentClient>();
        }

        [TestMethod]
        public void TestMethod1()
        {
        }
    }
}
