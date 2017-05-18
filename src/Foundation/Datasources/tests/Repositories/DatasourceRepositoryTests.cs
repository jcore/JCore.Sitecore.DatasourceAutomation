using JCore.Foundation.Testing.Attributes;
using Sitecore.FakeDb;
using Sitecore.Data;
using JCore.Foundation.Datasources.Models;
using Sitecore;
using FluentAssertions;
using Xunit;
using Sitecore.Data.Items;
using System.Linq;

namespace JCore.Foundation.Datasources.Repositories.Tests
{
    public class DatasourceRepositoryTests
    {
        [Theory, AutoDbData]
        public void CreateItemDatasourceTest(Db db, DatasourceRepository repository)
        {
            var datasourceId = ID.NewID;
            // create a fake site context
            Sitecore.FakeDb.Sites.FakeSiteContext fakeSite = CreateFakeTreeStructure(db, datasourceId);

            // switch the context site
            using (new Sitecore.Sites.SiteContextSwitcher(fakeSite))
            {
                // process home item
                var homeItem = Context.Site.Database.GetItem(Context.Site.RootPath + Context.Site.StartItem);
                homeItem.Should().NotBeNull();

                repository.CreateItemDatasource(homeItem);
                var datasource = homeItem[NonChildDatasourceSupport.DatasourceFolderFieldName];
                datasource.Should().NotBeNullOrWhiteSpace();

                var datasourceContentItem = Context.Site.Database.GetItem(datasource);
                datasourceContentItem.Should().NotBeNull();
                datasourceContentItem.Name.Should().Be("Content");
                datasourceContentItem.Parent.Name.Should().Be("Datasources");

                // process landing page
                var landingItem = homeItem.Children.FirstOrDefault(i=>i.Name == "Landing");
                landingItem.Should().NotBeNull();

                repository.CreateItemDatasource(landingItem);
                var landingDatasource = landingItem[NonChildDatasourceSupport.DatasourceFolderFieldName];
                landingDatasource.Should().NotBeNullOrWhiteSpace();

                var landingDatasourceContentItem = Context.Site.Database.GetItem(landingDatasource);
                landingDatasourceContentItem.Should().NotBeNull();
                landingDatasourceContentItem.Name.Should().Be("Content");
                landingDatasourceContentItem.Parent.Name.Should().Be("Landing");
            }
        }

        [Theory, AutoDbData]
        public void DeleteItemDatasourceTest(Db db, DatasourceRepository repository)
        {
            var datasourceId = ID.NewID;
            // create a fake site context
            Sitecore.FakeDb.Sites.FakeSiteContext fakeSite = CreateFakeTreeStructure(db, datasourceId);

            // switch the context site
            using (new Sitecore.Sites.SiteContextSwitcher(fakeSite))
            {
                // process home page
                var homeItem = Context.Site.Database.GetItem(Context.Site.RootPath + Context.Site.StartItem);
                homeItem.Should().NotBeNull();

                repository.CreateItemDatasource(homeItem);
                var datasource = homeItem[NonChildDatasourceSupport.DatasourceFolderFieldName];
                datasource.Should().NotBeNullOrWhiteSpace();

                var datasourceContentItem = Context.Site.Database.GetItem(datasource);
                datasourceContentItem.Should().NotBeNull();

                // process landing page
                var landingItem = homeItem.Children.FirstOrDefault(i => i.Name == "Landing");
                landingItem.Should().NotBeNull();

                repository.CreateItemDatasource(landingItem);
                var landingDatasource = landingItem[NonChildDatasourceSupport.DatasourceFolderFieldName];
                landingDatasource.Should().NotBeNullOrWhiteSpace();

                repository.DeleteItemDatasource(landingItem);
                datasourceContentItem = Context.Site.Database.GetItem(landingDatasource);
                datasourceContentItem.Should().BeNull();

                // deleting Home page datasource
                repository.DeleteItemDatasource(homeItem);
                datasourceContentItem = Context.Site.Database.GetItem(datasource);
                datasourceContentItem.Should().BeNull();
            }
        }

        [Theory, AutoDbData]
        public void RenameItemDatasourceTest(Db db, DatasourceRepository repository)
        {
            var datasourceId = ID.NewID;
            // create a fake site context
            Sitecore.FakeDb.Sites.FakeSiteContext fakeSite = CreateFakeTreeStructure(db, datasourceId);

            // switch the context site
            using (new Sitecore.Sites.SiteContextSwitcher(fakeSite))
            {
                // process home page
                var homeItem = Context.Site.Database.GetItem(Context.Site.RootPath + Context.Site.StartItem);
                homeItem.Should().NotBeNull();

                repository.CreateItemDatasource(homeItem);
                var datasource = homeItem[NonChildDatasourceSupport.DatasourceFolderFieldName];
                datasource.Should().NotBeNullOrWhiteSpace();

                // process landing page
                var landingItem = homeItem.Children.FirstOrDefault(i => i.Name == "Landing");
                landingItem.Should().NotBeNull();

                repository.CreateItemDatasource(landingItem);
                var landingDatasource = landingItem[NonChildDatasourceSupport.DatasourceFolderFieldName];
                landingDatasource.Should().NotBeNullOrWhiteSpace();

                var datasourceContentItem = Context.Site.Database.GetItem(landingDatasource);
                datasourceContentItem.Should().NotBeNull();

                using(new EditContext(homeItem))
                {
                    homeItem.Name = "Landing 2";
                }

                repository.RenameItemDatasource(homeItem);
                datasourceContentItem = Context.Site.Database.GetItem(datasource);
                datasourceContentItem.Should().NotBeNull();
                datasourceContentItem.Name.Should().Be("Content");
                datasourceContentItem.Parent.Name.Should().Be("Landing 2");
            }
        }

        [Theory, AutoDbData]
        public void MoveItemDatasourceTest(Db db, DatasourceRepository repository)
        {
            var datasourceId = ID.NewID;
            // create a fake site context
            Sitecore.FakeDb.Sites.FakeSiteContext fakeSite = CreateFakeTreeStructure(db, datasourceId);

            // switch the context site
            using (new Sitecore.Sites.SiteContextSwitcher(fakeSite))
            {
                var homeItem = Context.Site.Database.GetItem(Context.Site.RootPath + Context.Site.StartItem);
                homeItem.Should().NotBeNull();

                repository.CreateItemDatasource(homeItem);
                var datasource = homeItem[NonChildDatasourceSupport.DatasourceFolderFieldName];
                datasource.Should().NotBeNullOrWhiteSpace();

                var datasourceContentItem = Context.Site.Database.GetItem(datasource);
                datasourceContentItem.Should().NotBeNull();

                // process landing page
                var landingItem = homeItem.Children.FirstOrDefault(i => i.Name == "Landing");
                landingItem.Should().NotBeNull();

                repository.CreateItemDatasource(landingItem);
                var landingDatasource = landingItem[NonChildDatasourceSupport.DatasourceFolderFieldName];
                landingDatasource.Should().NotBeNullOrWhiteSpace();

                var landingDatasourceContentItem = Context.Site.Database.GetItem(landingDatasource);
                landingDatasourceContentItem.Should().NotBeNull();

                // process article page
                var articleItem = landingItem.Children.FirstOrDefault(i => i.Name == "Article");

                repository.CreateItemDatasource(articleItem);
                var articleDatasource = articleItem[NonChildDatasourceSupport.DatasourceFolderFieldName];
                articleDatasource.Should().NotBeNullOrWhiteSpace();

                var articleDatasourceContentItem = Context.Site.Database.GetItem(articleDatasource);
                articleDatasourceContentItem.Should().NotBeNull();

                articleItem.MoveTo(homeItem);

                repository.MoveItemDatasource(articleItem);

                var datasourceRootItem = Context.Site.Database.GetItem(datasourceId);
                datasourceRootItem.Should().NotBeNull();

                var articleItemSitePath = articleItem.Paths.FullPath.Replace(homeItem.Paths.FullPath, "") + "/Content";
                var articleItemDatasourcePath = articleDatasourceContentItem.Paths.FullPath.Replace(datasourceRootItem.Paths.FullPath, "");
                articleItemSitePath.Should().Be(articleItemDatasourcePath);
            }
        }

        private static Sitecore.FakeDb.Sites.FakeSiteContext CreateFakeTreeStructure(Db db, ID datasourceId)
        {
            var fakeSite = new Sitecore.FakeDb.Sites.FakeSiteContext(
                         new Sitecore.Collections.StringDictionary
                           {
                    { "name", "website" },
                    { "database", "master" },
                    { "rootPath", "/sitecore/content/test" },
                    { "startItem", "/home" },
                    { "datasourceRootItem", datasourceId.ToString() }
                           });

            var nonChildDatasourceTemplate = new DbTemplate("NonChildDatasourceSupport", NonChildDatasourceSupport.SitecoreTemplateId) { NonChildDatasourceSupport.DatasourceFolderFieldName };
            db.Add(nonChildDatasourceTemplate);

            var datasourceFolderTemplate = new DbTemplate("DatasourceFolder", DatasourceFolder.SitecoreTemplateId);
            db.Add(datasourceFolderTemplate);

            var datasourceSubfolderTemplate = new DbTemplate("DatasourceSubfolder", DatasourceSubfolder.SitecoreTemplateId);
            db.Add(datasourceSubfolderTemplate);

            var landingPage = new DbItem("Test", ID.NewID, NonChildDatasourceSupport.SitecoreTemplateId) {
                new DbItem("Home", ID.NewID, NonChildDatasourceSupport.SitecoreTemplateId)
                {
                    new DbItem("Landing", ID.NewID, NonChildDatasourceSupport.SitecoreTemplateId)
                    {
                        new DbItem("Article", ID.NewID, NonChildDatasourceSupport.SitecoreTemplateId)
                    }
                },
                new DbItem("Datasources", datasourceId)
            };

            db.Add(landingPage);
            return fakeSite;
        }
    }
}