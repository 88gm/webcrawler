using HtmlAgilityPack;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WebCrawler.Domain.ProxyPage;

namespace WebCrawler.Domain.Services
{
    public class ProxyPageService : IProxyPageService
    {
        private readonly IProxyPageRepository _proxyPageRepository;

        public ProxyPageService(IProxyPageRepository proxyPageRepository)
        {
            _proxyPageRepository = proxyPageRepository ?? throw new ArgumentNullException(nameof(proxyPageRepository));
        }

        public async Task ExtractProxyList()
        {
            try
            {
                var url = "https://proxyservers.pro/proxy/list/order/updated/order_dir/desc/page/1";

                var httpClient = new HttpClient();
                var html = await httpClient.GetStringAsync(url);

                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(html);

                var node = htmlDocument.DocumentNode.SelectSingleNode("//ul[@class='pagination justify-content-end']");
                var pages = node.Descendants("li").ToList().Count;

                for (var i = 1; i < pages; i++)
                {
                    // Pegar node <TABLE> onde class="table table-hover", e a partir daí, pegar os filhos <tr> pra poder ter as linhas de dados..
                    _proxyPageRepository.Insert(new ProxyPage.ProxyPage() { });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
