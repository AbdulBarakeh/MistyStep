using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using System.Globalization;
using MistyStep;
using MistyStep.Models;
using MistyStep.Services;
using MudBlazor.Services;
using TG.Blazor.IndexedDB;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<IIndexedDbService, IndexedDbService>();
builder.Services.AddScoped<IExerciseService, ExerciseService>();
builder.Services.AddScoped<ThemePreferenceService>();
builder.Services.AddScoped<LanguagePreferenceService>();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddMudServices();
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

// Configure IndexedDB
builder.Services.AddIndexedDB(dbStore =>
{
    dbStore.DbName = "MistyStep";
    dbStore.Version = 1;

    dbStore.Stores.Add(new StoreSchema
    {
        Name = "exercises",
        PrimaryKey = new IndexSpec { Name = "Id", KeyPath = "id", Unique = true }
    });

    dbStore.Stores.Add(new StoreSchema
    {
        Name = "exercisePrograms",
        PrimaryKey = new IndexSpec { Name = "Id", KeyPath = "id", Unique = true },
        Indexes = new List<IndexSpec>
        {
            new() { Name = "exerciseIds", KeyPath = "exerciseIds", Unique = false }
        }
    });

    dbStore.Stores.Add(new StoreSchema
    {
        Name = "exerciseRecords",
        PrimaryKey = new IndexSpec { Name = "Id", KeyPath = "id", Unique = true },
        Indexes = new List<IndexSpec>
        {
            new() { Name = "ExerciseId", KeyPath = "exerciseId", Unique = false },
            new() { Name = "ExerciseProgramId", KeyPath = "exerciseProgramId", Unique = false }
        }
    });
});

var host = builder.Build();
var jsRuntime = host.Services.GetRequiredService<IJSRuntime>();
var selectedLanguage = await jsRuntime.InvokeAsync<string>("mistyStepGetLanguage");
host.Services.GetRequiredService<LanguagePreferenceService>().Initialize(selectedLanguage);
var culture = CultureInfo.GetCultureInfo(selectedLanguage);
CultureInfo.DefaultThreadCurrentCulture = culture;
CultureInfo.DefaultThreadCurrentUICulture = culture;
CultureInfo.CurrentCulture = culture;
CultureInfo.CurrentUICulture = culture;

var dbService = host.Services.GetRequiredService<IIndexedDbService>();
await dbService.SeedPredefinedExercisesAsync();
await host.RunAsync();
