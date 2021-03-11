using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using PDF.Demo.Api.Business;
using PDF.Demo.Api.Business.Abstractions;
using Xunit;

namespace PDF.Demo.Api.Test.Business
{
    public class FormHtmlBuilderTests : BaseTestClass
    {
        private readonly IHtmlBuilder<JObject> htmlBuilder;
        private readonly Fixture fixture;

        public FormHtmlBuilderTests()
        {
            this.htmlBuilder = new FormHtmlBuilder();
            this.fixture = new Fixture();
        }

        [Fact]
        public async Task Create_WithValidFormAndFormData_ReturnsHtml()
        {
            // Arrange
            var form = GetJObjectFromFile("Form.json");
            var formData = GetJObjectFromFile("FormData.json");

            // Act
            var html = htmlBuilder.Create(form, formData);

            // Assert
            html.Should().NotBeNullOrEmpty();
        }

    }
}
