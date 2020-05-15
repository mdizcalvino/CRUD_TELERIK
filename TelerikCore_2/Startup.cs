using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using IdentityModel;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Modelos.Contexto;
using Newtonsoft.Json.Serialization;
using Services.HttpServices;


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

            

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            //services.Configure<ForwardedHeadersOptions>(options =>
            //{
            //    options.ForwardedHeaders = ForwardedHeaders.XForwardedProto;
            //});

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme; // "Cookies";
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme; // "oidc";

                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultForbidScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignOutScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
                        options =>
                        {
                            options.Cookie.Name = "cookie_nombre"; // adminConfiguration.IdentityAdminCookieName;

                            // Issue: https://github.com/aspnet/Announcements/issues/318
                            options.Cookie.SameSite = SameSiteMode.None;
                            options.SlidingExpiration = true;
                            options.ExpireTimeSpan = TimeSpan.FromMinutes(15);
                        })
            .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
            {
                options.Authority = "http://localhost:5000";
                options.RequireHttpsMetadata = false;

                options.ClientId = "id_mvc_TELERIK";
                options.ClientSecret = "4cc3e329-902f-b271-6dd6-eb9b39978780";

                //para HybridFlow
                //options.ResponseType = "code id_token";

                //* Code
                options.ResponseType = OpenIdConnectResponseType.Code; // "code id_token";
                options.ResponseMode = OpenIdConnectResponseMode.FormPost;
                options.UsePkce = true;
                //*
                



                //options.Events.OnRedirectToIdentityProvider = async n =>
                //{
                //    n.ProtocolMessage.RedirectUri = "https://localhost:9001/sigin-oidc";
                //    await Task.FromResult(0);
                //};

                options.Scope.Clear();

                options.Scope.Add("profile");
                options.Scope.Add("offline_access");
                options.Scope.Add("openid");
                options.Scope.Add("roles");
                options.Scope.Add("email");
                options.Scope.Add("TELERIK_API");


                options.ClaimActions.MapJsonKey("role", "role", "role"); // (adminConfiguration.TokenValidationClaimRole, adminConfiguration.TokenValidationClaimRole, adminConfiguration.TokenValidationClaimRole);

                options.SaveTokens = true;

                options.GetClaimsFromUserInfoEndpoint = true;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = "name", //adminConfiguration.TokenValidationClaimName,
                    RoleClaimType = "role" // adminConfiguration.TokenValidationClaimRole
                };

                //options.Events.OnTicketReceived = async (context) =>
                //{
                //    context.Properties.ExpiresUtc = DateTime.UtcNow.AddHours(1);
                //    await Task.FromResult(0);
                //};
                options.Events = new OpenIdConnectEvents
                {
                    OnMessageReceived = context => OnMessageReceived(context),
                    
                    //OnRedirectToIdentityProvider = context => OnRedirectToIdentityProvider(context, adminConfiguration)
                    

                };

                //options.Events.OnSignedOutCallbackRedirect += context =>
                //{
                //    context.Response.Redirect(context.Options.SignedOutRedirectUri);
                //    context.HandleResponse();

                //    return Task.CompletedTask;
                //};

                //options.SaveTokens = true;

                //options.GetClaimsFromUserInfoEndpoint = true;

            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireAdministratorRole",
                    policy => policy.RequireRole("SkorubaIdentityAdminAdministrator"));
            });


            

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

            //services.AddHttpClient("IS4", op =>
            //    op.BaseAddress = new Uri("http://localhost:5000/connect/token"));

            services.Configure<TokenClientOptions>(options =>
            {
                options.Address = "http://localhost:5000/connect/token";
                options.ClientId = "id_mvc_TELERIK";
                options.ClientSecret = "4cc3e329-902f-b271-6dd6-eb9b39978780";
            });
            services.AddTransient(sp => sp.GetRequiredService<IOptions<TokenClientOptions>>().Value);

            services.AddHttpClient<TokenClient>();

            services.AddHttpContextAccessor();

            services.AddScoped(typeof(IGenericHttpService<>), typeof(GenericHttpService<>));

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


        private static Task OnMessageReceived(MessageReceivedContext context)
        {
            //context.Properties.IsPersistent = true;
            context.Properties.ExpiresUtc = new DateTimeOffset(DateTime.Now.AddHours(1)); //.AddHours(adminConfiguration.IdentityAdminCookieExpiresUtcHours));

            return Task.FromResult(0);
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


            ////Registered before static files to always set header
            //app.UseHsts(hsts => hsts.MaxAge(365).IncludeSubdomains());
            //app.UseXContentTypeOptions();
            //app.UseReferrerPolicy(opts => opts.NoReferrer());
            //app.UseXXssProtection(options => options.EnabledWithBlockMode());
            //app.UseXfo(options => options.Deny());

            //app.UseCsp(opts => opts
            //    .BlockAllMixedContent()
            //    .StyleSources(s => s.Self())
            //    .StyleSources(s => s.UnsafeInline())
            //    .FontSources(s => s.Self())
            //    .FormActions(s => s.Self())
            //    .FrameAncestors(s => s.Self())
            //    .ImageSources(imageSrc => imageSrc.Self())
            //    .ImageSources(imageSrc => imageSrc.CustomSources("data:"))
            //    .ScriptSources(s => s.Self())
            //);






            app.UseStaticFiles();

            //app.UseAuthentication();

            app.UseRouting();

            app.UseAuthentication();

            //app.UseMiddleware<CheckAccessTokenValidityMiddleware>();

            app.UseAuthorization();



            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }


        //public class CheckAccessTokenValidityMiddleware
        //{
        //    private readonly RequestDelegate _next;
        //    private readonly IConfiguration _configuration;

        //    public CheckAccessTokenValidityMiddleware(RequestDelegate next, IConfiguration configuration)
        //    {
        //        _next = next;
        //        _configuration = configuration;
        //    }

        //    public async Task InvokeAsync(HttpContext context)
        //    {

        //        var disco =  DiscoveHttpClientDiscoveryExtensions   ryDocumentRequest DiscoveryClient.GetAsync("https://localhost:44356");

        //        var expireAt = await context.GetTokenAsync("expires_at");
        //        if (expireAt != null)
        //        {
        //            var dateExpireAt = DateTime.Parse(expireAt, null, DateTimeStyles.RoundtripKind);
        //            if (dateExpireAt != null)
        //            {
        //                if ((dateExpireAt - DateTime.Now).TotalMinutes < 10)
        //                {
        //                    var discoveryClient = new DiscoveryClient(_configuration["OIDC:Authority"]);
        //                    discoveryClient.Policy.RequireHttps = false;
        //                    var discovery = await discoveryClient.GetAsync();
        //                    if (!discovery.IsError)
        //                    {
        //                        using (var tokenClient = new TokenClient(discovery.TokenEndpoint, "id_mvc_TELERIK", "4cc3e329-902f-b271-6dd6-eb9b39978780"))
        //                        {
        //                            var refreshToken = await context.GetTokenAsync("refresh_token");
        //                            var tokenResult = await tokenClient.RequestRefreshTokenAsync(refreshToken);
        //                            if (!tokenResult.IsError)
        //                            {
        //                                var newIdToken = tokenResult.IdentityToken;
        //                                var newAccessToken = tokenResult.AccessToken;
        //                                var newRefreshToken = tokenResult.RefreshToken;
        //                                var tokens = new List<AuthenticationToken>
        //                        {
        //                            new AuthenticationToken {Name = OpenIdConnectParameterNames.IdToken, Value = newIdToken},
        //                            new AuthenticationToken
        //                            {
        //                                Name = OpenIdConnectParameterNames.AccessToken,
        //                                Value = newAccessToken
        //                            },
        //                            new AuthenticationToken
        //                            {
        //                                Name = OpenIdConnectParameterNames.RefreshToken,
        //                                Value = newRefreshToken
        //                            }
        //                        };
        //                                var expiresAt = DateTime.Now + TimeSpan.FromSeconds(tokenResult.ExpiresIn);
        //                                tokens.Add(new AuthenticationToken
        //                                {
        //                                    Name = "expires_at",
        //                                    Value = expiresAt.ToString("o", CultureInfo.InvariantCulture)
        //                                });
        //                                var info = await context.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        //                                info.Properties.StoreTokens(tokens);
        //                                await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, info.Principal, info.Properties);
        //                            }
        //                            else
        //                            {
        //                                await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        //                                await context.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        await _next.Invoke(context);
        //    }
        //}
    }
}