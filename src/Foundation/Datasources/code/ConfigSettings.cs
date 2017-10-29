using Sitecore.Data.Items;
using Sitecore.Configuration;
using Sitecore.Data;

namespace JCore.Foundation.Datasources
{
    internal static class ConfigSettings
    {
        internal static ID DefaultRenderingDatasourceLocation
        {
            get { return ID.Parse(Settings.GetSetting("JCore.Foundation.Datasources.DefaultRenderingDatasourceLocation", "{0DE95AE4-41AB-4D01-9EB0-67441B7C2450}")); }
        }

        internal static string DatasourcesRootId(Item item)
        {
            if (item == null)
                return string.Empty;
            var currentSiteContext = item.GetSiteContext();
            return currentSiteContext != null ? currentSiteContext.Properties["datasourceRootItem"] : null;
        }

        internal static string SiteHomeItem(this Item item)
        {
            if (item == null)
                return string.Empty;
            var currentSiteContext = item.GetSiteContext();
            return currentSiteContext != null ? string.Concat(currentSiteContext.RootPath, currentSiteContext.StartItem) : null;
        }
        
    }
}
