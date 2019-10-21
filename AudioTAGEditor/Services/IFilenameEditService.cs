using AudioTAGEditor.Models;
using System.Collections.Generic;

namespace AudioTAGEditor.Services
{
    public interface IFilenameEditService
    {
        IEnumerable<Audiofile> AllFirstCapitalLetters(IEnumerable<Audiofile> audiofiles);
        IEnumerable<Audiofile> CutDash(IEnumerable<Audiofile> audiofiles);
        IEnumerable<Audiofile> CutDot(IEnumerable<Audiofile> audiofiles);
        IEnumerable<Audiofile> CutSpace(IEnumerable<Audiofile> audiofiles);
        IEnumerable<Audiofile> CutUnderscore(IEnumerable<Audiofile> audiofiles);
        IEnumerable<Audiofile> CutText(IEnumerable<Audiofile> audiofiles, string textToCut);
        IEnumerable<Audiofile> CutText(IEnumerable<Audiofile> audiofiles, int position, int count);
        IEnumerable<Audiofile> FirstCapitalLetter(IEnumerable<Audiofile> audiofiles);
        IEnumerable<Audiofile> LowerCase(IEnumerable<Audiofile> audiofiles);
        IEnumerable<Audiofile> InsertNumbering(IEnumerable<Audiofile> audiofiles, int position);
        IEnumerable<Audiofile> InsertTextFromPosition(IEnumerable<Audiofile> audiofiles, int position, string text);
        IEnumerable<Audiofile> ReplaceDashToSpace(IEnumerable<Audiofile> audiofiles);
        IEnumerable<Audiofile> ReplaceDotToSpace(IEnumerable<Audiofile> audiofiles);
        IEnumerable<Audiofile> ReplaceSpaceToDash(IEnumerable<Audiofile> audiofiles);
        IEnumerable<Audiofile> ReplaceSpaceToDot(IEnumerable<Audiofile> audiofiles);
        IEnumerable<Audiofile> ReplaceSpaceToUnderscore(IEnumerable<Audiofile> audiofiles);
        IEnumerable<Audiofile> ReplaceUnderscoreToSpace(IEnumerable<Audiofile> audiofiles);
        IEnumerable<Audiofile> ReplaceText(IEnumerable<Audiofile> audiofiles, string oldText, string newText);
        IEnumerable<Audiofile> UpperCase(IEnumerable<Audiofile> audiofiles);
        IEnumerable<Audiofile> ChangeByPattern(IEnumerable<Audiofile> audiofiles, string pattern);
    }
}