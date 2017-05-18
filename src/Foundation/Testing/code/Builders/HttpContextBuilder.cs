﻿using System.Web;
using Ploeh.AutoFixture.Kernel;

namespace JCore.Foundation.Testing.Builders
{
    public class HttpContextBuilder : ISpecimenBuilder
  {
    public object Create(object request, ISpecimenContext context)
    {
      if (typeof(HttpContext).Equals(request))
      {
        return HttpContextMockFactory.Create();
      }

      return new NoSpecimen();
    }
  }
}