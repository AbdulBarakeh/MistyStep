using System.Text;
using System.Text.Json;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;

namespace MistyStep.PlaywrightTests.Tests;

[TestFixture]
[Parallelizable(ParallelScope.Self)]
public class MistyStepBrowserTests : PageTest
{
    private string AppUrl =>
        (Environment.GetEnvironmentVariable("MISTYSTEP_BASE_URL") ?? "http://127.0.0.1:5197").TrimEnd('/');

    [Test]
    public async Task HomeFabOpensCreateDialogAndImportFileChooser()
    {
        await OpenHomeAsync();

        await Page.Locator(".mud-fab-menu-button").ClickAsync();
        await Page.GetByRole(AriaRole.Menuitem, new() { Name = "Create exercise" }).ClickAsync();
        var dialog = Page.GetByRole(AriaRole.Dialog);
        await Expect(dialog).ToBeVisibleAsync();
        await Expect(dialog.GetByLabel("Exercise name")).ToBeVisibleAsync();
        await dialog.GetByRole(AriaRole.Button, new() { Name = "Cancel" }).ClickAsync();

        var chooser = await Page.RunAndWaitForFileChooserAsync(async () =>
        {
            await Page.Locator(".mud-fab-menu-button").ClickAsync();
            await Page.GetByRole(AriaRole.Menuitem, new() { Name = "Import exercises or a programme" }).ClickAsync();
        });

        Assert.That(chooser, Is.Not.Null);
    }

    [Test]
    public async Task LanguageSelectionReloadsAndPersists()
    {
        await OpenSettingsAsync();

        await Page.GetByRole(AriaRole.Button, new() { Name = "Danish" }).ClickAsync();
        await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Indstillinger" })).ToBeVisibleAsync();
        await Expect(Page.GetByText("Aktuelt sprog: Dansk")).ToBeVisibleAsync();
        Assert.That(await Page.Locator("html").GetAttributeAsync("lang"), Is.EqualTo("da-DK"));

        await Page.ReloadAsync();
        await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Indstillinger" })).ToBeVisibleAsync();
        await Page.GetByRole(AriaRole.Link, new() { Name = "Hjem" }).ClickAsync();
        await Expect(Page.GetByRole(AriaRole.Tab, new() { Name = "Træningsprogrammer" })).ToBeVisibleAsync();

        await Page.GetByRole(AriaRole.Link, new() { Name = "Indstillinger" }).ClickAsync();
        await Page.GetByRole(AriaRole.Button, new() { Name = "Engelsk (UK)" }).ClickAsync();
        await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Settings" })).ToBeVisibleAsync();
        Assert.That(await Page.Locator("html").GetAttributeAsync("lang"), Is.EqualTo("en-GB"));
    }

    [Test]
    public async Task ThemeSelectionPersistsIndependentlyOfLanguage()
    {
        await OpenSettingsAsync();

        await Page.GetByRole(AriaRole.Button, new() { Name = "Dark" }).ClickAsync();
        await Expect(Page.Locator(".app-shell")).ToHaveClassAsync(new System.Text.RegularExpressions.Regex("app-shell-dark"));
        await Page.ReloadAsync();
        await Expect(Page.Locator(".app-shell")).ToHaveClassAsync(new System.Text.RegularExpressions.Regex("app-shell-dark"));

        await Page.GetByRole(AriaRole.Button, new() { Name = "System" }).ClickAsync();
        var savedTheme = await Page.EvaluateAsync<string?>("() => localStorage.getItem('mistystep-theme-mode')");
        Assert.That(savedTheme, Is.Null);
    }

    [Test]
    public async Task LayoutFitsPhoneAndDesktopViewports()
    {
        await Page.SetViewportSizeAsync(390, 844);
        await OpenHomeAsync();
        Assert.That(await Page.EvaluateAsync<int>("() => document.documentElement.scrollWidth"), Is.EqualTo(390));

        await Page.SetViewportSizeAsync(1440, 900);
        Assert.That(await Page.EvaluateAsync<int>("() => document.documentElement.scrollWidth"), Is.EqualTo(1440));
    }

    [Test]
    public async Task BackupImportAndExportRetainProgramsPointBandsAndRecords()
    {
        var backup = CreateBackupFixture(programmeCount: 2);
        await ImportBackupAsync(backup.Json);

        await OpenHomeAsync();
        await Page.GetByRole(AriaRole.Tab, new() { Name = "Programmes" }).ClickAsync();
        foreach (var programmeName in backup.ProgrammeNames)
        {
            await Expect(Page.GetByText(programmeName, new() { Exact = true })).ToBeVisibleAsync();
        }

        await OpenSettingsAsync();
        var downloadTask = Page.WaitForDownloadAsync();
        await Page.GetByRole(AriaRole.Button, new() { Name = "Export all data" }).ClickAsync();
        var download = await downloadTask;
        var downloadPath = await download.PathAsync();
        Assert.That(downloadPath, Is.Not.Null);

        using var document = JsonDocument.Parse(await File.ReadAllTextAsync(downloadPath!));
        var root = document.RootElement;
        Assert.That(root.GetProperty("type").GetString(), Is.EqualTo("backup"));
        Assert.That(root.GetProperty("exercises").EnumerateArray()
            .Any(item => item.GetProperty("name").GetString() == backup.ExerciseName), Is.True);

        var exportedProgramme = root.GetProperty("programs").EnumerateArray()
            .Single(item => item.GetProperty("name").GetString() == backup.ProgrammeNames[0]);
        Assert.That(exportedProgramme.GetProperty("pointTable").GetArrayLength(), Is.EqualTo(2));
        Assert.That(exportedProgramme.GetProperty("pointTable")[1].GetProperty("category").GetString(), Is.EqualTo("Silver"));
        Assert.That(root.GetProperty("records").EnumerateArray()
            .Any(item => item.GetProperty("id").GetGuid() == backup.RecordId), Is.True);
    }

    [Test]
    public async Task DeletingExerciseRemovesItFromEveryProgramme()
    {
        var backup = CreateBackupFixture(programmeCount: 2);
        await ImportBackupAsync(backup.Json);
        await OpenHomeAsync();

        var exerciseCard = Page.Locator(".catalog-item")
            .Filter(new LocatorFilterOptions { HasTextString = backup.ExerciseName });
        await exerciseCard.GetByRole(AriaRole.Button, new() { Name = "Delete exercise" }).ClickAsync();
        var confirmation = Page.GetByRole(AriaRole.Dialog);
        await Expect(confirmation.GetByText("This also removes the exercise from every programme and deletes its workout records.")).ToBeVisibleAsync();
        await confirmation.GetByRole(AriaRole.Button, new() { Name = "Delete", Exact = true }).ClickAsync();
        await Expect(exerciseCard).ToHaveCountAsync(0);

        await Page.GetByRole(AriaRole.Tab, new() { Name = "Programmes" }).ClickAsync();
        foreach (var programmeName in backup.ProgrammeNames)
        {
            var programmeCard = Page.Locator(".catalog-item")
                .Filter(new LocatorFilterOptions { HasTextString = programmeName });
            await programmeCard.GetByRole(AriaRole.Button, new() { Name = "Edit programme" }).ClickAsync();
            await Expect(Page.GetByRole(AriaRole.Textbox, new() { Name = "Exercises" })).ToHaveValueAsync("");
            await Page.GetByRole(AriaRole.Button, new() { Name = "Cancel" }).ClickAsync();
        }
    }

    [Test]
    public async Task DeletingProgrammeKeepsExerciseAndRemovesItsRecords()
    {
        var backup = CreateBackupFixture();
        await ImportBackupAsync(backup.Json);
        await OpenHomeAsync();
        await Page.GetByRole(AriaRole.Tab, new() { Name = "Programmes" }).ClickAsync();

        var programmeCard = Page.Locator(".catalog-item")
            .Filter(new LocatorFilterOptions { HasTextString = backup.ProgrammeNames[0] });
        await programmeCard.GetByRole(AriaRole.Button, new() { Name = "Delete programme" }).ClickAsync();
        var confirmation = Page.GetByRole(AriaRole.Dialog);
        await Expect(confirmation.GetByText("This also deletes all workout records for this programme.")).ToBeVisibleAsync();
        await confirmation.GetByRole(AriaRole.Button, new() { Name = "Delete", Exact = true }).ClickAsync();
        await Expect(programmeCard).ToHaveCountAsync(0);

        await Page.GetByRole(AriaRole.Tab, new() { Name = "Exercises" }).ClickAsync();
        await Expect(Page.GetByText(backup.ExerciseName, new() { Exact = true })).ToBeVisibleAsync();
    }

    [Test]
    [CancelAfter(30000)]
    public async Task ExerciseAndPauseTimersAdvanceAutomatically()
    {
        var backup = CreateBackupFixture(exerciseCount: 2, exerciseDuration: 3, pauseDuration: 2, includeRecord: false);
        await ImportBackupAsync(backup.Json);
        await OpenHomeAsync();
        await Page.GetByRole(AriaRole.Tab, new() { Name = "Programmes" }).ClickAsync();
        await Page.GetByRole(AriaRole.Button, new() { Name = "Run programme" }).ClickAsync();

        var repetitions = Page.GetByLabel("Repetitions");
        await repetitions.FillAsync("7");
        await Expect(Page.GetByText("PAUSE", new() { Exact = true })).ToBeVisibleAsync();
        await Expect(repetitions).ToHaveValueAsync("7");
        await Expect(Page.GetByText("EXERCISE 2 OF 2", new() { Exact = true })).ToBeVisibleAsync();
        await Expect(repetitions).ToHaveValueAsync("0");
    }

    private async Task OpenHomeAsync()
    {
        await Page.GotoAsync(AppUrl);
        await Expect(Page.GetByRole(AriaRole.Tab, new() { Name = "Exercises" })).ToBeVisibleAsync();
    }

    private async Task OpenSettingsAsync()
    {
        await Page.GotoAsync($"{AppUrl}/settings");
        await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Settings" })).ToBeVisibleAsync();
    }

    private async Task ImportBackupAsync(string json)
    {
        await OpenSettingsAsync();
        await Page.Locator("#fullBackupInput").SetInputFilesAsync(new FilePayload
        {
            Name = "playwright-backup.json",
            MimeType = "application/json",
            Buffer = Encoding.UTF8.GetBytes(json)
        });
        await Expect(Page.GetByText("Backup imported.")).ToBeVisibleAsync();
    }

    private static BackupFixture CreateBackupFixture(
        int programmeCount = 1,
        int exerciseCount = 1,
        int exerciseDuration = 30,
        int pauseDuration = 10,
        bool includeRecord = true)
    {
        var exerciseName = $"Playwright exercise {Guid.NewGuid():N}";
        var exercises = Enumerable.Range(0, exerciseCount)
            .Select(index => new
            {
                id = Guid.NewGuid(),
                name = index == 0 ? exerciseName : $"{exerciseName} {index + 1}",
                pointsPerRep = 3d
            })
            .ToArray();
        var exerciseIds = exercises.Select(exercise => exercise.id).ToArray();
        var programmes = Enumerable.Range(0, programmeCount)
            .Select(index => new
            {
                id = Guid.NewGuid(),
                name = $"Playwright programme {Guid.NewGuid():N}",
                exerciseIds,
                exerciseDurationInSeconds = exerciseDuration,
                pauseDurationInSeconds = pauseDuration,
                pointTable = new[]
                {
                    new { category = "Bronze", minimumPoints = 0d, maximumPoints = 19d },
                    new { category = "Silver", minimumPoints = 20d, maximumPoints = 39d }
                }
            })
            .ToArray();
        var recordId = Guid.NewGuid();
        object[] records = includeRecord
            ?
            [
                new
                {
                    id = recordId,
                    recordSet = DateTime.UtcNow,
                    exerciseId = exercises[0].id,
                    exerciseProgramId = programmes[0].id,
                    reps = 12d
                }
            ]
            : [];

        var json = JsonSerializer.Serialize(new
        {
            formatVersion = 1,
            type = "backup",
            exercises,
            programs = programmes,
            records
        }, new JsonSerializerOptions(JsonSerializerDefaults.Web));

        return new BackupFixture(json, exerciseName, programmes.Select(programme => programme.name).ToArray(), recordId);
    }

    private sealed record BackupFixture(string Json, string ExerciseName, string[] ProgrammeNames, Guid RecordId);
}