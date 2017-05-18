using System.Collections.Generic;

namespace Car.Foundation.Indexing.Models
{
    public interface ISearchResults
  {
    IEnumerable<ISearchResult> Results { get; }
    int TotalNumberOfResults { get; }
  }
}