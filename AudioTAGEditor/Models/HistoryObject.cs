using System;
using System.Collections.Generic;

namespace AudioTAGEditor.Models
{
    public class HistoryObject
    {
        public string Path { get; set; }
        public ChangeActionType ChangeActionType { get; set; }
        public IEnumerable<AudioFile> AudioFiles { get; set; }
    }
}
