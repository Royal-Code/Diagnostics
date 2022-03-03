namespace RoyalCode.Diagnostics;

/// <summary>
/// <para>
///     Attribute for adapter types (classes) of the diagnostic event arguments.
/// </para>
/// <para>
///     Used by the component <see cref="EventArgumentGetterFactory"/> to adapt a diagnosis event object to another,
///     where the decorated class with this attribute will be instantiated
///     and the properties of the event object will be assigned to the object with this attribute.
/// </para>
/// <para>
///     Only properties declared with <see cref="GetFromArgumentAttribute"/> will be assigned.
/// </para>
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class ArgumentAdapterAttribute : Attribute
{
    /// <summary>
    /// <para>
    ///     Several ' Assembly Qualified' type names that may exist as an argument to diagnostic events.
    /// </para>
    /// <para>
    ///     Optional.
    /// </para>
    /// </summary>
    public string[]? GetFromTypes { get; set; }
}