namespace MoneyTracker.Test.Extensions;

using MoneyTracker.Domain.Extensions;
using Xunit;

public class StringExtensionTest
{
    [Fact]
    public void ToUpperFirstLetter_ShouldReturnStringWithFirstLetterUpperCase()
    {
        var str = "hello, world!";

        var result = str.ToUpperFirstLetter();

        Assert.Equal("Hello, world!", result);
    }

    [Fact]
    public void ToUpperFirstLetter_ShouldReturnEmptyString_WhenInputIsEmpty()
    {
        var str = string.Empty;

        var result = str.ToUpperFirstLetter();

        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void ToLowerFirstLetter_ShouldReturnStringWithFirstLetterLowerCase()
    {
        var str = "Hello, World!";

        var result = str.ToLowerFirstLetter();

        Assert.Equal("hello, World!", result);
    }

    [Fact]
    public void ToLowerFirstLetter_ShouldReturnEmptyString_WhenInputIsEmpty()
    {
        var str = string.Empty;

        var result = str.ToLowerFirstLetter();

        Assert.Equal(string.Empty, result);
    }
}