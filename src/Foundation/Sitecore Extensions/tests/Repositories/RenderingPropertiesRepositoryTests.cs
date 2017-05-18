﻿using JCore.Foundation.SitecoreExtensions.Repositories;
using JCore.Foundation.Testing.Attributes;
using FluentAssertions;
using Sitecore.Mvc.Common;
using Sitecore.Mvc.Presentation;
using Xunit;

namespace JCore.Foundation.SitecoreExtensions.Tests.Repositories
{
    public class RenderingPropertiesRepositoryTests
  {
    [Theory]
    [AutoDbData]
    public void ShouldInitObjectProperties()
    {
      var context = new RenderingContext();
      var rendering = new Rendering();
      var properties = new RenderingProperties(rendering);
      properties["Parameters"] = "property1=5&property2=10";
      context.Rendering = new Rendering
      {
        Properties = properties
      };
      var repository = new RenderingPropertiesRepository();
      ContextService.Get().Push(context);
      var resultObject = repository.Get<Model>();
      resultObject.Property1.ShouldBeEquivalentTo(5);
      resultObject.Property2.ShouldBeEquivalentTo(10);
    }

    public class Model
    {
      public string Property1 { get; set; }

      public string Property2 { get; set; }
    }
  }
}