namespace Linn.Stores2.IoC
{
    using Amazon;
    using Amazon.Runtime;
    using Amazon.Runtime.Credentials;

    using Linn.Common.Configuration;

    using Microsoft.Extensions.DependencyInjection;

    public static class AmazonCredentialsExtensions
    {
        public static IServiceCollection AddCredentialsExtensions(this IServiceCollection services)
        {
#if DEBUG
            AWSConfigs.AWSProfileName = "mfa";
#endif

            var regionName = ConfigurationManager.Configuration["awsRegion"]
                             ?? ConfigurationManager.Configuration["AWS_REGION"];

            return services
                .AddSingleton<AWSCredentials>(s => DefaultAWSCredentialsIdentityResolver.GetCredentials())
                .AddSingleton<RegionEndpoint>(a => RegionEndpoint.GetBySystemName(regionName));
        }
    }
}
