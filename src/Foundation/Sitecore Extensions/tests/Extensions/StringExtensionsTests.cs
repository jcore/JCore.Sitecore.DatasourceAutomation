using JCore.Foundation.SitecoreExtensions.Extensions;
using FluentAssertions;
using Xunit;

namespace JCore.Foundation.SitecoreExtensions.Tests.Extensions
{
    public class StringExtensionsTests
  {
    [Theory]
    [InlineData("TestString", "Test String")]
    [InlineData("Test String", "Test String")]
    public void Humanize_ShouldReturnValueSplittedWithWhitespaces(string input, string expected)
    {
      input.Humanize().Should().Be(expected);
    }

    [Theory]
    [InlineData("  ", "none")]
    [InlineData("", "none")]
    [InlineData("somePath", "url('somePath')")]
    public void ToCssUrlValue_ShouldReturnValueSplittedWithWhitespaces(string input, string expected)
    {
      input.ToCssUrlValue().Should().Be(expected);
    }
  }
}