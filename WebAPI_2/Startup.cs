using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Modelos.Contexto;
using Newtonsoft.Json.Serialization;
using WebAPI_2.Middleware;

namespace WebAPI_2
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
            services.AddDbContext<ApplicationDBContext>(options => 
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddControllers(); //.AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver  = new DefaultContractResolver());

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
                options.DefaultForbidScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
            })
               .AddIdentityServerAuthentication(options =>
               {
                   options.Authority = "http://localhost:5000";
                   options.ApiName = "TELERIK_API";
                   options.RequireHttpsMetadata = false; 
               });
        



        //services.AddAuthentication("Bearer")
        //      .AddJwtBearer("Bearer", options =>
        //      {
        //          options.Authority = "http://localhost:5000";
        //          options.RequireHttpsMetadata = false;

        //          options.Audience = "TELERIK_API";
        //      });



            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "WEB API TEST Documentación",
                    Version = "v1",
                    Description = "REST API  para Pruebas",
                    Contact = new OpenApiContact()
                    {
                        Name = "Name Example",
                        Email = "manueldizcalvino@gmail.com"
                    }

                });
                //var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.XML";
                //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                //c.IncludeXmlComments(xmlPath);
            });

            IoC.AddDependency(services);

            // Add framework services.
 


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Crea un middleware para exponer la documentación en el JSON.
            app.UseSwagger();
            // Crea  un middleware para exponer el UI (HTML, JS, CSS, etc.),
            // Especificamos en que endpoint buscara el json.
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "TEST Api V1");
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
