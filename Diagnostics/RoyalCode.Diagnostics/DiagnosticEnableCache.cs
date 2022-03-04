namespace RoyalCode.Diagnostics;

/// <summary>
/// Component to cache if the diagnostic is active for a given event.
/// </summary>
public class DiagnosticEnableCache
{
    private readonly Func<string, bool> checkEnable;
    private readonly Dictionary<string, EventEnableCache> eventCaches = new();

    internal TimeSpan CacheExpirationTime = TimeSpan.FromMinutes(1);

    /// <summary>
    /// Creates a new instance with a function to check is some event is active.
    /// </summary>
    /// <param name="checkEnable"></param>
    public DiagnosticEnableCache(Func<string, bool> checkEnable)
    {
        this.checkEnable = checkEnable;
    }

    /// <summary>
    /// Check is the event is active.
    /// </summary>
    /// <param name="eventName">The event name.</param>
    /// <param name="now">The current time to validate the cache.</param>
    /// <returns>True if the event listener is active, false otherwise.</returns>
    public bool IsEnable(string eventName, DateTimeOffset now)
    {
        if (eventCaches.TryGetValue(eventName, out var cache))
        {
            if (now < cache.CacheExpiration) 
                return cache.Enable;
            
            cache.Enable = checkEnable(eventName);
            cache.CacheExpiration = now + CacheExpirationTime;
        }
        else
        {
            cache = new()
            {
                Enable = checkEnable(eventName),
                CacheExpiration = now + CacheExpirationTime
            };
            eventCaches[eventName] = cache;
        }

        return cache.Enable;
    }
    
    private class EventEnableCache
    {
        internal bool Enable;
        internal DateTimeOffset CacheExpiration;
    }
}