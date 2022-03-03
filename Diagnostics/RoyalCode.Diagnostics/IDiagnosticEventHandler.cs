namespace RoyalCode.Diagnostics
{
    /// <summary>
    /// Handler for diagnostic events.
    /// </summary>
    public interface IDiagnosticEventHandler
    {
        /// <summary>
        /// Event name.
        /// </summary>
        string EventName { get; }

        /// <summary>
        /// Handles the event.
        /// </summary>
        /// <param name="eventArgs">The event argument.</param>
        void Handle(object eventArgs);
    }
}
