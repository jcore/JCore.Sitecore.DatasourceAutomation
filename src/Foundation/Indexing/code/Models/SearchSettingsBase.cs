using System.Collections.Generic;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace Car.Foundation.Indexing.Models
{
    public class SearchSettingsBase : ISearchSettings
  {
    public Item Root { get; set; }

    public IEnumerable<ID> Templates { get; set; }
  }
}