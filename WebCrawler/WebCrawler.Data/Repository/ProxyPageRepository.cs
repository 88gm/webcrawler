using Dapper;
using System.Data;
using System.Data.SQLite;
using WebCrawler.Domain.Config;
using WebCrawler.Domain.ProxyPage;

namespace WebCrawler.Data.Repository
{
    public class ProxyPageRepository : IProxyPageRepository
    {
        private readonly IDbConfig _dbconfig;

        public ProxyPageRepository(IDbConfig dbconfig)
        {
            _dbconfig = dbconfig;
        }

        public void Insert(ProxyPage item)
        {
            var sql = @"INSERT INTO ProxyPage 
                        (
                            Id, 
                            DateBegin, 
                            DateEnd, 
                            PageQty, 
                            LineQty, 
                            JsonFile
                        )
                        VALUES
                        (
                            @Id,
                            @DateBegin,
                            @DateEnd,
                            @PageQty,
                            @LineQty,
                            @JsonFile
                        );";

            using (IDbConnection cnn = new SQLiteConnection(_dbconfig.ConnectionString()))
            {
                cnn.Execute(sql, item);
            }
        }
    }
}
