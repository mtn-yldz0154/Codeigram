using BitirmeProjesiUI.Identity;
using BuisnessLayer.Abstract;
using BuisnessLayer.Concrete;
using DataAcsessLayer.Abstract;
using DataAcsessLayer.Concrete.EFCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BitirmeProjesiUI.Services
{
    public static class StartupService
    {
        public static void AddService(this IServiceCollection services)
        {
            services.AddDbContext<ApplicationContext>(options=>options.UseSqlServer(ConfigureService.ConnectionString));
            services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<ApplicationContext>().AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 6;

                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromDays(1);
                options.Lockout.AllowedForNewUsers = true;

                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;

            });

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/account/login";
                options.LogoutPath = "/account/logout";
                options.AccessDeniedPath = "/account/accessdenied";
                options.SlidingExpiration = true;
                options.ExpireTimeSpan = TimeSpan.FromDays(1);
                options.Cookie = new CookieBuilder
                {
                    HttpOnly = true,
                    Name = ".Metin.Security.Cookie",
                    SameSite = SameSiteMode.Strict
                };
            });


			services.AddScoped<IFollowService, FollowManager>();
			services.AddScoped<INotificationService, NotificationManager>();


			services.AddScoped<IFollowRepository, EFCoreFollowRepository>();
			services.AddScoped<INotificationRepository, EFCoreNotificationRepository>();


		}
    }
}
