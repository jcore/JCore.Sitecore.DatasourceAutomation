using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;

namespace Car.Feature.Navigation.Models
{
    public class SectionNavigationItem
    {
        public Item SectionItem { get; set; }
        public IList<Item> SectionItems { get; set; }
    }
}