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
    using Microsoft.AspNetCore.Authentication.Cookies;
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

            //https://stackoverflow.com/questions/50628262/asp-net-core-2-0-identity-twofactorrememberme-expiry
            //this sets the cookie for the RememberMe login
            //services.Configure<CookieAuthenticationOptions>
            //(IdentityConstants.TwoFactorRememberMeScheme, options =>
            //{
            //    //this will override the default 14 day expire time
            //    options.ExpireTimeSpan = TimeSpan.FromDays(30);
            //});


            //todo Commented this out to check for google auth uncomment it
            //services.AddAuthentication().AddGoogle(googleOptions =>
            //{
            //    googleOptions.ClientId = Configuration["Authentication:Google:ClientId"];
            //    googleOptions.ClientSecret = Configuration["Authentication:Google:ClientSecret"];
            //});

            services.AddTransient<ISessions, Sessions>();

            //services.AddDbContext<ApplicationDbContext>(options =>
            //    options.UseSqlServer(
            //        Configuration.GetConnectionString("BatAdmin")));

            // services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(Configuration.GetConnectionString("BatAdmin")));

            //temp as db won't connect
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite("Data Source= BatAdmin.db"));

            services.AddDbContext<SeatBookingDBContext>(options => options.UseSqlite("Data Source= Seating.db"));

            // services.AddDbContext<SeatBookingDBContext>(options => options.UseSqlite(Configuration.GetConnectionString("Seating")));

            //services.AddDefaultIdentity<IdentityUser>()
            //    .AddRoles<IdentityRole>()
            //    .AddEntityFrameworkStores<ApplicationDbContext>();

            //https://scottsauber.com/2018/07/07/walkthrough-creating-an-html-email-template-with-razor-and-razor-class-libraries-and-rendering-it-from-a-net-standard-class-library/

            //services.AddScoped<IRegisterAccountService, RegisterAccountService>();
            //services.AddScoped<IRazorViewToStringRenderer, RazorViewToStringRenderer>();

            //https://www.dotnettricks.com/learn/aspnetcore/authentication-authentication-aspnet-identity-example 

            //Identity Membership system allow us to defined role for the user and with the help of user role, we can identify whether user has privilege to access the page or not.
            //With default template, only UserManager class of Identity service is available but to do the role-based authentication, RoleManager class is also required.
            //Both the classes available together by using “AddIdentity” method that adds the identity configuration for specific role and user.

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddRoles<IdentityRole>()
                   .AddEntityFrameworkStores<ApplicationDbContext>();

            //https://docs.microsoft.com/en-us/aspnet/core/fundamentals/app-state?view=aspnetcore-2.2
            services.AddDistributedMemoryCache();


            // The AddAuthentication() and AddCookie() methods register cookie authentication service with the framework. Notice that AddAuthentication() accepts a string parameter indicating name of the security scheme. This can be any developer defined value or you can use the default as indicated by AuthenticationScheme property of CookieAuthenticationDefaults class.
            //http://www.binaryintellect.net/articles/9780ad51-20f6-48f3-989e-7c6511a44810.aspx
            //  services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            //      .AddCookie();
            //ABOVE KILLS THE LOGIN

            services.AddSession(options =>
                {
                    // Set a short timeout for easy testing. 1 hour
                    options.Cookie.Name = ".NZBat";
                    options.IdleTimeout = TimeSpan.FromMinutes(60);
                    options.Cookie.HttpOnly = true;
                });





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
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(60);
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Lockout.AllowedForNewUsers = true;

                // User settings
                options.User.RequireUniqueEmail = true;

                //Token Option
                options.Tokens.AuthenticatorTokenProvider = "Name of AuthenticatorTokenProvider";
            });

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                // options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
                options.Cookie.Expiration = TimeSpan.FromDays(150);
                options.LoginPath = "/Identity/Account/Login";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                options.SlidingExpiration = true;
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);


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
            app.UseSession(); //for session cookies
            app.UseAuthentication(); //for Identity and for cookies

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                   template: "{controller=SeatBookings}/{action=Create}/{id?}"
                     );

            });
            CreateUsersAndRoles(serviceProvider).Wait();
        }


        //https://www.dotnettricks.com/learn/aspnetcore/authentication-authentication-aspnet-identity-example

        private async Task CreateUsersAndRoles(IServiceProvider serviceProvider)
        {
            //initializing custom roles 
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            //a list of roles
            string[] roleNames = { "Admin", "User" };


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
                    IdentityResult roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
            //===============================================
            //assign a user to admin when they log in

            //look for a user - add in your users email address for when they log in

            Dictionary<string, string> AdminUsers = new Dictionary<string, string>();

            AdminUsers.Add("test@gmail.com", "testtest");
            AdminUsers.Add("Sarah.Chalmers@visioncollege.ac.nz", "SarahChalmers");

            foreach (var person in AdminUsers)
            {//does the person exist in the db?
                IdentityUser user = await UserManager.FindByEmailAsync(person.Key);

                //nope she isn't there so make her name and add to admin db
                if (user == null)
                {
                    //create a new user 
                    user = new IdentityUser()
                    {
                        UserName = person.Key,
                        Email = person.Key,
                    };

                    //add the user and the password as a string to the db
                    await UserManager.CreateAsync(user, person.Value);
                }

                //add a role of admin to Sarah change admin to other roles
                await UserManager.AddToRoleAsync(user, "Admin");
                //end adding using roles, you can loop through all members and assign if you want


            }

        }

        //string AdminUser = "Sarah.Chalmers@visioncollege.ac.nz";
        //           string AdminPW = "SarahChalmers";
        //IdentityUser user = await UserManager.FindByEmailAsync(AdminUser);
        //    //nope she isn't there so make her name and add to admin db
        //    if (user == null)
        //    {
        //        //create a new user 
        //        user = new IdentityUser()
        //        {
        //            UserName = AdminUser,
        //            Email = AdminUser,
        //        };

        //        //add the user and the password as a string to the db
        //        await UserManager.CreateAsync(user, AdminPW);
        //    }
        //    //add a role of admin to Sarah change admin to other roles
        //    await UserManager.AddToRoleAsync(user, "Admin");
        //    //end adding using roles, you can loop through all members and assign if you want
        //}

    }
}
