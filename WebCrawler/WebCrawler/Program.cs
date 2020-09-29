using HtmlAgilityPack;
using System;
using System.Linq;
using System.Net.Http;

namespace WebCrawler
{
    class Program
    {
        static void Main(string[] args)
        {
            StartCrawlerAsync();

            Console.ReadLine();
        }


        private static async void StartCrawlerAsync()
        {
            var url = "https://proxyservers.pro/proxy/list/order/updated/order_dir/desc/page/1";

            var httpClient = new HttpClient();
            var html = await httpClient.GetStringAsync(url);

            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            var node = htmlDocument.DocumentNode.SelectSingleNode("//ul[@class='pagination justify-content-end']");
            var pages = node.Descendants("li").ToList().Count;

            for(var i = 1; i < pages; i++)
            {
                // continuar daqui \/
                // Pegar node <TABLE> pela onde class="table table-hover", e a partir daí, pegar os filhos <tr> pra poder ter as linhas de dados..
            }
        }
    }
}
