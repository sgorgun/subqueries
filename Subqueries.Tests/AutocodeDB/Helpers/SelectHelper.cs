using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.Data.Sqlite;
using AutocodeDB.Models;
using AutocodeDB.SQLTemplates;

namespace AutocodeDB.Helpers
{
    public static class SelectHelper
    {
        private static RegexOptions options = RegexOptions.Compiled | RegexOptions.IgnoreCase;
        private static readonly Regex SelectFromRegex = new Regex(SelectEntity.SelectFrom,options);
        private static readonly Regex SelectRegex = new Regex(SelectEntity.Select,options);
        private static readonly Regex AggregationFuncRegex = new Regex(SelectEntity.AggregationFunctions, options);
        private static readonly Regex JoinRegex = new Regex(SelectEntity.Join, options);
        private static readonly Regex InnerJoinRegex = new Regex(SelectEntity.InnerJoin, options);
        private static readonly Regex LeftJoinRegex = new Regex(SelectEntity.LeftJoin, options);
        private static readonly Regex OrderByRegex = new Regex(SelectEntity.OrderBy, options);
        private static readonly Regex GroupByRegex = new Regex(SelectEntity.GroupBy, options);
        private static readonly Regex WhereRegex = new Regex(SelectEntity.Where, options);
        private static readonly Regex WhereIsNullRegex = new Regex(SelectEntity.WhereIsNull, options);
        private static readonly Regex UnionRegex = new Regex(SelectEntity.Union, options);
        private static readonly Regex DistinctRegex = new Regex(SelectEntity.Distinct,options);
        /// <summary>
        /// alternate regex for subqueries! 
        /// Please dont remove
        /// </summary>
        ///private static readonly Regex SubSelectRegex = new Regex(@"\s*SELECT((\s)|(\[)|(\())+?(.)+?\s((IN)|(JOIN)|(NOT IN)|(>)|(<)|(=)|(<=)|(>=)|(NOT EXISTS)|(EXISTS))\s*\(\s*SELECT((\s)|(\[)|(\())+?(.)+?\)",options);
        private static readonly Regex SubSelectRegex = new Regex(@"\s*SELECT((\s)|(\[)|(\())+?(.)+?\s*\(\s*SELECT((\s)|(\[)|(\())+?(.)+?\)",options);

        public static bool ContainsSelectFrom(string query) => SelectFromRegex.IsMatch(query);
        public static bool ContainsAggregationFunctions(string query) => AggregationFuncRegex.IsMatch(query);
        public static bool ContainsJoin(string query) => JoinRegex.IsMatch(query);
        public static bool ContainsInnerJoin(string query) => InnerJoinRegex.IsMatch(query);
        public static bool ContainsLeftJoin(string query) => LeftJoinRegex.IsMatch(query);
        public static bool ContainsOrderBy(string query) => OrderByRegex.IsMatch(query);
        public static bool ContainsGroupBy(string query) => GroupByRegex.IsMatch(query);
        public static bool ContainsWhere(string query) => WhereRegex.IsMatch(query);
        public static bool ContainsWhereIsNull(string query) => WhereIsNullRegex.IsMatch(query);
        public static bool ContainsUnion(string query) => UnionRegex.IsMatch(query);
        public static bool ContainsDistinct(string query) => DistinctRegex.IsMatch(query);
        public static bool ContainsSimpleSelect(string query) => SelectRegex.IsMatch(query);
        public static bool ContainsSubqueries(string query) => SubSelectRegex.IsMatch(query);

        public static SelectResult[] GetResults(IEnumerable<string> queries)
        {
            var results = new List<SelectResult>();
            foreach (var query in queries)
            {
                var result = GetResult(query);
                results.Add(result);
            }
            return results.ToArray();
        }

        public static SelectResult GetResult(string query)
        {
            var command = new SqliteCommand(query, SqliteHelper.Connection);
            var result = new SelectResult();
            try
            {
                var reader = command.ExecuteReader();
                result = Read(reader);
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.Message;
            }
            return result;
        }

        public static SelectResult DumpTable(string name)
        {
            string query = $"SELECT * FROM {name}";
            return GetResult(query);
        }
        public static SelectResult[] DumpTables(IEnumerable<string> names)
        {
            var queries = new List<string>();
            foreach (var name in names)
            {
                queries.Add($"SELECT * FROM {name}");
            }
            return GetResults(queries);
        }


        private static SelectResult Read(SqliteDataReader reader)
        {
            var data = new List<string[]>();
            var result = new SelectResult(reader.FieldCount);
            while (reader.Read())
            {
                var rowData = new string[reader.FieldCount];
                for (var i = 0; i < reader.FieldCount; i++)
                {
                    if (data.Count == 0)
                    {
                        result.Schema[i] = reader.GetName(i);
                        result.Types[i] = reader.GetDataTypeName(i);
                    }
                    if (!reader.IsDBNull(i))
                    {
                        rowData[i] = reader.GetString(i);
                    }
                }
                data.Add(rowData);
            }
            result.Data = data.ToArray();
            return result;
        }
    }
}