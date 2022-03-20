using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace RoyalCode.Diagnostics;

/// <summary>
/// <para>
///     A class with static methods to create <see cref="DiagnosticOperation"/> for a <see cref="DiagnosticListener"/>. 
/// </para>
/// <para>
///     This class manage, with a cache, if the event has observers.
/// </para>
/// <para>
///     To use this class, create a new class and inherit this class passing a unique class to generic parameter
///     <typeparamref name="TNames"/>.
/// </para>
/// <para>
///     The class passed to <typeparamref name="TNames"/> must have the attribute <see cref="DiagnosticListenerNameAttribute"/>.
/// </para>
/// </summary>
/// <example>
/// <code>
/// [DiagnosticListenerName("MyCompany.Diagnostics.MyComponent")]
/// public class MyComponentDiagnostics : DiagnosticOperationFactory&lt;MyComponentDiagnostics&gt;
/// {
///     public static DiagnosticOperation CreateDoingSomethingOperation() 
///         => CreateOperation("DoingSomething");
/// }
/// </code>
/// </example>
/// <typeparam name="TNames">Some unique type for each <see cref="DiagnosticListener"/>.</typeparam>
public class DiagnosticOperationFactory<TNames>
{
    protected static readonly DiagnosticListener Listener = new(typeof(TNames).GetDiagnosticListenerName());

    private static DiagnosticEnableCache cache = new(Listener.IsEnabled);

    protected static TimeSpan CacheExpirationTime
    {
        get => cache.CacheExpirationTime;
        set => cache.CacheExpirationTime = value;
    }

    /// <summary>
    /// The diagnostic listener name.
    /// </summary>
    public static string ListenerName => Listener.Name;

    /// <summary>
    /// <para>
    ///     If the current static class of diagnostics operation are active.
    /// </para>
    /// <para>
    ///     It is true by default.
    /// </para>
    /// </summary>
    public static bool Enable { get; set; } = true;

    /// <summary>
    /// <para>
    ///     Check if some event has active observers for this diagnostic listener.
    /// </para>
    /// </summary>
    /// <param name="eventName">The event name.</param>
    /// <returns></returns>
    public static bool IsEnable(string eventName)
        => Enable && cache.IsEnable(eventName, DateTimeOffset.Now);

    /// <summary>
    /// <para>
    ///     Creates a new operation to start <see cref="A"/>
    /// </para>
    /// </summary>
    /// <param name="operationName"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected static DiagnosticOperation CreateOperation(string operationName)
        => new (Listener, operationName);

}