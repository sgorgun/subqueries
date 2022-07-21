namespace AutocodeDB.SQLTemplates
{
    public static class TableEntity
    {
        public static readonly string TableName = @"(([_]+[A-Za-z0-9])|([A-Za-z]))[A-Za-z_0-9]*";
        public static readonly string TblNameIn = @"((\s)|(\[))";
        public static readonly string TblNameOut = @"((\])|(\s))";
        public static readonly string TblNameOutExtended = @"((\])|(\s)|(\())";
        public static readonly string ColNameIn = @"((\s)|(\[))";
        public static readonly string ColNameOut = @"((\])|(\s))";
        public static readonly string ColNameOutExtended = @"((\])|(\s)|)";
        //public static readonly string TableName2 = @"((\s)|(\[))(([_]+[A-Za-z0-9])|([A-Za-z]))[A-Za-z_0-9]*((\])|(\s)|(\())";
        //public static readonly string TableName3 = @"\s*CREATE\sTABLE\s\[?(?<tblName>[A-Za-z_]*)\[?\s*";
        //public static readonly string TableName4 = @"[?[A-Za-z_]*\[?";

        public static readonly string ColumnName = @"[A-Za-z_]*";
        public static readonly string ColumnType = @"[A-Za-z_]*";
        public static readonly string TableAndColumnName = $@"(\[?{TableName}\]?\.)?\[?{ColumnName}\]?";
        public static readonly string TableAndColumnNameExt = $@"({TblNameIn}{TableName}\]?\.)?\[?{ColumnName}{ColNameOut}";
        public static readonly string ForeignKeyExtraction = $@"\s+FOREIGN\s+KEY\s*[(]\s*(?<localId>{ColumnName})\s*[)]\s*REFERENCES\s+(?<refTable>{TableName})\s*[(]\s*(?<refId>{ColumnName})\s*[)]";
        public static readonly string Constraint = @"\s*CONSTRAINT\s";
        public static readonly string ColumnExtraction = $@"\s*\[?\s*(?<colName>{ColumnName}*)\s*\]?\s+(?<colType>{ColumnType})\s*";
    }
}
