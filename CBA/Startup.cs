using CBA.Data;
using CBA.Models;
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

namespace CBA
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

            services.AddDefaultIdentity<CBAUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<CBARole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultUI()
                .AddDefaultTokenProviders();
            services.AddControllersWithViews();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("CBA001", policy => policy.RequireClaim("CBA001"));
                options.AddPolicy("CBA002", policy => policy.RequireClaim("CBA002"));
                options.AddPolicy("CBA003", policy => policy.RequireClaim("CBA003"));
                options.AddPolicy("CBA004", policy => policy.RequireClaim("CBA004"));
                options.AddPolicy("CBA005", policy => policy.RequireClaim("CBA005"));
                options.AddPolicy("CBA006", policy => policy.RequireClaim("CBA006"));
                options.AddPolicy("CBA007", policy => policy.RequireClaim("CBA007"));
                options.AddPolicy("CBA008", policy => policy.RequireClaim("CBA008"));
                options.AddPolicy("CBA009", policy => policy.RequireClaim("CBA009"));
                options.AddPolicy("CBA010", policy => policy.RequireClaim("CBA010"));
                options.AddPolicy("CBA011", policy => policy.RequireClaim("CBA011"));
                options.AddPolicy("CBA012", policy => policy.RequireClaim("CBA012"));
                options.AddPolicy("CBA013", policy => policy.RequireClaim("CBA013"));
                options.AddPolicy("CBA014", policy => policy.RequireClaim("CBA014"));
                options.AddPolicy("CBA015", policy => policy.RequireClaim("CBA015"));
                options.AddPolicy("CBA016", policy => policy.RequireClaim("CBA016"));
                options.AddPolicy("CBA017", policy => policy.RequireClaim("CBA017"));
                options.AddPolicy("CBA018", policy => policy.RequireClaim("CBA018"));
                options.AddPolicy("CBA019", policy => policy.RequireClaim("CBA019"));
                options.AddPolicy("CBA020", policy => policy.RequireClaim("CBA020"));
                options.AddPolicy("CBA021", policy => policy.RequireClaim("CBA021"));
                options.AddPolicy("CBA022", policy => policy.RequireClaim("CBA022"));
                options.AddPolicy("CBA023", policy => policy.RequireClaim("CBA023"));
                options.AddPolicy("CBA024", policy => policy.RequireClaim("CBA024"));
                options.AddPolicy("CBA025", policy => policy.RequireClaim("CBA025"));
                options.AddPolicy("CBA026", policy => policy.RequireClaim("CBA026"));
                options.AddPolicy("CBA027", policy => policy.RequireClaim("CBA027"));
                options.AddPolicy("CBA028", policy => policy.RequireClaim("CBA028"));
                options.AddPolicy("CBA029", policy => policy.RequireClaim("CBA029"));
                options.AddPolicy("CBA030", policy => policy.RequireClaim("CBA030"));
                options.AddPolicy("CBA031", policy => policy.RequireClaim("CBA031"));
                options.AddPolicy("CBA032", policy => policy.RequireClaim("CBA032"));
            });
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
                endpoints.MapRazorPages();
            });
        }
    }
}
