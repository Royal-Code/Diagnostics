using System.Diagnostics;
using Xunit;

namespace RoyalCode.Diagnostics.Tests;

public class T03_DiagnosticEventHandlersTests
{
    [Fact]
    public void T01_ListenEvent()
    {
        bool observed = false;
        const string eventName = "ListenEvent";
        void Callback() => observed = true;

        var observer = DiagnosticListenerObserver.Create(TestsDiagnostic.ListenerName, addHandlers =>
        {
            addHandlers(DiagnosticEventHandlers.For(eventName, Callback));
        });
        DiagnosticListener.AllListeners.Subscribe(observer);
        
        var operation = TestsDiagnostic.CreateTestAlphaOperation();
        operation.FireEvent(eventName);
        
        Assert.True(observed);
    }
}