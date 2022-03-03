namespace RoyalCode.Diagnostics;

/// <summary>
/// Factory class for creating <see cref="IDiagnosticEventHandler"/>.
/// </summary>
public static class DiagnosticEventHandlers
{
    /// <summary>
    /// <para>
    ///     Creates a new <see cref="IDiagnosticEventHandler"/> of the type <see cref="DiagnosticEventHandler"/>.
    /// </para>
    /// <para>
    ///     To handle an event a delegate is used, which will receive the desired data type.
    /// </para>
    /// </summary>
    /// <param name="eventName">The event name.</param>
    /// <param name="handler">Event handler delegate.</param>
    /// <returns>A new instance of <see cref="DiagnosticEventHandler{TArgument}"/>.</returns>
    public static DiagnosticEventHandler For(string eventName, Action handler)
    {
        return new DiagnosticEventHandler(eventName, handler);
    }

    /// <summary>
    /// <para>
    ///     Creates a new <see cref="IDiagnosticEventHandler"/>
    ///     of the type <see cref="DiagnosticEventHandler{TArgument}"/>.
    /// </para>
    /// <para>
    ///     One argument type is informed (<typeparamref name="TArgument"/>)
    ///     that will be obtained from one of the properties of the event argument object.
    ///     It is also possible to specify the name of the property.
    /// </para>
    /// <para>
    ///     To handle an event a delegate is used, which will receive the desired data type.
    /// </para>
    /// </summary>
    /// <typeparam name="TArgument">The argument type.</typeparam>
    /// <param name="eventName">The event name.</param>
    /// <param name="handler">Event handler delegate.</param>
    /// <param name="propertyName">Name of the property to extract the argument, optional.</param>
    /// <returns>A new instance of <see cref="DiagnosticEventHandler{TArgument}"/>.</returns>
    public static DiagnosticEventHandler<TArgument> For<TArgument>(
        string eventName,
        Action<TArgument> handler,
        string? propertyName = null)
    {
        return propertyName == null
            ? new DiagnosticEventHandler<TArgument>(eventName, handler)
            : new DiagnosticEventHandler<TArgument>(eventName, handler, propertyName);
    }

    /// <summary>
    /// <para>
    ///     Creates a new <see cref="IDiagnosticEventHandler"/>
    ///     of the type <see cref="DiagnosticEventHandler{TArgument1, TArgument2}"/>.
    /// </para>
    /// <para>
    ///     Are informed the types of arguments
    ///     (<typeparamref name="TArgument1"/> and <typeparamref name="TArgument2"/>)
    ///     that will be obtained from the properties of the event argument object.
    ///     It is also possible to specify the names of the properties.
    /// </para>
    /// <para>
    ///     To handle an event a delegate is used, which will receive the desired data type.
    /// </para>
    /// </summary>
    /// <typeparam name="TArgument1">The argument type 1.</typeparam>
    /// <typeparam name="TArgument2">The argument type 2.</typeparam>
    /// <param name="eventName">The event name.</param>
    /// <param name="handler">Event handler delegate.</param>
    /// <param name="propertyName1">Name of the property to extract the argument 1, optional.</param>
    /// <param name="propertyName2">Name of the property to extract the argument 2, optional.</param>
    /// <returns>A new instance of <see cref="DiagnosticEventHandler{TArgument1, TArgument2}"/>.</returns>
    public static DiagnosticEventHandler<TArgument1, TArgument2> For<TArgument1, TArgument2>(
        string eventName,
        Action<TArgument1, TArgument2> handler,
        string? propertyName1 = null,
        string? propertyName2 = null)
    {
        return new DiagnosticEventHandler<TArgument1, TArgument2>(eventName, handler, propertyName1, propertyName2);
    }

    /// <summary>
    /// <para>
    ///     Creates a new <see cref="IDiagnosticEventHandler"/>
    ///     of the type <see cref="DiagnosticEventHandler{TArgument1, TArgument2, TArgument3}"/>.
    /// </para>
    /// <para>
    ///     Are informed the types of arguments
    ///     (<typeparamref name="TArgument1"/>, <typeparamref name="TArgument2"/> e <typeparamref name="TArgument3"/>) 
    ///     that will be obtained from the properties of the event argument object.
    ///     It is also possible to specify the names of the properties.
    /// </para>
    /// <para>
    ///     To handle an event a delegate is used, which will receive the desired data type.
    /// </para>
    /// </summary>
    /// <typeparam name="TArgument1">The argument type 1.</typeparam>
    /// <typeparam name="TArgument2">The argument type 2.</typeparam>
    /// <typeparam name="TArgument3">The argument type 3.</typeparam>
    /// <param name="eventName">The event name.</param>
    /// <param name="handler">Event handler delegate.</param>
    /// <param name="propertyName1">Name of the property to extract the argument 1, optional.</param>
    /// <param name="propertyName2">Name of the property to extract the argument 2, optional.</param>
    /// <param name="propertyName3">Name of the property to extract the argument 3, optional.</param>
    /// <returns>A new instance of <see cref="DiagnosticEventHandler{TArgument1, TArgument2}"/>.</returns>
    public static DiagnosticEventHandler<TArgument1, TArgument2, TArgument3> For<TArgument1, TArgument2, TArgument3>(
        string eventName,
        Action<TArgument1, TArgument2, TArgument3> handler,
        string? propertyName1 = null,
        string? propertyName2 = null,
        string? propertyName3 = null)
    {
        return new DiagnosticEventHandler<TArgument1, TArgument2, TArgument3>(
            eventName, handler, propertyName1, propertyName2, propertyName3);
    }
}