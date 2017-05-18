using System.Linq;
using Car.Feature.Metadata.Models;
using Car.Feature.Metadata.Repositories;
using Car.Foundation.Testing.Attributes;
using FluentAssertions;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.FakeDb;
using Sitecore.FakeDb.AutoFixture;
using Xunit;

namespace Car.Feature.Metadata.Tests
{
    public class MetadataRepositoryTests
    {
        [Theory]
        [AutoDbData]
        public void Get_ContextItemIsMatchingTemplate_ShouldReturnSelf(Db db, string contextItemName)
        {
            var contextItemId = ID.NewID;

            db.Add(new DbItem(contextItemName, contextItemId, Templates.SiteMetadata.ID));

            var contextItem = db.GetItem(contextItemId);
            var keywordsModel = MetadataRepository.Get(contextItem);
            keywordsModel.ID.Should().Be(contextItemId);
        }

        [Theory]
        [AutoDbData]
        public void Get_ContextItemParentIsMatchingTemplate_ShouldReturnParent(Db db)
        {
            var contextItemId = ID.NewID;

            db.Add(new DbItem("context", contextItemId, Templates.SiteMetadata.ID));
            var contextItem = db.GetItem(contextItemId);
            var keywordsModel = MetadataRepository.Get(contextItem.Add("child", new TemplateID(ID.NewID)));

            keywordsModel.ID.Should().Be(contextItemId);
        }

        [Theory]
        [AutoDbData]
        public void GetKeywords_ContextItem_ShouldReturnKeywordsModel(Db db, string contextItemName, string keyword1ItemName, string keyword2ItemName)
        {
            var contextItemId = ID.NewID;
            var keyword1Id = ID.NewID;
            var keyword2Id = ID.NewID;
            db.Add(new DbItem(contextItemName, contextItemId, Templates.PageMetadata.ID)
             {
               new DbField(Templates.PageMetadata.Fields.Keywords)
               {
                 {"en", $"{keyword1Id}|{keyword2Id}"}
               }
             });
            db.Add(new DbItem(keyword1ItemName, keyword1Id, Templates.Keyword.ID)
             {
               new DbField(Templates.Keyword.Fields.Keyword)
               {
                 {"en", keyword1ItemName}
               }
             });
            db.Add(new DbItem(keyword2ItemName, keyword2Id, Templates.Keyword.ID)
             {
               new DbField(Templates.Keyword.Fields.Keyword)
               {
                 {"en", keyword2ItemName}
               }
             });

            var contextItem = db.GetItem(contextItemId);
            var keywordsModel = MetadataRepository.GetKeywords(contextItem);
            keywordsModel.Should().BeOfType<MetaKeywordsModel>();
            keywordsModel.Keywords.Count().Should().Be(2);
        }

        [Theory]
        [AutoDbData]
        public void GetKeywords_ContextItemWithWrongTemplate_ShouldReturnNull([Content] Item contextItem)
        {
            MetadataRepository.GetKeywords(contextItem).Should().BeNull();
        }

        [Theory]
        [AutoDbData]
        public void GetRobots_ContextItem_ShouldReturnNullWhenItemIsNull()
        {
            MetadataRepository.GetRobots(null)
                .Should().BeNull();
        }

        [Theory]
        [AutoDbData]
        public void GetRobots_ContextItem_ShouldReturnNullWhenItemIsNotDerivedFromPageMetadata([Content] Item contextItem)
        {
            MetadataRepository.GetRobots(contextItem)
                .Should().BeNull();
        }

        [Theory]
        [AutoDbData]
        public void GetRobots_ContextItem_ShouldReturnIndexWhenCanIndexTrue(Db db)
        {
            var contextItemId = ID.NewID;
            db.Add(new DbItem("TestItem", contextItemId, Templates.PageMetadata.ID)
            {
                new DbField(Templates.PageMetadata.Fields.CanIndex)
                {
                    {"en", "1"}
                }
            });
            var contextItem = db.GetItem(contextItemId);

            var context = MetadataRepository.GetRobots(contextItem);
            context.Should().Contain("index");
        }

        [Theory]
        [AutoDbData]
        public void GetRobots_ContextItem_ShouldReturnNoindexWhenCanIndexFalse(Db db)
        {
            var contextItemId = ID.NewID;
            db.Add(new DbItem("TestItem", contextItemId, Templates.PageMetadata.ID)
            {
                new DbField(Templates.PageMetadata.Fields.CanIndex)
                {
                    {"en", ""}
                }
            });
            var contextItem = db.GetItem(contextItemId);

            var context = MetadataRepository.GetRobots(contextItem);
            context.Should().Contain("noindex");
        }

        [Theory]
        [AutoDbData]
        public void GetRobots_ContextItem_ShouldReturnFollowWhenCanFollowTrue(Db db)
        {
            var contextItemId = ID.NewID;
            db.Add(new DbItem("TestItem", contextItemId, Templates.PageMetadata.ID)
            {
                new DbField(Templates.PageMetadata.Fields.CanFollow)
                {
                    {"en", "1"}
                }
            });
            var contextItem = db.GetItem(contextItemId);

            var context = MetadataRepository.GetRobots(contextItem);
            context.Should().Contain("follow");
        }

        [Theory]
        [AutoDbData]
        public void GetRobots_ContextItem_ShouldReturnNofollowWhenCanFollowFalse(Db db)
        {
            var contextItemId = ID.NewID;
            db.Add(new DbItem("TestItem", contextItemId, Templates.PageMetadata.ID)
            {
                new DbField(Templates.PageMetadata.Fields.CanFollow)
                {
                    {"en", ""}
                }
            });
            var contextItem = db.GetItem(contextItemId);

            var context = MetadataRepository.GetRobots(contextItem);
            context.Should().Contain("nofollow");
        }
    }
}