using System;
using System.Collections.Generic;
using System.Text;
using Writer.Classes;

namespace Writer.Interface
{
    public interface IDocumentHandler
    {
        string[] SupportedExtensions { get; }
        Task<DocumentInfo> LoadAsync(Stream stream, string fileName);
        Task SaveAsync(DocumentInfo doc, Stream stream);
        Task<string> RenderToHtmlAsync(DocumentInfo doc);
    }
}
