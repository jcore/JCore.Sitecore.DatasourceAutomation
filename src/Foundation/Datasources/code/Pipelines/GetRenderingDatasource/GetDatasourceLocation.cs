using JCore.Foundation.Datasources.Models;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Pipelines.GetRenderingDatasource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            if (args.RenderingItem != null && string.IsNullOrEmpty(args.RenderingItem[Constants.DatasourceTemplateFieldName]))
                return;

            var currentItem = args.ContentDatabase.GetItem(args.ContextItemPath);
            if (currentItem == null) return;

            var datasourceReference = currentItem[NonChildDatasourceSupport.DatasourceFolderFieldId];
            if (string.IsNullOrWhiteSpace(datasourceReference))
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
                .FirstOrDefault(child => child.TemplateID == DatasourceSubfolder.SitecoreTemplateId);
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
            var datasourceReferenceItem = args.ContentDatabase.GetItem(datasourceReference);
            if (datasourceReferenceItem != null)
            {
                args.DatasourceRoots.Insert(0, datasourceReferenceItem);
            }
        }
    }
}
