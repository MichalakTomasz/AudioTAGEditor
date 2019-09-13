using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioTAGEditor.Models
{
    public class AudioFile
    {
        public bool HasTag { get; set; }
        public string Filename { get; set; }
        public string Title { get; set; }
        public string Album { get; set; }
        public string Artist { get; set; }
        public string TrackNumber { get; set; }
        public string Genre { get; set; }
        public string Comment { get; set; }
        public string Year { get; set; }
        public TagType TagType { get; set; }
        public TagVersion TagVersion { get; set; }
    }
}
