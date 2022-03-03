namespace RoyalCode.Diagnostics
{
    /// <summary>
    /// <para>
    ///     Delegate to access a property of a diagnostic event argument object
    ///     and get a particular argument for handling the diagnostic event.
    /// </para>
    /// </summary>
    /// <typeparam name="TArgument">The required argument type.</typeparam>
    /// <param name="eventArgs">The event argument instance.</param>
    /// <returns>O argumento extraído de uma propriedade do objeto de argumentos.</returns>
    public delegate TArgument EventArgumentGetter<out TArgument>(object eventArgs);
}
