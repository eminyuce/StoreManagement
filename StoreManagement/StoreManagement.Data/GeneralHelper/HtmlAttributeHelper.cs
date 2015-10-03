using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace StoreManagement.Data.GeneralHelper
{
    public class HtmlAttributeHelper
    {


        private static HtmlDocument GetHtml(string source)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(source);

            return doc;
        }

        public static String AddPaging(String html, String additionalHtml)
        {
            var r = AddHtml(html, "span", "data-paging", additionalHtml);

            return r;
        }

        public static String AddHtml(String html, String attributeTag, String dataAttribute, String additionalHtml)
        {
            String result = "";
            var htmlDocument = GetHtml(html);
            HtmlNodeCollection nodes =
                htmlDocument.DocumentNode.SelectNodes(String.Format("//{1}[@{0}]", dataAttribute, attributeTag));
            if (nodes != null)
            {
                foreach (HtmlNode divNode in nodes)
                {
                    // HtmlAttribute attribute = divNode.Attributes[String.Format("{0}", dataAttribute)];
                    divNode.InnerHtml = additionalHtml;
                    // links.Add(attribute.Value);
                }
            }
            else
            {

            }
            result = htmlDocument.DocumentNode.OuterHtml;
            return result;
        }
    }
}
