using Bubak.Shared.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bubak.Client
{
    public class TorrentClientSettings
    {
        public Range PortRange { get; set; }
        public string SavePath { get; set; }
        //public TimeSpan AddTorrentTimeout { get; set; }
    }
}
