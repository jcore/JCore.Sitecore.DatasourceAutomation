using System.Collections.Generic;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace Car.Foundation.Indexing.Models
{
    public interface ISearchSettings
  {
    Item Root { get; set; }

    IEnumerable<ID> Templates { get; set; }
  }
}