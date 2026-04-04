using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Shared.Mailing.Services;

namespace Shared.Mailing;

public static class Extensions
{
    public static IServiceCollection AddHeroMailing(this IServiceCollection services)
    {
        services.AddOptions<MailOptions>()
            .BindConfiguration(nameof(MailOptions))
            .ValidateOnStart();

        services.AddTransient<IMailService>(sp =>
        {
            var options = sp.GetRequiredService<IOptions<MailOptions>>().Value;
            if (options.UseSendGrid)
            {
                return new SendGridMailService(sp.GetRequiredService<IOptions<MailOptions>>());
            }
            return new SmtpMailService(sp.GetRequiredService<IOptions<MailOptions>>(), sp.GetRequiredService<Microsoft.Extensions.Logging.ILogger<SmtpMailService>>());
        });
        return services;
    }
}