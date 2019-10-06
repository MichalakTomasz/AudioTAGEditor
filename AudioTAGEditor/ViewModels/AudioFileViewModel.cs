using AudioTAGEditor.Models;
using EventAggregator;
using LibValidation;
using Prism.Events;
using System;
using System.ComponentModel.DataAnnotations;

namespace AudioTAGEditor.ViewModels
{
    public class AudioFileViewModel : BindableBaseWithValidation
    {
        #region Constructor

        public AudioFileViewModel(Guid id, IEventAggregator eventAggregator)
        {
            ID = id;
            this.eventAggregator = eventAggregator;
        }

        #endregion // Constructor

        #region Properties

        public virtual Guid ID { get; }

        private bool isChecked;
        public virtual bool IsChecked
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
        public virtual bool HasTag
        {
            get { return hasTag; }
            set { SetProperty(ref hasTag, value); }
        }

        private string filename;
        [Required]
        [RegularExpression("[^<>:\"\\|/?*]+[.]{1}[^<>:\"\\|/?*]+")]
        public virtual string Filename
        {
            get { return filename; }
            set { SetProperty(ref filename, value); }
        }

        private string title;
        public virtual string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        private string album;
        public virtual string Album
        {
            get { return album; }
            set { SetProperty(ref album, value); }
        }

        private string artist;
        public virtual string Artist
        {
            get { return artist; }
            set { SetProperty(ref artist, value); }
        }

        private string trackNumber;
        public virtual string TrackNumber
        {
            get { return trackNumber; }
            set { SetProperty(ref trackNumber, value); }
        }

        private string genre;
        public virtual string Genre
        {
            get { return genre; }
            set { SetProperty(ref genre, value); }
        }

        private string comment;
        public virtual string Comment
        {
            get { return comment; }
            set { SetProperty(ref comment, value); }
        }

        private string year;
        public virtual string Year
        {
            get { return year; }
            set { SetProperty(ref year, value); }
        }

        private TagType tagType;
        public virtual TagType TagType
        {
            get { return tagType; }
            set { SetProperty(ref tagType, value); }
        }

        private TagVersion tagVersion;
        public virtual TagVersion TagVersion
        {
            get { return tagVersion; }
            set { SetProperty(ref tagVersion, value); }
        }

        #endregion // Properties

        #region Fields

        private readonly IEventAggregator eventAggregator;

        #endregion // Fields
    }
}
