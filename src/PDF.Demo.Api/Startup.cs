using System;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetCoreHTMLToPDF;
using Newtonsoft.Json.Linq;
using PDF.Demo.Api.Business;
using PDF.Demo.Api.Business.Abstractions;
using PDF.Demo.Api.Business.DataGateway;

namespace PDF.Demo.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Third Party Libraries
            services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
            services.AddSingleton<HtmlConverter>();

            // Services
            services.AddScoped<IPdfService, DinkToPdfService>();
            services.AddSingleton<IHtmlBuilder<JObject>, FormHtmlBuilder>();

            // Apis
            services.AddHttpClient<IFormsApiClient, FormsApiClient>(client =>
            {
                client.BaseAddress = new Uri(Configuration.GetSection("AppSettings")["FormsApiUrl"].ToString());
            });

            // Swagger
            services.AddSwaggerGen();

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();

                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                });
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
