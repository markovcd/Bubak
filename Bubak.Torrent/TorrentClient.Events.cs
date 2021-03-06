﻿using Ragnar;
using System;

namespace Bubak.Client
{
    public delegate void TorrentResumeDataHandler(ITorrentClient sender, Torrent torent, byte[] resumeData);
    public delegate void TorrentHandler(ITorrentClient sender, Torrent torent);
    public delegate void TorrentFileHandler(ITorrentClient sender, Torrent torent, File file);
    public delegate void TorrentFileNameHandler(ITorrentClient sender, Torrent torent, File file, string fileName);
    public delegate void TorrentStateChangeHandler(ITorrentClient sender, Torrent torent, TorrentState currentState, TorrentState previousState);

    public partial class TorrentClient
    {
        public event TorrentHandler TorrentAdded;
        public event TorrentHandler TorrentUpdated;
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

            TorrentChecked?.Invoke(this, torrent);
        }

        protected virtual void OnTorrentResumed(TorrentResumedAlert torrentResumed)
        {
            _logger.Log(torrentResumed.Message);

            var torrent = EnsureTorrentExist(torrentResumed.Handle);

            TorrentResumed?.Invoke(this, torrent);
        }

        protected virtual void OnTorrentRemoved(TorrentRemovedAlert torentRemoved)
        {
            try
            {
                _logger.Log(torentRemoved.Message);

                if (!RemoveTorrent(_torrentToRemove)) throw new InvalidOperationException();
            }
            finally
            {
                _torrentToRemove = null;
            }
        }

        protected virtual void OnTorrentPaused(TorrentPausedAlert torrentPaused)
        {
            _logger.Log(torrentPaused.Message);

            var torrent = EnsureTorrentExist(torrentPaused.Handle);

            TorrentPaused?.Invoke(this, torrent);
        }

        protected virtual void OnTorrentFinished(TorrentFinishedAlert torrentFinished)
        {
            _logger.Log(torrentFinished.Message);

            var torrent = EnsureTorrentExist(torrentFinished.Handle);

            TorrentFinished?.Invoke(this, torrent);
        }

        protected virtual void OnTorrentAdded(TorrentAddedAlert torrentAdded)
        {
            _logger.Log(torrentAdded.Message);

            //var torrent = EnsureTorrentExist(torrentAdded.Handle);
        }

        protected virtual void OnTorrentStatsReceived(StatsAlert stats)
        {
            _logger.Log(stats.Message);

            var torrent = EnsureTorrentExist(stats.Handle);        

            TorrentStatsReceived?.Invoke(this, torrent); // TODO: implement more details
        }

        protected virtual void OnStateUpdate(StateUpdateAlert stateUpdate)
        {
            _logger.Log(stateUpdate.Message); 

            foreach (var status in stateUpdate.Statuses)
            {
                var infoHash = status.InfoHash.ToHex();
                TryGetTorrent(infoHash, out var torrent, out var handle);
                SetTorrent(torrent.Update(status, handle.GetFilePriorities(), handle.GetFileProgresses()));                
            }
        }

        protected virtual void OnTorrentStateChanged(StateChangedAlert stateChanged)
        {
            _logger.Log(stateChanged.Message);

            var torrent = EnsureTorrentExist(stateChanged.Handle);      

            TorrentStateChanged?.Invoke(this, torrent, (TorrentState)stateChanged.State, (TorrentState)stateChanged.PreviousState);
        }

        protected virtual void OnTorrentSaveResumeData(SaveResumeDataAlert saveResumeData)
        {
            _logger.Log(saveResumeData.Message);

            var torrent = EnsureTorrentExist(saveResumeData.Handle)
                .SetResumeData(saveResumeData.ResumeData);

            SetTorrent(torrent);

            TorrentResumeDataSaved?.Invoke(this, torrent, saveResumeData.ResumeData);
        }

        protected virtual void OnTorrentMetadataReceived(MetadataReceivedAlert metadataReceived)
        {
            _logger.Log(metadataReceived.Message);
           
            var torrent = EnsureTorrentExist(metadataReceived.Handle);

            TorrentMetadataReceived?.Invoke(this, torrent);
        }

        protected virtual void OnTorrentFileRenamed(FileRenamedAlert fileRenamed)
        {
            _logger.Log(fileRenamed.Message);

            var torrent = EnsureTorrentExist(fileRenamed.Handle)
                .SetFileName(fileRenamed.Name, fileRenamed.Index);

            SetTorrent(torrent);

            TorrentFileRenamed?.Invoke(this, torrent, torrent.Files[fileRenamed.Index], fileRenamed.Name);
        }

        protected virtual void OnTorrentFileCompleted(FileCompletedAlert fileCompleted)
        {
            _logger.Log(fileCompleted.Message);

            var torrent = EnsureTorrentExist(fileCompleted.Handle)
                .SetFileCompleted(fileCompleted.Index);

            SetTorrent(torrent);

            TorrentFileCompleted?.Invoke(this, torrent, torrent.Files[fileCompleted.Index]);
        }
    }
}
