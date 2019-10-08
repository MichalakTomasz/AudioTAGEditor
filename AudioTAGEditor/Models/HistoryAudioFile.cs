using System;

namespace AudioTAGEditor.Models
{
    public class HistoryAudiofile : Audiofile
    {
        public HistoryAudiofile(Guid id) : base(id) { }

        public string CurrentFilename { get; set; }
    }
}
