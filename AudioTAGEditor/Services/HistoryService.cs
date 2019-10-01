using AudioTAGEditor.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AudioTAGEditor.Services
{
    public class HistoryService : IHistoryService
    {
        private Stack<HistoryObject> history = new Stack<HistoryObject>();
        private Stack<HistoryObject> reserveHistory = new Stack<HistoryObject>();

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
            => history.Pop();

        public HistoryObject Peek()
            => history.Peek();

        public HistoryObject Prev()
        {
            if (history.Count > 0)
            {
                var tempHistoryObject = history.Pop();
                reserveHistory.Push(tempHistoryObject);
                return tempHistoryObject;
            }
            return default(HistoryObject);
        }

        public HistoryObject Next()
        {
            if (reserveHistory.Count > 0)
            {
                var tempHistoryObject = reserveHistory.Pop();
                history.Push(tempHistoryObject);
                return tempHistoryObject;
            }
            return default(HistoryObject);
        }

        public void Clear()
            => history.Clear();

        public int Count => history.Count;

        public IEnumerable<HistoryObject> History
            => history;
    }
}
