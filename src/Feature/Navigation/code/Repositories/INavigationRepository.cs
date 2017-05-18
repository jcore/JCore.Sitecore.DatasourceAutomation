using Sitecore.Data.IDTables;

namespace Car.Feature.Navigation.Repositories
{
  using Sitecore.Data.Items;
  using Models;

  public interface INavigationRepository
  {
        NavigationItems GetMainNavigationItems(Item menuRoot);
        SectionNavigationItems GetSectionNavigationItems(Item menuRoot);
    }
}