using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.IO;
using System.Reflection;

namespace SalesTaxApi
{
    public class SwaggerGenConfiguration : IConfigureOptions<SwaggerGenOptions>
    {
        public void Configure(SwaggerGenOptions options)
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "SalesTaxApi",
                Description = "A simple API created for interview process. This API will generate a receipt for a set of given products.",
                Contact = new OpenApiContact
                {
                    Name = "Joseph Rodriguez",
                    Email = "JRodriguez89@att.net",
                    Url = new Uri("https://www.linkedin.com/in/jrod730/"),
                },
                License = new OpenApiLicense
                {
                    Name = "Use under MIT Lincense",
                    Url = new Uri("https://mit-license.org"),
                }
            });

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            options.IncludeXmlComments(xmlPath);
        }
    }
}
