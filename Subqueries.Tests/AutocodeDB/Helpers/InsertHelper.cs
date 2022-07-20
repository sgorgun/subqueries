using System.Text.RegularExpressions;
using AutocodeDB.SQLTemplates;

namespace AutocodeDB.Helpers
{
    public static class InsertHelper
    {
        private static RegexOptions options = RegexOptions.Compiled | RegexOptions.IgnoreCase;
        private static readonly Regex InsertRegExp = new Regex(InsertEntity.Insert,options);
        //^\s*INSERT\s+INTO\s*((\s)|(\[))(([_]+[A-Za-z0-9])|([A-Za-z]))[A-Za-z_0-9]*((\])|(\s))\s*VALUES\s*\(
        public static bool ContainsCorrectInsertInstruction(string query) => InsertRegExp.IsMatch(query);
    }
}
