using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Server.Game;
using Server.Services;

namespace Server
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
            services.AddCors();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            
            // add some values while we don't have any persistence yet
            var locations = new[]
            {
                new Location("Home", new Coordinates(2,2))
                {
                    LocationFeatures = new[] {LocationFeature.Bed}
                },
                new Location("Restaurant", new Coordinates(4,4))
                {
                    LocationFeatures = new[] {LocationFeature.Table}
                },
                new Location("Playground", new Coordinates(6,2))
                {
                    LocationFeatures = new[] {LocationFeature.Playground}
                },
            };
            
            var hanses = new[]
            {
                new Hans("Peter")
                {
                    Location = locations[0]
                },
                new Hans("Rudolf")
                {
                    Location = locations[1]
                }
            };


            services.AddSingleton<IGameService>(new GameService
            {
                Hanses = hanses,
                Locations = locations
            });
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
                app.UseHttpsRedirection();
            }

            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseMvc();
        }
        
    }
}