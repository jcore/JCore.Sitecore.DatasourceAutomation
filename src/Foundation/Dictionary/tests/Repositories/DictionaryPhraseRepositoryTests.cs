using System;
using System.Collections.Generic;
using System.Linq;
using Car.Foundation.Dictionary.Repositories;
using Car.Foundation.Dictionary.Tests.Services;
using Car.Foundation.Testing.Attributes;
using FluentAssertions;
using Sitecore;
using Sitecore.Data.Items;
using Sitecore.FakeDb;
using Sitecore.FakeDb.AutoFixture;
using Sitecore.FakeDb.Sites;
using Xunit;

namespace Car.Foundation.Dictionary.Tests.Repositories
{
    public class DictionaryPhraseRepositoryTests
    {
        [Theory]
        [AutoDbData]
        public void Get_NullRElativePath_ThrowArgumentNullException(string defaultValue)
        {
            var repository = new DictionaryPhraseRepository(new Models.Dictionary());
            repository.Invoking(x => x.Get(null, defaultValue)).ShouldThrow<ArgumentNullException>();
        }

        [Theory]
        [AutoDbData]
        public void Get_DatabaseIsNull_ReturnDefaultValue(string relativePath, string defaultValue)
        {
            //Arrange
            Context.Database = null;
            var repository = new DictionaryPhraseRepository(new Models.Dictionary());

            //Act
            var result = repository.Get(relativePath, defaultValue);

            //Assert
            result.Should().Be(defaultValue);
        }

        [Theory]
        [AutoDbData]
        public void Get_AutocreateIsFalseEntryDoesntExists_ReturnDefaultValue(Db db, [Content]Item rootItem, string relativePath, string defaultValue)
        {
            //Arrange
            var repository = new DictionaryPhraseRepository(new Models.Dictionary() { Root = rootItem, AutoCreate = false });

            //Act
            var result = repository.Get(relativePath, defaultValue);

            //Assert
            result.Should().Be(defaultValue);
        }

        [Theory]
        [AutoDbData]
        public void Get_AutocreateIsTrueEntryDoesntExists_ShouldCreateItem(Db db, [Content]CreateDictionaryEntryServiceTests.DictionaryEntryTemplate entryTemplate, [Content]Item rootItem, IEnumerable<string> pathParts, string defaultValue)
        {
            //Arrange
            var relativePath = string.Join("/", pathParts.Select(ItemUtil.ProposeValidItemName));
            var repository = new DictionaryPhraseRepository(new Models.Dictionary() { Root = rootItem, AutoCreate = true });

            //Act
            var result = repository.Get(relativePath, defaultValue);

            //Assert
            result.Should().Be(defaultValue);
            rootItem.Axes.GetItem(relativePath).Should().NotBeNull();
        }

        [Theory]
        [AutoDbData]
        public void GetItem_NullRElativePath_ThrowArgumentNullException(string defaultValue)
        {
            var repository = new DictionaryPhraseRepository(new Models.Dictionary());
            repository.Invoking(x => x.GetItem(null, defaultValue)).ShouldThrow<ArgumentNullException>();
        }

        [Theory]
        [AutoDbData]
        public void GetItem_AutocreateIsFalseEntryDoesntExists_ReturnNull(Db db, [Content]Item rootItem, string relativePath, string defaultValue)
        {
            //Arrange
            var repository = new DictionaryPhraseRepository(new Models.Dictionary() { Root = rootItem, AutoCreate = false, Site = new FakeSiteContext("test") });

            //Act
            var result = repository.GetItem(relativePath, defaultValue);

            //Assert
            result.Should().BeNull();
        }

        [Theory]
        [AutoDbData]
        public void Get_AutocreateIsTrueEntryDoesntExists_ShouldreturnItem(Db db, [Content]CreateDictionaryEntryServiceTests.DictionaryEntryTemplate entryTemplate, [Content]Item rootItem, IEnumerable<string> pathParts, string defaultValue)
        {
            //Arrange
            var relativePath = string.Join("/", pathParts.Select(ItemUtil.ProposeValidItemName));
            var repository = new DictionaryPhraseRepository(new Models.Dictionary() { Root = rootItem, AutoCreate = true });

            //Act
            var result = repository.GetItem(relativePath, defaultValue);

            //Assert
            result[Templates.DictionaryEntry.Fields.Phrase].Should().Be(defaultValue);
        }

        [Theory]
        [AutoDbData]
        public void Get_IncorrectRelativePath_ThrowArgumentException(Db db, [Content]Item rootItem, string defaultValue)
        {
            //Arrange
            var relativePath = "/";
            var repository = new DictionaryPhraseRepository(new Models.Dictionary() { Root = rootItem, AutoCreate = true });

            //Assert
            repository.Invoking(x => x.Get(relativePath, defaultValue)).ShouldThrow<ArgumentException>();
        }
    }
}
