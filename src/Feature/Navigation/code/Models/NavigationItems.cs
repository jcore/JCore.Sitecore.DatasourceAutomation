using System.Collections.Generic;
using Sitecore.Mvc.Presentation;

namespace Car.Feature.Navigation.Models
{
    public class NavigationItems : RenderingModel
    {
        public IList<NavigationItem> Items { get; set; }
    }
}