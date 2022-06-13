using Amazon;
using Amazon.S3;
using CRM.ServiceCommon.Services.Files;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CRM.ServiceCommon.Configurations
{
    public static class S3Configuration
    {
        public static void ConfigureS3FileService(this IServiceCollection services,
            IConfiguration configuration)
        {
            var options = configuration.GetSection("S3Client");

            services.AddSingleton<IFileService>(new S3FileService(new AmazonS3Client(options.GetValue<string>("Key"),
                options.GetValue<string>("Secret"), new AmazonS3Config()
                {
                   ServiceURL = options.GetValue<string>("Host"),
                }), options.GetValue<string>("Bucket")));
        }
    }
}