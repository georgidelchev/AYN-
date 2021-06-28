using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(AYN.Web.Areas.Identity.IdentityHostingStartup))]

namespace AYN.Web.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}
