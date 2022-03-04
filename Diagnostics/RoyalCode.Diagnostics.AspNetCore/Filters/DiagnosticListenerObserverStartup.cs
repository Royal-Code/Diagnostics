using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

namespace RoyalCode.Diagnostics.AspNetCore.Filters
{
    /// <summary>
    /// Statup filter to initialize the <see cref="DiagnosticListenerObserver"/>.
    /// </summary>
    public class DiagnosticListenerObserverStartup : IStartupFilter
    {
        /// <summary>
        /// Initializes, along with the aspnetcore host, the <see cref="DiagnosticListenerObserver"/>.
        /// </summary>
        /// <param name="next">Action of pipeline to configure the services and subcribe the diagnostic listener.</param>
        /// <returns>A new action decorating the application builder pipeline.</returns>
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return app =>
            {
                next(app);
                var observer = app.ApplicationServices.GetRequiredService<DiagnosticListenerObserver>();
                DiagnosticListener.AllListeners.Subscribe(observer);
            };
        }
    }
}
