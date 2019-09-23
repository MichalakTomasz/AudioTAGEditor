using System;
using System.Collections.Generic;
using AudioTAGEditor.Models;

namespace AudioTAGEditor.Services
{
    public interface IHistoryService
    {
        int Count { get; }
        IEnumerable<HistoryObject> History { get; }

        void Clear();
        HistoryObject Peek();
        HistoryObject Pop();
        void PushChanges(Guid id, IEnumerable<AudioFile> audioFiles);
        void PushChange(Guid id, AudioFile audioFile);
        Guid PushOldValue(AudioFile audioFile, ChangeActionType changeActionType, string path);
        Guid PushOldValues(IEnumerable<AudioFile> audioFiles, ChangeActionType changeActionType, string path);
    }
}