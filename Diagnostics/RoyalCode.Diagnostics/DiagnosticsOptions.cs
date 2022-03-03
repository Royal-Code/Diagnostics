namespace RoyalCode.Diagnostics;

/// <summary>
/// Diagnostic observation options.
/// </summary>
public class DiagnosticsOptions
{
    private bool enabled = true;

    /// <summary>
    /// Determines whether the configuration is default.
    /// </summary>
    public bool IsDefaultConfiguration { get; private set; } = true;

    /// <summary>
    /// If it is enabled.
    /// </summary>
    public bool Enabled
    {
        get => enabled;
        set
        {
            enabled = value;
            IsDefaultConfiguration = false;
        }
    }
}