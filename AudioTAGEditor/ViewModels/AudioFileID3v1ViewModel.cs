using Prism.Events;
using System;
using System.ComponentModel.DataAnnotations;

namespace AudioTAGEditor.ViewModels
{
    public class AudiofileID3v1ViewModel :
        AudiofileViewModel
    {
        public AudiofileID3v1ViewModel(Guid id, IEventAggregator eventAggregator) : 
            base(id, eventAggregator) { }
        
        [MaxLength(30, ErrorMessage = max30characters)]
        public override string Title
        {
            get => base.Title;
            set => base.Title = value;
        }

        [MaxLength(30, ErrorMessage = max30characters)]
        public override string Album
        {
            get => base.Album;
            set => base.Album = value;
        }

        [MaxLength(30, ErrorMessage = max30characters)]
        public override string Artist
        {
            get => base.Artist;
            set => base.Artist = value;
        }

        [Range(0, 99)]
        public override string TrackNumber
        {
            get => base.TrackNumber;
            set => base.TrackNumber = value;
        }

        [MaxLength(4)]
        [Range(minimum:0, maximum:9999)]
        public override string Year
        {
            get => base.Year;
            set => base.Year = value;
        }

        private const string max30characters = "Max characters 30";
    }
}
