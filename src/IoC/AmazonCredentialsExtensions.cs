﻿namespace Linn.Stores2.IoC
{
    using Amazon;
    using Amazon.Runtime;

    using Linn.Common.Configuration;

    using Microsoft.Extensions.DependencyInjection;

    public static class AmazonCredentialsExtensions
    {
        public static IServiceCollection AddCredentialsExtensions(this IServiceCollection services)
        {
            return services
                .AddSingleton<AWSCredentials>(s => FallbackCredentialsFactory.GetCredentials())
                .AddSingleton<RegionEndpoint>(a => RegionEndpoint.GetBySystemName(AwsCredentialsConfiguration.Region));
        }
    }
}
