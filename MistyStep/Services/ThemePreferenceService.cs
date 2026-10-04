using Microsoft.JSInterop;

namespace MistyStep.Services;

public class ThemePreferenceService(IJSRuntime jsRuntime)
{
    private bool _initialized;

    public string Mode { get; private set; } = "system";

    public bool IsDarkMode { get; private set; }

    public event Action? ThemeChanged;

    public async Task InitializeAsync()
    {
        if (_initialized)
        {
            return;
        }

        Mode = await jsRuntime.InvokeAsync<string>("mistyStepGetThemeMode");
        IsDarkMode = await jsRuntime.InvokeAsync<bool>("mistyStepResolveTheme", Mode);
        _initialized = true;
    }

    public async Task SetModeAsync(string mode)
    {
        if (mode is not ("system" or "light" or "dark"))
        {
            throw new ArgumentOutOfRangeException(nameof(mode));
        }

        await jsRuntime.InvokeVoidAsync("mistyStepSetThemeMode", mode);
        Mode = mode;
        IsDarkMode = await jsRuntime.InvokeAsync<bool>("mistyStepResolveTheme", mode);
        ThemeChanged?.Invoke();
    }
}