using System.Collections.Generic;
using System.Configuration.Provider;
using Car.Foundation.Indexing.Models;
using Sitecore;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Data;

namespace Car.Foundation.Indexing.Infrastructure.Providers
{
    public class FallbackSearchResultFormatter : ProviderBase, ISearchResultFormatter
  {
    public string ContentType => "[Unknown]";

    public IEnumerable<ID> SupportedTemplates => new[]
    {
      TemplateIDs.StandardTemplate
    };

    public void FormatResult(SearchResultItem item, ISearchResult formattedResult)
    {
      formattedResult.Title = $"[{item.Name}]";
      formattedResult.Description = $"[This item is indexed but has no content provider: {item.Path}]";
    }
  }
}