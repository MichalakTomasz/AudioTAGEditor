using AudioTAGEditor.Models;
using System;
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
            reserveHistory.Clear();

            var historyObject = new HistoryObject
            {
                Path = path,
                ChangeActionType = changeActionType,
                Audiofiles = audioFiles
            };

            history.Push(historyObject);
            reserveHistory.Clear();
            RefreshStateProperties();
        }

        public void Push(
            IEnumerable<Audiofile> audioFiles,
            ChangeActionType changeActionType,
            string path,
            string newFilename)
        {
            if (changeActionType == ChangeActionType.Filename)
                audioFiles.ToList().ForEach(a => SetCurrentFilename(a.ID, a.Filename));
            else
                throw new Exception("Wrong call Push method parameters.");

            Push(audioFiles, changeActionType,
                path);
        }

        public void Push(
            Audiofile audioFile,
            ChangeActionType changeActionType,
            string path)
        {
            var audioFileCollection = new List<Audiofile> { audioFile };
            Push(audioFileCollection, changeActionType, path);
        }

        public void Push(
           Audiofile audioFile,
           ChangeActionType changeActionType,
           string path,
           string newFilename)
        {
            if (changeActionType == ChangeActionType.Filename)
                SetCurrentFilename(audioFile.ID, audioFile.Filename);
            else
                throw new Exception("Wrong call Push method parameters.");

            Push(audioFile, changeActionType, path);
        }

        public HistoryObject Undo(IEnumerable<Audiofile> audioFiles)
        {
            if (history.Count > 0)
            {
                currentHistoryObject = history.Pop();
                var historyAudioFiles = currentHistoryObject.Audiofiles;
                var audioFilesToHistory = new List<Audiofile>();

                historyAudioFiles
                    .ToList()
                    .ForEach(h =>
                    {
                        var tempAudioFile = audioFiles.FirstOrDefault(a => a.ID == h.ID);
                        
                        if (tempAudioFile != null)
                            audioFilesToHistory.Add(tempAudioFile);
                    });

                var historyObject = new HistoryObject
                {
                    Audiofiles = audioFilesToHistory,
                    Path = currentHistoryObject.Path,
                    ChangeActionType = currentHistoryObject.ChangeActionType
                };

                var resultHistoryObject = new HistoryObject
                {
                    Audiofiles = historyAudioFiles,
                    Path = currentHistoryObject.Path,
                    ChangeActionType = currentHistoryObject.ChangeActionType
                };

                reserveHistory.Push(historyObject);
                RefreshStateProperties();

                return resultHistoryObject;
            }
            return default;
        }

        public HistoryObject Redo(IEnumerable<Audiofile> audioFiles)
        {
            if (reserveHistory.Count > 0)
            {
                currentHistoryObject = reserveHistory.Pop();
                var historyAudioFiles = currentHistoryObject.Audiofiles;
                var audioFilesToHistory = new List<Audiofile>();

                historyAudioFiles
                    .ToList()
                    .ForEach(h =>
                    {
                        var tempAudiofile = audioFiles.FirstOrDefault(a => a.ID == h.ID);
                        
                        if (tempAudiofile != null)
                            audioFilesToHistory.Add(tempAudiofile);
                    });

                var historyObject = new HistoryObject
                {
                    Path = currentHistoryObject.Path,
                    ChangeActionType = currentHistoryObject.ChangeActionType,
                    Audiofiles = audioFilesToHistory
                };

                var resultHistoryObject = new HistoryObject
                {
                    Path = currentHistoryObject.Path,
                    ChangeActionType = currentHistoryObject.ChangeActionType,
                    Audiofiles = historyAudioFiles
                };

                history.Push(historyObject);
                RefreshStateProperties();

                return resultHistoryObject;
            }
            return default;
        }

        public void Clear()
        {
            history.Clear();
            RefreshStateProperties();
        }

        private int GetHistoryCount()
            => history.Count + reserveHistory.Count;

        private void RefreshStateProperties()
        {
            Position = history.Count;
            Count = GetHistoryCount();
        }

        public string TryGetCurrentFilename(Guid id)
        {
            var hasKey = currentFilenames.ContainsKey(id);
            if (hasKey)
                return currentFilenames[id];
            else
                return default; 
        }

        private void SetCurrentFilename(Guid id, string filename)
        {
            if (currentFilenames.ContainsKey(id))
                currentFilenames[id] = filename;
            else
                currentFilenames.Add(id, filename);
        }
            

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
        private HistoryObject currentHistoryObject;
        private IDictionary<Guid, string> currentFilenames = new Dictionary<Guid, string>();

        #endregion // Fields
    }
}
