using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Attributes;
using Sitecore.Data.Items;
using Sitecore.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Car.Foundation.Models
{
    public class BaseItem : IBaseItem
    {
        [SitecoreId]
        public Guid Id { get; set; }

        [SitecoreChildren]
        public IEnumerable<Item> Children { get; set; }

        [SitecoreInfo(SitecoreInfoType.Url, UrlOptions = SitecoreInfoUrlOptions.LanguageEmbeddingNever)]
        public string Url { get; set; }

        [SitecoreInfo(SitecoreInfoType.Language, UrlOptions = SitecoreInfoUrlOptions.LanguageEmbeddingNever)]
        public Language Language { get; set; }

        [SitecoreInfo(SitecoreInfoType.Version, UrlOptions = SitecoreInfoUrlOptions.LanguageEmbeddingNever)]
        public int Version { get; set; }

        [SitecoreInfo(SitecoreInfoType.TemplateId)]
        public Guid TemplateId { get; set; }

        [SitecoreInfo(SitecoreInfoType.BaseTemplateIds)]
        public IEnumerable<Guid> BaseTemplateIds { get; set; }

        [SitecoreInfo(SitecoreInfoType.TemplateName)]
        public string TemplateName { get; set; }

        [SitecoreInfo(SitecoreInfoType.Name)]
        public string Name { get; set; }

        [SitecoreParent(InferType = true)]
        public IBaseItem Parent { get; }

        [SitecoreItem]
        public Item InnerItem { get; }
    }
}