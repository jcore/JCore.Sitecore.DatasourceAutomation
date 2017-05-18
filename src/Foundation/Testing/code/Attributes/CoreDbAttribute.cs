using System;
using System.Reflection;
using JCore.Foundation.Testing.Builders;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Xunit2;
using Sitecore.FakeDb;

namespace JCore.Foundation.Testing.Attributes
{
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = true)]
  public class CoreDbAttribute : CustomizeAttribute
  {
    public override ICustomization GetCustomization(ParameterInfo dbParameter)
    {
      if (dbParameter == null)
      {
        throw new ArgumentNullException(nameof(dbParameter));
      }

      if (dbParameter.ParameterType != typeof(Db))
      {
        throw new InvalidOperationException($"{this.GetType().Name} can be applied only to {typeof(Db).Name} parameter");
      }

      return new CoreDbCustomization();
    }
  }
}