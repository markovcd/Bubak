using Ragnar;

namespace Bubak.Client
{
    public partial class TorrentClient
    {
        public delegate void UrlHandler(TorrentClient sender, string url);
        public delegate void ResumeDataHandler(TorrentClient sender, byte[] resumeData, string message);
        public delegate void MessageHandler(TorrentClient sender, string message);
        public delegate void TorrentHandler(TorrentClient sender, Torrent torent, string message);
        public delegate void TorrentFileHandler(TorrentClient sender, Torrent torent, File file, string message);
        public delegate void TorrentFileNameHandler(TorrentClient sender, Torrent torent, File file, string fileName, string message);
        public delegate void TorrentStateChangeHandler(TorrentClient sender, Torrent torent, TorrentState currentState, TorrentState previousState, string message);

        public event UrlHandler TorrentAddFailed;
        public event TorrentHandler TorrentAdded;
        public event TorrentHandler TorrentChecked;
        public event TorrentHandler TorrentResumed;
        public event TorrentHandler TorrentRemoved;
        public event TorrentHandler TorrentPaused;
        public event TorrentHandler TorrentFinished;
        public event TorrentHandler TorrentMetadataReceived;
        public event TorrentFileHandler TorrentFileCompleted;
        public event TorrentFileNameHandler TorrentFileRenamed;
        public event TorrentStateChangeHandler TorrentStateChanged;
        public event ResumeDataHandler ResumeDataSaved;

        public event MessageHandler UnknownAlert;

        protected void RaiseEvent(Alert alert)
        {
            switch (alert)
            {
                case FileCompletedAlert fileCompleted:
                    OnFileCompleted(fileCompleted);
                    break;
                case FileRenamedAlert fileRenamed:
                    OnFileRenamed(fileRenamed);
                    break;
                case MetadataReceivedAlert metadataReceived:
                    OnMetadataReceived(metadataReceived);
                    break;
                case SaveResumeDataAlert saveResumeData:
                    OnSaveResumeData(saveResumeData);
                    break;
                case StateChangedAlert stateChanged:
                    OnStateChanged(stateChanged);
                    break;
                case StateUpdateAlert stateUpdate:
                    OnStateUpdate(stateUpdate);
                    break;
                case StatsAlert stats:
                    OnStats(stats);
                    break;
                case TorrentAddedAlert torrentAdded:
                    OnTorrentAdded(torrentAdded);
                    break;
                case TorrentFinishedAlert torrentFinished:
                    OnTorrentFinished(torrentFinished);
                    break;
                case TorrentPausedAlert torrentPaused:
                    OnTorrentPaused(torrentPaused);
                    break;
                case TorrentRemovedAlert torentRemoved:
                    OnTorrentRemoved(torentRemoved);
                    break;
                case TorrentResumedAlert torrentResumed:
                    OnTorrentResumed(torrentResumed);
                    break;
                case TorrentCheckedAlert torrentChecked:
                    OnTorrentChecked(torrentChecked);
                    break;
                default:
                    OnUknownAlert(alert);
                    break;
            }
        }

        protected virtual void OnTorrentAddFailed(string url)
        {
            _logger.Log($"Failed adding torrent: {url}");

            TorrentAddFailed?.Invoke(this, url);
        }

        protected virtual void OnUknownAlert(Alert alert)
        {
            _logger.Log(alert.Message);
            UnknownAlert?.Invoke(this, alert.Message);
        }

        protected virtual void OnTorrentChecked(TorrentCheckedAlert torrentChecked)
        {
            _logger.Log(torrentChecked.Message);
            TorrentChecked?.Invoke(this, GetTorrentByHandle(torrentChecked.Handle), torrentChecked.Message);
        }

        protected virtual void OnTorrentResumed(TorrentResumedAlert torrentResumed)
        {
            _logger.Log(torrentResumed.Message);
            TorrentResumed?.Invoke(this, GetTorrentByHandle(torrentResumed.Handle), torrentResumed.Message);
        }

        protected virtual void OnTorrentRemoved(TorrentRemovedAlert torentRemoved)
        {
            _logger.Log(torentRemoved.Message);
            TorrentRemoved?.Invoke(this, GetTorrentByHandle(torentRemoved.Handle), torentRemoved.Message);
        }

        protected virtual void OnTorrentPaused(TorrentPausedAlert torrentPaused)
        {
            _logger.Log(torrentPaused.Message);
            TorrentPaused?.Invoke(this, GetTorrentByHandle(torrentPaused.Handle), torrentPaused.Message);
        }

        protected virtual void OnTorrentFinished(TorrentFinishedAlert torrentFinished)
        {
            _logger.Log(torrentFinished.Message);
            TorrentFinished?.Invoke(this, GetTorrentByHandle(torrentFinished.Handle), torrentFinished.Message);
        }

        protected virtual void OnTorrentAdded(TorrentAddedAlert torrentAdded)
        {
            _logger.Log(torrentAdded.Message);
            TorrentAdded?.Invoke(this, GetTorrentByHandle(torrentAdded.Handle), torrentAdded.Message);
        }

        protected virtual void OnStats(StatsAlert stats)
        {
            _logger.Log(stats.Message);
        }

        protected virtual void OnStateUpdate(StateUpdateAlert stateUpdate)
        {
            _logger.Log(stateUpdate.Message);
        }

        protected virtual void OnStateChanged(StateChangedAlert stateChanged)
        {
            _logger.Log(stateChanged.Message);
            TorrentStateChanged?.Invoke(this, GetTorrentByHandle(stateChanged.Handle), (TorrentState)stateChanged.State, (TorrentState)stateChanged.PreviousState, stateChanged.Message);
        }

        protected virtual void OnSaveResumeData(SaveResumeDataAlert saveResumeData)
        {
            _logger.Log(saveResumeData.Message);
            ResumeDataSaved?.Invoke(this, saveResumeData.ResumeData, saveResumeData.Message);
        }

        protected virtual void OnMetadataReceived(MetadataReceivedAlert metadataReceived)
        {
            _logger.Log(metadataReceived.Message);
            TorrentMetadataReceived?.Invoke(this, GetTorrentByHandle(metadataReceived.Handle), metadataReceived.Message);
        }

        protected virtual void OnFileRenamed(FileRenamedAlert fileRenamed)
        {
            _logger.Log(fileRenamed.Message);
            var torrent = GetTorrentByHandle(fileRenamed.Handle);
            TorrentFileRenamed?.Invoke(this, torrent, torrent.Files[fileRenamed.Index], fileRenamed.Name, fileRenamed.Message);
        }

        protected virtual void OnFileCompleted(FileCompletedAlert fileCompleted)
        {
            _logger.Log(fileCompleted.Message);
            var torrent = GetTorrentByHandle(fileCompleted.Handle);
            TorrentFileCompleted?.Invoke(this, torrent, torrent.Files[fileCompleted.Index], fileCompleted.Message);
        }
    }
}
