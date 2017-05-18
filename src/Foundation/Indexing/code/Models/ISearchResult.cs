using System;
using Sitecore.Data.Items;

namespace Car.Foundation.Indexing.Models
{
    public interface ISearchResult
  {
    Item Item { get; }
    string Title { get; set; }
    string ContentType { get; set; }
    string Description { get; set; }
    Uri Url { get; set; }
    string ViewName { get; set; }
  }
}