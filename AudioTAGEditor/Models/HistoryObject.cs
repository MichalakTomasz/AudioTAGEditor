using System;
using System.Collections.Generic;

namespace AudioTAGEditor.Models
{
    public class HistoryObject
    {
        public Guid ID { get; set; }
        public string Path { get; set; }
        public ChangeActionType ChangeActionType { get; set; }
        public IEnumerable<AudioFileChange> AudioFileChanges { get; set; }
    }
}
