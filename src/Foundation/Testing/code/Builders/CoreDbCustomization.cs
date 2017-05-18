using System.Linq;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Dsl;
using Sitecore.FakeDb;

namespace JCore.Foundation.Testing.Builders
{
    public class CoreDbCustomization : ICustomization
  {
    public void Customize(IFixture fixture)
    {

      foreach (var customization in fixture.Customizations.Where(c => c is NodeComposer<Db>))
      {
        fixture.Customizations.Remove(customization);
      }

      fixture.Inject(new Db("core"));
    }
  }
}