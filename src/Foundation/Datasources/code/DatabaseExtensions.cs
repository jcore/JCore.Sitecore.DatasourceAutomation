using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JCore.Foundation.Datasources
{
    public static class DatabaseExtensions
    {
        public static Item CreateItemPath(this Database database, string path, CustomItemBase folderTemplate, CustomItemBase itemTemplate)
        {
            Assert.ArgumentNotNullOrEmpty(path, "path");
            Assert.ArgumentNotNull(folderTemplate, "folderTemplate");
            Assert.ArgumentNotNull(itemTemplate, "itemTemplate");
            if (path.Length > 0)
            {
                Item obj1 = database.GetRootItem(itemTemplate.InnerItem.Language);
                if (obj1 != null && (path.StartsWith("/" + obj1.Name, StringComparison.OrdinalIgnoreCase) || path.StartsWith("/" + obj1.ID, StringComparison.InvariantCultureIgnoreCase)))
                {
                    string[] strArray = path.Split('/');
                    for (int index = 2; index < strArray.Length; ++index)
                    {
                        if (!string.IsNullOrEmpty(strArray[index]))
                        {
                            Item obj2 = obj1.Children[strArray[index]];
                            if (obj2 == null)
                            {
                                CustomItemBase template = folderTemplate;
                                if (index == strArray.Length - 1)
                                    template = itemTemplate;

                                if (template is TemplateItem)
                                {
                                    obj2 = obj1.Add(strArray[index], (TemplateItem)template);
                                }
                                else if (template is BranchItem)
                                {
                                    obj2 = obj1.Add(strArray[index], (BranchItem)template);
                                }
                            }
                            obj1 = obj2;
                        }
                    }
                    return obj1;
                }
            }
            return null;
        }
    }
}