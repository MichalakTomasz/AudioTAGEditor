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
        void Push(AudioFile audioFile, ChangeActionType changeActionType, string path);
        void Push(IEnumerable<AudioFile> audioFiles, ChangeActionType changeActionType, string path);
        HistoryObject Prev();
        HistoryObject Next();
    }
}