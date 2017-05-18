using System;
using System.Collections.Generic;
using System.Linq;
using Car.Feature.Metadata.Models;
using Car.Foundation.Testing.Attributes;
using FluentAssertions;
using Xunit;

namespace Car.Feature.Metadata.Tests
{
    public class MetaKeywordsModelTests
  {
    [Theory]
    [AutoDbData]
    public void ToStrign_ShouldReturnCommaSeparatedListOfKeywords(List<string> keywords, MetaKeywordsModel model)
    {
      model.Keywords = keywords;
      var result = model.ToString();
      var keywordCollection = result.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries).ToList();
      var intersection = keywordCollection.Except(keywords);
      intersection.Count().Should().Be(0);
    }
  }
}