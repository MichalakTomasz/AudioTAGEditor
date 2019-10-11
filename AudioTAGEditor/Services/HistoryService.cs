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

        public void Add(
            IEnumerable<Audiofile> audioFiles,
            ChangeActionType changeActionType,
            string path)
        {
            var historyObject = new HistoryObject
            {
                Path = path,
                ChangeActionType = changeActionType,
                Audiofiles = audioFiles
            };

            if (Position < history.Count) history.RemoveAt(Position);
            history.Add(historyObject);
            Position = history.Count;
            Count = history.Count;
        }

        public void Add(
            IEnumerable<Audiofile> audioFiles,
            ChangeActionType changeActionType,
            string path,
            string newFilename)
        {
            if (changeActionType == ChangeActionType.Filename)
                audioFiles.ToList().ForEach(a => SetCurrentFilename(a.ID, a.Filename));
            else
                throw new Exception("Wrong call Push method parameters.");

            Add(audioFiles, changeActionType, path);
        }

        public void Add(
            Audiofile audioFile,
            ChangeActionType changeActionType,
            string path)
        {
            var audioFileCollection = new List<Audiofile> { audioFile };
            Add(audioFileCollection, changeActionType, path);
        }

        public void Add(
           Audiofile audioFile,
           ChangeActionType changeActionType,
           string path,
           string newFilename)
        {
            if (changeActionType == ChangeActionType.Filename)
                SetCurrentFilename(audioFile.ID, audioFile.Filename);
            else
                throw new Exception("Wrong call Push method parameters.");

            Add(audioFile, changeActionType, path);
        }

        public HistoryObject Undo(IEnumerable<Audiofile> audioFiles)
        {
            if (Position > 0)
            {

                currentHistoryObject = history[--Position];
                
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

                history[Position] = historyObject;

                return resultHistoryObject;
            }
            throw new Exception("Wrong data");
        }

        public HistoryObject Redo(IEnumerable<Audiofile> audioFiles)
        {
            if (Position < Count)
            {
                currentHistoryObject = history[Position++];
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

                history[Position -1] = historyObject;

                return resultHistoryObject;
            }
            throw new Exception("Wrong data");
        }

        public void Clear()
        {
            history.Clear();
            Count = 0;
            Position = 0;
        }
                
        public string GetCurrentFilename(Guid id)
        {
            var hasKey = currentFilenames.ContainsKey(id);
            if (hasKey)
                return currentFilenames[id];
            else
                return history
                    .FirstOrDefault()?
                    .Audiofiles?
                    .FirstOrDefault(a => a.ID == id)?
                    .Filename; 
        }

        private void SetCurrentFilename(Guid id, string filename)
        {
            if (currentFilenames.ContainsKey(id))
                currentFilenames[id] = filename;
            else
                currentFilenames.Add(id, filename);
        }

        public void ResetPosition()
            => Position = history.Count;
                
        #endregion // Methods

        #region Properties

        public int Count { get; private set; }
        public int Position { get; private set; }

        public HistoryObject LastChanges => history.LastOrDefault();
                
        #endregion // Properties

        #region Fields

        private readonly List<HistoryObject> history = new List<HistoryObject>();
        private readonly IAudiofileConverter audioFileConverter;
        private HistoryObject currentHistoryObject;
        private Dictionary<Guid, string> currentFilenames = new Dictionary<Guid, string>();

        #endregion // Fields
    }
}
