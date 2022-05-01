using System.Text.RegularExpressions;

namespace AutocodeDB.Helpers
{
    public static class InsertHelper
    {
        private static readonly Regex InsertRegExp = 
            new Regex(@"^\s*INSERT\s+INTO\s*((\s)|(\[))(([_]+[A-Za-z0-9])|([A-Za-z]))[A-Za-z_0-9]*((\])|(\s)|(\())(\s?.+\))?\s*VALUES\s*\(", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        //^\s*INSERT\s+INTO\s*((\s)|(\[))(([_]+[A-Za-z0-9])|([A-Za-z]))[A-Za-z_0-9]*((\])|(\s))\s*VALUES\s*\(
        public static bool ContainsCorrectInsertInstruction(string query) => InsertRegExp.IsMatch(query);
    }
}
