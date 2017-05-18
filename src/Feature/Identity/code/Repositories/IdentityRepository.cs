using System;
using Car.Foundation.SitecoreExtensions.Extensions;
using Sitecore;
using Sitecore.Data.Items;

namespace Car.Feature.Identity.Repositories
{
    public static class IdentityRepository
  {
    public static Item Get([NotNull] Item contextItem)
    {
      if (contextItem == null)
        throw new ArgumentNullException(nameof(contextItem));
            
      return contextItem.GetAncestorOrSelfOfTemplate(Templates.Identity.ID) ?? Context.Site.GetContextItem(Templates.Identity.ID);
    }
  }
}