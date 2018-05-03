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
                Console.ReadKey();

                var url1 = "magnet:?xt=urn:btih:1a8f9f4c7bd395cda3deaef5af8d22e31f6379ba&dn=Queen+Discography+%40+320Kbps&tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&tr=udp%3A%2F%2Fzer0day.ch%3A1337&tr=udp%3A%2F%2Fopen.demonii.com%3A1337&tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&tr=udp%3A%2F%2Fexodus.desync.com%3A6969";
                var url2 = "magnet:?xt=urn:btih:8540440a9bffaf69af24c0cc5dba7311499482fc&dn=Pink+Floyd+-+Discography+%5B1967-2014%40320Kbps%5D&tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&tr=udp%3A%2F%2Fzer0day.ch%3A1337&tr=udp%3A%2F%2Fopen.demonii.com%3A1337&tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&tr=udp%3A%2F%2Fexodus.desync.com%3A6969";
                client.AddTorrent(url1);
                //client.AddTorrent(url2);

                while (true)
                {
                    
                    Console.ReadKey();
                    client.Pause();
                    Console.ReadKey();
                    client.Resume();
                    Console.ReadKey();
                    client.RemoveTorrent(client.Torrents.FirstOrDefault(), true);

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
