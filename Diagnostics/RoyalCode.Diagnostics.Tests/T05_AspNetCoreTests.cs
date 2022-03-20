
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Xunit;

namespace RoyalCode.Diagnostics.Tests;

public class T05_AspNetCoreTests
{
    [Fact]
    public async Task T01_ObserveFromServiceForWorker()
    {
        var app = Host.CreateDefaultBuilder()
            .ConfigureServices(services =>
            {
                services.AddDiagnosticListenerObserver().AddObserver<AspNetCoreDiagnosticEventObserver>();
            }).Build();

        app.SubscribeDiagnosticListenerObserver();

        var startTask = app.RunAsync();

        var observer = app.Services.GetService<IDiagnosticEventObserver>() as AspNetCoreDiagnosticEventObserver;

        Assert.NotNull(observer);
        Assert.Empty(observer!.Operations);

        var operation = TestsDiagnostic.CreateTestAlphaOperation();
        operation.FireEvent(nameof(T01_ObserveFromServiceForWorker));

        Assert.Single(observer!.Operations);

        await app.StopAsync(new CancellationToken(true));
    }

    [Fact]
    public async Task T02_ObserveFromServiceForWeb()
    {

        var app = WebHost.CreateDefaultBuilder<TestStartup>(new string[0])
            .ConfigureServices(services =>
            {
                services.AddDiagnosticListenerObserver().AddObserver<AspNetCoreDiagnosticEventObserver>();
            })
            .Build();

        WebHost.CreateDefaultBuilder();

        app.Start();

        var observer = app.Services.GetService<IDiagnosticEventObserver>() as AspNetCoreDiagnosticEventObserver;

        Assert.NotNull(observer);
        Assert.Empty(observer!.Operations);

        var operation = TestsDiagnostic.CreateTestAlphaOperation();
        operation.FireEvent(nameof(T02_ObserveFromServiceForWeb));

        Assert.Single(observer!.Operations);

        await app.StopAsync(new CancellationToken(true));
    }
}

public class AspNetCoreDiagnosticEventObserver : DiagnosticEventObserverBase
{
    public ICollection<DiagnosticOperation> Operations { get; } = new List<DiagnosticOperation>();

    public override string DiagnosticListenerName => "RoyalCode.Diagnostics.Tests";

    protected override IEnumerable<IDiagnosticEventHandler> CreateHandlers()
    {
        yield return DiagnosticEventHandlers.For<DiagnosticOperation>(
                nameof(T05_AspNetCoreTests.T01_ObserveFromServiceForWorker),
                EventObserverHandler);

        yield return DiagnosticEventHandlers.For<DiagnosticOperation>(
                nameof(T05_AspNetCoreTests.T02_ObserveFromServiceForWeb),
                EventObserverHandler);
    }

    private void EventObserverHandler(DiagnosticOperation obj)
    {
        Operations.Add(obj);
    }
}

public class TestStartup
{
    public void Configure(IApplicationBuilder app)
    {
        app.UseRouting();
    }
}