using Sitecore.Data;
using Sitecore.FakeDb;

namespace JCore.Foundation.SitecoreExtensions.Tests.Extensions
{
    public class MediaTemplate : DbTemplate
  {
    public MediaTemplate()
    {
      Add(new DbField("medialink", FieldId));
    }

    public ID FieldId { get; } = ID.NewID;
  }
}