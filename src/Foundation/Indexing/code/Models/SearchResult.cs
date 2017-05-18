using System;
using Car.Foundation.SitecoreExtensions.Extensions;
using Sitecore.Data.Items;

namespace Car.Foundation.Indexing.Models
{
    public class SearchResult : ISearchResult
  {
    private Uri _url;

    public SearchResult(Item item)
    {
      this.Item = item;
    }

    public Item Item { get; }
    public string Title { get; set; }
    public string ContentType { get; set; }
    public string Description { get; set; }

    public Uri Url
    {
      get
      {
        return this._url ?? new Uri(this.Item.Url(), UriKind.Relative);
      }
      set
      {
        this._url = value;
      }
    }

    public string ViewName { get; set; }
  }
}