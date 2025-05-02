namespace MoneyTracker.Test.Extensions;

using MoneyTracker.Domain.Extensions;
using Xunit;

public class ObjectExtensionsTest
{
    [Fact]
    public void Bind_ShouldBindProperties()
    {
        var source = new TestClass
        {
            Property1 = "Property1"
        };
        var destination = new TestClass();

        destination.Bind(source);

        Assert.Equal(source.Property1, destination.Property1);
    }

    private sealed class TestClass
    {
        public string? Property1 { get; set; }
    }
}