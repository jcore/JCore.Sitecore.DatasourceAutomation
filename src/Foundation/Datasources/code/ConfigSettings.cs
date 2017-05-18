using Sitecore.Data.Items;
using Sitecore.Configuration;

namespace JCore.Foundation.Datasources
{
    internal static class ConfigSettings
    {
        internal static string ChildDatasourceFolderTemplateId
        {
            get { return Settings.GetSetting("JCore.Foundation.Datasources.ChildDatasourceFolderTemplateId", "{AC919CC0-6C40-456D-BA6E-23B64231118F}"); }
        }
        internal static string ParentDatasourceFolderTemplateId
        {
            get { return Settings.GetSetting("JCore.Foundation.Datasources.ParentDatasourceFolderTemplateId", "{A8502920-D146-4545-83D0-CE9FB24046DB}"); }
        }

        internal static string DatasourcesRootId(Item item)
        {
            if (item == null)
                return string.Empty;
            var currentSiteContext = item.Paths.Path.GetSiteContext();
            return currentSiteContext != null ? currentSiteContext.Properties["datasourceRootItem"] : null;
        }

        internal static string SiteHomeItem(this Item item)
        {
            if (item == null)
                return string.Empty;
            var currentSiteContext = item.Paths.Path.GetSiteContext();
            return currentSiteContext != null ? string.Concat(currentSiteContext.RootPath, currentSiteContext.StartItem) : null;
        }
        
    }
}
