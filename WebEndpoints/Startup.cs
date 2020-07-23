using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessDayCalculatorApi.ServiceInterfaces;
using BusinessDayCalculatorApi.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WebEndpoints
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
            var appSettings = new AppSettings();
            Configuration.GetSection("AppSettings").Bind(appSettings);

            services.AddControllers();
            services.AddSingleton<IPublicHolidayDataService>(new PublicHolidayCsvFileService(appSettings.PublicHolidaysCsvFile));
            services.AddSingleton<IPublicHolidayService, PublicHolidayService>();
            services.AddSingleton<IBusinessDayCalculationService, BusinessDayCalculationService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
