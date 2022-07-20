using System.Text;

namespace AutocodeDB.Helpers
{
    public static class MessageComposer
    {
        public static string Compose(string[] schema, string[][] data)
        {
            var sb = new StringBuilder();
            var header = string.Join("|", schema);
            var line = GenerateLine(header.Length);
            sb.AppendLine(header);
            sb.AppendLine(line);

            foreach (var row in data)
                sb.AppendLine(string.Join("|", row));

            return sb.ToString().Trim();
        }

        public static string Compose(string[] data)
        {
            return string.Join("|", data).Trim();
        }

        private static string GenerateLine(int lenght)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < lenght; i++)
            {
                sb.Append("-");
            }
            return sb.ToString();
        }
    }
}
