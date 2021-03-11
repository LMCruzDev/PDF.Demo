using System.Threading.Tasks;
using PDF.Demo.Api.Models;

namespace PDF.Demo.Api.Business.Abstractions
{
    public interface IPdfService
    {
        Task<byte[]> Create(PdfRequest model);
    }
}
