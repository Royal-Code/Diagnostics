using RoyalCode.Diagnostics;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Hosting;
using RoyalCode.Diagnostics.AspNetCore.Filters;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extensions de <see cref="IServiceCollection"/> e <see cref="IDiagnosticEventListenerBuilder"/>.
    /// </summary>
    public static class DiagnosticsServiceCollectionExtensions
    {
        /// <summary>
        /// Adds diagnostics observing services and returns a builder to configure diagnostics event observers.
        /// </summary>
        /// <param name="services">Service Collection.</param>
        /// <returns>A instance of <see cref="IDiagnosticEventListenerBuilder"/> to configure diagnostics event observers.</returns>
        public static IDiagnosticEventListenerBuilder AddDiagnosticListenerObserver(this IServiceCollection services)
        {
            if (services.Any(d => d.ServiceType == typeof(DiagnosticListenerObserver)))
                return new DefaultDiagnosticEventListenerBuilder(services);
            
            services.AddSingleton<DiagnosticListenerObserver>();
            services.AddSingleton<IStartupFilter, DiagnosticListenerObserverStartup>();

            return new DefaultDiagnosticEventListenerBuilder(services);
        }

        /// <summary>
        /// <para>
        ///     Subscribe the <see cref="DiagnosticListenerObserver"/> to <see cref="DiagnosticListener"/>.
        /// </para>
        /// <para>
        ///     This is necessary when you want to listen for diagnostic events via services 
        ///     and the host is a worker and <see cref="IStartupFilter"/> is not invoked.
        /// </para>
        /// </summary>
        /// <param name="host">The app host.</param>
        /// <returns>Same instace of <paramref name="host"/>.</returns>
        public static IHost SubscribeDiagnosticListenerObserver(this IHost host)
        {
            var observer = host.Services.GetRequiredService<DiagnosticListenerObserver>();
            DiagnosticListener.AllListeners.Subscribe(observer);
            return host;
        }

        /// <summary>
        /// <para>
        ///     Adds a service to observe diagnostic events.
        /// </para>
        /// <para>
        ///     The lifecycle of the service will be singleton.
        /// </para>
        /// </summary>
        /// <typeparam name="TService">The service type.</typeparam>
        /// <param name="builder"><see cref="IDiagnosticEventListenerBuilder"/>.</param>
        /// <returns>The same instance of <paramref name="builder"/> for chained calls.</returns>
        public static IDiagnosticEventListenerBuilder AddObserver<TService>(this IDiagnosticEventListenerBuilder builder)
            where TService : class, IDiagnosticEventObserver
        {
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IDiagnosticEventObserver, TService>());
            return builder;
        }

        /// <summary>
        /// Applies configurations to the <see cref="DiagnosticsOptions"/>.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="configure">The configuration action.</param>
        /// <returns>The same instance of <paramref name="services"/> for chained calls.</returns>
        public static IServiceCollection ConfigureDiagnosticsOptions(this IServiceCollection services,
            Action<DiagnosticsOptions> configure)
        {
            if (services is null)
                throw new ArgumentNullException(nameof(services));

            if (configure is null)
                throw new ArgumentNullException(nameof(configure));

            return services.Configure(configure);
        }
    }
}
