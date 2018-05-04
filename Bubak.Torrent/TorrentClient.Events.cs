using Ragnar;
using System;

namespace Bubak.Client
{
    public delegate void TorrentResumeDataHandler(TorrentClient sender, ITorrent torent, byte[] resumeData);
    public delegate void TorrentHandler(TorrentClient sender, ITorrent torent);
    public delegate void TorrentFileHandler(TorrentClient sender, ITorrent torent, IFile file);
    public delegate void TorrentFileNameHandler(TorrentClient sender, ITorrent torent, IFile file, string fileName);
    public delegate void TorrentStateChangeHandler(TorrentClient sender, ITorrent torent, TorrentState currentState, TorrentState previousState);

    public partial class TorrentClient
    {
        public event TorrentHandler TorrentAdded;
        public event TorrentHandler TorrentChecked;
        public event TorrentHandler TorrentResumed;
        public event TorrentHandler TorrentRemoved;
        public event TorrentHandler TorrentPaused;
        public event TorrentHandler TorrentFinished;
        public event TorrentHandler TorrentMetadataReceived;
        public event TorrentHandler TorrentStatsReceived;
        public event TorrentFileHandler TorrentFileCompleted;
        public event TorrentFileNameHandler TorrentFileRenamed;
        public event TorrentStateChangeHandler TorrentStateChanged;
        public event TorrentResumeDataHandler TorrentResumeDataSaved;

        protected void RaiseEvent(Alert alert)
        {
            switch (alert)
            {
                case FileCompletedAlert fileCompleted:
                    OnTorrentFileCompleted(fileCompleted);
                    break;
                case FileRenamedAlert fileRenamed:
                    OnTorrentFileRenamed(fileRenamed);
                    break;
                case MetadataReceivedAlert metadataReceived:
                    OnTorrentMetadataReceived(metadataReceived);
                    break;
                case SaveResumeDataAlert saveResumeData:
                    OnTorrentSaveResumeData(saveResumeData);
                    break;
                case StateChangedAlert stateChanged:
                    OnTorrentStateChanged(stateChanged);
                    break;
                case StateUpdateAlert stateUpdate:
                    OnStateUpdate(stateUpdate);
                    break;
                case StatsAlert stats:
                    OnTorrentStatsReceived(stats);
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

        protected virtual void OnUknownAlert(Alert alert)
        {
            _logger.Log(alert.Message);
        }

        protected virtual void OnTorrentChecked(TorrentCheckedAlert torrentChecked)
        {
            _logger.Log(torrentChecked.Message);

            var torrent = EnsureTorrentExist(torrentChecked.Handle);
            torrent.Update();

            TorrentChecked?.Invoke(this, torrent);
        }

        protected virtual void OnTorrentResumed(TorrentResumedAlert torrentResumed)
        {
            _logger.Log(torrentResumed.Message);

            var torrent = EnsureTorrentExist(torrentResumed.Handle);
            torrent.Update();

            TorrentResumed?.Invoke(this, torrent);
        }

        protected virtual void OnTorrentRemoved(TorrentRemovedAlert torentRemoved)
        {
            _logger.Log(torentRemoved.Message);

            TorrentRemoved?.Invoke(this, _torrentToRemove);

            RemoveTorrentFromList(_torrentToRemove);
            (_torrentToRemove as IDisposable)?.Dispose();
            _torrentToRemove = null;
        }

        protected virtual void OnTorrentPaused(TorrentPausedAlert torrentPaused)
        {
            _logger.Log(torrentPaused.Message);

            var torrent = EnsureTorrentExist(torrentPaused.Handle);
            torrent.Update();

            TorrentPaused?.Invoke(this, torrent);
        }

        protected virtual void OnTorrentFinished(TorrentFinishedAlert torrentFinished)
        {
            _logger.Log(torrentFinished.Message);

            var torrent = EnsureTorrentExist(torrentFinished.Handle);
            torrent.Update();   

            TorrentFinished?.Invoke(this, torrent);
        }

        protected virtual void OnTorrentAdded(TorrentAddedAlert torrentAdded)
        {
            _logger.Log(torrentAdded.Message);

            var torrent = EnsureTorrentExist(torrentAdded.Handle);
            torrent.Update();
        }

        protected virtual void OnTorrentStatsReceived(StatsAlert stats)
        {
            _logger.Log(stats.Message);

            var torrent = EnsureTorrentExist(stats.Handle);
            torrent.Update();          

            TorrentStatsReceived?.Invoke(this, torrent); // TODO: implement more details
        }

        protected virtual void OnStateUpdate(StateUpdateAlert stateUpdate)
        {
            _logger.Log(stateUpdate.Message); // TODO: check if received for all torrents
        }

        protected virtual void OnTorrentStateChanged(StateChangedAlert stateChanged)
        {
            _logger.Log(stateChanged.Message);

            var torrent = EnsureTorrentExist(stateChanged.Handle);
            
            torrent.Update();           

            TorrentStateChanged?.Invoke(this, torrent, (TorrentState)stateChanged.State, (TorrentState)stateChanged.PreviousState);
        }

        protected virtual void OnTorrentSaveResumeData(SaveResumeDataAlert saveResumeData)
        {
            _logger.Log(saveResumeData.Message);

            var torrent = EnsureTorrentExist(saveResumeData.Handle);
            torrent.ResumeData = saveResumeData.ResumeData;
            torrent.Update();          

            TorrentResumeDataSaved?.Invoke(this, torrent, saveResumeData.ResumeData);
        }

        protected virtual void OnTorrentMetadataReceived(MetadataReceivedAlert metadataReceived)
        {
            _logger.Log(metadataReceived.Message);
           
            var torrent = EnsureTorrentExist(metadataReceived.Handle);
            torrent.Update();

            TorrentMetadataReceived?.Invoke(this, torrent);
        }

        protected virtual void OnTorrentFileRenamed(FileRenamedAlert fileRenamed)
        {
            _logger.Log(fileRenamed.Message);

            var torrent = EnsureTorrentExist(fileRenamed.Handle);
            torrent.Update();

            TorrentFileRenamed?.Invoke(this, torrent, torrent.Files[fileRenamed.Index], fileRenamed.Name);
        }

        protected virtual void OnTorrentFileCompleted(FileCompletedAlert fileCompleted)
        {
            _logger.Log(fileCompleted.Message);

            var torrent = EnsureTorrentExist(fileCompleted.Handle);
            torrent.Files[fileCompleted.Index].IsFinished = true;
            torrent.Update();           

            TorrentFileCompleted?.Invoke(this, torrent, torrent.Files[fileCompleted.Index]);
        }
    }
}
