using System;
using System.IO;
using System.Threading.Tasks;
using AutoFixture;
using DinkToPdf;
using DinkToPdf.Contracts;
using FluentAssertions;
using Moq;
using Newtonsoft.Json.Linq;
using PDF.Demo.Api.Business;
using PDF.Demo.Api.Business.Abstractions;
using PDF.Demo.Api.Models;
using Xunit;

namespace PDF.Demo.Api.Test.Business
{
    public class DinkToPdfServiceTests : BaseTestClass
    {
        private readonly IConverter converter;
        private readonly Mock<IHtmlBuilder<JObject>> htmlBuilder;
        private readonly Mock<IFormsApiClient> formsApiClient;
        private readonly DinkToPdfService service;
        private readonly Fixture fixture;

        public DinkToPdfServiceTests()
        {
            fixture = new Fixture();
            converter = new SynchronizedConverter(new PdfTools());
            htmlBuilder = new Mock<IHtmlBuilder<JObject>>();
            formsApiClient = new Mock<IFormsApiClient>();
            service = new DinkToPdfService(
                converter,
                formsApiClient.Object,
                htmlBuilder.Object);
        }

        [Fact]
        public async Task Create_WithValidModel_ReturnsPdf()
        {
            // Arrange
            var formId = Guid.NewGuid();

            const string FileUrl = @"c:\PDFCreator\DinkPdfTest.pdf";
            var model = this.fixture.Create<ConsumerModel>();
            model.TemplateGuid = formId;
            model.FormData = GetJObjectFromFile("FormData.json");

            this.htmlBuilder
                .Setup(h => h.Create(It.IsAny<JObject>(), It.IsAny<JObject>()))
                .Returns(GetTextFromFile("FormFilled.html"));

            this.formsApiClient
                .Setup(f => f.GetById(formId))
                .ReturnsAsync(new FormResult
                {
                    Form = GetJObjectFromFile("Form.json")
                });

            // Act
            var result = await service.Create(model);
            await File.WriteAllBytesAsync(FileUrl, result);
            var file = File.Open(FileUrl, FileMode.Open);

            // Assert
            result.Should().NotBeNull();
            file.Should().NotBeNull();
        }

        [Fact]
        public Task CreatePDF_WithEmptyModel_ReturnsError()
        {
            // Arrange
            var model = new ConsumerModel();

            // Act
            Func<Task> act = () => service.Create(model);

            // Assert
            return act.Should().ThrowAsync<ArgumentException>();
        }
    }
}
