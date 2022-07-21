namespace AutocodeDB.SQLTemplates
{
    public static class CreateTableEntity
    {
        public static readonly string CreateTable = $@"^\s*CREATE\sTABLE\s\[?{TableEntity.TableName}\]?\s*";
        public static readonly string PrimaryKey = @"\s+PRIMARY\s+KEY";
        public static readonly string ForeignKey = @"\s+FOREIGN\s+KEY";
        public static readonly string UniqueKey = @"\s+UNIQUE";
        public static readonly string OnDelete = @"\s+ON\s+DELETE\s+CASCADE";
    }
}
