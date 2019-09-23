using AudioTAGEditor.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AudioTAGEditor.Services
{
    public class HistoryService : IHistoryService
    {
        private Stack<HistoryObject> history = new Stack<HistoryObject>();

        public Guid PushOldValues(
            IEnumerable<AudioFile> audioFiles,
            ChangeActionType changeActionType,
            string path)
        {
            var oldAudioFileChanges = new List<AudioFileChange>();
            audioFiles.ToList().ForEach(a =>
            {
                var audioFileChange = new AudioFileChange { Old = a };
                oldAudioFileChanges.Add(audioFileChange);
            });

            var id = Guid.NewGuid();
            var historyObject = new HistoryObject
            {
                ID = id,
                Path = path,
                ChangeActionType = changeActionType,
                AudioFileChanges = oldAudioFileChanges
            };

            history.Push(historyObject);
            return id;
        }

        public Guid PushOldValue(
            AudioFile audioFile,
            ChangeActionType changeActionType,
            string path)
        {
            var audioFileCollection = new List<AudioFile> { audioFile };
            return PushOldValues(audioFileCollection, changeActionType, path);
        }

        public void PushChanges(
            Guid id,
            IEnumerable<AudioFile> audioFiles)
        {
            var lastHistoty = history.Peek();
            if (lastHistoty.ID == id)
            {
                var changes = lastHistoty.AudioFileChanges.ToList();
                var listAudioFiles = audioFiles.ToList();
                for (int i = 0; i < changes.Count; i++)
                    changes[i].New = listAudioFiles[i];
            }
        }

        public void PushChange(
            Guid id,
            AudioFile audioFile)
        {
            var audioFileCollection = new List<AudioFile> { audioFile };
            PushChanges(id, audioFileCollection);
        }

        public HistoryObject Pop()
            => history.Pop();

        public HistoryObject Peek()
            => history.Peek();

        public void Clear()
            => history.Clear();

        public int Count => history.Count;
        public IEnumerable<HistoryObject> History
            => history;
    }
}
