using AudioTAGEditor.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AudioTAGEditor.Services
{
    public class FilenameEditService : IFilenameEditService
    {
        public FilenameEditService(IAudiofileCloneService audiofieleCloneService)
            => this.audiofileCloneService = audiofieleCloneService;

        public IEnumerable<Audiofile> CutSpace(IEnumerable<Audiofile> audiofiles)
            => ReplaceIntoFilenames(audiofiles, " ", "");

        public IEnumerable<Audiofile> CutDot(IEnumerable<Audiofile> audiofiles)
        {
            var clonedAudiofiles = audiofileCloneService.Clone(audiofiles);
            
            clonedAudiofiles.ToList().ForEach(a =>
            {
                var tempFilename = Path.GetFileNameWithoutExtension(a.Filename)
                .Replace(".", "");
                var tempExtension = Path.GetExtension(a.Filename);
                a.Filename = $"{tempFilename}{tempExtension}";
            });

            return clonedAudiofiles;
        }

        public IEnumerable<Audiofile> CutUnderscore(IEnumerable<Audiofile> audiofiles)
            => ReplaceIntoFilenames(audiofiles, "_", "");

        public IEnumerable<Audiofile> CutDash(IEnumerable<Audiofile> audiofiles)
            => ReplaceIntoFilenames(audiofiles, "-", "");

        public IEnumerable<Audiofile> ReplaceDotToSpace(IEnumerable<Audiofile> audiofiles)
            => ReplaceIntoFilenames(audiofiles, ".", " ");

        public IEnumerable<Audiofile> ReplaceUnderscoreToSpace(IEnumerable<Audiofile> audiofiles)
            => ReplaceIntoFilenames(audiofiles, "_", " ");

        public IEnumerable<Audiofile> ReplaceDashToSpace(IEnumerable<Audiofile> audiofiles)
            => ReplaceIntoFilenames(audiofiles, "-", " ");

        public IEnumerable<Audiofile> ReplaceSpaceToDot(IEnumerable<Audiofile> audiofiles)
            => ReplaceIntoFilenames(audiofiles, " ", ".");

        public IEnumerable<Audiofile> ReplaceSpaceToUnderscore(IEnumerable<Audiofile> audiofiles)
            => ReplaceIntoFilenames(audiofiles, " ", "_");

        public IEnumerable<Audiofile> ReplaceSpaceToDash(IEnumerable<Audiofile> audiofiles)
            => ReplaceIntoFilenames(audiofiles, " ", "-");

        public IEnumerable<Audiofile> ReplaceText(
            IEnumerable<Audiofile> audiofiles, string oldText, string newText)
            => ReplaceIntoFilenames(audiofiles, oldText, newText);

        public IEnumerable<Audiofile> CutText(IEnumerable<Audiofile> audiofiles, string textToCut)
        {
            var clonedAudiofiles = audiofileCloneService.Clone(audiofiles);
            clonedAudiofiles.ToList().ForEach(element =>
            {
                int startIndex;
                if ((startIndex = element.Filename.IndexOf(textToCut)) > -1)
                {
                    var count = element.Filename.Length;
                    element.Filename = element.Filename.Remove(startIndex, count);
                }
            });

            return clonedAudiofiles;
        }

        public IEnumerable<Audiofile> CutText(
            IEnumerable<Audiofile> audiofiles, int position, int count)
        {
            var clonedAudiofiles = audiofileCloneService.Clone(audiofiles);
            clonedAudiofiles.ToList().ForEach(element =>
            {
                if (position - 1 < element.Filename.Length)
                    element.Filename = element.Filename.Remove(position - 1, count);
            });

            return clonedAudiofiles;
        }

        public IEnumerable<Audiofile> InsertTextFromPosition(
            IEnumerable<Audiofile> audiofiles, int position, string text)
        {
            var clonedAudiofiles = audiofileCloneService.Clone(audiofiles);
            clonedAudiofiles.ToList().ForEach(element =>
            {
                if (position - 1 < element.Filename.Length)
                {
                    element.Filename = element.Filename.Insert(position - 1, text);
                }
            });

            return clonedAudiofiles;
        }

        public IEnumerable<Audiofile> FirstCapitalLetter(IEnumerable<Audiofile> audiofiles)
        {
            var clonedAudiofiles = audiofileCloneService.Clone(audiofiles);
            clonedAudiofiles.ToList().ForEach(c =>
            {
                var first = c.Filename[0].ToString().ToUpper();
                c.Filename = $"{first}{c.Filename.Substring(1)}";
            });

            return clonedAudiofiles;
        }

        public IEnumerable<Audiofile> AllFirstCapitalLetters(IEnumerable<Audiofile> audiofiles)
        {
            var firstLetterIndexes = new List<int>();
            var resultString = new StringBuilder();
            var clonedAudiofiles = audiofileCloneService.Clone(audiofiles);

            clonedAudiofiles.ToList().ForEach(element =>
            {
                firstLetterIndexes.Clear();
                if (element.Filename.Length > 0) firstLetterIndexes.Add(0);
                for (var i = 0; i < element.Filename.Length; i++)
                    if (element.Filename[i] == ' ' && element.Filename[i + 1] != ' ')
                        firstLetterIndexes.Add(i + 1);
                   
                var position = 0;
                resultString.Clear();
                for (var i = 0; i < element.Filename.Length; i++)
                {
                    if (position < firstLetterIndexes.Count() && i == firstLetterIndexes[position])
                    {
                        resultString.Append(element.Filename[i].ToString().ToUpper());
                        position++;
                    }   
                    else resultString.Append(element.Filename[i].ToString().ToLower());
                }
                element.Filename = resultString.ToString();
            });

            return clonedAudiofiles;
        }

        public IEnumerable<Audiofile> UpperCase(IEnumerable<Audiofile> audiofiles)
        {
            var clonedAudiofiles = audiofileCloneService.Clone(audiofiles);
            audiofiles.ToList().ForEach(c =>
                {
                    var newFilename = c.Filename.ToUpper();
                    c.Filename = newFilename;
                });

            return clonedAudiofiles;
        }

        public IEnumerable<Audiofile> LowerCase(IEnumerable<Audiofile> audiofiles)
        {
            var clonedAudiofiles = audiofileCloneService.Clone(audiofiles);
            clonedAudiofiles.ToList().ForEach(c =>
                c.Filename = c.Filename.ToLower());

            return clonedAudiofiles;
        }

        public IEnumerable<Audiofile> InsertNumbering(IEnumerable<Audiofile> audiofiles, int position)
        {
            var clonedCollection = audiofileCloneService.Clone(audiofiles).ToList();
            var count = clonedCollection.Count;
            var counterLength = count.ToString().Length;
            var tempString = new StringBuilder();
            for (int i = 0; i < count; i++)
            {
                if (position - 1 < clonedCollection[i].Filename.Length)
                {
                    tempString.Clear();
                    tempString.Append(i + 1);
                    while (tempString.Length < counterLength)
                        tempString.Insert(0, "0");
                    clonedCollection[i].Filename = clonedCollection[i].Filename
                        .Insert(position - 1, tempString.ToString());
                }
            }

            return clonedCollection;
        }

        public IEnumerable<Audiofile> ChangeByPattern(
            IEnumerable<Audiofile> audiofiles, string pattern)
        {
            var clonedCollection = audiofileCloneService.Clone(audiofiles);
            var tempString = new StringBuilder();
            var tempIndex = -1;

            clonedCollection.ToList().ForEach(c =>
            {
                tempString.Clear();
                tempString.Append(pattern);

                foreach (var tag in tags)
                {
                    tempIndex = pattern.IndexOf(tag.Value);
                    if (tempIndex > -1)
                    {
                        switch (tag.Key)
                        {
                            case CommonTagProperties.Title:
                                tempString.Replace(tag.Value, c.Title);
                                break;
                            case CommonTagProperties.Album:
                                tempString.Replace(tag.Value, c.Album);
                                break;
                            case CommonTagProperties.Artist:
                                tempString.Replace(tag.Value, c.Artist);
                                break;
                            case CommonTagProperties.TrackNumber:
                                tempString.Replace(tag.Value, c.TrackNumber);
                                break;
                            case CommonTagProperties.Genre:
                                tempString.Replace(tag.Value, c.Genre);
                                break;
                            case CommonTagProperties.Year:
                                tempString.Replace(tag.Value, c.Year);
                                break;
                        }
                    }
                }
                var extension = Path.GetExtension(c.Filename);
                var newFilename = $"{tempString.ToString()}{extension}";
                c.Filename = newFilename;
            });
        
            return clonedCollection;
        }

        private IEnumerable<Audiofile> ReplaceIntoFilenames(
            IEnumerable<Audiofile> audiofiles,
            string oldCharacter,
            string newCharacter)
        {
            var clonedCollection = audiofileCloneService.Clone(audiofiles);
            clonedCollection.ToList().ForEach(c =>
                c.Filename = c.Filename.Replace(oldCharacter, newCharacter));

            return clonedCollection;
        }

        #region Const Strings

        private readonly Dictionary<CommonTagProperties, string> tags = 
            new Dictionary<CommonTagProperties, string>()
        {
                {CommonTagProperties.Title, "<title>" },
                {CommonTagProperties.Album, "<album>" },
                {CommonTagProperties.Artist, "<artist>" },
                {CommonTagProperties.TrackNumber, "<no>" },
                {CommonTagProperties.Genre, "<genre>" },
                {CommonTagProperties.Year, "<year>" }
        };
        private readonly IAudiofileCloneService audiofileCloneService;

        #endregion // Const Strings
    }
}
