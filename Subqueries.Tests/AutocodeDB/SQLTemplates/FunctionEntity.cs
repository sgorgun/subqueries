namespace AutocodeDB.SQLTemplates
{
    public static class FunctionEntity
    {
        public static readonly string Agregation = @"\s((COUNT)|(AVG)|(SUM)|(MIN)|(MAX))\s*\(\s*(.)+?\s*\)";
        public static readonly string String = @"";
        public static readonly string Math = @"";
        public static readonly string Date = @"";
        public static readonly string Any = $@"[A-Za-z]+[(]\s*{TableEntity.TableAndColumnName}\s*[)]";
    }
}
