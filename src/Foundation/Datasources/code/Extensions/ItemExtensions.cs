using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Data.Managers;
using Sitecore.Diagnostics;
using Sitecore.Links;
using Sitecore.Resources.Media;
using Sitecore.Xml.Xsl;

namespace JCore.Foundation.Datasources.Extensions
{
    public static class ItemExtensions
    {
        public static bool IsDerived(this Item item, ID templateId)
        {
            if (item == null)
                return false;

            return !templateId.IsNull && item.IsDerived(item.Database.Templates[templateId]);
        }

        private static bool IsDerived(this Item item, Item templateItem)
        {
            if (item == null)
                return false;

            if (templateItem == null)
                return false;

            var itemTemplate = TemplateManager.GetTemplate(item);
            return itemTemplate != null && (itemTemplate.ID == templateItem.ID || itemTemplate.DescendsFrom(templateItem.ID));
        }

    }
}
