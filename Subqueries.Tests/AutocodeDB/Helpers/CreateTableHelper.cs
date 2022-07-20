using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AutocodeDB.Models;
using AutocodeDB.Parsers;
using AutocodeDB.SQLTemplates;

namespace AutocodeDB.Helpers
{
    public static class CreateTableHelper
    {
        private static RegexOptions options = RegexOptions.Compiled | RegexOptions.IgnoreCase;
        private static readonly Regex CreateRegExp = new Regex(CreateTableEntity.CreateTable,options);
        private static readonly Regex PrimaryKeyRegExp = new Regex(CreateTableEntity.PrimaryKey,options);
        private static readonly Regex ForeignKeyRegExp = new Regex(CreateTableEntity.ForeignKey,options);
        private static readonly Regex UniqueKeyRegExp = new Regex(CreateTableEntity.UniqueKey,options);
        private static readonly Regex OnDeleteRegExp = new Regex(CreateTableEntity.OnDelete,options);

        private static Dictionary<string, DbTable> _tableMap;
        
        public static void ValidateConstrainKeyIntegrity(string query, IEnumerable<string> queries)
        {
            LoadTables(queries);
            var table = QueryParser.ParseTable(query);
            foreach(var fk in table.ForeignKeys)
            {
                var sequenceNumber = _tableMap[table.TableName].SequenceNumber;
                var refTable = _tableMap[fk.RefTable];
                if (sequenceNumber < refTable.SequenceNumber)
                    throw new ArgumentException($"Table '{fk.RefTable}' must be created before table '{table.TableName}'.");
                if (!refTable.ColumnList.ContainsKey(fk.RefColumn))
                    throw new ArgumentException($"Foreign key '{fk.LocalColumn}' in table '{table.TableName}' REFERENCES not existing column '{fk.RefColumn}' in table '{fk.RefTable}'.");
                if (refTable.ColumnList[fk.RefColumn] != table.ColumnList[fk.LocalColumn])
                    throw new ArgumentException($"Column '{table.TableName}.{fk.LocalColumn}' and '{refTable.TableName}.{fk.RefColumn}' have different types.");
            }
        }
        
        public static IEnumerable<string> GetOnlyQueriesWithForeignKeys(IEnumerable<string> queries) => queries.Where(ContainsForeignKey);
        public static bool ContainsCreateTableStatement(string query) => CreateRegExp.IsMatch(query);
        public static bool ContainsPrimaryKey(string query) => PrimaryKeyRegExp.IsMatch(query);
        public static bool ContainsForeignKey(string query) => ForeignKeyRegExp.IsMatch(query);
        public static bool ContainsUniqueKey(string query) => UniqueKeyRegExp.IsMatch(query);

        public static bool ContainsOnDeleteCascade(string query) => OnDeleteRegExp.IsMatch(query);

        public static void LoadTables(IEnumerable<string> queries)
        {
            if (_tableMap is { }) 
                return;
            _tableMap = new Dictionary<string, DbTable>(13);
            int i = 0;
            foreach(var query in queries)
            {
                var table = QueryParser.ParseTable(query);
                table.SequenceNumber = i;
                ++i;
                if(_tableMap.ContainsKey(table.TableName))  
                    throw new ArgumentException($"Table {table.TableName} alredy exists in Table Map.");
                _tableMap.Add(table.TableName, table);
            }
        }
        public static void ResetMap()
        {
            _tableMap=null; 
        }

        public static string MapToString()
        {
            if(_tableMap==null)
                return "string.Empty";
            return string.Join(",", _tableMap.Keys);
        }
    }
}