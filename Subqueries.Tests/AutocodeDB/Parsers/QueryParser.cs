using System;
using System.Text.RegularExpressions;
using AutocodeDB.Models;

namespace AutocodeDB.Parsers
{
    public static class QueryParser
    {
        private static readonly Regex TableNameMatcher = new Regex(@"\s*CREATE\sTABLE\s\[?(?<tblName>[A-Za-z_]*)\[?\s*",
            RegexOptions.Compiled|RegexOptions.IgnoreCase); 
        private static readonly Regex ForeignKeyRegExp = new Regex(@"\s+FOREIGN\s+KEY\s*[(]\s*(?<localId>[A-Za-z_]*)\s*[)]\s*REFERENCES\s+(?<refTable>[A-Za-z_]*)\s*[(]\s*(?<refId>[A-Za-z_]*)\s*[)]",
            RegexOptions.Compiled|RegexOptions.IgnoreCase);
        private static readonly Regex ConstraintRegExp = new Regex(@"\s*CONSTRAINT\s",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex ColumnRegExp = new Regex(@"\s*\[?\s*(?<colName>[A-Za-z_]*)\s*\]?\s+(?<colType>[A-Za-z_]*)\s*",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static readonly string multiColumnPattern = @"[(]\s*[A-Za-z_]*\s*(?:[,]\s*[A-Za-z_]*\s*)+[)]";

        public static DbTable ParseTable(string query)
        {
            if (string.IsNullOrWhiteSpace(query) || query.Length == 0)
                throw new ArgumentException("Incorrect create table query.");
            
            var matches=TableNameMatcher.Match(query);
            if(!matches.Success)
                throw new ArgumentException("Incorrect create table query.");
            
            var table = new DbTable
                {
                    TableName = matches.Groups["tblName"].Value
                };
            var body = ParseTableBody(query);
            body= Regex.Replace(body, multiColumnPattern, "");
            var tblColRow = body.Split(",");
            foreach (var rec in tblColRow)
            {
                var m = ForeignKeyRegExp.Match(rec);
                if (m.Success)
                {
                    table.ForeignKeys.Add(new DbTableForeignKey(m.Groups["localId"].Value,m.Groups["refTable"].Value,m.Groups["refId"].Value));
                }
                else
                {
                    if (ConstraintRegExp.IsMatch(rec))
                        continue;
                    var rm=ColumnRegExp.Match(rec);
                    if (rm.Success)
                    {
                        if (null == rm.Groups["colName"].Value || null == rm.Groups["colType"].Value)
                            throw new ArgumentException($"Incorrect column definition: '{rec}' in create table '{table.TableName}' query.");
                        if (table.ColumnList.ContainsKey(rm.Groups["colName"].Value))
                            throw new ArgumentException($"Duplicate column name {rm.Groups["colName"].Value} In table {table.TableName}."+table);
                        table.ColumnList.Add(rm.Groups["colName"].Value, rm.Groups["colType"].Value.ToUpper());
                    }   
                }
            }
            return table;
        }

        private static string ParseTableBody(string query)
        {
            var bodyBegin = query.IndexOf('(');
            var bodyEnd = query.LastIndexOf(')');
            if (bodyBegin < 0 || bodyEnd < 0)
                throw new ArgumentException("Incorrect create table query.");
            
            return query.Substring(bodyBegin + 1, bodyEnd - bodyBegin - 1);
        }
        private static string[] ParseTableColumn(string query)
        {
            throw new NotImplementedException();
        }

    }
}