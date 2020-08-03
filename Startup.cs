using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Products.Models;

namespace Products
{
    public class Startup
    {
        public Startup(IConfiguration configuration/*Microsoft.AspNetCore.Hosting.IHostingEnvironment env*/)
        {
            Configuration = configuration;
          /*  var builder = new ConfigurationBuilder()
            .SetBasePath(env.ContentRootPath)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            Configuration = builder.Build();*/
        }

        public static IConfiguration Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {            
            services.AddMvc().SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_3_0);
            //string ConnectionStrings = @"Server=DESKTOP-ICBGB16;Database=ProductsDatabaseData;User ID=;Password=;Trusted_Connection=True;ConnectRetryCount=0";
            //services.AddDbContext<ProductsDbContext>(options => options.UseSqlServer(ConnectionStrings));


            // Add functionality to inject IOptions<T>
            //services.AddOptions();


            //services.Configure<ProductsDbContext>(Configuration.GetSection("ConnectionString"));

            services.AddSingleton(Configuration.GetSection("ConnectionString").Get<ProductsDbContext>());
        }
       /* public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            //services.AddRazorPages().AddRazorRuntimeCompilation();

            string connectionString = GetConnectionStringFromConfig();

            services.AddEntityFrameworkSqlServer()
                .AddDbContext<ProductsDbContext>(o => o
                .UseSqlServer(connectionString)
                .UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll)
                .EnableSensitiveDataLogging(true), ServiceLifetime.Scoped);
        }

        public static string GetConnectionStringFromConfig()
        {
            byte[] data = Convert.FromBase64String(Configuration.GetSection("ConnectionString").Value);
            string connectionString = System.Text.UTF8Encoding.UTF8.GetString(data);
            return connectionString;
        }*/

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
