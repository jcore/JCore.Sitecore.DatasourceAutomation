using System.Web;
using Car.Foundation.Dictionary.Repositories;
using Car.Foundation.Testing.Attributes;
using FluentAssertions;
using Sitecore.Collections;
using Sitecore.FakeDb;
using Sitecore.FakeDb.AutoFixture;
using Sitecore.FakeDb.Sites;
using Sitecore.Sites;
using Xunit;

namespace Car.Foundation.Dictionary.Tests.Repositories
{
    public class DictionaryPhraseRepositoryCacheTests
    {
        [Theory]
        [AutoDbData]
        public void Current_HttpContextIsntInitialized_CreateNewRepository([Content]DbItem item)
        {
            //Arrange
            var siteContext = new FakeSiteContext(new StringDictionary()
            {
                ["dictionaryPath"] = item.FullPath,
                ["database"] = "master"
            });
            HttpContext.Current = null;

            using (new SiteContextSwitcher(siteContext))
            {
                //Assert
                DictionaryPhraseRepository.Current.Should().BeOfType<DictionaryPhraseRepository>();
            }
        }

        [Theory]
        [AutoDbData]
        public void Current_HttpContextHasNotDictionaryItem_CreateNewRepositoryAndCacheIt([Content]DbItem item, HttpContext context)
        {
            //Arrange
            var siteContext = new FakeSiteContext(new StringDictionary()
            {
                ["dictionaryPath"] = item.FullPath,
                ["database"] = "master"
            });
            HttpContext.Current = context;

            using (new SiteContextSwitcher(siteContext))
            {
                //Act
                var result = DictionaryPhraseRepository.Current;
                //Assert
                result.Should().BeOfType<DictionaryPhraseRepository>();
                HttpContext.Current.Items["DictionaryPhraseRepository.Current"].Should().Be(result);
            }
        }
        [Theory]
        [AutoDbData]
        public void Current_HttpContextHasDictionaryItem_TakeReposiotryFromCache([Content]DbItem item, HttpContext context, IDictionaryPhraseRepository dictionaryPhraseRepository)
        {
            //Arrange
            var siteContext = new FakeSiteContext(new StringDictionary()
            {
                ["dictionaryPath"] = item.FullPath,
                ["database"] = "master"
            });

            HttpContext.Current = context;
            HttpContext.Current.Items["DictionaryPhraseRepository.Current"] = dictionaryPhraseRepository;

            using (new SiteContextSwitcher(siteContext))
            {
                //Act
                var result = DictionaryPhraseRepository.Current;
                //Assert
                result.Should().Be(dictionaryPhraseRepository);
            }
        }
    }
}
