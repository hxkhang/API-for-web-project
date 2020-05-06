using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TestAPI.Models;

namespace TestAPI
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
            services.AddDbContext<WebApiContext>(item => item.UseSqlServer(Configuration.GetConnectionString("myconn")));

            //Authentication
            //services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            //    .AddCookie(option=>
            //    {
            //        option.AccessDeniedPath = "/api/login/ErrorForbidden";
            //        option.LoginPath = "/api/login/ErrorNotLogIn";
            //    });
            //services.AddAuthorization(option => {
            //    option.AddPolicy("Must Be Admin", p => p.RequireAuthenticatedUser().RequireRole("Admin"));
            //    option.AddPolicy("Must Be User", p => p.RequireAuthenticatedUser().RequireRole("User"));
            //});

            //add cors
            services.AddCors(options =>
            {
                //allow another web to connect
                options.AddPolicy("AllowMyOrigin",
                builder => builder.WithOrigins("https://localhost:44320"));
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            //app.UseAuthentication();

            //use cors
            app.UseCors("AllowMyOrigin");

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
