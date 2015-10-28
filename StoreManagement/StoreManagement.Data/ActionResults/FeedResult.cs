using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace StoreManagement.Data.ActionResults
{

    public class FeedResult : ActionResult
    {
        public Encoding ContentEncoding { get; set; }
        public string ContentType { get; set; }
        private StringBuilder Comment { get; set; }
        private readonly SyndicationFeedFormatter feed;
        public SyndicationFeedFormatter Feed
        {
            get { return feed; }
        }

        public FeedResult(SyndicationFeedFormatter feed, StringBuilder comment)
        {
            this.feed = feed;
            this.Comment = comment;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            HttpResponseBase response = context.HttpContext.Response;
            response.ContentType = !string.IsNullOrEmpty(ContentType) ? ContentType : "application/rss+xml";

            if (ContentEncoding != null)
                response.ContentEncoding = ContentEncoding;


            String[] lines = Comment
                .ToString()
                .Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None)
                 .Where(r => !String.IsNullOrEmpty(r))
                .Select(p => p.Trim()).ToArray();

            foreach (var line in lines)
            {
                response.Output.WriteLine(String.Format("<!-- {0}  -->", line));
            }



            if (feed != null)
                using (var xmlWriter = new XmlTextWriter(response.Output))
                {
                    xmlWriter.Formatting = Formatting.Indented;
                    feed.WriteTo(xmlWriter);
                }
        }
    }
}
