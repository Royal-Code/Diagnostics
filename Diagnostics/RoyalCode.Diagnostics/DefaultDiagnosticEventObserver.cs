namespace RoyalCode.Diagnostics;

/// <summary>
/// <para>
///     A default implementation of <see cref="IDiagnosticEventObserver"/> for manual
///     configure diagnostic listeners.
/// </para>
/// <para>
///     Is possible to use the <see cref="DiagnosticListenerObserver.Create(string, Action{Action{IDiagnosticEventHandler}})"/>.
/// </para>
/// </summary>
public sealed class DefaultDiagnosticEventObserver : DiagnosticEventObserverBase
{
    /// <summary>
    /// Creates a new observer for the diagnostic listener name.
    /// </summary>
    /// <param name="diagnosticListenerName">The name of the listener.</param>
    /// <param name="handlers">The event handlers</param>
    public DefaultDiagnosticEventObserver(string diagnosticListenerName,
        params IDiagnosticEventHandler[] handlers)
        : base(handlers)
    {
        DiagnosticListenerName = diagnosticListenerName;
    }
    
    /// <summary>
    /// The name of the listener.
    /// </summary>
    public override string DiagnosticListenerName { get; }
    
    /// <inheritdoc />
    protected override IEnumerable<IDiagnosticEventHandler> CreateHandlers() 
        => Enumerable.Empty<IDiagnosticEventHandler>();
}