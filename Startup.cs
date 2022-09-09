using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dievas.Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace Dievas {

    public class Startup {

        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add
        //      services to the container.
        public void ConfigureServices(IServiceCollection services) {
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
        }

        // This method gets called by the runtime. Use this method to configure
        //  the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, 
                              IWebHostEnvironment env) {
            
            string _appDescription = Configuration["AppName"] + 
                                     " v" + 
                                     Configuration["Version"];

            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => 
                    c.SwaggerEndpoint(Configuration["SwaggerEndpoint"],
                                      _appDescription));
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

            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
                endpoints.MapHub<DashboardHub>("/dashboardHub");
            });
        }
    }
}
