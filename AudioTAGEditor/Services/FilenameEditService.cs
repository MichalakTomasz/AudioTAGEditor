using AudioTAGEditor.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AudioTAGEditor.Services
{
    public class FilenameEditService : IFilenameEditService
    {
        public IEnumerable<Audiofile> CutSpace(IEnumerable<Audiofile> audiofiles)
            => ReplaceIntoFilenames(audiofiles, " ", "");

        public IEnumerable<Audiofile> CutDot(IEnumerable<Audiofile> audiofiles)
        {
            var resultCollection = new List<Audiofile>();
            audiofiles.ToList().ForEach(a =>
            {
                var tempFilename = Path.GetFileNameWithoutExtension(a.Filename)
                .Replace(".", "");
                var tempExtension = Path.GetExtension(a.Filename);
                a.Filename = $"{tempFilename}{tempExtension}";
                resultCollection.Add(a);
            });

            return resultCollection;
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
            var resultCollection = new List<Audiofile>();
            audiofiles.ToList().ForEach(element =>
            {
                int startIndex;
                if ((startIndex = element.Filename.IndexOf(textToCut)) > -1)
                {
                    var count = element.Filename.Length;
                    element.Filename = element.Filename.Remove(startIndex, count);
                }

                resultCollection.Add(element);
            });

            return resultCollection;
        }

        public IEnumerable<Audiofile> CutText(
            IEnumerable<Audiofile> audiofiles, int position, int count)
        {
            var resultCollection = new List<Audiofile>();
            audiofiles.ToList().ForEach(element =>
            {
                if (position - 1 < element.Filename.Length)
                    element.Filename = element.Filename.Remove(position - 1, count);

                resultCollection.Add(element);
            });

            return resultCollection;
        }

        public IEnumerable<Audiofile> InsertTextFromPosition(
            IEnumerable<Audiofile> audiofiles, int position, string text)
        {
            var resultCollection = new List<Audiofile>();
            audiofiles.ToList().ForEach(element =>
            {
                if (position - 1 < element.Filename.Length)
                {
                    element.Filename = element.Filename.Insert(position - 1, text);
                }

                resultCollection.Add(element);
            });

            return resultCollection;
        }

        public IEnumerable<Audiofile> FirstCapitalLetter(IEnumerable<Audiofile> audiofiles)
        {
            var resultCollection = new List<Audiofile>();

            audiofiles.ToList().ForEach(c =>
            {
                var first = c.Filename[0].ToString().ToUpper();
                c.Filename = $"{first}{c.Filename.Substring(1)}";
                resultCollection.Add(c);
            });

            return resultCollection;
        }

        public IEnumerable<Audiofile> AllFirstCapitalLetters(IEnumerable<Audiofile> audiofile)
        {
            var firstLetterIndexes = new List<int>();
            var resultCollection = new List<Audiofile>();
            var resultString = new StringBuilder();

            audiofile.ToList().ForEach(element =>
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
                resultCollection.Add(element);
            });

            return resultCollection;
        }

        public IEnumerable<Audiofile> UpperCase(IEnumerable<Audiofile> audiofiles)
        {
            var resultCollection = new List<Audiofile>();
            audiofiles.ToList().ForEach(c =>
                {
                    var newFilename = c.Filename.ToUpper();
                    c.Filename = newFilename;
                    resultCollection.Add(c);
                });

            return resultCollection;
        }

        public IEnumerable<Audiofile> LowerCase(IEnumerable<Audiofile> audiofiles)
        {
            var resultCollection = new List<Audiofile>();
            audiofiles.ToList().ForEach(c =>
            {
                c.Filename = c.Filename.ToLower();
                resultCollection.Add(c);
            });

            return resultCollection;
        }

        public IEnumerable<Audiofile> InsertNumbering(IEnumerable<Audiofile> audiofiles, int position)
        {
            var resultCollection = new List<Audiofile>();
            var elements = audiofiles.ToList();
            var count = elements.Count;
            var counterLength = count.ToString().Length;
            var tempString = new StringBuilder();
            for (int i = 0; i < count; i++)
            {
                if (position - 1 < elements[i].Filename.Length)
                {
                    tempString.Clear();
                    tempString.Append(i + 1);
                    while (tempString.Length < counterLength)
                        tempString.Insert(0, "0");
                    elements[i].Filename = elements[i].Filename
                        .Insert(position - 1, tempString.ToString());
                }
                resultCollection.Add(elements[i]);
            }
            return resultCollection;
        }

        public IEnumerable<Audiofile> ChangeByPattern(
            IEnumerable<Audiofile> audiofiles, string pattern)
        {
            var resultCollection = new List<Audiofile>();
            var tempString = new StringBuilder();
            var tempIndex = -1;

            audiofiles.ToList().ForEach(c =>
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
                resultCollection.Add(c);
            });
        
            return resultCollection;
        }

        private IEnumerable<Audiofile> ReplaceIntoFilenames(
            IEnumerable<Audiofile> audiofiles,
            string oldCharacter,
            string newCharacter)
        {
            var resultCollection = new List<Audiofile>();
            audiofiles.ToList().ForEach(c =>
            {
                c.Filename = c.Filename.Replace(oldCharacter, newCharacter);
                resultCollection.Add(c);
            });

            return resultCollection;
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

        #endregion // Const Strings
    }
}
