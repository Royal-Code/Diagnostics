using System.Diagnostics;

namespace RoyalCode.Diagnostics;

/// <summary>
/// Attribute to define the <see cref="DiagnosticListener"/> name
/// used by the <see cref="DiagnosticOperationFactory{TNames}"/>.
/// </summary>
public class DiagnosticListenerNameAttribute : Attribute
{
    /// <summary>
    /// The diagnostic listener name.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Creates a new attribute.
    /// </summary>
    /// <param name="name">The diagnostic listener name.</param>
    public DiagnosticListenerNameAttribute(string name)
    {
        Name = name;
    }
}