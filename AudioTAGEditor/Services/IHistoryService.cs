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
        void Push(AudioFile audioFile, ChangeActionType changeActionType, string path);
        void Push(IEnumerable<AudioFile> audioFiles, ChangeActionType changeActionType, string path);
        HistoryObject Prev(IEnumerable<AudioFile> audioFiles, ChangeActionType changeActionType, string path);
        HistoryObject Prev(AudioFile audioFile, ChangeActionType changeActionType, string path);
        HistoryObject Next(IEnumerable<AudioFile> audioFiles, ChangeActionType changeActionType, string path);
        HistoryObject Next(AudioFile audioFile, ChangeActionType changeActionType, string path);
    }
}