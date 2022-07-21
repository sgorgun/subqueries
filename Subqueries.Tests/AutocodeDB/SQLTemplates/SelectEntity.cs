namespace AutocodeDB.SQLTemplates
{
    public static class SelectEntity
    {
        public static readonly string SelectFrom = @"\s*SELECT((\s)|(\[)|(\())+?(.)+?\s+FROM[^\w]";
        public static readonly string Select = @"SELECT\s+";
        public static readonly string AggregationFunctions = @"\s((COUNT)|(AVG)|(SUM)|(MIN)|(MAX))\s*\(\s*(.)+?\s*\)";
        public static readonly string Join = @"((\s)|(\])|(\)))+?JOIN((\s)|(\[)|(\())+?(.)+?((\s)|(\])|(\)))+?ON((\s)|(\[)|(\())+?(.)+?=";
        public static readonly string InnerJoin = @"((\s)|(\])|(\)))+?INNER\s+JOIN((\s)|(\[)|(\())+?(.)+?((\s)|(\])|(\)))+?ON((\s)|(\[)|(\())+?(.)+?=";
        public static readonly string LeftJoin = @"((^)|(\s)|(\])|(\)))+?LEFT\s+JOIN((\s)|(\[)|(\())+?(.)+?((\s)|(\])|(\)))+?ON((\s)|(\[)|(\())+?(.)+?=";
        public static readonly string OrderBy = @"((\s)|(\])|(\)))+?ORDER\s+BY((\s)|(\[)|(\())+?[^\s]+";
        public static readonly string GroupBy = @"((\s)|(\])|(\)))+?GROUP\s+BY((\s)|(\[)|(\())+?[^\s]+";
        public static readonly string Where = @"\s+FROM\s+(.)+?((\s)|(\])|(\)))+?WHERE((\s)|(\[)|(\())+?";
        public static readonly string WhereIsNull = @"\s+FROM\s+(.)+?((\s)|(\])|(\)))+?WHERE((\s)|(\[)|(\())+?(.)+?((\s)|(\))|(\]))((IS NULL)|(IS NOT NULL))((\s+?)|($))";
        public static readonly string Union = @"\s+UNION\s+";
        public static readonly string Distinct = @"\s+?DISTINCT((\s)|(\[)|(\())+?";
        /// <summary> 
        /// alternate string for subqueries! Please dont remove
        /// </summary>
        //public static readonly string SubSelectstring = new string(@"\s*SELECT((\s)|(\[)|(\())+?(.)+?\s((IN)|(JOIN)|(NOT IN)|(>)|(<)|(=)|(<=)|(>=)|(NOT EXISTS)|(EXISTS))\s*\(\s*SELECT((\s)|(\[)|(\())+?(.)+?\)";
        public static readonly string SubSelectstring = @"\s*SELECT((\s)|(\[)|(\())+?(.)+?\s*\(\s*SELECT((\s)|(\[)|(\())+?(.)+?\)";
    }
}
