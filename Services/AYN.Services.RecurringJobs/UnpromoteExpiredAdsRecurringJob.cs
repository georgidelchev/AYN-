using AYN.Data;
using Hangfire;
using Hangfire.Console;
using Hangfire.Server;

namespace AYN.Services.RecurringJobs;

public class UnpromoteExpiredAdsRecurringJob
{
    private readonly ApplicationDbContext dbContext;

    public UnpromoteExpiredAdsRecurringJob(
        ApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    [AutomaticRetry(Attempts = 2)]
    public async Task StartWorking(PerformContext context)
    {
        var expiredAds = this.dbContext
            .Ads
            .Where(a => a.IsPromoted &&
                        a.PromotedUntil <= DateTime.Now)
            .ToList();

        foreach (var expiredAd in expiredAds.WithProgress(context.WriteProgressBar()))
        {
            expiredAd.IsPromoted = false;
            expiredAd.PromotedOn = null;
            expiredAd.PromotedUntil = null;

            this.dbContext.Update(expiredAd);
            await this.dbContext.SaveChangesAsync();

            context.WriteLine($"Successfully un-promoted ad with title: {expiredAd.Name}");
        }
    }
}