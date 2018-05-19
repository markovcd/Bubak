namespace Bubak.Client.Wrappers
{
    public class AnnounceEntry
    {
        public bool SendStats { get; }
        public bool CompleteSent { get; }
        public bool StartSent { get; }
        public bool Verified { get; }
        public bool Updating { get; }
        public int Fails { get; }
        public int FailLimit { get; }
        public int Tier { get; }
        public int ScrapeDownloaded { get; }
        public int ScrapeComplete { get; }
        public int ScrapeIncomplete { get; }
        public string Message { get; }
        public string TrackerId { get; }
        public string Url { get; }

        public AnnounceEntry()
        {
        }

        public AnnounceEntry(Ragnar.AnnounceEntry announceEntry)
        {
            try
            {
                SendStats = announceEntry.SendStats;
                CompleteSent = announceEntry.CompleteSent;
                StartSent = announceEntry.StartSent;
                Verified = announceEntry.Verified;
                Updating = announceEntry.Updating;
                Fails = announceEntry.Fails;
                FailLimit = announceEntry.FailLimit;
                Tier = announceEntry.Tier;
                ScrapeDownloaded = announceEntry.ScrapeDownloaded;
                ScrapeComplete = announceEntry.ScrapeComplete;
                ScrapeIncomplete = announceEntry.ScrapeIncomplete;
                Message = announceEntry.Message;
                TrackerId = announceEntry.TrackerId;
                Url = announceEntry.Url;
            }
            finally
            {
                announceEntry.Dispose();
            }
        }
    }
}