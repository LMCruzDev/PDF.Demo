using System;
using System.IO;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Microsoft.OpenApi.Any;
using Moq;
using Newtonsoft.Json.Linq;
using PDF.Demo.Api.Business;
using PDF.Demo.Api.Business.Abstractions;
using PDF.Demo.Api.Models;
using Xunit;

namespace PDF.Demo.Api.Test.Business
{
    public class HtmlRendererPdfSharpServiceTests : BaseTestClass
    {
        private readonly IPdfService _pdfService;
        private readonly Fixture _fixture;

        public HtmlRendererPdfSharpServiceTests()
        {
            _fixture = new Fixture();
            _pdfService = new HtmlRendererPdfSharpService();
        }
        
        [Fact]
        public async Task Create_WithHtml_ReturnsPdf()
        {
            // Arrange
            const string fileUrl = @"c:\PDFCreator\HtmlRendererPdfSharp.pdf";
            var model = this._fixture
                .Build<PdfRequest>()
                .With(p => p.HtmlPage, GetTextFromFile("FormFilled.html"))
                .Create();
            
            // Act
            var result = await _pdfService.Create(model);
            await File.WriteAllBytesAsync(fileUrl, result);
            var file = File.Open(fileUrl, FileMode.Open);

            // Assert
            result.Should().NotBeNull();
            file.Should().NotBeNull();
        }
    }
}