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
            var r = AddHtml(html, "div", "data-paging", additionalHtml);

            return r;
        }

        public static String AddHtml(String html, String attributeTag, String dataAttribute, String additionalHtml)
        {
            //String result = html;
            //var htmlDocument = GetHtml(html);
            //HtmlNodeCollection nodes =
            //    htmlDocument.DocumentNode.SelectNodes(String.Format("//{0}[@{1}]", attributeTag, dataAttribute));
            ////string classToFind = "sitepaging";
            ////nodes = htmlDocument.DocumentNode.SelectNodes(string.Format("//*[contains(@class,'{0}')]", classToFind));
            ////var nodes1 = htmlDocument.DocumentNode.Descendants("div").Where(d =>
            ////                d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains(classToFind)
            ////            ).ToList();


            //if (nodes.Any())
            //{
            //    foreach (HtmlNode divNode in nodes)
            //    {
            //        // HtmlAttribute attribute = divNode.Attributes[String.Format("{0}", dataAttribute)];
            //        divNode.InnerHtml = additionalHtml;
            //        // links.Add(attribute.Value);
            //    }
            //    result = htmlDocument.DocumentNode.OuterHtml;
            //}
            //else
            //{

            //}

            //return result;

            html = html.Replace("<StorePagingHtml>", additionalHtml);

            return html;
        }
    }
}
