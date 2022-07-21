namespace AutocodeDB.SQLTemplates
{
    public static class InsertEntity
    {
        public static readonly string Insert = $@"^\s*INSERT\s+INTO\s*((\s)|(\[)){TableEntity.TableName}((\])|(\s)|(\())(\s?.+\))?\s*VALUES\s*\(";
    }
}
