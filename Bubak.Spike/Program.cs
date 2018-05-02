using Bubak.Client;
using Ragnar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Bubak.Spike
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var client = new TorrentClient())
            {
                client.Settings.SavePath = @"C:\Users\marko\Desktop\libtorrent";
                var url = "http://rarbgmirror.org/download.php?id=3naco1p&f=DoctorAdventures%20-%20Alina%20Lopez%20-%20Pumping%20Under%20Pressure%20480p%20mp4-[rarbg.to].torrent";
                var torrent = client.AddTorrent(url);
                while (true)
                {
             
                    Thread.Sleep(1000);
                    client.Update();

                    Console.WriteLine(torrent.Name);
                    Console.WriteLine((float)torrent.DownloadedBytes / (float)torrent.TotalBytes);
                    Console.WriteLine($"{torrent.Progress}");
                    Console.WriteLine();
                   
                }
            }

            
            //using (var session = new Session())
            //{
            //    var addParams = new AddTorrentParams
            //    {
            //        SavePath = @"C:\Users\marko\Desktop\libtorrent",
            //        Url = "http://rarbgmirror.org/download.php?id=3naco1p&f=DoctorAdventures%20-%20Alina%20Lopez%20-%20Pumping%20Under%20Pressure%20480p%20mp4-[rarbg.to].torrent",
                     
            //    };

            //    var handle = session.AddTorrent(addParams);
                

            //    while (true)
            //    {
            //        // Get a `TorrentStatus` instance from the handle.
            //        var status = handle.QueryStatus();
            //        //var f = handle.TorrentFile.FileAt(0);
            //        var files = handle.GetFilePriorities().Select((p, i) => handle.TorrentFile.FileAt(i)).ToList();
            //        // If we are seeding, our job here is done.
            //        if (status.IsSeeding)
            //        {
            //            break;
            //        }

            //        // Print our progress and sleep for a bit.
            //        Console.WriteLine("{0}% downloaded", status.Progress * 100);
            //        Thread.Sleep(1000);
            //    }
            //}
        }
    }
}
