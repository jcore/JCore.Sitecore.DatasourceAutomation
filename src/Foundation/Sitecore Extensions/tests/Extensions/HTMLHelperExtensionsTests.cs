using System;
using System.Web.Mvc;
using System.Xml.Linq;
using JCore.Foundation.SitecoreExtensions.Extensions;
using JCore.Foundation.Testing.Attributes;
using FluentAssertions;
using Sitecore.Extensions.XElementExtensions;
using Sitecore.Mvc;
using Sitecore.Mvc.Common;
using Sitecore.Mvc.Presentation;
using Xunit;

namespace JCore.Foundation.SitecoreExtensions.Tests.Extensions
{
    public class HTMLHelperExtensionsTests
  {
    [Theory]
    [AutoDbData]
    public void AddUniqueFormId_CurrentRenderingNull_ShouldReturnNull(HtmlHelper helper)
    {
      helper.AddUniqueFormId().Should().BeNull();
    }

    [Theory]
    [AutoDbData]
    public void AddUniqueFormId_CurrentRenderingNotNull_ShouldReturnHiddenInput(HtmlHelper helper)
    {
      var id = Guid.NewGuid();
      ContextService.Get().Push(new RenderingContext() {Rendering =  new Rendering() {UniqueId = id} });
      helper.Sitecore().CurrentRendering.Should().NotBeNull();
      var xml = XDocument.Parse(helper.AddUniqueFormId().ToString());
      xml.Root.Name.LocalName.Should().Be("input");
      xml.Root.GetAttributeValue("name").Should().Be("uid");
      Guid.Parse(xml.Root.GetAttributeValue("value")).Should().Be(id);
    }
  }
}
