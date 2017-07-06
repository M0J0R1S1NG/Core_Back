using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Session;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Core.Data;
using Core.Models;
using Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Net;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Twitter;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;
using Microsoft.AspNetCore.Http;

namespace Core
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);
            
            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see https://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets<Startup>();

            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }
        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services
            services.AddDbContext<ApplicationDbContext>( options => 
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
                );
           
           // Add useful interface for accessing the ActionContext outside a controller.
                // services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
                // Add useful interface for accessing the HttpContext outside a controller.
                services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddIdentity<ApplicationUser, IdentityRole>(config =>
            {
                config.SignIn.RequireConfirmedEmail = true;
                config.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                config.Lockout.MaxFailedAccessAttempts = 10;
                config.Lockout.AllowedForNewUsers = false;
                config.Password.RequireDigit = true;
                config.Password.RequiredLength = 8;
                config.Password.RequireNonAlphanumeric = true;
                config.Password.RequireUppercase = true;
                config.Password.RequireLowercase = true;
                config.User.RequireUniqueEmail = true;
                config.SignIn.RequireConfirmedPhoneNumber = false;
            
                //config.Tokens.EmailConfirmationTokenProvider = "";

            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
                

            // services.AddMvc().AddRazorOptions(options =>
            // {
            
            //    options.
                     
            // });
            services.AddDistributedMemoryCache();
            
            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromSeconds(10);
                options.CookieHttpOnly = true;
            });
            
            
            
             if (Environment.GetEnvironmentVariable("DEVHOST")==null)
            {
               
                    services.AddMvc(options =>
                        {
                            options.SslPort = 443;
                            options.Filters.Add(new RequireHttpsAttribute());
                        }
                    ).AddJsonOptions(options =>
                            {
                                options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver();
                            }
                    );

            }else{
            
                    services.AddMvc(options =>
                        {
                            options.SslPort = 443;
                            //options.Filters.Add(new RequireHttpsAttribute());
                        }
                    ).AddJsonOptions(options =>
                            {
                                options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver();
                            }
                    );
            }
          
              
            services.AddAuthorization(options =>
            {
                options.AddPolicy("ElevatedRights", policy =>
                  policy.RequireRole("Admin", "PowerUser", "BackupAdministrator"));
            });
            // Add application services.
            
            services.AddTransient<ISmsSender, AuthMessageSender>();
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.Configure<AuthMessageSenderOptions>(Configuration);
            services.Configure<SiteSettings>(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory,ApplicationDbContext context)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            loggerFactory.AddConsole(LogLevel.Debug);

           // Add a new middleware issuing tokens.
    
// app.UseOAuthValidation();


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
              // app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();
            app.UseIdentity();
            
            //Add external authentication middleware below. To configure them please see https://go.microsoft.com/fwlink/?LinkID=532715
            app.UseGoogleAuthentication(new GoogleOptions()
            {
               ClientId = "198140796890-e69d8lj0roo0rj1i3uja3al8lofre84u.apps.googleusercontent.com", //Configuration["Authentication:Google:ClientId"],
               ClientSecret = "xBd5Rdb6Drv2ZjtVrjKDN3hQ"  //Configuration["Authentication:Google:ClientSecret"]
            });
            app.UseMicrosoftAccountAuthentication(new MicrosoftAccountOptions()
            {
               ClientId = "ca3e0e09-9038-41c8-ba76-0a4fa320c104", //Configuration["Authentication:Microsoft:ClientId"],
               ClientSecret = "ytBAkH1kSMNLafcZAhzrgGn" //Configuration["Authentication:Microsoft:ClientSecret"]
            });
            app.UseFacebookAuthentication(new FacebookOptions()
            {
               AppId = "1418831251496182", //Configuration["Authentication:Facebook:AppId"],
               AppSecret = "1d06e49668b85858e973f017b4c1fcbf" //Configuration["Authentication:Facebook:AppSecret"]
            });
            app.UseTwitterAuthentication(new TwitterOptions()
            {
               ConsumerKey = "n0lzf0BliHRgaU7mEQVhfBUXl", //Configuration["Authentication:Twitter:ConsumerKey"],
               ConsumerSecret = "xMgkswlp5DMRhwlNsIaq5Yl3bks68CZu12cFQX7W3iFWAnVgS1" //Configuration["Authentication:Twitter:ConsumerSecret"]
            });
            //app.UseDirectoryBrowser();
            app.UseSession();
            app.UseMvc(routes =>
            {   
                routes.MapRoute(
                     name:"Pages",
                     template: "{controller=Admin}/{action=Index}/{id?}");
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
                   
                
            });
            DbInitializer.Initialize(context);
        }
        private Task OnAuthenticationFailed(FailureContext context)

        {

            context.HandleResponse();

            context.Response.Redirect("/Home/Error?message=" + context.Failure.Message);

            return Task.FromResult(0);

        }
    }
}
