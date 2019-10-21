using AudioTAGEditor.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AudioTAGEditor.Services
{
    public class FilenameEditService : IFilenameEditService
    {
        public IEnumerable<string> CutSpace(IEnumerable<string> collection)
            => ReplaceIntoCollection(collection, " ", "");

        public IEnumerable<string> CutDot(IEnumerable<string> collection)
            => ReplaceIntoCollection(collection, ".", "");

        public IEnumerable<string> CutUnderscore(IEnumerable<string> collection)
            => ReplaceIntoCollection(collection, "_", "");

        public IEnumerable<string> CutDash(IEnumerable<string> collection)
            => ReplaceIntoCollection(collection, "-", "");

        public IEnumerable<string> ReplaceDotToSpace(IEnumerable<string> collection)
            => ReplaceIntoCollection(collection, ".", " ");

        public IEnumerable<string> ReplaceUnderscoreToSpace(IEnumerable<string> collection)
            => ReplaceIntoCollection(collection, "_", " ");

        public IEnumerable<string> ReplaceDashToSpace(IEnumerable<string> collection)
            => ReplaceIntoCollection(collection, "-", " ");

        public IEnumerable<string> ReplaceSpaceToDot(IEnumerable<string> collection)
            => ReplaceIntoCollection(collection, " ", ".");

        public IEnumerable<string> ReplaceSpaceToUnderscore(IEnumerable<string> collection)
            => ReplaceIntoCollection(collection, " ", "_");

        public IEnumerable<string> ReplaceSpaceToDash(IEnumerable<string> collection)
            => ReplaceIntoCollection(collection, " ", "-");

        public IEnumerable<string> ReplaceText(
            IEnumerable<string> collection, string oldText, string newText)
            => ReplaceIntoCollection(collection, oldText, newText);

        public IEnumerable<string> CutText(IEnumerable<string> collection, string textToCut)
        {
            var resultCollection = new List<string>();
            collection.ToList().ForEach(element =>
            {
                int startIndex;
                if ((startIndex = element.IndexOf(textToCut)) > -1)
                {
                    var count = element.Length;
                    var tempString = element.Remove(startIndex, count);
                    resultCollection.Add(tempString);
                }
            });

            return resultCollection;
        }

        public IEnumerable<string> CutText(
            IEnumerable<string> collection, int position, int count)
        {
            var resultCollection = new List<string>();
            collection.ToList().ForEach(element =>
            {
                if (position - 1 < element.Length)
                {
                    var tempString = element.Remove(position - 1, count);
                    resultCollection.Add(tempString);
                }
                else
                    resultCollection.Add(element);
            });

            return resultCollection;
        }

        public IEnumerable<string> InsertTextFromPosition(
            IEnumerable<string> collection, int position, string text)
        {
            var resultCollection = new List<string>();
            collection.ToList().ForEach(element =>
            {
                if (position - 1 < element.Length)
                {
                    var tempString = element.Insert(position - 1, text);
                    resultCollection.Add(tempString);
                }
                else
                    resultCollection.Add(element);
            });

            return resultCollection;
        }

        public IEnumerable<string> FirstCapitalLetter(IEnumerable<string> collection)
        {
            var resultCollection = new List<string>();

            collection.ToList().ForEach(c =>
            {
                var first = c[0].ToString().ToUpper();
                var tempString = $"{first}{c.Substring(1)}";
                resultCollection.Add(tempString);
            });

            return resultCollection;
        }

        public IEnumerable<string> AllFirstCapitalLetters(IEnumerable<string> collection)
        {
            var firstLetterIndexes = new List<int>();
            var resultCollection = new List<string>();

            collection.ToList().ForEach(element =>
            {
                for (var i = 0; i < element.Length; i++)
                    if (element[i] == ' ' && element[i + 1] != ' ')
                        firstLetterIndexes.Add(i + 1);

                var position = 0;
                var resultString = new StringBuilder();
                for (var i = 0; i < element.Length; i++)
                {
                    if (i == element[position++])
                        resultString.Append(element[i].ToString().ToUpper());
                    else resultString.Append(element[i].ToString());
                }
                resultCollection.Add(resultString.ToString());
            });

            return resultCollection;
        }

        public IEnumerable<string> UpperCase(IEnumerable<string> collection)
        {
            var resultCollection = new List<string>();
            collection.ToList().ForEach(c => resultCollection.Add(c.ToUpper()));

            return resultCollection;
        }

        public IEnumerable<string> LowerCase(IEnumerable<string> collection)
        {
            var resultCollection = new List<string>();
            collection.ToList().ForEach(c => resultCollection.Add(c.ToLower()));

            return resultCollection;
        }

        public IEnumerable<string> InsertNumbering(IEnumerable<string> collection, int position)
        {
            var resultCollection = new List<string>();
            var elements = collection.ToList();
            var count = elements.Count;
            var counterLength = count.ToString().Length;
            var tempString = new StringBuilder();
            for (int i = 0; i < count; i++)
            {
                if (position - 1 < elements[i].Length)
                {
                    tempString.Clear();
                    tempString.Append(i);
                    while (tempString.Length < counterLength)
                        tempString.Insert(0, "0");
                    var resultString = elements[i]
                        .Insert(position - 1, tempString.ToString());
                    resultCollection.Add(resultString);
                }
                else
                    resultCollection.Add(elements[i]);
            }
            return resultCollection;
        }

        public IEnumerable<string> ChangeByPattern(
            IEnumerable<Audiofile> collection, string pattern)
        {
            var resultCollection = new List<string>();
            var tempString = new StringBuilder();
            var tempIndex = -1;

            collection.ToList().ForEach(c =>
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
                resultCollection.Add(tempString.ToString());
            });
            
        
            return resultCollection;
        }

        private IEnumerable<string> ReplaceIntoCollection(
            IEnumerable<string> collection,
            string oldCharacter,
            string newCharacter)
        {
            var resultCollection = new List<string>();
            collection.ToList().ForEach(c =>
                resultCollection.Add(c.Replace(oldCharacter, newCharacter)));

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
