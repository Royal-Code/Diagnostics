using System.Diagnostics;
using Xunit;

namespace RoyalCode.Diagnostics.Tests;

public class T02_DiagnosticsOperationFactoryTests
{
    [Fact]
    public void T01_AssertNames()
    {
        Assert.Equal("RoyalCode.Diagnostics.Tests", TestsDiagnostic.ListenerName);
        Assert.Equal("RoyalCode.Diagnostics.Other", OtherTestsDiagnostic.ListenerName);
    }

    [Fact]
    public void T02_SendEvent()
    {
        List<Tuple<string, object?>> items = new();
        object testData = new object();

        void callback(string eventName, object? eventData) =>
            items.Add(new Tuple<string, object?>(eventName, eventData));

        DiagnosticListener.AllListeners.Subscribe(new TestListnerObserver(callback));

        var operation = TestsDiagnostic.CreateTestAlphaOperation();
        
        operation.AddItem(testData);
        operation.FireEvent("TestAlpha");
        
        Assert.Single(items);
        Assert.Same(operation, items[0].Item2);
    }
    
    [Fact]
    public void T03_Event_Start_Stop()
    {
        List<Tuple<string, object?>> items = new();

        void callback(string eventName, object? eventData) =>
            items.Add(new Tuple<string, object?>(eventName, eventData));

        DiagnosticListener.AllListeners.Subscribe(new TestListnerObserver(callback));

        var operation = TestsDiagnostic.CreateTestAlphaOperation();
        
        operation.Start();
        operation.Dispose();
        
        Assert.Equal(2, items.Count);
        Assert.EndsWith(".Start", items[0].Item1);
        Assert.EndsWith(".Stop", items[1].Item1);
    }
}

[DiagnosticListenerName("RoyalCode.Diagnostics.Tests")]
public class TestsDiagnostic : DiagnosticOperationFactory<TestsDiagnostic>
{
    public static DiagnosticOperation CreateTestAlphaOperation()
        => CreateOperation("TestAlpha");
}

[DiagnosticListenerName("RoyalCode.Diagnostics.Other")]
public class OtherTestsDiagnostic : DiagnosticOperationFactory<OtherTestsDiagnostic>
{
    
}

public class TestListnerObserver : IObserver<DiagnosticListener>
{
    private readonly Action<string, object?> callback;

    public TestListnerObserver(Action<string, object?> callback)
    {
        this.callback = callback;
    }

    public void OnCompleted() { }

    public void OnError(Exception error) { }

    public void OnNext(DiagnosticListener value)
    {
        value.Subscribe(new TestEventObserver(callback));
    }
}

public class TestEventObserver : IObserver<KeyValuePair<string, object?>>
{
    private readonly Action<string, object?> callback;

    public TestEventObserver(Action<string, object?> callback)
    {
        this.callback = callback;
    }

    public void OnCompleted() { }

    public void OnError(Exception error) { }

    public void OnNext(KeyValuePair<string, object?> value)
    {
        callback(value.Key, value.Value);
    }
}