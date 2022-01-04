using AYN.Web.Hubs;
using Microsoft.AspNetCore.Builder;

namespace AYN.Web.Infrastructure.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseEndpointss(this IApplicationBuilder app)
        => app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute("areaRoute", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
            endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            endpoints.MapHub<ChatHub>("/chat");
            endpoints.MapRazorPages();
        });
}
