using FactuLib.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FactuLib
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            //services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            //    .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddControllersWithViews();
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
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

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapAreaControllerRoute("Users","Users","{controller=Users}/{action=Users}/{id?}");
                endpoints.MapAreaControllerRoute("Principal", "Principal", "{controller=Principal}/{action=Principal}/{id?}");
                endpoints.MapAreaControllerRoute("Clientes", "Clientes", "{controller=Clientes}/{action=Clientes}/{id?}");
                endpoints.MapAreaControllerRoute("Proveedores", "Proveedores", "{controller=Proveedores}/{action=Proveedores}/{id?}");
                endpoints.MapAreaControllerRoute("Compras", "Compras", "{controller=Compras}/{action=Compras}/{id?}");
                endpoints.MapAreaControllerRoute("Productos", "Productos", "{controller=Productos}/{action=Productos}/{id?}");
                endpoints.MapAreaControllerRoute("Ventas", "Ventas", "{controller=Ventas}/{action=Ventas}/{id?}");
                endpoints.MapAreaControllerRoute("Cajas", "Cajas", "{controller=Cajas}/{action=Cajas}/{id?}");
                endpoints.MapRazorPages();
                
            });
            // Configuramos Rotativa indicándole el Path RELATIVO donde se
            // encuentran los archivos de la herramienta wkhtmltopdf.
            Rotativa.AspNetCore.RotativaConfiguration.Setup((Microsoft.AspNetCore.Hosting.IHostingEnvironment)env, "..\\Rotativa\\Windows\\");
        }
    }
}
