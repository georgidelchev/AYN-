using AYN.Data;
using AYN.Data.Seeding;
using AYN.Web.Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AYN.Web.Infrastructure.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseEndpoints(this IApplicationBuilder app)
        => app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute("areaRoute", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
            endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            endpoints.MapHub<ChatHub>("/chat");
            endpoints.MapRazorPages();
        });

    public static IApplicationBuilder PrepareDatabase(this IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();

        var dbContext = serviceScope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

        MigrateDatabase(dbContext);
        SetupSeeding(dbContext, serviceScope);

        return app;
    }

    private static void MigrateDatabase(ApplicationDbContext dbContext)
        => dbContext.Database.Migrate();

    private static void SetupSeeding(ApplicationDbContext dbContext, IServiceScope serviceScope)
        => new ApplicationDbContextSeederBase()
            .SeedAsync(dbContext, serviceScope.ServiceProvider)
            .GetAwaiter()
            .GetResult();
}
