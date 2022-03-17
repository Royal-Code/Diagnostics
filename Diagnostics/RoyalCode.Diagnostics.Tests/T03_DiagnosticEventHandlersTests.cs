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
    
    [Fact]
    public void T02_ListenEventWithOneArg()
    {
        OperationArgumentOne? arg1 = null;
        const string eventName = "ListenEvent";
        void Callback(OperationArgumentOne a1) => arg1 = a1;

        var observer = DiagnosticListenerObserver.Create(TestsDiagnostic.ListenerName, addHandlers =>
        {
            addHandlers(DiagnosticEventHandlers.For<OperationArgumentOne>(eventName, Callback));
        });
        DiagnosticListener.AllListeners.Subscribe(observer);
        
        var operation = TestsDiagnostic.CreateTestAlphaOperation();
        operation.AddItem(new OperationArgumentOne {Value = "One"});
        operation.FireEvent(eventName);
        
        Assert.NotNull(arg1);
        Assert.Equal("One", arg1!.Value);
    }
    
    [Fact]
    public void T03_ListenEventWithTwoArg()
    {
        OperationArgumentOne? arg1 = null;
        IOperationArgumentWithValue? arg2 = null;
        const string eventName = "ListenEvent";
        void Callback(OperationArgumentOne a1, IOperationArgumentWithValue a2)
        {
            arg1 = a1;
            arg2 = a2;
        }

        var observer = DiagnosticListenerObserver.Create(TestsDiagnostic.ListenerName, addHandlers =>
        {
            addHandlers(DiagnosticEventHandlers.For<OperationArgumentOne, IOperationArgumentWithValue>(eventName, Callback));
        });
        DiagnosticListener.AllListeners.Subscribe(observer);
        
        var operation = TestsDiagnostic.CreateTestAlphaOperation();
        operation.With(new OperationArgumentOne {Value = "Two"})
            .Add()
            .AddAs<IOperationArgumentWithValue>();
        operation.FireEvent(eventName);
        
        Assert.NotNull(arg1);
        Assert.Equal("Two", arg1!.Value);
        Assert.NotNull(arg2);
        Assert.Equal("Two", arg2!.Value);
    }
    
    [Fact]
    public void T04_ListenEventWithThreeArg()
    {
        OperationArgumentOne? arg1 = null;
        IOperationArgumentWithValue? arg2 = null;
        OperationArgumentThree? arg3 = null;

        var date = DateTime.Now;
        
        const string eventName = "ListenEvent";
        void Callback(OperationArgumentOne a1, IOperationArgumentWithValue a2, OperationArgumentThree a3)
        {
            arg1 = a1;
            arg2 = a2;
            arg3 = a3;
        }

        var observer = DiagnosticListenerObserver.Create(TestsDiagnostic.ListenerName, addHandlers =>
        {
            addHandlers(DiagnosticEventHandlers.For<OperationArgumentOne, IOperationArgumentWithValue, OperationArgumentThree>(eventName, Callback));
        });
        DiagnosticListener.AllListeners.Subscribe(observer);
        
        var operation = TestsDiagnostic.CreateTestAlphaOperation();
        operation.With(new OperationArgumentThree() {Value = "Three", DateTime = date})
            .Add()
            .AddAs<OperationArgumentOne>()
            .AddAs<IOperationArgumentWithValue>();
        operation.FireEvent(eventName);
        
        Assert.NotNull(arg1);
        Assert.Equal("Three", arg1!.Value);
        Assert.NotNull(arg2);
        Assert.Equal("Three", arg2!.Value);
        Assert.NotNull(arg3);
        Assert.Equal("Three", arg3!.Value);
        Assert.Equal(date, arg3!.DateTime);
    }
}

public interface IOperationArgumentWithValue
{
    public string? Value { get; }
}

public class OperationArgumentOne : IOperationArgumentWithValue
{
    public string? Value { get; init; }
}

public class OperationArgumentThree : OperationArgumentOne
{
    public DateTime DateTime { get; init; }
}