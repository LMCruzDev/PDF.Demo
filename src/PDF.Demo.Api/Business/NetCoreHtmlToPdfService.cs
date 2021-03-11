using System.Threading.Tasks;
using NetCoreHTMLToPDF;
using PDF.Demo.Api.Business.Abstractions;
using PDF.Demo.Api.Models;

namespace PDF.Demo.Api.Business
{
    public class NetCoreHtmlToPdfService : IPdfService
    {
        private readonly HtmlConverter _htmlConverter;

        public NetCoreHtmlToPdfService(HtmlConverter htmlConverter)
        {
            _htmlConverter = htmlConverter;
        }
        
        public Task<byte[]> Create(PdfRequest model)
        {
            return Task.FromResult(_htmlConverter.FromHtmlString(model.HtmlPage));
        }
    }
}