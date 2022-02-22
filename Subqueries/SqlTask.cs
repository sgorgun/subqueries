using System;
using System.IO;
using System.Linq;

namespace Subqueries
{
    public static class SqlTask
    {
        private const string FolderName = "sql_queries";
        
        public static string GetFilePath(string fileName)
        {
            return Path.Combine(FolderName, fileName);
        }

        public static string[] GetFilePaths(string[] fileNames)
        {
            return fileNames.Select(x => GetFilePath(x)).ToArray();
        }
    }
}
