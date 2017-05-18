using System.Web.Mvc;
using JCore.Foundation.Testing.Attributes;

namespace JCore.Foundation.SitecoreExtensions.Tests
{
    public class AutoDbMvcDataAttribute : AutoDbDataAttribute
  {
    public AutoDbMvcDataAttribute()
    {
      Fixture.Customize<ControllerContext>(c => c.Without(x => x.DisplayMode));
    }
  }
}