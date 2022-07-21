using System;

namespace AutocodeDB.SQLTemplates
{
    public static class DeleteEntity
    {
        //Yurii's
/*        public static readonly string DeleteFrom = $@"^\s*DELETE\s+TABLE\s*{TableEntity.TblNameIn}{TableEntity.TableName}{TableEntity.TblNameOut}\s*";
        public static readonly string DeleteFromWhere = DeleteFrom+$@"WHERE\s*{TableEntity.ColNameIn}{TableEntity.TableName}{TableEntity.ColNameOut}\s+{OperationEntity.RelationalOperator}";
        public static readonly string DeleteFromSubselect = 
            $@"WHERE\s*{TableEntity.ColNameIn}{TableEntity.TableName}{TableEntity.ColNameOut}\s+{OperationEntity.RelationalOperator}\s*(\s*SELECT\s+{TableEntity.ColNameIn}{TableEntity.ColumnName}{TableEntity.ColNameOut}\s+FROM\s+{TableEntity.TblNameIn}{TableEntity.TableName})";
*/
        

        public static readonly string DeleteFrom = $@"^\s*DELETE\s+FROM\s*{TableEntity.TblNameIn}{TableEntity.TableName}{TableEntity.TblNameOut}\s*";
        ///@"{0}WHERE\s*({1}{2}{3}\.)?{4}{5}{6}{7}
        public static readonly string DeleteFromWhere = String.Format(@"{0}WHERE\s*{1}\s*{2}", DeleteFrom,TableEntity.TableAndColumnName, OperationEntity.RelationalOperator);
        ///@"{0}\s*[(]\s*SELECT\s*{1}\s*FROM\s*{2}{3}{4}[)]
        public static readonly string DeleteFromSubselect = String.Format(@"{0}\s*[(]\s*SELECT\s*{1}\s*FROM\s*{2}{3}{4}", DeleteFromWhere
            , $"(({FunctionEntity.Any})|({TableEntity.TableAndColumnName}))"
            , TableEntity.TblNameIn
            , TableEntity.TableName
            , TableEntity.TblNameOut);
    }
}
