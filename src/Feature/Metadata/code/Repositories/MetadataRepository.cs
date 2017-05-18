using System.Collections.Generic;
using System.Linq;
using Car.Feature.Metadata.Models;
using Car.Foundation.SitecoreExtensions.Extensions;
using Sitecore;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;

namespace Car.Feature.Metadata.Repositories
{
    public static class MetadataRepository
    {
        public static Item Get(Item contextItem)
        {
            return contextItem.GetAncestorOrSelfOfTemplate(Templates.SiteMetadata.ID) ?? Context.Site.GetContextItem(Templates.SiteMetadata.ID);
        }

        public static MetaKeywordsModel GetKeywords(Item item)
        {
            if (item.IsDerived(Templates.PageMetadata.ID))
            {
                var keywordsField = item.Fields[Templates.PageMetadata.Fields.Keywords];
                if (keywordsField == null)
                {
                    return null;
                }

                var keywordMultilist = new MultilistField(keywordsField);
                var keywords = keywordMultilist.GetItems().Select(keywrdItem => keywrdItem[Templates.Keyword.Fields.Keyword]);
                var metaKeywordModel = new MetaKeywordsModel
                {
                    Keywords = keywords.ToList()
                };

                return metaKeywordModel;
            }

            return null;
        }

        public static string GetRobots(Item item)
        {
            if (item.IsDerived(Templates.PageMetadata.ID))
            {
                var robotContent = new List<string>();

                robotContent.Add(
                    MainUtil.GetBool(item[Templates.PageMetadata.Fields.CanIndex], false)
                        ? "index"
                        : "noindex");

                robotContent.Add(
                    MainUtil.GetBool(item[Templates.PageMetadata.Fields.CanFollow], false)
                        ? "follow"
                        : "nofollow");

                return string.Join(", ", robotContent);
            }

            return null;
        }
    }
}