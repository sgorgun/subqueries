using Microsoft.Data.Sqlite;
using Subqueries.Tests.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Subqueries.Tests.Helpers
{
    internal class SelectHelper
    {
        private static readonly Regex SelectFrom_Regex = new Regex(@"\s*SELECT\s+[\w\s\.,\(\)*='\[\]_\-\>\<\!]*\s+FROM", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex SelectDistinctFrom_Regex = new Regex(@"\s*SELECT\s+(DISTINCT\s+)*((\w+(\.\w+)*(\s+AS\s+((\w+)|('\w+')))*)|(\*))(\s*[\S\s][^;]*\s+FROM\s+\w+)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex SelectFromAggregate_Regex = new Regex(@"\s*SELECT\s+((COUNT)|(AVG)|(SUM)|(MIN)|(MAX))(\s+DISTINCT)*\s*\(\s*((\w+)|(\*))\s*\)(\s+AS\s+((\w+)|('\w+')*)|(\*))*(\s*[\S\s][^;]*\s+FROM\s+\w+)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex Join_Regex = new Regex(@"\s*JOIN\s+([\s\w]+ON){1}\s+([\s\.\w]*[^=]){1}\s*=", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex InnerJoin_Regex = new Regex(@"\s*INNER\s+JOIN\s+([\s\w]+ON){1}\s+([\s\.\w]*[^=]){1}\s*=", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex LeftJoin_Regex = new Regex(@"\s*LEFT\s+JOIN\s+[\w\s\.,\(\)*='\[\]_\-\>\<\!]*\s+ON[\w\s\.,\(\)*='\[\]_\-\>\<\!]*[^;]=", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex OrderBy_Regex = new Regex(@"ORDER\s+BY\s+\w+", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex GroupBy_Regex = new Regex(@"GROUP\s+BY\s+\w+", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex Where_Regex = new Regex(@"\s+WHERE\s+\w+", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex WhereIsNull_Regex = new Regex(@"\s*WHERE\s+[\w\s\.,\(\)*='\[\]_\-\>\<\!]*((IS NULL)|(IS NOT NULL))", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex Union_Regex = new Regex(@"\s+UNION\s+", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public static bool ContainsSelectFrom(string query) => SelectFrom_Regex.IsMatch(query);
        public static bool ContainsSelectDistinctFrom(string query) => SelectDistinctFrom_Regex.IsMatch(query);
        public static bool ContainsSelectFromAggregate(string query) => SelectFromAggregate_Regex.IsMatch(query);
        public static bool ContainsJoin(string query) => Join_Regex.IsMatch(query);
        public static bool ContainsInnerJoin(string query) => InnerJoin_Regex.IsMatch(query);
        public static bool ContainsLeftJoin(string query) => LeftJoin_Regex.IsMatch(query);
        public static bool ContainsOrderBy(string query) => OrderBy_Regex.IsMatch(query);
        public static bool ContainsGroupBy(string query) => GroupBy_Regex.IsMatch(query);
        public static bool ContainsWhere(string query) => Where_Regex.IsMatch(query);
        public static bool ContainsWhereIsNull(string query) => WhereIsNull_Regex.IsMatch(query);
        public static bool ContainsUnion(string query) => Union_Regex.IsMatch(query);

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