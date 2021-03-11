using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using PDF.Demo.Api.Business.Abstractions;

namespace PDF.Demo.Api.Business
{
    public class FormHtmlBuilder : IHtmlBuilder<JObject>
    {
        private const string HtmlTag = "<html>{0}{1}</html>";
        private const string HeadTag = "<head>{0}</head>";
        private const string BodyTag = "<body>{0}{1}</body>";
        private const string TableTag = "<table class='table table-bordered'><tbody>{0}</tbody></table>";
        private string[] cssLinks = new string[]
        {
            @"<link href='https://cdn.jsdelivr.net/npm/bootstrap@5.0.0-beta2/dist/css/bootstrap.min.css' rel='stylesheet' integrity = 'sha384-BmbxuPwQa2lc/FVzBcNJ7UAyJxM6wuqIj61tLrc4wSX0szH/Ev+nYRRuWlolflfl' crossorigin = 'anonymous' >",
            @"<link rel='stylesheet' href='./Assets/PdfStyleSheet.css'>"
        };

        public string Create(JObject form, JObject formData)
        {
            return string.Format(
                HtmlTag,
                CreateHeader(cssLinks),
                CreateBody(form, formData));
        }

        private string CreateHeader(string[] lines)
        {
            var sb = new StringBuilder();

            lines.Select(l => sb.AppendLine(l));

            return string.Format(
                HeadTag,
                sb.ToString());
        }

        private string CreateBody(JObject form, JObject formData)
        {
            return string.Format(
                BodyTag,
                CreateTitle(),
                CreateTable(form, formData));
        }

        private string CreateTable(JObject form, JObject formData)
        {
            return string.Format(
                TableTag,
                CreateTableLines(form, formData));
        }

        private string CreateTitle()
        {
            return @"
            <h3>
                <span class='color-black'>Care Plan & Risk Assessment - Proposal template in progress</span>
                <span class='color-red'>- Proposal template in progress</span>
            </h3>";
        }

        private string CreateTableLines(JObject form, JObject formData)
        {
            var sb = new StringBuilder();

            var lines = form["sections"].Select(s =>
            {
                var sectionNameId = s["fieldNameId"];

                return s["questions"].Select(q =>
                {
                    var fieldName = q["fieldName"];
                    var fieldNameId = q["fieldNameId"];
                    var fieldValue = formData[$"{sectionNameId}__{fieldNameId}"];

                    return string.Format(
                        @"
                        <tr class='border border-dark border-2'>
                            <td class='table-secondary w-25 fw-bold'>{0}</td>
                            <td class='w-75 color-red'>{1}</td>
                        </tr>",
                        fieldName,
                        fieldValue);
                });
            });

            lines.Select(l => l.Select(li => sb.AppendLine(li)));

            return sb.ToString();
        }
    }
}
