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
        TestItems items = new();
        DiagnosticListener.AllListeners.Subscribe(new TestListnerObserver(items.Callback));

        var operation = TestsDiagnostic.CreateTestAlphaOperation();
        
        operation.FireEvent("TestAlpha");
        
        Assert.Single(items);
        Assert.Same(operation, items[0].Item2);
    }
    
    [Fact]
    public void T03_Event_Start_Stop()
    {
        TestItems items = new();
        DiagnosticListener.AllListeners.Subscribe(new TestListnerObserver(items.Callback));

        var operation = TestsDiagnostic.CreateTestAlphaOperation();
        
        operation.Start();
        operation.Dispose();
        
        Assert.Equal(2, items.Count);
        Assert.EndsWith(".Start", items[0].Item1);
        Assert.EndsWith(".Stop", items[1].Item1);
    }
    
    [Fact]
    public void T04_Event_Start_Error_Stop()
    {
        TestItems items = new();
        DiagnosticListener.AllListeners.Subscribe(new TestListnerObserver(items.Callback));

        var operation = TestsDiagnostic.CreateTestAlphaOperation();
        
        operation.Start();
        operation.FireError(new Exception("TestException"));
        
        operation.Dispose();
        
        Assert.Equal(3, items.Count);
        Assert.EndsWith(".Start", items[0].Item1);
        Assert.EndsWith(".Error", items[1].Item1);
        Assert.EndsWith(".Stop", items[2].Item1);
    }

    [Fact]
    public void T05_AddAndGetItem()
    {
        TestItems items = new();
        DiagnosticListener.AllListeners.Subscribe(new TestListnerObserver(items.Callback));
        
        var operation = TestsDiagnostic.CreateTestAlphaOperation();
        var data = new object();
        
        operation.Start();
        operation.AddItem(data);
        operation.Dispose();
        
        Assert.Equal(2, items.Count);
        Assert.Same(items[0].Item2, items[1].Item2);

        var items1 = (OperationItems) items[0].Item2;
        var items2 = (OperationItems) items[1].Item2;
        
        Assert.Same(items1.TryGetItem<object>(), items2.TryGetItem<object>());
    }

    [Fact]
    public void T06_Child_Operation()
    {
        TestItems items = new();
        DiagnosticListener.AllListeners.Subscribe(new TestListnerObserver(items.Callback));
        
        var operation = TestsDiagnostic.CreateTestAlphaOperation();
        operation.Start();
        
        var child = operation.Child("ChildOperation", true, true);
        child.Dispose();
        
        operation.Dispose();
        
        Assert.Equal(4, items.Count);
        Assert.Equal("TestAlpha.Start", items[0].Item1);
        Assert.Equal("ChildOperation.Start", items[1].Item1);
        Assert.Equal("ChildOperation.Stop", items[2].Item1);
        Assert.Equal("TestAlpha.Stop", items[3].Item1);
    }
    
    [Fact]
    public void T07_Child_Error()
    {
        TestItems items = new();
        DiagnosticListener.AllListeners.Subscribe(new TestListnerObserver(items.Callback));
        
        var operation = TestsDiagnostic.CreateTestAlphaOperation();
        operation.Start();
        
        var child = operation.ChildError(new Exception("Test"));
        child.Dispose();
        
        operation.Dispose();
        
        Assert.Equal(4, items.Count);
        Assert.Equal("TestAlpha.Start", items[0].Item1);
        Assert.Equal("TestAlphaError.Start", items[1].Item1);
        Assert.Equal("TestAlphaError.Stop", items[2].Item1);
        Assert.Equal("TestAlpha.Stop", items[3].Item1);
    }
}

public class TestItems : List<Tuple<string, object>>
{
    public void Callback(string eventName, object? eventData) =>
        Add(new Tuple<string, object>(eventName, eventData!));
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