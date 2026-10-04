using System.Globalization;
using Microsoft.JSInterop;

namespace MistyStep.Services;

public class LanguagePreferenceService(IJSRuntime jsRuntime)
{
    private bool _initialized;

    public const string Danish = "da-DK";
    public const string EnglishUk = "en-GB";

    public string Language { get; private set; } = EnglishUk;

    public event Action? LanguageChanged;

    public async Task InitializeAsync()
    {
        if (_initialized)
        {
            return;
        }

        var language = await jsRuntime.InvokeAsync<string>("mistyStepGetLanguage");
        Initialize(language);
    }

    public void Initialize(string language)
    {
        if (_initialized)
        {
            return;
        }

        ApplyLanguage(language);
        _initialized = true;
    }

    public async Task SetLanguageAsync(string language)
    {
        if (language is not (Danish or EnglishUk))
        {
            throw new ArgumentOutOfRangeException(nameof(language));
        }

        await jsRuntime.InvokeVoidAsync("mistyStepSetLanguage", language);
        ApplyLanguage(language);
        LanguageChanged?.Invoke();
    }

    private void ApplyLanguage(string language)
    {
        Language = language is Danish or EnglishUk ? language : EnglishUk;
        var culture = CultureInfo.GetCultureInfo(Language);
        CultureInfo.DefaultThreadCurrentCulture = culture;
        CultureInfo.DefaultThreadCurrentUICulture = culture;
        ApplyCurrentCulture();
    }

    public void ApplyCurrentCulture()
    {
        var culture = CultureInfo.GetCultureInfo(Language);
        CultureInfo.CurrentCulture = culture;
        CultureInfo.CurrentUICulture = culture;
    }
}