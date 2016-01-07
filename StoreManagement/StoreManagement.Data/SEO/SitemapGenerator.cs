using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace StoreManagement.Data.SEO
{
    /// <summary>
    /// A class for creating XML Sitemaps (see http://www.sitemaps.org/protocol.html)
    /// </summary>
    public class SitemapGenerator : ISitemapGenerator
    {
        private static readonly XNamespace xmlns = "http://www.sitemaps.org/schemas/sitemap/0.9";
        private static readonly XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";
        private static readonly XNamespace newsXsi = "http://www.google.com/schemas/sitemap-news/0.9";

        public XDocument GenerateSiteMap(IEnumerable<ISitemapItem> items)
        {
            //   Ensure.Argument.NotNull(items, "items");

            var sitemap = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                    new XElement(xmlns + "urlset",
                      new XAttribute("xmlns", xmlns),
                      new XAttribute(XNamespace.Xmlns + "xsi", xsi),
                      new XAttribute(xsi + "schemaLocation", "http://www.sitemaps.org/schemas/sitemap/0.9 http://www.sitemaps.org/schemas/sitemap/0.9/sitemap.xsd"),
                      from item in items
                      select CreateItemElement(item)
                      )
                 );

            return sitemap;
        }

        public virtual XDocument GenerateNewsSiteMap(IEnumerable<ISitemapItem> items)
        {
            var sitemap = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                    new XElement(xmlns + "urlset",
                      new XAttribute("xmlns", xmlns),
                      new XAttribute(XNamespace.Xmlns + "news", newsXsi),
                //new XAttribute(XNamespace.Xmlns + "xsi", xsi),
                //new XAttribute(xsi + "schemaLocation", "http://www.sitemaps.org/schemas/sitemap/0.9 http://www.sitemaps.org/schemas/sitemap/0.9/sitemap.xsd"),
                      from item in items
                      select CreateNewsItemElement(item)
                      )
                 );

            return sitemap;
        }

        private XElement CreateItemElement(ISitemapItem item)
        {
            var itemElement = new XElement(xmlns + "url", new XElement(xmlns + "loc", item.Url.ToLowerInvariant()));

            // all other elements are optional

            if (item.LastModified.HasValue)
                itemElement.Add(new XElement(xmlns + "lastmod", item.LastModified.Value.ToString("yyyy-MM-dd")));

            if (item.ChangeFrequency.HasValue)
                itemElement.Add(new XElement(xmlns + "changefreq", item.ChangeFrequency.Value.ToString().ToLower()));

            if (item.Priority.HasValue)
                itemElement.Add(new XElement(xmlns + "priority", item.Priority.Value.ToString("F1", CultureInfo.InvariantCulture)));

            return itemElement;
        }

        private XElement CreateNewsItemElement(ISitemapItem item)
        {
            var newsItem = (NewsSitemapItem)item;

            var itemElement = new XElement(xmlns + "url", new XElement(xmlns + "loc", item.Url.ToLowerInvariant()));

            // all other elements are optional

            if (item.LastModified.HasValue)
                itemElement.Add(new XElement(xmlns + "lastmod", item.LastModified.Value.ToString("yyyy-MM-dd")));

            if (item.ChangeFrequency.HasValue)
                itemElement.Add(new XElement(xmlns + "changefreq", item.ChangeFrequency.Value.ToString().ToLower()));

            if (item.Priority.HasValue)
                itemElement.Add(new XElement(xmlns + "priority", item.Priority.Value.ToString("F1", CultureInfo.InvariantCulture)));

            var elem = new XElement(newsXsi + "news");
            var newsPublication = new XElement(newsXsi + "publication");
            elem.Add(newsPublication);

            if (!String.IsNullOrEmpty(newsItem.PublicationName))
                newsPublication.Add(new XElement(newsXsi + "name", newsItem.PublicationName));

            if (!String.IsNullOrEmpty(newsItem.PublicationLanguage))
                newsPublication.Add(new XElement(newsXsi + "language", newsItem.PublicationLanguage));


            if (!String.IsNullOrEmpty(newsItem.Title))
                elem.Add(new XElement(newsXsi + "title", newsItem.Title));

            if (!String.IsNullOrEmpty(newsItem.Access))
                elem.Add(new XElement(newsXsi + "access", newsItem.Access));

            if (!String.IsNullOrEmpty(newsItem.Keywords))
                elem.Add(new XElement(newsXsi + "keywords", newsItem.Keywords));


            if (!String.IsNullOrEmpty(newsItem.StockTickers))
                elem.Add(new XElement(newsXsi + "stock_tickers", newsItem.StockTickers));

            if (newsItem.PublicationDate.HasValue)
                elem.Add(new XElement(newsXsi + "publication_date", newsItem.PublicationDate.Value.ToString("yyyy-MM-dd")));

            itemElement.Add(elem);

            return itemElement;
        }
    }
}
