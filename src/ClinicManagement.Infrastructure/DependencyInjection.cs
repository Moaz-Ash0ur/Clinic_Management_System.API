using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.EntityFrameworkCore;
using ClinicManagement.Infrastructure.BackgroundJobs;
using ClinicManagement.Infrastructure.Data;
using ClinicManagement.Infrastructure.Data.Interceptors;
using ClinicManagement.Infrastructure.Identity;
using ClinicManagement.Infrastructure.Services;
using ClinicManagement.Application.Common.Interfaces;
using ClinicManagement.Infrastructure.Repsitories;
using ClinicManagement.Infrastructure.Settings;
using ClinicManagement.Domain.Identity;


namespace ClinicManagement.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(TimeProvider.System);

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            ArgumentNullException.ThrowIfNull(connectionString);

            services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();

            services.AddDbContext<AppDbContext>((sp, options) =>
            {
                options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
                options.UseSqlServer(connectionString);
            });
          
            services.AddScoped<ApplicationDbContextInitialiser>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                var jwtSettings = configuration.GetSection("JwtSettings");

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                           Encoding.UTF8.GetBytes(jwtSettings["Secret"]!)),
                };
            });

            services.AddIdentityCore<AppUser>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequiredUniqueChars = 1;
                options.SignIn.RequireConfirmedAccount = false;
            }).AddRoles<IdentityRole>()
              .AddEntityFrameworkStores<AppDbContext>();



            services.AddHybridCache(options => options.DefaultEntryOptions = new HybridCacheEntryOptions
            {
                Expiration = TimeSpan.FromMinutes(10), 
                LocalCacheExpiration = TimeSpan.FromSeconds(30),
            });


            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<ITokenProvider, TokenProvider>();
            services.AddScoped<IAppointmentService, AppointmentService>();
            services.AddScoped<IPrescriptionPdfGenerator, PrescriptionPdfGenerator>();


            services.AddScoped<IIdentityService, IdentityService>();

            services.AddScoped<IAuthService>(sp =>
                sp.GetRequiredService<IIdentityService>());

            services.AddScoped<IUserManagementService>(sp =>
                sp.GetRequiredService<IIdentityService>());
         
            services.AddScoped<IPasswordService>(sp =>
                sp.GetRequiredService<IIdentityService>());

            services.AddScoped<IEmailConfirmationService>(sp =>
                sp.GetRequiredService<IIdentityService>());



            services.AddHostedService<ReminderForAppointment>();

            services.Configure<TwilioSettings>(configuration.GetSection("Twilio"));
            services.AddScoped<ISmsService, TwilioSmsService>();

            return services;
        }
    }
}
