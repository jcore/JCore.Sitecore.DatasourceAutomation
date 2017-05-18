﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Data.Items;
using Sitecore.SecurityModel;
using Sitecore.Data.Managers;
using Sitecore.Data;
using Sitecore;
using Sitecore.Data.Fields;
using Sitecore.Layouts;
using JCore.Foundation.Datasources.Models;
using JCore.Foundation.SitecoreExtensions.Extensions;

namespace JCore.Foundation.Datasources.Repositories
{
    public class DatasourceRepository : IDatasourceRepository
    {
        public void CreateItemDatasource(Item item)
        {
            if (!item.IsDerived(NonChildDatasourceSupport.SitecoreTemplateId) || !string.IsNullOrWhiteSpace(item[NonChildDatasourceSupport.DatasourceFolderFieldId]))
            {
                return;
            }
            var datasourceItemId = ConfigSettings.DatasourcesRootId(item);

            if (!string.IsNullOrWhiteSpace(datasourceItemId))
            {
                var datasourceItem = item.Database.GetItem(datasourceItemId);
                if (datasourceItem == null)
                {
                    return;
                }
                string requireDatasourcePath;
                Item pageDatasourceItem = null;
                var homeItemPath = item.SiteHomeItem();
                var homeItem = item.Database.GetItem(homeItemPath);
                if (homeItem != null && item.Parent.ID == homeItem.ID)
                {
                    requireDatasourcePath = string.Format("{0}/{1}", datasourceItem.Paths.Path, item.Name);
                    pageDatasourceItem = CreateDataSourceItem(item.Database, requireDatasourcePath, ConfigSettings.ParentDatasourceFolderTemplateId, true);
                    if (pageDatasourceItem == null)
                    {
                        return;
                    }
                    requireDatasourcePath = string.Format("{0}/{1}/Content", datasourceItem.Paths.Path, pageDatasourceItem.Name);
                    pageDatasourceItem = CreateDataSourceItem(item.Database, requireDatasourcePath, ConfigSettings.ChildDatasourceFolderTemplateId, true);
                }
                else if (homeItem != null)
                {
                    requireDatasourcePath = string.Concat(datasourceItem.Paths.Path, item.Paths.FullPath.Replace(homeItem.Paths.FullPath, ""));
                    pageDatasourceItem = CreateDataSourceItem(item.Database, requireDatasourcePath, ConfigSettings.ParentDatasourceFolderTemplateId, true);
                    if (pageDatasourceItem == null)
                    {
                        return;
                    }
                    requireDatasourcePath = string.Format("{0}/Content", pageDatasourceItem.Paths.FullPath);
                    pageDatasourceItem = CreateDataSourceItem(item.Database, requireDatasourcePath, ConfigSettings.ChildDatasourceFolderTemplateId, true);
                }

                PopulateDatasourceFolderField(pageDatasourceItem, item);

                if (pageDatasourceItem != null)
                {
                    UpdateItemRenderingDataSources(pageDatasourceItem, item);
                }
            }
        }

        public void DeleteItemDatasource(Item item)
        {
            if (string.IsNullOrWhiteSpace(item[NonChildDatasourceSupport.DatasourceFolderFieldName]))
            {
                return;
            }
            var datasourceField = (ReferenceField)item.Fields[NonChildDatasourceSupport.DatasourceFolderFieldName];
            if(datasourceField != null && datasourceField.TargetItem != null)
            {
                using (new SecurityDisabler())
                {
                    if (datasourceField.TargetItem.HasChildren)
                    {
                        foreach (Item child in datasourceField.TargetItem.Children)
                        {
                            RemoveDatasourceItem(child, item);
                        }
                    }

                    if (!datasourceField.TargetItem.HasChildren)
                    {
                        if (datasourceField.TargetItem.TemplateID == DatasourceSubfolder.SitecoreTemplateId && datasourceField.TargetItem.Parent.TemplateID == DatasourceFolder.SitecoreTemplateId)
                        {
                            RemoveDatasourceItem(datasourceField.TargetItem.Parent, item);
                        }
                        else if (datasourceField.TargetItem.TemplateID == DatasourceSubfolder.SitecoreTemplateId)
                        {
                            RemoveDatasourceItem(datasourceField.TargetItem, item);
                        }
                    }
                }
            }
        }

        public void MoveItemDatasource(Item item)
        {
            if (string.IsNullOrWhiteSpace(item[NonChildDatasourceSupport.DatasourceFolderFieldName]))
            {
                return;
            }
            var datasourceField = (ReferenceField)item.Fields[NonChildDatasourceSupport.DatasourceFolderFieldName];
            if (datasourceField != null && datasourceField.TargetItem != null)
            {
                using (new SecurityDisabler())
                {
                    if (datasourceField.TargetItem.TemplateID == DatasourceSubfolder.SitecoreTemplateId && datasourceField.TargetItem.Parent.TemplateID == DatasourceFolder.SitecoreTemplateId)
                    {
                        var datasourceItemId = ConfigSettings.DatasourcesRootId(item);
                        if (!string.IsNullOrWhiteSpace(datasourceItemId))
                        {
                            var datasourceItem = item.Database.GetItem(datasourceItemId);
                            if (datasourceItem == null)
                            {
                                return;
                            }
                            var homeItemPath = item.SiteHomeItem();
                            var homeItem = item.Database.GetItem(homeItemPath);
                            if (homeItem != null)
                            {
                                var newDatasourcePath = string.Concat(datasourceItem.Paths.Path, item.Parent.Paths.FullPath.Replace(homeItem.Paths.FullPath, ""));
                                var newDatasourceParent = item.Database.GetItem(newDatasourcePath);
                                if (newDatasourceParent != null && datasourceField.TargetItem.Parent.Paths.FullPath != newDatasourceParent.Paths.FullPath)
                                    datasourceField.TargetItem.Parent.MoveTo(newDatasourceParent);
                            }
                        }             
                    }
                }
            }
        }

        public void RenameItemDatasource(Item item)
        {
            if (string.IsNullOrWhiteSpace(item[NonChildDatasourceSupport.DatasourceFolderFieldName]))
            {
                return;
            }
            var datasourceField = (ReferenceField)item.Fields[NonChildDatasourceSupport.DatasourceFolderFieldName];
            if (datasourceField != null && datasourceField.TargetItem != null)
            {
                using (new SecurityDisabler())
                {
                    if (datasourceField.TargetItem.TemplateID == DatasourceSubfolder.SitecoreTemplateId && datasourceField.TargetItem.Parent.TemplateID == DatasourceFolder.SitecoreTemplateId)
                    {
                        var datasourceItem = datasourceField.TargetItem.Parent;
                        using (new EditContext(datasourceItem))
                        {
                            datasourceItem.Name = item.Name;
                        }
                    }
                }
            }
        }

        private void RemoveDatasourceItem(Item datasource, Item item)
        {
            var references = Globals.LinkDatabase.GetItemReferrers(datasource, true);
            if (references == null)
                datasource.Delete();
            else if (!references.Any(l=> l.TargetItemID != item.ID))
                datasource.Delete();
        }
        /// <summary>
        ///     Updates the item rendering data sources.
        /// </summary>
        /// <param name="dataSourceItem">The data source item.</param>
        /// <param name="item">The item.</param>
        private void UpdateItemRenderingDataSources(Item dataSourceItem, Item item)
        {
            var device = item.Database.GetItem(Constants.DefaultDevice);
            if (device == null || dataSourceItem == null || item == null)
            {
                return;
            }
            var renderings = item.Visualization.GetRenderings(device, false);
            LayoutField layoutField = item.Fields[FieldIDs.LayoutField];
            var layoutDefinition = LayoutDefinition.Parse(layoutField.Value);
            var deviceDefinition = layoutDefinition.GetDevice(device.ID.ToString());
            foreach (var rendering in renderings)
            {
                if (rendering == null || rendering.RenderingItem == null) continue;
                var renderingDatasourceTemplatePath = rendering.RenderingItem.InnerItem[Constants.DatasourceTemplateFieldName];
                if (string.IsNullOrWhiteSpace(renderingDatasourceTemplatePath)) continue;
                using (new SecurityDisabler())
                {
                    var template = item.Database.GetItem(renderingDatasourceTemplatePath);
                    var renderingDefinition = deviceDefinition.GetRenderingByUniqueId(rendering.UniqueId);
                    if (!string.IsNullOrWhiteSpace(renderingDefinition.Datasource)) continue;
                    var rendering1 = rendering;
                    var existingItemsWithTheSameName = dataSourceItem.Children.Where(i => i.Name.Contains(rendering1.RenderingItem.Name));

                    var templateItem = TemplateManager.GetTemplate(template.ID, template.Database);
                    if (templateItem != null && templateItem.GetBaseTemplates().Any(t => t.ID == ID.Parse(Constants.StandardRenderingParametersTemplate)))
                    {
                        continue;
                    }
                    var widgetDataSourceItem = dataSourceItem.Add(!existingItemsWithTheSameName.Any() ? rendering.RenderingItem.Name : rendering.RenderingItem.InnerItem.UniqueName(), new TemplateID(template.ID));

                    if (widgetDataSourceItem != null)
                    {
                        renderingDefinition.Datasource = widgetDataSourceItem.ID.ToString();
                    }
                }
            }
            // Save the layout changes
            using (new EditContext(item))
            {
                layoutField.Value = layoutDefinition.ToXml();
            }
        }

        /// <summary>
        ///     Populates the datasource folder field.
        /// </summary>
        /// <param name="dataSourceItem">The data source item.</param>
        /// <param name="item">The item.</param>
        private static void PopulateDatasourceFolderField(Item dataSourceItem, Item item)
        {
            if (item.Fields[NonChildDatasourceSupport.DatasourceFolderFieldName] == null) return;
            using (new SecurityDisabler())
            {
                using (new EditContext(item, SecurityCheck.Disable))
                {
                    item.Fields[NonChildDatasourceSupport.DatasourceFolderFieldName].Value = dataSourceItem.ID.ToString();
                }
            }
        }
        /// <summary>
        ///     Creates the data source item.
        /// </summary>
        /// <param name="targetItemPath">The target item path.</param>
        /// <param name="templateId">The template identifier.</param>
        /// <param name="useExisting"></param>
        /// <returns></returns>
        private Item CreateDataSourceItem(Database db, string targetItemPath, string templateId, bool useExisting)
        {
            if (string.IsNullOrWhiteSpace(targetItemPath) || string.IsNullOrWhiteSpace(templateId))
            {
                return null;
            }
            try
            {
                var datasourceFolderItem = db.GetItem(targetItemPath);
                Item dataSourceItem;
                if (datasourceFolderItem == null || !useExisting)
                {
                    using (new SecurityDisabler())
                    {
                        var templateItem = db.GetTemplate(ID.Parse(templateId));
                        if (templateItem == null) return null;

                        var template = TemplateManager.GetTemplate(templateItem.InnerItem);
                        if (template != null && template.GetBaseTemplates().Any(t => t.ID == ID.Parse(Constants.StandardRenderingParametersTemplate)))
                        {
                            return null;
                        }
                        if (datasourceFolderItem != null)
                        {
                            var uniqueName = datasourceFolderItem.UniqueName();
                            var folderPath = StringUtil.RemovePostfix(datasourceFolderItem.Name, targetItemPath);
                            targetItemPath = string.Concat(folderPath, uniqueName);
                        }
                        dataSourceItem = db.CreateItemPath(targetItemPath, templateItem);
                        var parentFolderTemplateId = ID.Parse(ConfigSettings.ParentDatasourceFolderTemplateId);
                        if (dataSourceItem != null && dataSourceItem.Parent.TemplateID != parentFolderTemplateId)
                        {
                            var datasourceFolderTemplate = db.GetTemplate(parentFolderTemplateId);
                            if (datasourceFolderTemplate != null)
                            {
                                using (new EditContext(dataSourceItem.Parent, false, true))
                                {
                                    dataSourceItem.Parent.ChangeTemplate(datasourceFolderTemplate);
                                }
                            }
                        }
                    }
                }
                else
                {
                    dataSourceItem = datasourceFolderItem;
                }
                return dataSourceItem;
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("DataSourceReferenceFixer Log : Error in Creating Data Source Item:" + ex.Message, ex, this);
                throw;
            }
        }


    }
}