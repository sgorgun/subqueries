using Microsoft.Data.Sqlite;
using AutocodeDB.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace AutocodeDB.Helpers
{
    internal class SelectHelper
    {
        private static readonly Regex SelectFromRegex = new Regex(@"\s*SELECT\s+[\w\s\.,\(\)*='\[\]_\-\>\<\!]*\s+FROM", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex AggregationFuncRegex = new Regex(@"((COUNT)|(AVG)|(SUM)|(MIN)|(MAX))\([\w\s\.,\(\)*='\[\]_\-\>\<\!]+\)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex JoinRegex = new Regex(@"\s*JOIN\s+([\s\w \[ \] \.]+ON){1}\s+([\s\w \[ \] \.]*[^=]){1}\s*=", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex InnerJoinRegex = new Regex(@"\s*INNER\s+JOIN\s+([\s\w \[ \] \.]+ON){1}\s+([\s\.\w \[ \]]*[^=]){1}\s*=", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex LeftJoinRegex = new Regex(@"\s*LEFT\s+JOIN\s+[\w\s\.,\(\)*='\[\]_\-\>\<\!]*\s+ON[\w\s\.,\(\)*='\[\]_\-\>\<\!]*[^;]=", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex OrderByRegex = new Regex(@"ORDER\s+BY\s+[^\s]+", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex GroupByRegex = new Regex(@"GROUP\s+BY\s+[^\s]+", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex WhereRegex = new Regex(@"\s+WHERE\s+\w+", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex WhereIsNullRegex = new Regex(@"\s*WHERE\s+[\w\s\.,\(\)*='\[\]_\-\>\<\!]*((IS NULL)|(IS NOT NULL))", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex UnionRegex = new Regex(@"\s+UNION\s+", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex DistinctRegex = new Regex(@"\s*DISTINCT\s+", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public static bool ContainsSelectFrom(string query) => SelectFromRegex.IsMatch(query);
        public static bool ContainsSelectDistinctFrom(string query) => SelectFromRegex.IsMatch(query);
        public static bool ContainsSelectFromAggregate(string query) => SelectFromRegex.IsMatch(query) || AggregationFuncRegex.IsMatch(query);
        public static bool ContainsJoin(string query) => JoinRegex.IsMatch(query);
        public static bool ContainsInnerJoin(string query) => InnerJoinRegex.IsMatch(query);
        public static bool ContainsLeftJoin(string query) => LeftJoinRegex.IsMatch(query);
        public static bool ContainsOrderBy(string query) => OrderByRegex.IsMatch(query);
        public static bool ContainsGroupBy(string query) => GroupByRegex.IsMatch(query);
        public static bool ContainsWhere(string query) => WhereRegex.IsMatch(query);
        public static bool ContainsWhereIsNull(string query) => WhereIsNullRegex.IsMatch(query);
        public static bool ContainsUnion(string query) => UnionRegex.IsMatch(query);
        public static bool ContainsDistinct(string query) => DistinctRegex.IsMatch(query);
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

        public static void SerializeResult(SelectResult selectResult, string file)
        {
            File.WriteAllText(file, selectResult.ToString());
        }

        public static SelectResult DeserializeResult(string file)
        {
            if (!File.Exists(file))
                throw new FileNotFoundException($"The file '{file}' was not found in the 'Data' folder.");
            var lines = File.ReadAllLines(file);
            var schema = lines[0].Split(",");
            var types = lines[1].Split(",");
            var data = new List<string[]>();
            for (var i = 2; i < lines.Length; i++)
            {
                var line = lines[i];
                var lineData = line.Split(",");

                for (int j = 0; j < lineData.Length; j++)
                {
                    if (string.IsNullOrEmpty(lineData[j]))
                    {
                        lineData[j] = null;
                    }
                }
                data.Add(lineData);
            }
            return new SelectResult
            {
                Schema = schema,
                Types = types,
                Data = data.ToArray()
            };
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