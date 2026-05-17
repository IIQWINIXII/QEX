using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;
using Writer.Classes;

namespace Writer.Interface
{
    public interface IEditorService
    {
        Task NewAsync();
        Task<DocumentInfo> OpenAsync(Stream stream, string fileName);
        Task SaveAsync(DocumentInfo doc, Stream targetStream);
        Task SaveAsAsync(DocumentInfo doc, Stream targetStream, string fileName);
        IDocumentHandler GetHandler(string fileName);
    }
}
