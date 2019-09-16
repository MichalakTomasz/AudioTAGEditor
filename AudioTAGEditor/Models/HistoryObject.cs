using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioTAGEditor.Models
{
    public class HistoryObject
    {
        public string Path { get; set; }
        public ChangeActionType ChangeActionType { get; set; }
        public IEnumerable<AudioFileChange> AudioFileChanges { get; set; }
    }
}
