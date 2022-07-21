namespace AutocodeDB.SQLTemplates
{
    public static class AlterTableEntity
    {                                                                                                                                                 
        public static readonly string Simple = $@"^\s*ALTER\s+TABLE\s*{TableEntity.TblNameIn}{TableEntity.TableName}{TableEntity.TblNameOut}\s*";
        public static readonly string RenameTable = $@"^\s*ALTER\s+TABLE\s*{TableEntity.TblNameIn}{TableEntity.TableName}{TableEntity.TblNameOut}\s*RENAME\s+TO{TableEntity.TblNameIn}";
        public static readonly string RenameColumn = $@"^\s*ALTER\s+TABLE\s*{TableEntity.TblNameIn}{TableEntity.TableName}{TableEntity.TblNameOut}\s*RENAME\s*(COLUMN\s*)?{TableEntity.ColNameIn}{TableEntity.ColumnName}{TableEntity.ColNameOut}\s*TO{TableEntity.ColNameIn}";
        public static readonly string AddColumn = $@"^\s*ALTER\s+TABLE\s*{TableEntity.TblNameIn}{TableEntity.TableName}{TableEntity.TblNameOut}\s*ADD\s*(COLUMN\s*)?{TableEntity.ColNameIn}{TableEntity.ColumnName}{TableEntity.ColNameOut}";
        public static readonly string DropColumn = $@"^\s*ALTER\s+TABLE\s*{TableEntity.TblNameIn}{TableEntity.TableName}{TableEntity.TblNameOut}\s*DROP\s*(COLUMN\s*)?{TableEntity.ColNameIn}{TableEntity.ColumnName}{TableEntity.ColNameOutExtended}";
    }
}
