using System;
using System.Reflection;

using AYN.Data;
using AYN.Data.Common;
using AYN.Data.Common.Repositories;
using AYN.Data.Models;
using AYN.Data.Repositories;
using AYN.Services.Mapping;
using AYN.Services.Messaging;
using AYN.Services.RecurringJobs;
using AYN.Web.Infrastructure.Extensions;
using AYN.Web.Validators;
using AYN.Web.ViewModels;
using AYN.Web.ViewModels.Ads;
using CloudinaryDotNet;
using Hangfire;
using Hangfire.Console;
using Hangfire.Dashboard;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetCore.AutoRegisterDi;
using Stripe;

namespace AYN.Web;

public class Startup
{
    private readonly IConfiguration configuration;

    public Startup(
        IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(this.configuration.GetConnectionString("DefaultConnection"));
        });

        services.AddDefaultIdentity<ApplicationUser>(IdentityOptionsProvider.GetIdentityOptions)
            .AddRoles<ApplicationRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

        var cloudinaryCredentials = new CloudinaryDotNet.Account(
            this.configuration["Cloudinary:CloudName"],
            this.configuration["Cloudinary:ApiKey"],
            this.configuration["Cloudinary:ApiSecret"]);

        var cloudinaryUtility = new Cloudinary(cloudinaryCredentials);

        services.Configure<CookiePolicyOptions>(options =>
        {
            options.CheckConsentNeeded = _ => true;
            options.MinimumSameSitePolicy = SameSiteMode.None;
        });

        services.AddControllersWithViews(options =>
        {
            options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
        }).AddRazorRuntimeCompilation();

        services.AddRazorPages();

        services.AddAntiforgery(options =>
        {
            options.HeaderName = "X-CSRF-TOKEN";
        });

        services.AddDatabaseDeveloperPageExceptionFilter();

        services.AddSingleton(cloudinaryUtility);
        services.AddSingleton(this.configuration);

        services.AddHangfire(config => config
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSqlServerStorage(
                this.configuration.GetConnectionString("DefaultConnection"), new SqlServerStorageOptions
                {
                    UseRecommendedIsolationLevel = true,
                    UsePageLocksOnDequeue = true,
                    DisableGlobalLocks = true,
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                }).UseConsole())
            .AddHangfireServer(c => c.WorkerCount = 2);

        // Data repositories
        services.AddScoped<IDbQueryRunner, DbQueryRunner>();
        services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
        services.AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>));
        services.AddSignalR();

        // Application services
        services.RegisterAssemblyPublicNonGenericClasses(Assembly.Load("AYN.Services.Data"))
            .Where(s => !s.IsAbstract &&
                        s.Name.EndsWith("Service"))
            .AsPublicImplementedInterfaces();

        services.AddTransient<IValidator<CreateAdInputModel>, CreateAdValidator>();
        services.AddTransient<IEmailSender>(sp => new SendGridEmailSender(this.configuration["Sendgrid:ApiKey"]));
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IRecurringJobManager recurringJobManager)
    {
        AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

        StripeConfiguration.ApiKey = this.configuration["Stripe:SecretKey"];

        // Seed data on application startup
        app.PrepareDatabase();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        this.LoadAllHangfireRecurringJobs(recurringJobManager);

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseCookiePolicy();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseHangfireDashboard("/hangfire", new DashboardOptions { Authorization = new[] { new HangfireAuthFilter() } });

        app.UseEndpoints();
    }

    private void LoadAllHangfireRecurringJobs(IRecurringJobManager recurringJobManager)
    {
        recurringJobManager.AddOrUpdate<UnpromoteExpiredAdsRecurringJob>(
            "Ad un-promoter",
            x => x.StartWorking(null),
            "*/59 * * * *");
    }

    private class HangfireAuthFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext dashboardContext)
            => dashboardContext.GetHttpContext().User.IsAdmin();
    }
}
