using AudioTAGEditor.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AudioTAGEditor.Services
{
    public class HistoryService : IHistoryService
    {
        #region Constructor

        public HistoryService(
            IAudiofileConverter audioFileConverter,
            IAudiofileComparerService audiofileComparer)
            => this.audiofileComparer = audiofileComparer;

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
            IDictionary<Guid, string> newFilenames)
        {
            if (changeActionType == ChangeActionType.Filename || 
                changeActionType == ChangeActionType.Mixed)
            {
                foreach (var item in newFilenames)
                    SetCurrentFilename(item.Key, item.Value);
            } 
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
            if (changeActionType == ChangeActionType.Filename ||
                changeActionType == ChangeActionType.Mixed)
                SetCurrentFilename(audioFile.ID, newFilename);
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
            currentFilenames.Clear();
        }

        /// <summary>
        /// Gets current filename by analyzing all history changes until now.
        /// </summary>
        /// <param name="id">File identifier.</param>
        /// <returns>Returns string filename.</returns>
        public string GetCurrentFilename(Guid id)
        {
            var hasKey = currentFilenames.ContainsKey(id);
            if (hasKey)
                return currentFilenames[id];
            else
                return history
                    .FirstOrDefault(f => f.Audiofiles.Any(a => a.ID == id))
                    .Audiofiles.FirstOrDefault(f => f.ID == id).Filename; 
        }

        private void SetCurrentFilename(Guid id, string filename)
        {
            if (currentFilenames.ContainsKey(id))
                currentFilenames[id] = filename;
            else
                currentFilenames.Add(id, filename);
        }


        /// <summary>
        /// Checks if file at specified ID has any changes since current history position
        /// </summary>
        /// <param name="id">Identifier of file</param>
        /// <returns>Boolean result</returns>
        public bool HasChangesSinceCurrentHistoryPosition(Guid id)
        {
            return history.Where((h, i) => i >= Position)?
               .SelectMany(s => s.Audiofiles.Where(a => a.ID == id))?
               .Any() ?? false;
        }

        public bool HasTagChangesSienceCurrentPosition(Audiofile audiofile)
        {
            if (!HasChangesSinceCurrentHistoryPosition(audiofile.ID))
                return false;

            return history.Where((h, i) => i >= Position)?
               .SelectMany(s => s.Audiofiles.Where(a => a.ID == audiofile.ID))?
               .Any(a => !audiofileComparer.AreTheSameTags(audiofile, a)) ?? false;
        }

        public HistoryObject GoToLast()
        {
            Position = history.Count;
            Count = history.Count;
            return history.LastOrDefault();
        }

                
        #endregion // Methods

        #region Properties

        public int Count { get; private set; }
        public int Position { get; private set; }
                        
        #endregion // Properties

        #region Fields

        private readonly List<HistoryObject> history = new List<HistoryObject>();
        private readonly IAudiofileComparerService audiofileComparer;
        private HistoryObject currentHistoryObject;
        private Dictionary<Guid, string> currentFilenames = new Dictionary<Guid, string>();

        #endregion // Fields
    }
}
