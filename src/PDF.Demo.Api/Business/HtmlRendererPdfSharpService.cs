using System.IO;
using System.Threading.Tasks;
using PDF.Demo.Api.Business.Abstractions;
using PDF.Demo.Api.Models;
using TheArtOfDev.HtmlRenderer.PdfSharp;

namespace PDF.Demo.Api.Business
{
    public class HtmlRendererPdfSharpService : IPdfService
    {
        public Task<byte[]> Create(PdfRequest model)
        {
            byte[] res = null;
            using (var ms = new MemoryStream())
            {
                var pdf = PdfGenerator.GeneratePdf(model.HtmlPage, PdfSharp.PageSize.A4);
                pdf.Save(ms);
                res = ms.ToArray();
            }
            return Task.FromResult(res);
        }
    }
}