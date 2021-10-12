using System.Reflection;

using AYN.Data;
using AYN.Data.Common;
using AYN.Data.Common.Repositories;
using AYN.Data.Models;
using AYN.Data.Repositories;
using AYN.Data.Seeding;
using AYN.Services.Data.Implementations;
using AYN.Services.Data.Interfaces;
using AYN.Services.Mapping;
using AYN.Services.Messaging;
using AYN.Web.Hubs;
using AYN.Web.Validators;
using AYN.Web.ViewModels;
using AYN.Web.ViewModels.Ads;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Stripe;

namespace AYN.Web
{
    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(
                            options => options.UseSqlServer(this.configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<ApplicationUser>(IdentityOptionsProvider.GetIdentityOptions)
                .AddRoles<ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            var cloudinaryCredentials = new CloudinaryDotNet.Account(
                this.configuration["Cloudinary:CloudName"],
                this.configuration["Cloudinary:ApiKey"],
                this.configuration["Cloudinary:ApiSecret"]);

            var cloudinaryUtility = new Cloudinary(cloudinaryCredentials);

            services.Configure<CookiePolicyOptions>(
                options =>
                    {
                        options.CheckConsentNeeded = context => true;
                        options.MinimumSameSitePolicy = SameSiteMode.None;
                    });

            services.AddControllersWithViews(
                options =>
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

            // Data repositories
            services.AddScoped<IDbQueryRunner, DbQueryRunner>();
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>));
            services.AddSignalR();

            // Application services
            services.AddTransient<IAdsService, AdsService>()
                .AddTransient<ITownsService, TownsService>()
                .AddTransient<IUsersService, UsersService>()
                .AddTransient<IPostsService, PostsService>()
                .AddTransient<IEmojisService, EmojisService>()
                .AddTransient<IReportsService, ReportsService>()
                .AddTransient<IEmailSender, NullMessageSender>()
                .AddTransient<ISettingsService, SettingsService>()
                .AddTransient<IFeedbackService, FeedbackService>()
                .AddTransient<ICommentsService, CommentsService>()
                .AddTransient<IMessagesService, MessagesService>()
                .AddTransient<IAddressesService, AddressesService>()
                .AddTransient<IWishlistsService, WishlistsService>()
                .AddTransient<ICategoriesService, CategoriesService>()
                .AddTransient<IPostReactsService, PostReactsService>()
                .AddTransient<ICloudinaryService, CloudinaryService>()
                .AddTransient<IUserAdsViewsService, UserAdsViewsService>()
                .AddTransient<ISubCategoriesService, SubCategoriesService>()
                .AddTransient<INotificationsService, NotificationsService>()
                .AddTransient<IWordsBlacklistService, WordsBlacklistService>()
                .AddTransient<IValidator<CreateAdInputModel>, CreateAdValidator>()
                .AddTransient<IUserLatestAdViewsService, UserLatestAdViewsService>()
                .AddTransient<IEmailSender>(sp => new SendGridEmailSender(this.configuration["Sendgrid"]));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            StripeConfiguration.SetApiKey(this.configuration["Stripe:SecretKey"]);

            // Seed data on application startup
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider
                    .GetRequiredService<ApplicationDbContext>();

                dbContext.Database.Migrate();

                new ApplicationDbContextSeeder()
                    .SeedAsync(dbContext, serviceScope.ServiceProvider)
                    .GetAwaiter()
                    .GetResult();
            }

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

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(
                endpoints =>
                    {
                        endpoints.MapControllerRoute("areaRoute", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                        endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
                        endpoints.MapHub<ChatHub>("/chat");
                        endpoints.MapRazorPages();
                    });
        }
    }
}
