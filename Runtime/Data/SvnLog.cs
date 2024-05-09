namespace EZ.TortoiseSVN.Data
{
    public class SvnLog
    {
        public int Revision { get; set; }
        public string ContextMessage { get; set; }
        public string ChangedPath { get; set; }
        public string Author { get; set; }
        public string Date { get; set; }

        public SvnLog()
        {
            Revision = -1;
            ContextMessage = string.Empty;
            ChangedPath = string.Empty;
            Author = string.Empty;
            Date = string.Empty;
        }

        public override string ToString()
        {
            var builder = new System.Text.StringBuilder();
            builder.AppendLine($"Revision: {Revision}");
            builder.AppendLine($"ContextMessage: {ContextMessage}");
            builder.AppendLine($"ChangedPath: {ChangedPath}");
            builder.AppendLine($"Author: {Author}");
            builder.AppendLine($"Date: {Date}");
            return builder.ToString();
        }
    }
}