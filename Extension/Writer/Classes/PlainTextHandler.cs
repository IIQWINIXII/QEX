using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;
using Writer.Interface;

namespace Writer.Classes
{
    public class PlainTextHandler : IDocumentHandler
    {
        public string[] SupportedExtensions => new[] { ".txt" };
        public async Task<DocumentInfo> LoadAsync(Stream stream, string fileName)
        {
            using var sr = new StreamReader(stream, leaveOpen: true);
            var text = await sr.ReadToEndAsync();
            return new DocumentInfo
            {
                FileName = fileName,
                MimeType = "text/plain",
                Text = text,
                Content = Encoding.UTF8.GetBytes(text)
            };

        }

        public Task<string> RenderToHtmlAsync(DocumentInfo doc)
        {
            var html = $"<pre>{System.Net.WebUtility.HtmlEncode(doc.Text ?? string.Empty)}</pre>";
            return Task.FromResult(html);
        }

        public async Task SaveAsync(DocumentInfo doc, Stream stream)
        {
            using var sw = new StreamWriter(stream, leaveOpen: true);
            await sw.WriteAsync(doc.Text ?? string.Empty);
            await sw.FlushAsync();
        }
    }
}
