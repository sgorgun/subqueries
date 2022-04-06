using System.Text.RegularExpressions;

namespace AutocodeDB.Helpers
{
    public static class InsertHelper
    {
        private static readonly Regex InsertRegExp = new Regex(@"^\s*INSERT\s+INTO.*\s*VALUES", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public static bool ContainsCorrectInsertInstruction(string query) => InsertRegExp.IsMatch(query);
    }
}
