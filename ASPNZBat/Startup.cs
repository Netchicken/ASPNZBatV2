using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ASPNZBat.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using ASPNZBat.Business;
using Microsoft.AspNetCore.Identity.UI.Services;
using IEmailSender = ASPNZBat.Business.IEmailSender;

namespace ASPNZBat
{
    using Business.ICal;
    using DTO;
    using Microsoft.Extensions.Logging;
    //using RazorHtmlEmails.Common;
    //using RazorHtmlEmails.RazorClassLib.Services;

    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }



        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.Configure<RequestLocalizationOptions>(options =>
            {
                //set the culture for New Zealand for the whole project this might not be working, check it out by commenting it out.
                var cultureInfo = new CultureInfo("en-NZ");
                CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
                CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
                options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("en-NZ");
                // options.SupportedCultures = new List<CultureInfo> { new CultureInfo("en-NZ") };
                //options.SupportedCultures = (IList<CultureInfo>)cultureInfo;
                //options.SupportedUICultures = (IList<CultureInfo>)cultureInfo;
            });



            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddAuthentication().AddGoogle(googleOptions =>
            {
                googleOptions.ClientId = Configuration["Authentication:Google:ClientId"];
                googleOptions.ClientSecret = Configuration["Authentication:Google:ClientSecret"];
            });

            services.AddTransient<ISessions, Sessions>();

            //services.AddDbContext<ApplicationDbContext>(options =>
            //    options.UseSqlServer(
            //        Configuration.GetConnectionString("BatAdmin")));

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(Configuration.GetConnectionString("BatAdmin")));

            services.AddDbContext<SeatBookingDBContext>(options =>
                options.UseSqlite(Configuration.GetConnectionString("Seating")));

            services.AddDefaultIdentity<IdentityUser>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            //https://scottsauber.com/2018/07/07/walkthrough-creating-an-html-email-template-with-razor-and-razor-class-libraries-and-rendering-it-from-a-net-standard-class-library/

            //services.AddScoped<IRegisterAccountService, RegisterAccountService>();
            //services.AddScoped<IRazorViewToStringRenderer, RazorViewToStringRenderer>();




            //https://www.dotnettricks.com/learn/aspnetcore/authentication-authentication-aspnet-identity-example
            //   services.AddIdentity<IdentityUser, IdentityRole>()
            //       .AddEntityFrameworkStores<ApplicationDbContext>();


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // https://docs.microsoft.com/en-us/aspnet/core/security/authentication/accconfirm?view=aspnetcore-2.1&tabs=visual-studio
            // using Microsoft.AspNetCore.Identity.UI.Services;
            // using WebPWrecover.Services;


            //  services.AddTransient<IEmailSender, DevEmailSender>();
            services.AddTransient<IOverdueStudents, OverdueStudents>();
            services.AddSingleton<IDBCallsSessionDataDTO, DbCallsSessionDataDto>();
            services.AddTransient<ICalService, CalService>();
            services.AddTransient<IGenerateCalendarEventsForControllers, GenerateCalendarEventsForControllers>();


            services.AddTransient<IEmailSender, EmailSender>();
            services.Configure<AuthMessageSenderOptions>(Configuration);

            services.AddTransient<IStudentNameDTO, StudentNameDTO>();
            services.AddTransient<IAddUserToStudentTable, AddUserToStudentTable>();


            //Change the password requirements of the login system
            //https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity?view=aspnetcore-2.1&tabs=visual-studio%2Caspnetcore2x
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 2;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredUniqueChars = 0;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Lockout.AllowedForNewUsers = true;

                // User settings
                options.User.RequireUniqueEmail = true;
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseRequestLocalization(); //use NZ localization from above
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                   template: "{controller=SeatBookings}/{action=Create}/{id?}"
                     );

            });
            //    CreateUsersAndRoles(serviceProvider).Wait();
        }


        //https://www.dotnettricks.com/learn/aspnetcore/authentication-authentication-aspnet-identity-example

        private async Task CreateUsersAndRoles(IServiceProvider serviceProvider)
        {
            //initializing custom roles 
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            //a list of roles
            string[] roleNames = { "Admin", "User" };
            IdentityResult roleResult;

            //===============================================
            //hard code in some rolls

            //loop through them and send them to the db
            foreach (var roleName in roleNames)
            {
                //check if the role exists
                var roleExist = await RoleManager.RoleExistsAsync(roleName);
                //if not add it
                if (!roleExist)
                {
                    //create the roles and seed them to the database:
                    roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
            //===============================================
            //assign a user to admin when they log in

            //look for a user - add in your users email address for when they log in

            string AdminUser = "Sarah.Chalmers@visioncollege.ac.nz";
            string AdminPW = "SarahChalmers";

            IdentityUser user = await UserManager.FindByEmailAsync(AdminUser);
            //nope she isn't there so make her name and add to admin db
            if (user == null)
            {
                //creae a new user 
                user = new IdentityUser()
                {
                    UserName = AdminUser,
                    Email = AdminUser,
                };

                //add the user and the password as a string to the db
                await UserManager.CreateAsync(user, AdminPW);
            }
            //add a role of admin to Sarah change admin to other roles
            await UserManager.AddToRoleAsync(user, "Admin");
            //end adding using roles, you can loop through all members and assign if you want
        }

    }
}
