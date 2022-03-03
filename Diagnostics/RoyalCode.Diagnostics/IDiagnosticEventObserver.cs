
namespace RoyalCode.Diagnostics;

/// <summary>
/// <para>
///     Observer of diagnostic events.
/// </para>
/// <para>
///     This component is used by the <see cref="DiagnosticListenerObserver"/>.
/// </para>
/// <para>
///     See also the abstract implementation <see cref="DiagnosticEventObserverBase"/>.
/// </para>
/// </summary>
public interface IDiagnosticEventObserver : IObserver<KeyValuePair<string, object?>>
{
    /// <summary>
    /// <para>
    ///     Event listener name.
    /// </para>
    /// </summary>
    string DiagnosticListenerName { get; }

    /// <summary>
    /// If observations of a certain event are activated.
    /// </summary>
    /// <param name="eventName">The event name.</param>
    /// <returns>True if activated, false otherwise.</returns>
    bool IsEnabled(string eventName);
}