using System.Linq;
using Ploeh.AutoFixture.Kernel;
using Sitecore.FakeDb;

namespace JCore.Foundation.Testing.Commands
{
    public class AddContentDbItemsCommand : GenericCommand<DbItem[]>
  {
    protected override void ExecuteAction(DbItem[] specimen, ISpecimenContext context)
    {
      var db = (Db)context.Resolve(typeof(Db));
      specimen.ToList().ForEach(db.Add);
    }
  }
}