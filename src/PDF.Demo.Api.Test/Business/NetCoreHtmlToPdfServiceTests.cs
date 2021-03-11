using System.IO;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Microsoft.OpenApi.Any;
using Moq;
using NetCoreHTMLToPDF;
using Newtonsoft.Json.Linq;
using PDF.Demo.Api.Business;
using PDF.Demo.Api.Business.Abstractions;
using PDF.Demo.Api.Models;
using Xunit;

namespace PDF.Demo.Api.Test.Business
{
    public class NetCoreHtmlToPdfServiceTests : BaseTestClass
    {
        private readonly IPdfService _pdfService;
        private readonly Fixture _fixture;
        
        public NetCoreHtmlToPdfServiceTests()
        {
            _fixture = new Fixture();
            _pdfService = new NetCoreHtmlToPdfService(new HtmlConverter());
        }

        [Fact]
        public async Task Create_WithValidModel_ReturnsPdf()
        {
            // Arrange
            const string FileUrl = @"c:\PDFCreator\NetCoreHtmlToPdfTest.pdf";
            var model = this._fixture
                .Build<PdfRequest>()
                .With(p => p.HtmlPage, GetTextFromFile("FormFilled.html"))
                .Create();

            // Act
            var result = await _pdfService.Create(model);
            await File.WriteAllBytesAsync(FileUrl, result);
            var file = File.Open(FileUrl, FileMode.Open);

            // Assert
            result.Should().NotBeNull();
            file.Should().NotBeNull();
        }
    }
}