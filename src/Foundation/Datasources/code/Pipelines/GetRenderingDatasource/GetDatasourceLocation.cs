using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Pipelines.GetRenderingDatasource;
using System.Linq;
using static JCore.Foundation.Datasources.Templates;
using JCore.Foundation.SitecoreExtensions.Extensions;

namespace JCore.Foundation.Datasources.Pipelines.GetRenderingDatasource
{

    public class GetDatasourceLocation
    {
        /// <summary>
        /// Processes the specified arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public void Process(GetRenderingDatasourceArgs args)
        {
            Assert.IsNotNull(args, "args");

            if (string.IsNullOrWhiteSpace(args.ContextItemPath) || args.ContentDatabase == null) return;

            var currentItem = args.ContentDatabase.GetItem(args.ContextItemPath);
            if (currentItem == null) return;

            var datasourceReference = currentItem[NonChildDatasourceSupport.Fields.DatasourceFolderFieldId];
            if (string.IsNullOrWhiteSpace(datasourceReference) && !currentItem.IsDerived(NonChildDatasourceSupport.ID))
            {
                SetChildDataSourceItem(args, currentItem);
            }
            else
            {
                SetNonChildDataSourceItem(args, datasourceReference);
            }
        }

        /// <summary>
        /// Adds child datasource folder as a datasource root.
        /// </summary>
        /// <param name="args"></param>
        /// <param name="currentItem"></param>
        private static void SetChildDataSourceItem(GetRenderingDatasourceArgs args, Item currentItem)
        {

            var childDatasourceItem = currentItem.Children
                .FirstOrDefault(child => child.TemplateID == DatasourceSubfolderTemplate.ID);
            if (childDatasourceItem != null)
            {
                args.DatasourceRoots.Insert(0, childDatasourceItem);
            }
        }

        /// <summary>
        /// Adds specified folder as a datasource root.
        /// </summary>
        /// <param name="args"></param>
        /// <param name="datasourceReference"></param>
        private static void SetNonChildDataSourceItem(GetRenderingDatasourceArgs args, string datasourceReference)
        {
            if (ConfigSettings.DefaultRenderingDatasourceLocation != ID.Null)
            {
                var defaultLocationItem = args.ContentDatabase.GetItem(ConfigSettings.DefaultRenderingDatasourceLocation);
                if (defaultLocationItem != null)
                {
                    args.DatasourceRoots.Insert(0, defaultLocationItem);
                }
            }

            var datasourceReferenceItem = args.ContentDatabase.GetItem(datasourceReference);
            if (datasourceReferenceItem != null)
            {
                args.DatasourceRoots.Insert(0, datasourceReferenceItem);
            }
        }
    }
}
