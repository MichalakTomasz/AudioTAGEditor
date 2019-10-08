using System;
using System.Collections.Generic;
using AudioTAGEditor.Models;

namespace AudioTAGEditor.Services
{
    public interface IHistoryService
    {
        int Count { get; }
        int Position { get; }

        void Clear();
        HistoryObject Peek { get; }
        HistoryObject Pop();
        void Push(Audiofile audioFile, ChangeActionType changeActionType, string path);
        void Push(IEnumerable<Audiofile> audioFiles, ChangeActionType changeActionType, string path);
        HistoryObject Prev(IEnumerable<Audiofile> audioFiles);
        HistoryObject Next(IEnumerable<Audiofile> audioFiles);
    }
}