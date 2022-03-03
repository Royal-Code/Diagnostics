using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace RoyalCode.Diagnostics
{
    /// <summary>
    /// <para>
    ///     A <see cref="IObserver{T}"/> of <see cref="DiagnosticListener"/>,
    ///     registered through the <see cref="DiagnosticListener.AllListeners"/>
    ///     to enroll observers of diagnostic events.
    /// </para>
    /// <para>
    ///     This component uses all the <see cref="IDiagnosticEventObserver"/> to register them as event observers.
    /// </para>
    /// <para>
    ///     Implement and register <see cref="IDiagnosticEventObserver"/> to listen for events.
    /// </para>
    /// <para>
    ///     It requires the initialization of the component at system startup.
    /// </para>
    /// </summary>
    public class DiagnosticListenerObserver : IObserver<DiagnosticListener>
    {
        private readonly IEnumerable<IDiagnosticEventObserver> eventObservers;
        private readonly DiagnosticsOptions options;

        /// <summary>
        /// Create a new diagnostic observer.
        /// </summary>
        /// <param name="eventObservers">The event observer.</param>
        /// <param name="options">Options.</param>
        public DiagnosticListenerObserver(
            IEnumerable<IDiagnosticEventObserver> eventObservers,
            IOptions<DiagnosticsOptions> options)
        {
            if (options is null)
                throw new ArgumentNullException(nameof(options));

            this.eventObservers = eventObservers ?? throw new ArgumentNullException(nameof(eventObservers));
            this.options = options.Value;
        }

        /// <summary>
        /// Nop.
        /// </summary>
        public void OnCompleted() { }

        /// <summary>
        /// Nop.
        /// </summary>
        /// <param name="error"></param>
        public void OnError(Exception error) { }

        /// <summary>
        /// If the option is enabled, event observers (<see cref="IDiagnosticEventObserver"/>) will be subscribed.
        /// </summary>
        /// <param name="listener">Um <see cref="DiagnosticListener"/>.</param>
        public void OnNext(DiagnosticListener listener)
        {
            if (!options.Enabled) 
                return;
            
            foreach (var observer in eventObservers)
            {
                if (observer.DiagnosticListenerName.Equals(listener.Name, StringComparison.OrdinalIgnoreCase))
                {
                    listener.Subscribe(observer, observer.IsEnabled);
                }
            }
        }
    }
}
