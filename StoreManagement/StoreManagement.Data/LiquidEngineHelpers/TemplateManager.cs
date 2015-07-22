using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotLiquid;

namespace StoreManagement.Data.LiquidEngineHelpers
{
    public class TemplateManager
    {
        public Dictionary<string, Template> Templates { get; protected set; }

        public TemplateManager()
        {
            Templates = new Dictionary<string, Template>();
        }

        public void AddTemplate(string name, string template)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");
            if (string.IsNullOrEmpty(template))
                throw new ArgumentNullException("template");

            if (Templates.ContainsKey(name))
                Templates[name] = Template.Parse(template);
            else
                Templates.Add(name, Template.Parse(template));
        }

        public void RegisterTag<T>(string tagName) where T : Tag, new()
        {
            Template.RegisterTag<T>(tagName);
        }

        public void RegisterFilter(Type type)
        {
            Template.RegisterFilter(type);
        }

        //public string Render(string nameOrTemplate, IDictionary<string, object> values)
        //{
        //    Template template;

        //    if (Templates.ContainsKey(nameOrTemplate))
        //        template = Templates[nameOrTemplate];
        //    else
        //        template = Template.Parse(nameOrTemplate);

        //    SharePointDrop sp = new SharePointDrop();

        //    if (values != null)
        //    {
        //        foreach (KeyValuePair<string, object> kvp in values)
        //            sp.AddValue(kvp.Key, kvp.Value);
        //    }

        //    return template.Render(new RenderParameters
        //    {
        //        LocalVariables = Hash.FromAnonymousObject(new { SP = sp }),
        //        RethrowErrors = true
        //    });
        //}
    }


}
