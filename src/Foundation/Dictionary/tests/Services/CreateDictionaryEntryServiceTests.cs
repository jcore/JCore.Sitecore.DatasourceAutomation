using System.Collections.Generic;
using System.Linq;
using Car.Foundation.Testing.Attributes;
using FluentAssertions;
using Ploeh.AutoFixture;
using Sitecore.Data.Items;
using Sitecore.FakeDb;
using Sitecore.FakeDb.AutoFixture;
using Xunit;

namespace Car.Foundation.Dictionary.Tests.Services
{
    using Car.Foundation.Dictionary.Models;
    using Car.Foundation.Dictionary.Services;

    public class CreateDictionaryEntryServiceTests
    {
        [Theory]
        [AutoDbData]
        public void CreateDictionaryEntry_Call_CreateItems(Db db, [Content]DictionaryEntryTemplate entryTemplate, [Content]DbItem rootItem, IEnumerable<string> pathParts, string defaultValue)
        {
            //Arrange
            var dictionary = new Dictionary()
            {
                Root = db.GetItem(rootItem.ID)
            };

            var path = string.Join("/", pathParts.Select(ItemUtil.ProposeValidItemName));

            //Act
            var phraseItem = CreateDictionaryEntryService.CreateDictionaryEntry(dictionary, path, defaultValue);

            //Assert
            phraseItem.Should().NotBeNull();
            phraseItem.TemplateID.Should().Be(Templates.DictionaryEntry.ID);
            phraseItem.Paths.FullPath.Should().Be($"{rootItem.FullPath}/{path}");
            phraseItem[Templates.DictionaryEntry.Fields.Phrase].Should().Be(defaultValue);
        }

        public class DictionaryEntryTemplate : DbTemplate
        {
            public DictionaryEntryTemplate() : base(Templates.DictionaryEntry.ID)
            {
                this.Add(Templates.DictionaryEntry.Fields.Phrase);
            }
        }
    }
}
