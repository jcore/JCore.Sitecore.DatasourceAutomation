using Car.Foundation.Indexing.Models;
using Car.Foundation.Indexing.Services;

namespace Car.Foundation.Indexing.Repositories
{
    public class SearchServiceRepository : ISearchServiceRepository
  {
    private readonly ISearchSettings settings;

    public SearchServiceRepository() : this(new SearchSettingsBase())
    {
    }

    public SearchServiceRepository(ISearchSettings searchSettings)
    {
      this.settings = searchSettings;
    }

    public virtual SearchService Get()
    {
      return new SearchService(this.settings);
    }
  }
}