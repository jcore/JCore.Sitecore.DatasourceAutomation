using System;
using System.Collections.Generic;
using System.Linq;
using Car.Feature.Navigation.Models;
using Car.Foundation.SitecoreExtensions.Extensions;
using Sitecore;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;

namespace Car.Feature.Navigation.Repositories
{
    public class NavigationRepository : INavigationRepository
    {
        public NavigationRepository(Item contextItem)
        {
            ContextItem = contextItem;
            NavigationRoot = GetNavigationRoot(ContextItem);
            if (NavigationRoot == null)
            {
                throw new InvalidOperationException(
                    $"Cannot determine navigation root from '{ContextItem.Paths.FullPath}'");
            }
        }

        public Item ContextItem { get; }
        public Item NavigationRoot { get; }


        public NavigationItems GetMainNavigationItems(Item menuRoot)
        {
            var navItems = GetChildNavigationItems(menuRoot, 0, 1);

            return navItems;
        }

        public SectionNavigationItems GetSectionNavigationItems(Item menuRoot)
        {
            var sectionNavigationItems = new SectionNavigationItems();

            var navItems = GetChildNavigationItems(menuRoot, 0, 1);

            IList<SectionNavigationItem> temp = new List<SectionNavigationItem>();
            foreach (var navItem in navItems.Items)
            {
                var sectionNavigationItem = new SectionNavigationItem {SectionItem = navItem.Item};
                MultilistField selectedSectionItems = navItem.Item.Fields[Templates.SectionLink.Fields.Section];
                var isAnyItemSelected = selectedSectionItems.GetItems().Where(i => IncludeInNavigation(i)).ToList();
                if (isAnyItemSelected.Any())
                {
                    sectionNavigationItem.SectionItems = isAnyItemSelected;
                }
                temp.Add(sectionNavigationItem);
                sectionNavigationItems.Items = temp;
            }

            return sectionNavigationItems;
        }


        private NavigationItems GetChildNavigationItems(Item parentItem, int level, int maxLevel)
        {
            if (level > maxLevel || !parentItem.HasChildren)
            {
                return null;
            }
            var childItems =
                parentItem.Children.Where(item => IncludeInNavigation(item))
                    .Select(i => CreateNavigationItem(i, level, maxLevel));
            return new NavigationItems
            {
                Items = childItems.ToList()
            };
        }

        private bool IncludeInNavigation(Item item, bool forceShowInMenu = false)
        {
            return item.HasContextLanguage() && item.IsDerived(Templates.Navigable.ID) &&
                   (forceShowInMenu || MainUtil.GetBool(item[Templates.Navigable.Fields.ShowInNavigation], false));
        }

        private NavigationItem CreateNavigationItem(Item item, int level, int maxLevel = -1)
        {
            var targetItem = item.IsDerived(Templates.Link.ID) ? item.TargetItem(Templates.Link.Fields.Link) : item;
            return new NavigationItem
            {
                Item = item,
                Url = item.IsDerived(Templates.Link.ID) ? item.LinkFieldUrl(Templates.Link.Fields.Link) : item.Url(),
                Target = item.IsDerived(Templates.Link.ID) ? item.LinkFieldTarget(Templates.Link.Fields.Link) : "",
                IsActive = IsItemActive(targetItem ?? item),
                Children = GetChildNavigationItems(item, level + 1, maxLevel),
                ShowChildren =
                    !item.IsDerived(Templates.Navigable.ID) ||
                    item.Fields[Templates.Navigable.Fields.ShowChildren].IsChecked()
            };
        }

        private bool IsItemActive(Item item)
        {
            return ContextItem.ID == item.ID || ContextItem.Axes.GetAncestors().Any(a => a.ID == item.ID);
        }


        public Item GetNavigationRoot(Item contextItem)
        {
            return contextItem.GetAncestorOrSelfOfTemplate(Templates.NavigationRoot.ID) ??
                   Context.Site.GetContextItem(Templates.NavigationRoot.ID);
        }
    }
}