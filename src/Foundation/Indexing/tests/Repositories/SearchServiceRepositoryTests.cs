using Car.Foundation.Indexing.Models;
using Car.Foundation.Indexing.Repositories;
using Car.Foundation.Indexing.Services;
using Car.Foundation.Testing.Attributes;
using FluentAssertions;
using Ploeh.AutoFixture.Xunit2;
using Xunit;

namespace Car.Foundation.Indexing.Tests.Repositories
{
    public class SearchServiceRepositoryTests
  {
    [Theory]
    [AutoDbData]
    public void Get_ReturnsSearchService([Frozen] ISearchSettings settings, SearchServiceRepository serviceRepository)
    {
      var result = serviceRepository.Get();
      result.Should().BeOfType<SearchService>();
    }
  }
}
