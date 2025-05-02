namespace MoneyTracker.Domain.Extensions;

public static class ObjectExtensions
{
    public static void Bind<T>(this T destination, T source)
    {
        var type = typeof(T);
        var properties = type.GetProperties();

        foreach (var property in properties)
        {
            if (property.CanWrite && property.CanRead && !property.GetGetMethod()!.IsVirtual)
            {
                var sourceValue = property.GetValue(source);
                property.SetValue(destination, sourceValue);
            }
        }
    }
}