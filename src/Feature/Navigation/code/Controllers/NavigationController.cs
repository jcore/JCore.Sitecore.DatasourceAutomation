using System;
using System.Linq;
using System.Web.Mvc;
using Car.Feature.Navigation.Models;
using Car.Feature.Navigation.Repositories;
using Car.Foundation.SitecoreExtensions.Extensions;
using Sitecore;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;

namespace Car.Feature.Navigation.Controllers
{
    public class NavigationController : Controller
    {

        private readonly INavigationRepository _navigationRepository;

        public NavigationController() : this(new NavigationRepository(RenderingContext.Current.ContextItem))
        {
        }

        public NavigationController(INavigationRepository navigationRepository)
        {
            _navigationRepository = navigationRepository;
        }
        public ActionResult MainNavigation()
        {
            if (string.IsNullOrEmpty(RenderingContext.Current.Rendering.DataSource))
            {
                return null;
            }
            var item = RenderingContext.Current.Rendering.Item;
            var items = this._navigationRepository.GetMainNavigationItems(item);

            return View("MainNavigation", items);
        }
        public ActionResult SubMainNavigation()
        {
            if (string.IsNullOrEmpty(RenderingContext.Current.Rendering.DataSource))
            {
                return null;
            }
            var item = RenderingContext.Current.Rendering.Item;
            var items = this._navigationRepository.GetSectionNavigationItems(item);

            return View("SubMainNavigation", items);
        }
        

    }
}