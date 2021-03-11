using System;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PDF.Demo.Api.Business.Abstractions;
using PDF.Demo.Api.Models;

namespace PDF.Demo.Api.Controllers
{
    [ApiController]
    [Route("v1/pdf")]
    public class PdfController : ControllerBase
    {
        private readonly IPdfService pdfService;

        public PdfController(IPdfService pdfService)
        {
            this.pdfService = pdfService;
        }

        [HttpPost("/create")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateFromHtml([FromBody] PdfRequest request)
        {
            try
            {
                var file = await pdfService.Create(request);

                if (file != null)
                {
                    return File(
                    file,
                    "application/pdf",
                    $"{request.FormName}.pdf");
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
