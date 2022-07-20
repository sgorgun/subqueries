using NUnit.Framework;
using Microsoft.Data.Sqlite;
using AutocodeDB.Helpers;
using System;
using AutocodeDB.Models;
using System.IO;

namespace Subqueries.Tests
{
    [TestFixture]
    public class SqlTaskTests
    {
        private const int FilesCount = 3;
        private readonly string DatabaseFile;
        private readonly string EmptyDatabaseFile;
        private readonly string InsertFile;
        private readonly string[] FileNames;
        private readonly string[] QueryFiles;
        private readonly string[] Queries;
        private SelectResult[] ActualResults;
        private SelectResult[] ExpectedResults;

        public SqlTaskTests()
        {
            FileIOHelper.FilesCount = FilesCount;
            FileIOHelper.GenerateProjectDirectory(Environment.CurrentDirectory);
            DatabaseFile = FileIOHelper.GetDBFullPath("marketplace.db");
            EmptyDatabaseFile = FileIOHelper.GetEmptyDBFullPath("empty_tables.db");
            InsertFile = FileIOHelper.GetInsertFileFullPath("insert.sql");
            FileNames = FileIOHelper.GetFilesNames();
            QueryFiles = SqlTask.GetFilePaths(FileNames);
            Queries = QueryHelper.GetQueries(QueryFiles);
        }

        [OneTimeSetUp]
        public void Setup()
        {
            SqliteHelper.OpenConnection(DatabaseFile);
            ExpectedResults = FileIOHelper.DeserializeResultFiles(FileNames);
            ActualResults = SelectHelper.GetResults(Queries);
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            SqliteHelper.CloseConnection();
        }

        [Test]
        public void FileWithQueries_Exists([Range(1, FilesCount)] int index)
        {
            AssertFileExist(index - 1);
        }

        [Test]
        public void FileWithQueries_NotEmpty([Range(1, FilesCount)] int index)
        {
            AssertFileNotEmpty(index - 1);
        }

        [Test]
        public void AllInsertQueries_ExecuteSuccessfully([Range(1, FilesCount)] int index)
        {
            AssertData(index - 1);
        }

        [Test]
        public void SelectQuery_ReturnsCorrectRowsCount([Range(1, FilesCount)] int index)
        {
            index -= 1;
            AssertData(index);
            var expected = ExpectedResults[index].Data.Length;
            var actual = ActualResults[index].Data.Length;
            Assert.AreEqual(expected, actual, ActualResults[index].ErrorMessage);
        }

        [Test]
        public void SelectQuery_ReturnsCorrectSchema([Range(1, FilesCount)] int index)
        {
            index -= 1;
            AssertData(index);
            var expected = ExpectedResults[index].Schema;
            var actual = ActualResults[index].Schema;
            var expectedMessage = MessageComposer.Compose(expected);
            var actualMessage = MessageComposer.Compose(actual);
            Assert.AreEqual(expected, actual, "\nExpected:\n{0}\n\nActual:\n{1}\n", expectedMessage, actualMessage);
        }

        [Test]
        public void SelectQuery_ReturnsCorrectTypes([Range(1, FilesCount)] int index)
        {
            index -= 1;
            AssertData(index);
            var expected = ExpectedResults[index].Types;
            var actual = ActualResults[index].Types;
            var expectedMessage = MessageComposer.Compose(expected);
            var actualMessage = MessageComposer.Compose(actual);
            Assert.AreEqual(expected, actual, "\nExpected:\n{0}\n\nActual:\n{1}\n", expectedMessage, actualMessage);
        }

        [Test]
        public void SelectQuery_ReturnsCorrectData([Range(1, FilesCount)] int index)
        {
            index -= 1;
            AssertData(index);
            var expected = ExpectedResults[index].Data;
            var actual = ActualResults[index].Data;
            var expectedMessage = MessageComposer.Compose(ExpectedResults[index].Schema, expected);
            var actualMessage = MessageComposer.Compose(ActualResults[index].Schema, actual);
            Assert.AreEqual(expected, actual, "\nExpected:\n{0}\n\nActual:\n{1}\n", expectedMessage, actualMessage);
        }

        #region Additional checks

        [Test]
        public void SelectQuery_ContainsSelectFrom([Range(1, FilesCount)] int index)
        {
            var actual = Queries[index - 1];
            Assert.IsTrue(SelectHelper.ContainsSubqueries(actual), "Query should contain 'SELECT' and 'FROM' statements.");
        }

        [Test]
        public void SelectQuery_ContainsWhere([Range(1, FilesCount)] int index)
        {
            var actual = Queries[index - 1];
            Assert.IsTrue(SelectHelper.ContainsWhere(actual), "Query should contain 'WHERE' statements.");
        }

        [Test]
        public void SelectQuery_ContainsOrderBy([Range(1, FilesCount)] int index)
        {
            var actual = Queries[index - 1];
            Assert.IsTrue(SelectHelper.ContainsOrderBy(actual), "Query should contain 'ORDER BY' statement.");
        }

        [Test]
        [TestCase(2)]
        [TestCase(3)]
        public void SelectQuery_ContainsGroupBy(int index)
        {
            var actual = Queries[index - 1];
            Assert.IsTrue(SelectHelper.ContainsGroupBy(actual), "Query should contain 'GROUP BY' statement.");
        }

        [Test]
        [TestCase(1)]
        public void SelectQuery_ContainsInnerJoin(int index)
        {
            var actual = Queries[index - 1];
            Assert.IsTrue(SelectHelper.ContainsInnerJoin(actual), "Query should contain 'INNER JOIN' statement.");
        }

        [Test]
        [TestCase(2)]
        [TestCase(3)]
        public void SelectQuery_ContainsJoin(int index)
        {
            var actual = Queries[index - 1];
            Assert.IsTrue(SelectHelper.ContainsJoin(actual), "Query should contain 'JOIN' statement.");
        }

        #endregion

        #region Private methods

        private void AssertData(int index)
        {
            AssertFileExist(index);
            AssertFileNotEmpty(index);
            AssertErrors(index);
        }

        private void AssertErrors(int index)
        {
            if (!string.IsNullOrEmpty(ActualResults[index].ErrorMessage))
                Assert.Fail(ActualResults[index].ErrorMessage);
        }

        private void AssertFileExist(int index)
        {
            var actual = Queries[index];
            var message = $"The file '{FileNames[index]}' was not found.";
            if (actual == null)
                Assert.Fail(message);
        }

        private void AssertFileNotEmpty(int index)
        {
            var actual = Queries[index];
            var message = $"The file '{FileNames[index]}' contains no entries.";
            if (string.IsNullOrWhiteSpace(actual))
                Assert.Fail(message);
        }
        #endregion
    }
}