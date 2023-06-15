using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Dievas.Data;
using Dievas.Hubs;
using Dievas.Models;
using Dievas.Models.Auth;

namespace Dievas {

    /// <summary>
    ///     Startup Class <c>Startup</c> Bootstraps Dievas
    ///     
    /// </summary>
    public class Startup {

        /// <summary>
        ///     Application configuration for this Class
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        ///     Default constructor for Class <c>Startup/c>
        /// </summary>
        /// <param name="configuration">IConfiguration configuration informaiton</param>
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        /// <summary>
        ///     This method gets called by the runtime. Use this method to add
        ///     services to the container.
        /// </summary>
        /// <param name="services">IServiceCollection Application Servies List</param>
        public void ConfigureServices(IServiceCollection services) {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite(Configuration.GetConnectionString("DBConnection")));
            services.AddControllers();
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo { 
                                        Title = Configuration["AppName"],
                                        Version = "v1"
                                    });
            });
            services.AddSignalR();
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddSingleton<CAD>();

            //JWT Authentication
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
                options.TokenValidationParameters = new TokenValidationParameters {
                    ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["Jwt:Issuer"],
                        ValidAudience = Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                };
            });
        }

        /// <summary>
        ///     This method gets called by the runtime. Use this method to configure
        ///     the HTTP request pipeline.
        /// </summary>
        /// <param name="app">IApplicationBuilder factory to create this app</param>
        /// <param name="env">IWebHostEnvironment web host configuration</param>
        /// <param name="logger">Iloggger logging configuration</param> 
        public void Configure(IApplicationBuilder app, 
                              IWebHostEnvironment env,
                              ILogger<Startup> logger) {
            
            string _appDescription = Configuration["AppName"] + 
                                     " v" + 
                                     Configuration["Version"];

            if (env.IsDevelopment()) {
                logger.LogInformation("Using development pipeline.");
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => 
                    c.SwaggerEndpoint(Configuration["SwaggerEndpoint"],
                                      _appDescription));
            } else {
                logger.LogInformation("Using production pipeline.");
            }

            // Split the Origins string by semi-colin to allow multiple origins
            string[] _origins = Configuration["Origins"].Split(";");

            // We setup CORS to allow for our frontend to access this API
            app.UseCors(builder =>
                builder
                    .WithOrigins(_origins)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
            );

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
                endpoints.MapHub<DashboardHub>(Configuration["DashboardHubEndPoint"]);
            });
        }
    }
}
