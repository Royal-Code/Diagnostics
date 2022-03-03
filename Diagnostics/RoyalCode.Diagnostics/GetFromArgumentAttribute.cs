namespace RoyalCode.Diagnostics;

/// <summary>
/// <para>
///     Attribute for class properties with <see cref="ArgumentAdapterAttribute"/>.
/// </para>
/// <para>
///     Only properties declared with this attribute will be assigned.
/// </para>
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
public class GetFromArgumentAttribute : Attribute
{
    /// <summary>
    /// <para>
    ///     Name of the property, or Path (CamelCase) to access it.
    /// </para>
    /// <para>
    ///     Defines where the value that will be assigned to the property decorated with this attribute will come from.
    /// </para>
    /// <para>
    ///     This value is optional, when not informed,
    ///     the same name of the property decorated with this attribute will be used.
    /// </para>
    /// </summary>
    public string? PropertyName { get; set; }

    /// <summary>
    /// <para>
    ///     If property is required, it must exist.
    /// </para>
    /// <para>
    ///     When it is required and the property is not found,
    ///     an exception will be thrown. If the property is not required,
    ///     only the property decorated with this attribute will not be assigned.
    /// </para>
    /// </summary>
    public bool Required { get; set; } = true;

    /// <summary>
    /// Creates a new attribute, without informing the property from which the value will come.
    /// </summary>
    public GetFromArgumentAttribute()
    {
    }

    /// <summary>
    /// Creates a new attribute, informing the property where the value will come from.
    /// </summary>
    /// <param name="propertyName">Name of the property, or Path (CamelCase) to access it.</param>
    public GetFromArgumentAttribute(string propertyName)
    {
        PropertyName = propertyName;
    }
}