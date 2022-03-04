
namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Internal default implementation of <see cref="IDiagnosticEventListenerBuilder"/>;
    /// </summary>
    internal class DefaultDiagnosticEventListenerBuilder : IDiagnosticEventListenerBuilder
    {
        public IServiceCollection Services { get; }

        internal DefaultDiagnosticEventListenerBuilder(IServiceCollection services)
        {
            Services = services ?? throw new ArgumentNullException(nameof(services));
        }
    }
}
