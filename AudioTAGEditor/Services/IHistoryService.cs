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
        void Push(Audiofile audioFile, ChangeActionType changeActionType, string path);
        void Push(IEnumerable<Audiofile> audioFiles, ChangeActionType changeActionType, string path);
        void Push(IEnumerable<Audiofile> audioFiles, ChangeActionType changeActionType, string path, string newFilename);
        void Push(Audiofile audioFile, ChangeActionType changeActionType, string path, string newFilename);
        HistoryObject Undo(IEnumerable<Audiofile> audioFiles);
        HistoryObject Redo(IEnumerable<Audiofile> audioFiles);
        string TryGetCurrentFilename(Guid id);
    }
}