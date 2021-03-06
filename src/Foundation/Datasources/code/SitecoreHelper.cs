﻿using Sitecore.Data.Items;
using Sitecore.Sites;
using System.Linq;
using Sitecore.StringExtensions;

namespace JCore.Foundation.Datasources
{
    internal static class SitecoreHelper
    {
        internal static SiteContext GetSiteContext(this Item item)
        {
            foreach (var siteInfo in SiteContextFactory.Sites.Where(site => site.Database.ToLowerInvariant() == item.Database.Name.ToLowerInvariant() && !string.IsNullOrWhiteSpace(string.Concat(site.RootPath, site.StartItem)) && item.Paths.FullPath.StartsWith(string.Concat(site.RootPath, site.StartItem), true)))
            {
                return SiteContextFactory.GetSiteContext(siteInfo.Name);
            }
            if(SiteContextFactory.GetSiteContext("website") == null)
            {
                return Sitecore.Context.Site;
            }
            return SiteContextFactory.GetSiteContext("website");
        }

        public static string UniqueName(this Item item)
        {
            var parent = item.Parent;

            var text = item.Name;
            var num = 1;
            while (parent.Axes.GetChild(text) != null)
            {
                text = item.Name + " " + num;
                num++;
            }
            return text;
        }
    }
}
