using System.Collections.Generic;

namespace Wired.Razor
{
    public interface IParser
    {
        /// <summary>
        /// If set to true, it enables debugging on your view code
        /// </summary>
        bool Debug { get; set; }

        /// <summary>
        /// Renders a view as an HTML string
        /// </summary>
        /// <typeparam name="T">The type of the model passed in to the view, it must match the @model directive.</typeparam>
        /// <param name="viewPath">The absolute path of the view</param>
        /// <param name="model">The model to pass in to the view</param>
        /// <returns>Rendered HTML output</returns>
        string RenderView<T>(string viewPath, T model);

        /// <summary>
        /// Renders a view as an HTML string
        /// </summary>
        /// <typeparam name="T">The type of the model passed in to the view, it must match the @model directive.</typeparam>
        /// <param name="viewPath">The absolute path of the view</param>
        /// <param name="model">The model to pass in to the view</param>
        /// <param name="templates">Any templates the view may use (e.g. layouts)</param>
        /// <returns>Rendered HTML output</returns>
        string RenderView<T>(string viewPath, T model, IEnumerable<Template> templates);
    }
}