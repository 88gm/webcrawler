using WebCrawler.Domain.Config;
using Microsoft.Extensions.Configuration;
using System;

namespace WebCrawler.Data.Config
{
    public class DbConfig : IDbConfig
    {
        private readonly IConfiguration _configuration;

        public DbConfig(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public string ConnectionString()
        {
            return _configuration.GetValue<string>("ConnectionString");
        }
    }
}
