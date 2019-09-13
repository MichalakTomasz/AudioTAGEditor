using AudioTAGEditor.Models;
using EventAggregator;
using Prism.Events;
using Prism.Mvvm;

namespace AudioTAGEditor.ViewModels
{
    public class AudioFileViewModel : BindableBase
    {
        #region Constructor

        public AudioFileViewModel(IEventAggregator eventAggregator)
            => this.eventAggregator = eventAggregator;

        #endregion//Constructor

        #region Properties

        private bool isChecked;
        public bool IsChecked
        {
            get { return isChecked; }
            set
            {
                SetProperty(ref isChecked, value);
                eventAggregator.GetEvent<AudioFileMessageSentEvent>()
                    .Publish(new Commons.AudioFileMessage { IsSelectedFile = IsChecked });
            }
        }

        private bool hasTag;
        public bool HasTag
        {
            get { return hasTag; }
            set { SetProperty(ref hasTag, value); }
        }

        private string filename;
        public string Filename
        {
            get { return filename; }
            set { SetProperty(ref filename, value); }
        }

        private string title;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        private string album;
        public string Album
        {
            get { return album; }
            set { SetProperty(ref album, value); }
        }

        private string artist;
        public string Artist
        {
            get { return artist; }
            set { SetProperty(ref artist, value); }
        }

        private string trackNumber;
        public string TrackNumber
        {
            get { return trackNumber; }
            set { SetProperty(ref trackNumber, value); }
        }

        private string genre;
        public string Genre
        {
            get { return genre; }
            set { SetProperty(ref genre, value); }
        }

        private string comment;
        public string Comment
        {
            get { return comment; }
            set { SetProperty(ref comment, value); }
        }

        private string year;
        public string Year
        {
            get { return year; }
            set { SetProperty(ref year, value); }
        }

        private TagType tagType;
        public TagType TagType
        {
            get { return tagType; }
            set { SetProperty(ref tagType, value); }
        }

        private TagVersion tagVersion;
        

        public TagVersion TagVersion
        {
            get { return tagVersion; }
            set { SetProperty(ref tagVersion, value); }
        }

        #endregion//Properties

        #region Methods

        

        #endregion//Methods

        #region Fields

        private readonly IEventAggregator eventAggregator;

        #endregion//Fields
    }
}
