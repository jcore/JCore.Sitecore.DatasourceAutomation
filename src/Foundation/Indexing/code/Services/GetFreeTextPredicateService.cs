using System;
using System.Linq;
using System.Linq.Expressions;
using Car.Foundation.Indexing.Models;
using Sitecore.ContentSearch.Linq.Utilities;
using Sitecore.ContentSearch.SearchTypes;

namespace Car.Foundation.Indexing.Services
{
    public static class GetFreeTextPredicateService
  {
    public static Expression<Func<SearchResultItem, bool>> GetFreeTextPredicate(string[] fieldNames, IQuery query)
    {
      var predicate = PredicateBuilder.False<SearchResultItem>();
      if (string.IsNullOrWhiteSpace(query.QueryText))
      {
        return predicate;
      }
      return fieldNames.Aggregate(predicate, (current, fieldName) => current.Or(i => i[fieldName].Contains(query.QueryText)));
    }
  }
}