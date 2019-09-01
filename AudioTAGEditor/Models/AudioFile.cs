
namespace AudioTAGEditor.Models
{
    public class AudioFile
    {
        public bool IsSelected { get; set; }
        public bool HasTag { get; set; }
        public string FileName { get; set; }
        public string Title { get; set; }
        public string Album { get; set; }
        public string Artist { get; set; }
        public int TrackNumber { get; set; }
        public string Genre { get; set; }
        public string  Comment { get; set; }
        public TAGType TAGType { get; set; }
    }
}
