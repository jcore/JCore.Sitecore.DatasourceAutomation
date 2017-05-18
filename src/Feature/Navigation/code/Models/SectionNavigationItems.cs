using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;

namespace Car.Feature.Navigation.Models
{
    public class SectionNavigationItems : RenderingModel
    {
        public IList<SectionNavigationItem> Items { get; set; }
    }
}