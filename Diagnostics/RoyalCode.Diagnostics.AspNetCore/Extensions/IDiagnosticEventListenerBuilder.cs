namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Builder for add and configure 'Diagnostic' services.
    /// </summary>
    public interface IDiagnosticEventListenerBuilder 
    {
        /// <summary>
        /// The <see cref="IServiceCollection"/>.
        /// </summary>
        IServiceCollection Services { get; }
    }
}
