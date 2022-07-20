using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.Sqlite;

namespace AutocodeDB.Helpers
{
    public static class SqliteHelper
    {
        public static SqliteConnection Connection { get; set; }

        static SqliteHelper()
        {
            var connectionString = InMemoryConnectionString();
            Connection = new SqliteConnection(connectionString);
        }

        public static void OpenConnection()
        {
            var connectionString = new SqliteConnectionStringBuilder
            {
                Mode = SqliteOpenMode.Memory,
                ForeignKeys = true,

            }.ConnectionString;

            Connection = new SqliteConnection(connectionString);
            Connection.Open();
        }
        public static void OpenConnection(string file)
        {
            Connection.Open();
            var connectionString = FileConnectionString(file, readOnly: true);
            using var source = new SqliteConnection(connectionString);
            source.Open();
            source.BackupDatabase(Connection);
            source.Close();
        }

        public static void OpenConnection(string importFile, string exportFile)
        {
            var exportString = FileConnectionString(exportFile);
            Connection = new SqliteConnection(exportString);
            Connection.Open();

            var importString = FileConnectionString(importFile, readOnly: true);
            using var importConnection = new SqliteConnection(importString);
            importConnection.Open();
            importConnection.BackupDatabase(Connection);
            importConnection.Close();
        }

        public static void CloseConnection()
        {
            if (Connection.State is ConnectionState.Open)
                Connection.Close();
        }

        private static string InMemoryConnectionString() => new SqliteConnectionStringBuilder
        {
            Mode = SqliteOpenMode.Memory
        }.ConnectionString;

        private static string FileConnectionString(string fileName, bool readOnly = false) => new SqliteConnectionStringBuilder
        {
            DataSource = fileName,
            Mode = readOnly ? SqliteOpenMode.ReadOnly : SqliteOpenMode.ReadWriteCreate
        }.ConnectionString;

        public static int CountRows(string tableName)
        {
            var countCmd = new SqliteCommand($"SELECT COUNT(*) FROM {tableName}", Connection);
            return Convert.ToInt32(countCmd.ExecuteScalar());
        }

        public static int[] CountRows(string tableName, string columnName)
        {
            var countCmd = new SqliteCommand($"SELECT {columnName}, COUNT({columnName}) FROM {tableName} GROUP BY {columnName}", Connection);
            var reader = countCmd.ExecuteReader();
            List<int> ListRes = new List<int>();
            while (reader.Read())
            {
                ListRes.Add(reader.GetInt32(1));
            }
            return ListRes.ToArray();
        }

        public static void InsertData(string InsertFile)
        {
            var queries = QueryHelper.GetQueries(InsertFile);
            foreach (var query in queries)
            {
                var command = new SqliteCommand(query, SqliteHelper.Connection);
                command.ExecuteNonQuery();
            }
        }

    }

}