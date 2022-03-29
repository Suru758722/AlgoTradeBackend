using DotNetify;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using SudhirTest.Data;
using SudhirTest.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudhirTest
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
            
            services.AddHttpClient<IMarketService, MarketService>();
            services.AddScoped<ILiveChartService, LiveChartService>();
            services.AddScoped<IAnalysisService, AnalysisService>();
            services.AddHttpClient<IOptionService, OptionService>();
            //        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //.AddJwtBearer(options =>
            //{
            //    options.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidateIssuer = true,
            //        ValidateAudience = true,
            //        ValidateLifetime = true,
            //        ValidateIssuerSigningKey = true,
            //        ValidIssuer = Configuration["Jwt:Issuer"],
            //        ValidAudience = Configuration["Jwt:Issuer"],
            //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
            //    };
            //});
            services.AddRazorPages();
            services.AddCors();
            services.AddSignalR();
            services.AddDotNetify();
            //services.AddDbContext<ApplicationDbContext>(item => item.UseNpgsql(Configuration.GetConnectionString("connection")));
           
             services.AddDbContextFactory<ApplicationDbContext>(options =>
             options.UseNpgsql(Configuration.GetConnectionString("connection")),
                 ServiceLifetime.Scoped);

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
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseCors(options =>
            options.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader().AllowCredentials());
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

           // app.UseAuthorization();
           // app.UseAuthentication();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
            app.UseCors("CorsPolicy");
            app.UseWebSockets();
            app.UseDotNetify();

            app.UseRouting();
            //app.UseDotNetify(config => {

            //    config.RegisterAssembly(/* name of the assembly where the view model classes are located */);
            //    config.SetFactoryMethod((type, args) => /* let your favorite IoC library creates the view model instance */);
            //});
            app.UseEndpoints(endpoints => endpoints.MapHub<DotNetifyHub>("/dotnetify", options =>
            {
                options.Transports =
                    HttpTransportType.WebSockets;
                    //HttpTransportType.LongPolling;
            }));
        }
    }
}
