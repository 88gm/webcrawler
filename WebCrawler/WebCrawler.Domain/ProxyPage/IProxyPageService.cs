using System.Threading.Tasks;

namespace WebCrawler.Domain.ProxyPage
{
    public interface IProxyPageService
    {
        Task ExtractProxyList();
    }
}