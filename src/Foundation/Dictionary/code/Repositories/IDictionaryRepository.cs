using Sitecore.Sites;

namespace Car.Foundation.Dictionary.Repositories
{
    public interface IDictionaryRepository
    {
        Models.Dictionary Get(SiteContext site);
    }
}