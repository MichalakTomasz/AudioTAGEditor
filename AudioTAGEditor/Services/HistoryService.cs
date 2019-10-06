using AudioTAGEditor.Models;
using System.Collections.Generic;
using System.Linq;

namespace AudioTAGEditor.Services
{
    public class HistoryService : IHistoryService
    {
        private readonly Stack<HistoryObject> history = new Stack<HistoryObject>();
        private readonly Stack<HistoryObject> reserveHistory = new Stack<HistoryObject>();

        public void Push(
            IEnumerable<AudioFile> audioFiles,
            ChangeActionType changeActionType,
            string path)
        {
            var historyObject = new HistoryObject
            {
                Path = path,
                ChangeActionType = changeActionType,
                AudioFiles = audioFiles
            };

            history.Push(historyObject);
            reserveHistory.Clear();
            Position = history.Count;
            Count = GetHistoryCount();
        }

        public void Push(
            AudioFile audioFile,
            ChangeActionType changeActionType,
            string path)
        {
            var audioFileCollection = new List<AudioFile> { audioFile };
            Push(audioFileCollection, changeActionType, path);
        }

        public HistoryObject Pop()
        {
            var result = history.Pop();
            Position = history.Count;
            Count = GetHistoryCount();

            return result;
        }
        
        public HistoryObject Peek
            => history.Peek();

        public HistoryObject Prev(IEnumerable<AudioFile> audioFiles)
        {
            if (history.Count > 0)
            {
                var lastHistoryObject = history.Pop();
                var historyAudioFiles = lastHistoryObject.AudioFiles;
                var audioFilesToHistory = new List<AudioFile>();

                historyAudioFiles
                    .ToList()
                    .ForEach(h => 
                    {
                        var temp = audioFiles.FirstOrDefault(a => a.ID == h.ID);
                        if (temp != null) audioFilesToHistory.Add(temp);
                    });

                var historyObject = new HistoryObject
                {
                    AudioFiles = audioFilesToHistory,
                    Path = lastHistoryObject.Path,
                    ChangeActionType = lastHistoryObject.ChangeActionType
                };

                var resultHistoryObject = new HistoryObject
                {
                    AudioFiles = historyAudioFiles,
                    Path = lastHistoryObject.Path,
                    ChangeActionType = lastHistoryObject.ChangeActionType
                };

                reserveHistory.Push(historyObject);
                Position = history.Count;
                Count = GetHistoryCount();

                return resultHistoryObject;
            }
            return default;
        }

        public HistoryObject Next(IEnumerable<AudioFile> audioFiles)
        {
            if (reserveHistory.Count > 0)
            {
                var lastHistoryObject = reserveHistory.Pop();
                var historyAudioFiles = lastHistoryObject.AudioFiles;
                var audioFilesToHistory = new List<AudioFile>();

                historyAudioFiles
                    .ToList()
                    .ForEach(h =>
                    {
                        var temp = audioFiles.FirstOrDefault(a => a.ID == h.ID);
                        if (temp != null) audioFilesToHistory.Add(temp);
                    });

                var historyObject = new HistoryObject
                {
                    Path = lastHistoryObject.Path,
                    ChangeActionType = lastHistoryObject.ChangeActionType,
                    AudioFiles = audioFilesToHistory
                };

                var resultHistoryObject = new HistoryObject
                {
                    Path = lastHistoryObject.Path,
                    ChangeActionType = lastHistoryObject.ChangeActionType,
                    AudioFiles = historyAudioFiles
                };

                history.Push(historyObject);
                Position = history.Count;
                Count = GetHistoryCount();
                return resultHistoryObject;
            }
            return default;
        }

        public void Clear()
        {
            history.Clear();
            Position = history.Count;
            Count = GetHistoryCount();
        }

        private int GetHistoryCount()
            => history.Count + reserveHistory.Count;

        public int Count { get; private set; }
        public int Position { get; private set; }
    }
}
