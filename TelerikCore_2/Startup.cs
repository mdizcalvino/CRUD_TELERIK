using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Modelos.Contexto;
using Newtonsoft.Json.Serialization;

namespace TelerikCore_2
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
            //services.Configure<RequestLocalizationOptions>(options =>
            //{
            //    options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("en-GB");
            //    options.SupportedCultures = new List<CultureInfo> { new CultureInfo("en-GB") };
            //    options.RequestCultureProviders.Clear();
            //});


            //services.Configure<RequestLocalizationOptions>(options =>
            //{
            //    options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("es-ES");
            //    options.SupportedCultures = new List<CultureInfo> { new CultureInfo("es-ES") , new CultureInfo("en-US") };
            //});
            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                        new CultureInfo("en-US"),
                        new CultureInfo("es-ES")
                };

                options.DefaultRequestCulture = new RequestCulture(culture: "es-ES", uiCulture: "es-ES");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
                options.RequestCultureProviders.Clear();
            });

            //services.AddLocalization();


            services.AddHttpClient("TEST",op => 
            op.BaseAddress = new Uri("https://localhost:44319/api/"));

            services.AddDbContext<ApplicationDBContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            //var mvcviews = services.AddControllersWithViews();
            //#if (DEBUG)
            //            mvcviews.AddRazorRuntimeCompilation();
            //#endif


            // Add framework services.
            services
                .AddControllersWithViews()
               
                .AddRazorRuntimeCompilation()
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                // Maintain property names during serialization. See:
                // https://github.com/aspnet/Announcements/issues/194
                .AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());

            // Add Kendo UI services to the services container
            services.AddKendo();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //            var supportedCultures = new[]{
            //    new CultureInfo("es-ES")
            //};
            //            app.UseRequestLocalization(new RequestLocalizationOptions
            //            {
            //                DefaultRequestCulture = new RequestCulture("es-ES"),
            //                SupportedCultures = supportedCultures,
            //                SupportedUICultures = supportedCultures
            //                //FallBackToParentCultures = false
            //            });
            //            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.CreateSpecificCulture("es-ES");

            app.UseRequestLocalization();

            //RequestLocalizationOptions localizationOptions = new RequestLocalizationOptions
            //{
            //    SupportedCultures = new List<CultureInfo> { new CultureInfo("es-ES") },
            //    SupportedUICultures = new List<CultureInfo> { new CultureInfo("es-ES") },
            //    DefaultRequestCulture = new RequestCulture("es-ES"),
            //    //RequestCultureProviders = new AcceptLanguageHeaderRequestCultureProvider();

            //};

            //app.UseRequestLocalization(); // localizationOptions);

            app.UseHttpsRedirection();


          

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}