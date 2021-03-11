using System;
using System.Threading.Tasks;
using DinkToPdf;
using DinkToPdf.Contracts;
using Newtonsoft.Json.Linq;
using PDF.Demo.Api.Business.Abstractions;
using PDF.Demo.Api.Models;

namespace PDF.Demo.Api.Business
{
    public class DinkToPdfService : IPdfService
    {
        private const string Utf8 = "utf-8";
        private const string ArialFont = "Arial";
        private const int FontSize = 9;
        private const string RightSettings = "Page [page] of [toPage]";
        private const string CenterSettings = "Report Footer";
        private readonly IConverter converter;
        private readonly IFormsApiClient formsApiClient;
        private readonly IHtmlBuilder<JObject> htmlBuilder;

        public DinkToPdfService(
            IConverter converter,
            IFormsApiClient formsApiClient,
            IHtmlBuilder<JObject> htmlBuilder)
        {
            this.converter = converter;
            this.formsApiClient = formsApiClient;
            this.htmlBuilder = htmlBuilder;
        }

        public async Task<byte[]> Create(ConsumerModel consumerModel)
        {
            ValidateConsumerModel(consumerModel);

            var formResult = await formsApiClient.GetById(consumerModel.TemplateGuid);

            ValidateFormResult(formResult);

            var html = htmlBuilder.Create(formResult.Form, consumerModel.FormData);

            var document = GeneratePdf(html, consumerModel.TemplateGuid.ToString());

            return converter.Convert(document);
        }

        public Task<byte[]> Create(PdfRequest pdfRequest)
        {
            if (pdfRequest == null ||
                string.IsNullOrWhiteSpace(pdfRequest.HtmlPage))
            {
                throw new ArgumentNullException(nameof(pdfRequest));
            }

            var document = GeneratePdf(pdfRequest.HtmlPage, pdfRequest.FormName);

            return Task.FromResult(converter.Convert(document));
        }

        private static void ValidateFormResult(FormResult formResult)
        {
            if (formResult.Form == null)
            {
                throw new ArgumentNullException(nameof(formResult));
            }
        }

        private static void ValidateConsumerModel(ConsumerModel consumerModel)
        {
            if (consumerModel.FormData == null)
            {
                throw new ArgumentNullException(nameof(consumerModel));
            }
        }

        private HtmlToPdfDocument GeneratePdf(string htmlPage, string formName)
        {
            return new HtmlToPdfDocument
            {
                GlobalSettings = ConfigureGlobalSettings(formName),
                Objects = { ConfigureObjects(htmlPage) }
            };
        }

        private ObjectSettings ConfigureObjects(string htmlPage)
        {
            return new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = htmlPage,
                WebSettings = { DefaultEncoding = Utf8 },
                HeaderSettings = { FontName = ArialFont, FontSize = FontSize, Right = RightSettings, Line = true },
                FooterSettings = { FontName = ArialFont, FontSize = FontSize, Line = true, Center = CenterSettings }
            };
        }

        private GlobalSettings ConfigureGlobalSettings(string formName)
        {
            return new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 10 },
                DocumentTitle = formName,
            };
        }
    }
}
