using AudioTAGEditor.Models;
using System.Collections.Generic;

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

        public HistoryObject Prev(
            IEnumerable<AudioFile> audioFiles,
            ChangeActionType changeActionType,
            string path)
        {
            if (history.Count > 0)
            {
                var result = history.Pop();
                var historyObject = new HistoryObject
                {
                    Path = path,
                    ChangeActionType = changeActionType,
                    AudioFiles = audioFiles
                };
                reserveHistory.Push(historyObject);
                Position = history.Count;
                Count = GetHistoryCount();
                return result;
            }
            return default;
        }

        public HistoryObject Prev(
            AudioFile audioFile,
            ChangeActionType changeActionType,
            string path)
        {
            var audioFiles = new List<AudioFile>() { audioFile };
            return Prev(audioFiles, changeActionType, path);
        }

        public HistoryObject Next(
            IEnumerable<AudioFile> audioFiles,
            ChangeActionType changeActionType,
            string path)
        {
            if (reserveHistory.Count > 0)
            {
                var reusult = reserveHistory.Pop();
                var historyObject = new HistoryObject
                {
                    Path = path,
                    ChangeActionType = changeActionType,
                    AudioFiles = audioFiles
                };
                history.Push(historyObject);
                Position = history.Count;
                Count = GetHistoryCount();
                return reusult;
            }
            return default;
        }

        public HistoryObject Next(
            AudioFile audioFile,
            ChangeActionType changeActionType,
            string path)
        {
            var audioFiles = new List<AudioFile> { audioFile };
            return Next(audioFiles, changeActionType, path);
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
