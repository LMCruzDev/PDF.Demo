using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json.Linq;

namespace PDF.Demo.Api.Models
{
    public class ConsumerModel
    {
        [Required]
        public Guid TemplateGuid { get; set; }

        [Required]
        public JObject FormData { get; set; }
    }
}
