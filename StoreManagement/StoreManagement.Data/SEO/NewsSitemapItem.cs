using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreManagement.Data.SEO
{
    public class NewsSitemapItem : SitemapItem
    {
        /// <summary>
        /// URL of the page.
        /// </summary>
        public string Url { get; protected set; }

        /// <summary>
        /// The date of last modification of the file.
        /// </summary>
        public DateTime? LastModified { get; protected set; }

        /// <summary>
        /// How frequently the page is likely to change.
        /// </summary>
        public SitemapChangeFrequency? ChangeFrequency { get; protected set; }

        /// <summary>
        /// The priority of this URL relative to other URLs on your site. Valid values range from 0.0 to 1.0.
        /// </summary>
        public double? Priority { get; protected set; }

        public NewsSitemapItem(string url, DateTime? lastModified = null, SitemapChangeFrequency? changeFrequency = null, double? priority = null)
            : base(url, lastModified, changeFrequency, priority)
        {
            Url = url;
            LastModified = lastModified;
            ChangeFrequency = changeFrequency;
            Priority = priority;
        }

        public string PublicationName { get; set; }
        public string PublicationLanguage { get; set; }
        public string Title { get; set; }
        public string Keywords { get; set; }
        public string StockTickers { get; set; }
        public string Access { get; set; }
        public string Genres { get; set; }
        public DateTime? PublicationDate { get; set; }
    }

}
