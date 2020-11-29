using Domain.Interfaces.Engines;
using Domain.Interfaces.Helpers;
using Domain.Interfaces.Storage;
using Domain.Interfaces.Storage.Helpers;
using Domain.Interfaces.Storage.Repositories;
using Domain.Interfaces.Strategies;
using Engines;
using Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Storage;
using Storage.Helpers;
using Storage.Repositories;
using Strategies;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SalesTaxApi
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
            services.AddControllers();

            services.AddSingleton<IProductEngine, ProductEngine>();
            services.AddSingleton<IReceiptEngine, ReceiptEngine>();
            services.AddSingleton<IProductRepository, ProductRepository>();
            services.AddSingleton<IConnectionFactory, ConnectionFactory>();
            services.AddSingleton<IConnectionFactory, ConnectionFactory>();
            services.AddSingleton<ISqlLiteDbHelper, SqlLiteDbHelper>();
            services.AddSingleton<ITaxCalculationStrategy, NoTaxCalculationStrategy>();
            services.AddSingleton<ITaxCalculationStrategy, BasicTaxCalculationStratgey>();
            services.AddSingleton<ITaxCalculationStrategy, ImportTaxCalculationStrategy>();
            services.AddSingleton<ITaxCalculationStrategy, ImportNoBasicTaxCalculationStrategy>();
            services.AddSingleton<ISettingsHelper, SettingsHelper>();



#pragma warning disable ASP0000 // Do not call 'IServiceCollection.BuildServiceProvider' in 'ConfigureServices'
            services.BuildServiceProvider().GetService<ISqlLiteDbHelper>().SetupDb();
#pragma warning restore ASP0000 // Do not call 'IServiceCollection.BuildServiceProvider' in 'ConfigureServices'
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, SwaggerGenConfiguration>();
            services.AddSwaggerGen();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "SalesTaxApi");
                c.RoutePrefix = string.Empty;
            });
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
