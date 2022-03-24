namespace AutocodeDB.Models
{
    public class DbTableForeignKey
    {
        public string LocalColumn { get; }
        public string RefTable { get; }
        public string RefColumn { get; }

        public DbTableForeignKey(string localColumn, string refTable, string refColumn)
        {
            LocalColumn = localColumn;
            RefTable = refTable;
            RefColumn = refColumn;
        }

        public override string ToString()
        {
            return $"{LocalColumn}=>{RefTable}:{RefColumn}";
        }
    }
}