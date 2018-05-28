﻿using Bubak.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bubak.ViewModel
{
    public interface ITorrentWrapperFactory
    {
        ITorrentWrapper Create(Torrent torrent);
    }
}
