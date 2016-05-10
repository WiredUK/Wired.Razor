using System.Collections.Generic;
using System.IO;
using RazorEngine.Configuration;
using RazorEngine.Templating;

namespace Wired.Razor
{
    public class Parser : IParser
    {
        public bool Debug { get; set; }

        public string RenderView<T>(string viewPath, T model)
        {
            return RenderView(viewPath, model, null);
        }

        public string RenderView<T>(string viewPath, T model, IEnumerable<Template> templates)
        {
            var viewContent = File.ReadAllText(viewPath);
            var config = new TemplateServiceConfiguration
            {
                //Add support for Html.Raw
                BaseTemplateType = typeof(HtmlSupportTemplateBase<>)
            };

            if (this.Debug)
                config.Debug = true;

            var service = RazorEngineService.Create(config);

            if (templates != null)
            {
                foreach (var template in templates)
                    service.AddTemplate(template.Name, template.Source);
            }

            return service.RunCompile(new LoadedTemplateSource(viewContent, viewPath), viewPath, typeof(T), model);
        }
    }
}