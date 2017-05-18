using System;
using Glass.Mapper.Sc;
using Sitecore.Mvc.Presentation;

namespace Car.Foundation.Models
{
    public class ControllerScContext : SitecoreContext, IControllerScContext
    {
        private IGlassHtml _glassHtml;

        public ControllerScContext(IGlassHtml glassHtml)
        {
            _glassHtml = glassHtml;
        }

        public T GetRenderingParameters<T>() where T : class
        {
            if (RenderingContext.CurrentOrNull != null)
            {
                string parameters = RenderingContext.CurrentOrNull.Rendering["Parameters"];
                if (String.IsNullOrEmpty(parameters))
                {
                    return _glassHtml.GetRenderingParameters<T>(parameters);
                }
            }
            return default(T);

        }

        public T GetDataSource<T>() where T : class
        {
            string dataSource = RenderingContext.CurrentOrNull.Rendering.DataSource;

            if (String.IsNullOrEmpty(dataSource))
            {
                return default(T);
            }

            Guid dataSourceId;

            return Guid.TryParse(dataSource, out dataSourceId)

                ? GetItem<T>(dataSourceId)

                : GetItem<T>(dataSource);
        }

        /// <summary>
        /// if the rendering context and data source has been set then returns the data source item, otherwise returns the context item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="isLazy"></param>
        /// <param name="inferType"></param>
        /// <returns></returns>
        public T GetControllerItem<T>(bool isLazy = false, bool inferType = false)
        where T : class
        {
            T renderingItem;
            if (RenderingContext.Current == null || RenderingContext.Current.Rendering == null || string.IsNullOrEmpty(RenderingContext.Current.Rendering.DataSource))
            {
                return GetCurrentItem<T>(false, false);
            }
            try
            {
                renderingItem = this.GetRenderingItem<T>(isLazy, inferType);
            }
            catch (InvalidOperationException)
            {
                renderingItem = GetCurrentItem<T>(false, false);
            }
            return renderingItem;
        }

        /// <summary>
        /// Returns the data source item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="isLazy"></param>
        /// <param name="inferType"></param>
        /// <returns></returns>
        public virtual T GetRenderingItem<T>(bool isLazy = false, bool inferType = false)
        where T : class
        {
            if (RenderingContext.Current == null || RenderingContext.Current.Rendering == null || string.IsNullOrEmpty(RenderingContext.Current.Rendering.DataSource))
            {
                return default(T);
            }
            return GetItem<T>(RenderingContext.Current.Rendering.DataSource, isLazy, inferType);
        }
    }
}