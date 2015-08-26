using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace StoreManagement.Data.GeneralHelper
{
    public class HtmlCleanHelper
    {
        private static Dictionary<string, List<String>> _softHtmlTags = new Dictionary<string, List<String>>();

        private static Dictionary<string, List<String>> SoftHtmlTags
        {
            get { return _softHtmlTags; }
            set { _softHtmlTags = value; }
        }
        public static string SanitizeHtmlSoft(String htmlTags, string source)
        {
            Dictionary<string, List<String>> softHtmlTags = GetTags(htmlTags);
            return SanitizeHtmlSoft(softHtmlTags, source);
        }
        /// <summary>
        /// Takes raw HTML input and cleans against a whitelist
        /// </summary>
        /// <param name="source">Html source</param>
        /// <returns>Clean output</returns>
        public static string SanitizeHtmlSoft(Dictionary<string, List<String>> softHtmlTags, string source)
        {
            var customValidHtmlTags = softHtmlTags;
            SoftHtmlTags = softHtmlTags;

            HtmlDocument html = GetHtml(source);
            if (html == null) return String.Empty;

            // All the nodes
            HtmlNode allNodes = html.DocumentNode;

            // Select whitelist tag names
            string[] whitelist = (from kv in customValidHtmlTags
                                  select kv.Key).ToArray();

            // Scrub tags not in whitelist
            CleanNodes(allNodes, whitelist);

            // Filter the attributes of the remaining
            foreach (KeyValuePair<string, List<String>> tag in customValidHtmlTags)
            {
                IEnumerable<HtmlNode> nodes = (from n in allNodes.DescendantsAndSelf()
                                               where n.Name == tag.Key
                                               select n).ToList();



                foreach (var n in nodes)
                {
                    if (!n.HasAttributes) continue;

                    // Get all the allowed attributes for this tag
                    HtmlAttribute[] attr = n.Attributes.ToArray();
                    foreach (HtmlAttribute a in attr)
                    {
                        if (tag.Value == null || tag.Value.Count == 0 || !tag.Value.Contains(a.Name))
                        {
                            a.Remove(); // Wasn't in the list
                        }
                        else
                        {
                            // AntiXss
                            a.Value = a.Value;
                            // Microsoft.Security.Application.Encoder.UrlPathEncode(a.Value);
                        }
                    }
                }
            }

            return allNodes.InnerHtml;
        }

        private static HtmlDocument GetHtml(string source)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(source);

            return doc;
        }

        /// <summary>
        /// Recursively delete nodes not in the whitelist
        /// </summary>
        private static void CleanNodes(HtmlNode node, string[] whitelist)
        {
            if (node.NodeType != HtmlNodeType.Document)
            {
                if (!whitelist.Contains(node.Name))
                {
                    node.ParentNode.RemoveChild(node);
                    return; // We're done
                }
            }

            if (node.HasChildNodes)
                CleanChildren(node, whitelist);
        }

        /// <summary>
        /// Apply CleanNodes to each of the child nodes
        /// </summary>
        private static void CleanChildren(HtmlNode parent, string[] whitelist)
        {
            for (int i = parent.ChildNodes.Count - 1; i >= 0; i--)
                CleanNodes(parent.ChildNodes[i], whitelist);
        }

        public static Dictionary<String, List<String>> GetTags(String fileText)
        {
            var config = new Dictionary<String, List<String>>();
            string line = String.Empty;
            var stringReader = new StringReader(fileText);
            // Read the file and display it line by line.
            var file = stringReader;
            while ((line = file.ReadLine()) != null)
            {
                if (line.StartsWith("//"))
                    continue;

                if (!String.IsNullOrEmpty(line))
                {
                    String[] lineParts = line.Split(',').Select(p => p.Trim()).ToArray();
                    if (!config.ContainsKey(lineParts[0]))
                        config.Add(lineParts[0], lineParts.Skip(1).ToList());
                }
            }
            file.Close();
            return config;
        }


        public static String CleanStyleAttributes(string html = "", List<String> attributes = null)
        {
            // string html = @"<span style=""background:lime;Color:Red;"">Contrary to popular belief,.....</span>";

            if (String.IsNullOrEmpty(html))
                return String.Empty;

            if (attributes == null)
            {
                return String.Empty;
            }


            try
            {


                var doc = new HtmlDocument();
                doc.LoadHtml(html);

                foreach (var span in doc.DocumentNode.DescendantsAndSelf())
                {
                    if (span.HasAttributes && span.Attributes["style"] != null)
                    {
                        var style = span.Attributes["style"].Value;
                        foreach (var attribute in attributes)
                        {
                            string attribute1 = attribute;
                            style = String.Join(";", style.Split(';').Where(s => !s.ToLower().Trim().StartsWith(attribute1)));
                        }
                        span.Attributes["style"].Value = style;
                    }

                }

                var newHtml = doc.DocumentNode.InnerHtml;

                return newHtml;

            }
            catch (Exception ex)
            {


            }

            return html;
        }
    }
}
