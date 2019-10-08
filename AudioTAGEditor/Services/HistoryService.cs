using AudioTAGEditor.Models;
using System.Collections.Generic;
using System.Linq;

namespace AudioTAGEditor.Services
{
    public class HistoryService : IHistoryService
    {
        #region Constructor

        public HistoryService(IAudiofileConverter audioFileConverter)
            => this.audioFileConverter = audioFileConverter;

        #endregion //Constructor

        #region Methods

        public void Push(
            IEnumerable<Audiofile> audioFiles,
            ChangeActionType changeActionType,
            string path)
        {
            var historyObject = new HistoryObject
            {
                Path = path,
                ChangeActionType = changeActionType,
                HistoryAudiofiles = audioFileConverter
                .AudioFilesToHistoryAudioFiles(audioFiles)
            };

            history.Push(historyObject);
            reserveHistory.Clear();
            Position = history.Count;
            Count = GetHistoryCount();
        }

        public void Push(
            Audiofile audioFile,
            ChangeActionType changeActionType,
            string path)
        {
            var audioFileCollection = new List<Audiofile> { audioFile };
            Push(audioFileCollection, changeActionType, path);
        }

        public HistoryObject Pop()
        {
            var result = history.Pop();
            Position = history.Count;
            Count = GetHistoryCount();

            return result;
        }

        public HistoryObject Prev(IEnumerable<Audiofile> audioFiles)
        {
            if (history.Count > 0)
            {
                var lastHistoryObject = history.Pop();
                var historyAudioFiles = lastHistoryObject.HistoryAudiofiles;
                var audioFilesToHistory = new List<HistoryAudiofile>();

                historyAudioFiles
                    .ToList()
                    .ForEach(h =>
                    {
                        var tempAudioFile = audioFiles.FirstOrDefault(a => a.ID == h.ID);
                        
                        if (tempAudioFile != null)
                        {
                            var tempHistoryAudioFile = audioFileConverter
                            .AudioFileToHistoryAudioFile(tempAudioFile);
                            audioFilesToHistory.Add(tempHistoryAudioFile);
                        }
                    });

                var historyObject = new HistoryObject
                {
                    HistoryAudiofiles = audioFilesToHistory,
                    Path = lastHistoryObject.Path,
                    ChangeActionType = lastHistoryObject.ChangeActionType
                };

                var resultHistoryObject = new HistoryObject
                {
                    HistoryAudiofiles = historyAudioFiles,
                    Path = lastHistoryObject.Path,
                    ChangeActionType = lastHistoryObject.ChangeActionType
                };

                reserveHistory.Push(historyObject);
                Position = history.Count;
                Count = GetHistoryCount();

                return resultHistoryObject;
            }
            return default;
        }

        public HistoryObject Next(IEnumerable<Audiofile> audioFiles)
        {
            if (reserveHistory.Count > 0)
            {
                var lastHistoryObject = reserveHistory.Pop();
                var historyAudioFiles = lastHistoryObject.HistoryAudiofiles;
                var audioFilesToHistory = new List<HistoryAudiofile>();

                historyAudioFiles
                    .ToList()
                    .ForEach(h =>
                    {
                        var tempAudiofile = audioFiles.FirstOrDefault(a => a.ID == h.ID);
                        
                        if (tempAudiofile != null)
                        {
                            var tempHistoryAudiofile = audioFileConverter
                            .AudioFileToHistoryAudioFile(tempAudiofile);
                            audioFilesToHistory.Add(tempHistoryAudiofile);
                        }
                    });

                var historyObject = new HistoryObject
                {
                    Path = lastHistoryObject.Path,
                    ChangeActionType = lastHistoryObject.ChangeActionType,
                    HistoryAudiofiles = audioFilesToHistory
                };

                var resultHistoryObject = new HistoryObject
                {
                    Path = lastHistoryObject.Path,
                    ChangeActionType = lastHistoryObject.ChangeActionType,
                    HistoryAudiofiles = historyAudioFiles
                };

                history.Push(historyObject);
                Position = history.Count;
                Count = GetHistoryCount();

                return resultHistoryObject;
            }
            return default;
        }

        public void Clear()
        {
            history.Clear();
            Position = history.Count;
            Count = GetHistoryCount();
        }

        private int GetHistoryCount()
            => history.Count + reserveHistory.Count;

        #endregion // Methods

        #region Properties

        public HistoryObject Peek
            => history.Peek();

        public int Count { get; private set; }
        public int Position { get; private set; }

        #endregion // Properties

        #region Fields

        private readonly Stack<HistoryObject> history = new Stack<HistoryObject>();
        private readonly Stack<HistoryObject> reserveHistory = new Stack<HistoryObject>();
        private readonly IAudiofileConverter audioFileConverter;

        #endregion // Fields
    }
}
