using System;
using System.Collections.Generic;
using System.Text;

namespace WebCrawler.Domain.Config
{
    public interface IDbConfig
    {
        string ConnectionString();
    }
}
