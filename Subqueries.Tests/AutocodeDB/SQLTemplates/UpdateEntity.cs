using System;

namespace AutocodeDB.SQLTemplates
{
    public static class UpdateEntity
    {
        public static readonly string UpdateSet =
            $@"^\s*UPDATE\s*{TableEntity.TblNameIn}{TableEntity.TableName}{TableEntity.TblNameOut}\s*SET\s*((\s)|(\[))";

        public static readonly string UpdateSetWhere = String.Format($@"{UpdateSet}[\s\S]*?WHERE\s*{TableEntity.TableAndColumnName}\s*{OperationEntity.RelationalOperator}");

/*        public static readonly string UpdateSetWhere = String.Format(@"{0}[\s\S]*?WHERE\s*{1}{2}{3}\s*{4}", UpdateSet,
            TableEntity.ColNameIn, TableEntity.ColumnName, TableEntity.ColNameOut + "*", OperationEntity.RelationalOperator);*/

        public static readonly string UpdateSetWhereSubselect = String.Format(@"{0}\s*[(]\s*SELECT\s*{1}\s*FROM\s*{2}{3}{4}", UpdateSetWhere
            , $"(({FunctionEntity.Any})|({TableEntity.TableAndColumnName}))"
            , TableEntity.TblNameIn
            , TableEntity.TableName
            , TableEntity.TblNameOut);

    }
}
