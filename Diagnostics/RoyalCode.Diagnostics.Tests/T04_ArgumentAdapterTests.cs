using System.Diagnostics;
using Xunit;

namespace RoyalCode.Diagnostics.Tests;

public class T04_ArgumentAdapterTests
{
    [Fact]
    public void T01_SimpleObserverEvent()
    {
        object? arg1 = null;
        void Callback(object a1) => arg1 = a1;
        var observer = DiagnosticListenerObserver.Create(nameof(T04_ArgumentAdapterTests), addHandlers =>
        {
            addHandlers(DiagnosticEventHandlers.For<object>(nameof(T01_SimpleObserverEvent), Callback));
        });
        DiagnosticListener.AllListeners.Subscribe(observer);

        var diagnostic = new DiagnosticListener(nameof(T04_ArgumentAdapterTests));
        diagnostic.Write(nameof(T01_SimpleObserverEvent), new ArgumentAdapterEventFoo("Test", "01", ArgumentAdapterEventFooEnum.A));

        Assert.NotNull(arg1);
        Assert.Equal(typeof(ArgumentAdapterEventFoo), arg1!.GetType());
    }

    [Fact]
    public void T02_ObserverPropertyNames()
    {
        string? argName = null;
        string? argValue = null;
        void Callback(string name, string value)
        {
            argName = name;
            argValue = value;
        };

        var observer = DiagnosticListenerObserver.Create(nameof(T04_ArgumentAdapterTests), addHandlers =>
        {
            addHandlers(DiagnosticEventHandlers.For<string, string>(nameof(T02_ObserverPropertyNames), Callback, "Name", "Value"));
        });
        DiagnosticListener.AllListeners.Subscribe(observer);

        var diagnostic = new DiagnosticListener(nameof(T04_ArgumentAdapterTests));
        diagnostic.Write(nameof(T02_ObserverPropertyNames), new ArgumentAdapterEventFoo("Test", "02", ArgumentAdapterEventFooEnum.A));

        Assert.Equal("Test", argName);
        Assert.Equal("02", argValue);
    }

    [Fact]
    public void T03_SimpleArgumentAdapter()
    {
        ArgumentAdapterSingleObserverAlpha? arg1 = null;
        void Callback(ArgumentAdapterSingleObserverAlpha a1) => arg1 = a1;
        var observer = DiagnosticListenerObserver.Create(nameof(T04_ArgumentAdapterTests), addHandlers =>
        {
            addHandlers(DiagnosticEventHandlers.For<ArgumentAdapterSingleObserverAlpha>(nameof(T03_SimpleArgumentAdapter), Callback));
        });
        DiagnosticListener.AllListeners.Subscribe(observer);

        var diagnostic = new DiagnosticListener(nameof(T04_ArgumentAdapterTests));
        diagnostic.Write(nameof(T03_SimpleArgumentAdapter), new ArgumentAdapterEventFoo("Test", "03", ArgumentAdapterEventFooEnum.A));

        Assert.NotNull(arg1);
        Assert.Equal("Test", arg1!.Name);
        Assert.Equal("03", arg1!.Value);
    }

    [Fact]
    public void T04_ArgumentAdapterWithPropertyNames()
    {
        ArgumentAdapterSingleObserverBetha? arg1 = null;
        void Callback(ArgumentAdapterSingleObserverBetha a1) => arg1 = a1;
        var observer = DiagnosticListenerObserver.Create(nameof(T04_ArgumentAdapterTests), addHandlers =>
        {
            addHandlers(DiagnosticEventHandlers.For<ArgumentAdapterSingleObserverBetha>(nameof(T04_ArgumentAdapterWithPropertyNames), Callback));
        });
        DiagnosticListener.AllListeners.Subscribe(observer);

        var diagnostic = new DiagnosticListener(nameof(T04_ArgumentAdapterTests));
        diagnostic.Write(nameof(T04_ArgumentAdapterWithPropertyNames), new ArgumentAdapterEventFoo("Test", "04", ArgumentAdapterEventFooEnum.A));

        Assert.NotNull(arg1);
        Assert.Equal("Test", arg1!.BethaName);
        Assert.Equal("04", arg1!.BethaValue);
    }

    [Fact]
    public void T05_ArgumentAdapterMultiTypes()
    {
        ArgumentAdapterSingleObserverGamma? arg1 = null;
        void Callback(ArgumentAdapterSingleObserverGamma a1) => arg1 = a1;
        var observer = DiagnosticListenerObserver.Create(nameof(T04_ArgumentAdapterTests), addHandlers =>
        {
            addHandlers(DiagnosticEventHandlers.For<ArgumentAdapterSingleObserverGamma>(nameof(T05_ArgumentAdapterMultiTypes), Callback));
        });
        DiagnosticListener.AllListeners.Subscribe(observer);

        var diagnostic = new DiagnosticListener(nameof(T04_ArgumentAdapterTests));
        diagnostic.Write(nameof(T05_ArgumentAdapterMultiTypes), new ArgumentAdapterEventFoo("TestFoo", "05Foo", ArgumentAdapterEventFooEnum.A));

        Assert.NotNull(arg1);
        Assert.Equal("TestFoo", arg1!.Name);
        Assert.Equal("05Foo", arg1!.Value);
        Assert.Null(arg1.Description);
        Assert.Null(arg1.Information);

        arg1 = null;
        diagnostic.Write(nameof(T05_ArgumentAdapterMultiTypes), new ArgumentAdapterEventBar("TestBar", "05Bar", ArgumentAdapterEventBarEnum.A));

        Assert.NotNull(arg1);
        Assert.Equal("TestBar", arg1!.Description);
        Assert.Equal("05Bar", arg1!.Information);
        Assert.Null(arg1.Name);
        Assert.Null(arg1.Value);
    }
}

public class ArgumentAdapterEventFoo
{
    public ArgumentAdapterEventFoo(string name, string value, ArgumentAdapterEventFooEnum type)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Value = value ?? throw new ArgumentNullException(nameof(value));
        Type = type;
    }

    public string Name { get; }

    public string Value { get; }

    public ArgumentAdapterEventFooEnum Type { get; }
}

public enum ArgumentAdapterEventFooEnum
{
    A, B, C
}

public class ArgumentAdapterEventBar
{
    public ArgumentAdapterEventBar(string description, string information, ArgumentAdapterEventBarEnum type)
    {
        Description = description ?? throw new ArgumentNullException(nameof(description));
        Information = information ?? throw new ArgumentNullException(nameof(information));
        Type = type;
    }

    public string Description { get; }

    public string Information { get; }

    public ArgumentAdapterEventBarEnum Type { get; }
}

public enum ArgumentAdapterEventBarEnum
{
    A, B, C
}

[ArgumentAdapter]
public class ArgumentAdapterSingleObserverAlpha
{
    [GetFromArgument]
    public string? Name { get; set; }

    [GetFromArgument]
    public string? Value { get; set; }
}

[ArgumentAdapter]
public class ArgumentAdapterSingleObserverBetha
{
    [GetFromArgument("Name")]
    public string? BethaName { get; set; }

    [GetFromArgument("Value")]
    public string? BethaValue { get; set; }
}


[ArgumentAdapter(GetFromTypes = new [] {
    "RoyalCode.Diagnostics.Tests.ArgumentAdapterEventFoo, RoyalCode.Diagnostics.Tests",
    "RoyalCode.Diagnostics.Tests.ArgumentAdapterEventBar, RoyalCode.Diagnostics.Tests"
})]
public class ArgumentAdapterSingleObserverGamma
{
    [GetFromArgument(Required = false)]
    public string? Name { get; set; }

    [GetFromArgument(Required = false)]
    public string? Value { get; set; }

    [GetFromArgument(Required = false)]
    public string? Description { get; set; }

    [GetFromArgument(Required = false)]
    public string? Information { get; set; }
}