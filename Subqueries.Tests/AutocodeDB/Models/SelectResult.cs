using System.Text;

namespace AutocodeDB.Models
{
    public class SelectResult
    {
        public string[] Schema { get; set; }
        public string[] Types { get; set; }
        public string[][] Data { get; set; }
        public string ErrorMessage { get; set; }
        
        public SelectResult() { }
       
        public SelectResult(int length)
        {
            Schema = new string[length];
            Types = new string[length];
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            var schema = string.Join(",", Schema).Trim();
            var types = string.Join(",", Types).Trim();
            var data = new StringBuilder();
            foreach (var row in Data)
            {
                var dataRow = string.Join(",", row).Trim();
                data.AppendLine(dataRow);
            }
            sb.AppendLine(schema);
            sb.AppendLine(types);
            sb.AppendLine(data.ToString().Trim());
            return sb.ToString().Trim();
        }
    }
}
