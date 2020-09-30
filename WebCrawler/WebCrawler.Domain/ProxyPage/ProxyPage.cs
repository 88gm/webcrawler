using System;

namespace WebCrawler.Domain.ProxyPage
{
    public class ProxyPage
    {
        public Guid Id { get; set; }
        public DateTime DateBegin { get; set; }
        public DateTime DateEnd { get; set; }
        public int PageQty { get; set; }
        public int LineQty { get; set; }
        public string JsonFile { get; set; }
    }
}
