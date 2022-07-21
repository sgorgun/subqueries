using System.Text.RegularExpressions;
using AutocodeDB.SQLTemplates;

namespace AutocodeDB.Helpers
{
    public class DeleteHelper
    {
        private static RegexOptions options = RegexOptions.Compiled | RegexOptions.IgnoreCase;
        private static readonly Regex DeleteFromRegex = new Regex(DeleteEntity.DeleteFrom, options);
        private static readonly Regex DeleteFromWhereRegex = new Regex(DeleteEntity.DeleteFromWhere, options);
        private static readonly Regex DeleteFromSublelectRegex = new Regex(DeleteEntity.DeleteFromSubselect, options);

        public static bool ContainsDeleteFrom(string query) => DeleteFromRegex.IsMatch(query);
        public static bool ContainsDeleteFromWhere(string query) => DeleteFromWhereRegex.IsMatch(query);
        public static bool ContainsDeleteFromSubselect(string query) => DeleteFromSublelectRegex.IsMatch(query);
    }
}
