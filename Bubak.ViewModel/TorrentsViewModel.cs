﻿using Bubak.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Bubak.Shared.Misc;

namespace Bubak.ViewModel
{
    public class TorrentsViewModel
    {
        private readonly ITorrentClient _client;
        private readonly IEventAggregator _eventAggregator;
        private readonly ILogger _logger;

        public ObservableCollection<TorrentViewModel> Torrents { get; }

        

        public TorrentsViewModel(ITorrentClient client, IEventAggregator eventAggregator, ILogger logger)
        {
            _logger = logger;
            _client = client;
            _eventAggregator = eventAggregator;

            Torrents = new ObservableCollection<TorrentViewModel>();

        }

        public void AddTorrent(string url)
        {

        }
    
    }
}
