using System.Data;
using Microsoft.Data.Sqlite;

namespace Subqueries.Tests.Helpers
{
    internal static class SqliteHelper
    {
        public static SqliteConnection Connection { get; set; }

        static SqliteHelper()
        {
            var connectionString = InMemoryConnectionString();
            Connection = new SqliteConnection(connectionString);
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
    }
}