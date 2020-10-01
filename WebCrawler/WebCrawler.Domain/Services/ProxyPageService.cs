using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
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
                //Instancio variável proxyPage para inserir dados extraídos no banco.
                var proxyPage = new ProxyPage.ProxyPage();
                proxyPage.DateBegin = DateTime.Now;

                //url inicial a ser utilizada
                var url = "https://proxyservers.pro/proxy/list/order/updated/order_dir/desc/";

                //instancio cliente para baixar o html
                var httpClient = new HttpClient();
                var html = await httpClient.GetStringAsync(url);

                //serialização do HTML com HtmlAgilityPack pra facilitar as extrações
                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(html);

                //Utilizo xpath e alguns métodos do HtmlAgilityPack pra contabilizar o total de páginas da lista de proxys
                var node = htmlDocument.DocumentNode.SelectSingleNode("//ul[@class='pagination justify-content-end']");
                var pages = node.Descendants("li").ToList().Count;
                proxyPage.PageQty = pages;

                //Instancio a minha lista de proxys
                var proxyList = new List<Proxy.Proxy>();

                for (var i = 1; i <= pages;)
                {
                    //Salva print (arquivo html) da página [i]
                    File.WriteAllText($"{Directory.GetCurrentDirectory()}/Prints_Pagina{i}.html", htmlDocument.ParsedText);

                    //Pego todas as linhas da lista de proxys na página [i]
                    var rows = htmlDocument.DocumentNode.SelectNodes("//tbody/tr[@valign='top']").ToList();
                    proxyPage.LineQty += rows.Count();

                    foreach(var row in rows)
                    {
                        //instancio o proxy e preencho todos os campos com os dados do item atual do foreach
                        var proxy = new Proxy.Proxy();

                        proxy.IpAdress = row.SelectSingleNode("td[2]/a[1]").InnerText;
                        proxy.Port = row.SelectSingleNode("td[3]/span").InnerText;
                        var rawCountry = row.SelectSingleNode("td[4]").InnerText;
                        proxy.Country = Regex.Replace(rawCountry, @"\s+", string.Empty);
                        proxy.Protocol = row.SelectSingleNode("td[7]").InnerText;
                        
                        proxyList.Add(proxy);
                    }

                    //pego e serializo o HTML da próxima página, atualizando o índice [i] do For
                    html = await httpClient.GetStringAsync($"{url}/page/{i++}");
                    htmlDocument.LoadHtml(html);
                }

                //Serializo a lista pra uma string json e salvo como arquivo no diretorio root
                var jsonFile = JsonConvert.SerializeObject(proxyList);
                File.WriteAllText($"{Directory.GetCurrentDirectory()}/proxyList.json", jsonFile);
                proxyPage.JsonFile = jsonFile;

                proxyPage.Id = Guid.NewGuid();
                proxyPage.DateEnd = DateTime.Now;
                //finalizo inserindo dados da extração no banco
                _proxyPageRepository.Insert(proxyPage);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
