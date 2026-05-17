using System;
using System.Collections.Generic;
using System.Text;

namespace Writer.Classes
{
    public class DocumentInfo
    {
        public string? FileName { get; set; }
        public string? MimeType { get; set; }
        public byte[]? Content { get; set; } 
        public string? Text { get; set; } 
        public IDictionary<string, object> Metadata { get; set; } = new Dictionary<string, object>();
    }
}
