using System;
using System.Collections.Generic;
using AudioTAGEditor.Models;

namespace AudioTAGEditor.Services
{
    public interface IHistoryService
    {
        int Count { get; }
        int Position { get; }

        void ResetPosition();
        void Clear();
        void Add(Audiofile audioFile, ChangeActionType changeActionType, string path);
        void Add(IEnumerable<Audiofile> audioFiles, ChangeActionType changeActionType, string path);
        void Add(IEnumerable<Audiofile> audioFiles, ChangeActionType changeActionType, string path, IEnumerable<string> newFilenames);
        void Add(Audiofile audioFile, ChangeActionType changeActionType, string path, string newFilename);
        HistoryObject Undo(IEnumerable<Audiofile> audioFiles);
        HistoryObject Redo(IEnumerable<Audiofile> audioFiles);
        string GetCurrentFilename(Guid id);
        bool HasChangesSinceCurrentHistoryPosition(Guid id);
        bool HasTagChangesSienceCurrentPosition(Audiofile audiofile);
    }
}