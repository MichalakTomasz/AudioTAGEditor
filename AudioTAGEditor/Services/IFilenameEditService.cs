using AudioTAGEditor.Models;
using System.Collections.Generic;

namespace AudioTAGEditor.Services
{
    public interface IFilenameEditService
    {
        IEnumerable<string> AllFirstCapitalLetters(IEnumerable<string> collection);
        IEnumerable<string> CutDash(IEnumerable<string> collection);
        IEnumerable<string> CutDot(IEnumerable<string> collection);
        IEnumerable<string> CutSpace(IEnumerable<string> collection);
        IEnumerable<string> CutUnderscore(IEnumerable<string> collection);
        IEnumerable<string> CutText(IEnumerable<string> collection, string textToCut);
        IEnumerable<string> CutText(IEnumerable<string> collection, int position, int count);
        IEnumerable<string> FirstCapitalLetter(IEnumerable<string> collection);
        IEnumerable<string> LowerCase(IEnumerable<string> collection);
        IEnumerable<string> InsertNumbering(IEnumerable<string> collection, int position);
        IEnumerable<string> InsertTextFromPosition(IEnumerable<string> collection, int position, string text);
        IEnumerable<string> ReplaceDashToSpace(IEnumerable<string> collection);
        IEnumerable<string> ReplaceDotToSpace(IEnumerable<string> collection);
        IEnumerable<string> ReplaceSpaceToDash(IEnumerable<string> collection);
        IEnumerable<string> ReplaceSpaceToDot(IEnumerable<string> collection);
        IEnumerable<string> ReplaceSpaceToUnderscore(IEnumerable<string> collection);
        IEnumerable<string> ReplaceUnderscoreToSpace(IEnumerable<string> collection);
        IEnumerable<string> ReplaceText(IEnumerable<string> collection, string oldText, string newText);
        IEnumerable<string> UpperCase(IEnumerable<string> collection);
        IEnumerable<string> ChangeByPattern(IEnumerable<Audiofile> collection, string pattern);
    }
}