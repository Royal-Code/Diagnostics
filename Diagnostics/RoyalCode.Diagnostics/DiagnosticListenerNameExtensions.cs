using System.Reflection;

namespace RoyalCode.Diagnostics;

/// <summary>
/// Extension methods for <see cref="Type"/> and <see cref="DiagnosticListenerNameAttribute"/>.
/// </summary>
public static class DiagnosticListenerNameExtensions
{
    /// <summary>
    /// Gets the diagnostic listener name from the <paramref name="type"/> through the <see cref="DiagnosticListenerNameAttribute"/>. 
    /// </summary>
    /// <param name="type">The <c>TName</c> of the <see cref="DiagnosticOperationFactory{TNames}"/></param>
    /// <returns>The diagnostic listener name</returns>
    /// <exception cref="InvalidOperationException">
    ///     If the attribute is not found in the type.
    /// </exception>
    public static string GetDiagnosticListenerName(this Type type)
    {
        return type.GetCustomAttribute<DiagnosticListenerNameAttribute>()?.Name
               ?? throw new InvalidOperationException(
                   $"The type '{type.FullName}' does not contains the attribute {nameof(DiagnosticListenerNameAttribute)}" +
                   $" and can not be used for the {nameof(DiagnosticOperationFactory<object>)}");
    }
}