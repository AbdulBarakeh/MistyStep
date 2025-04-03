using AutoInterfaceAttributes;
using MistyStep.Models;
using System.Text.Json;
using TG.Blazor.IndexedDB;
namespace MistyStep.Services;

[AutoInterface]
public class IndexedDbService : IIndexedDbService
{

    private readonly IndexedDBManager _dbManager;

    public IndexedDbService(IndexedDBManager dbManager)
    {
        _dbManager = dbManager;
    }

    public async Task AddExerciseAsync(Exercise exercise)
    {
        var storeRecord = new StoreRecord<Exercise>
        {
            Storename = "exercises",
            Data = exercise
        };

        await _dbManager.AddRecord(storeRecord);
    }

    public async Task<List<Exercise>> GetExercisesAsync()
    {
        return await _dbManager.GetRecords<Exercise>("exercises");
    }
    
    public async Task AddProgramAsync(ExerciseProgram program)
    {
        var storeRecord = new StoreRecord<ExerciseProgram>
        {
            Storename = "exercisePrograms",
            Data = program
        };
        await _dbManager.AddRecord(storeRecord);
    }

    public async Task<List<ExerciseProgram>> GetProgramsAsync()
    {
        return await _dbManager.GetRecords<ExerciseProgram>("exercisePrograms");
    }

    public async Task<ExerciseProgram> GetProgramByIdAsync(Guid id)
    {
        return await _dbManager.GetRecordById<Guid, ExerciseProgram>("exercisePrograms", id);
    }

    public async Task AddRecordAsync(ExerciseRecord record)
    {
        var storeRecord = new StoreRecord<ExerciseRecord>
        {
            Storename = "exerciseRecords",
            Data = record
        };
        await _dbManager.AddRecord(storeRecord);
    }

    public async Task<List<ExerciseRecord>> GetRecordsAsync()
    {
        return await _dbManager.GetRecords<ExerciseRecord>("exerciseRecords");
    }


    public async Task SeedPredefinedExercisesAsync()
    {
        var existingExercises = await GetExercisesAsync();

        if (existingExercises.Count == 0)
        {
            foreach (var exercise in PredefinedExercises)
            {
                await AddExerciseAsync(exercise);
            }
        }
    }

    public async Task ExportDataAsync()
    {
        await Task.Delay(1);
        throw new NotImplementedException();
        //var exercises = await GetExercisesAsync();
        //var programs = await GetProgramsAsync();
        //var records = await GetRecordsAsync();

        //var data = new
        //{
        //    exercises,
        //    programs,
        //    records
        //};

        //string json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });

        // Implement file save logic (e.g., File System API or download as a blob)
    }

    private static readonly List<Exercise> PredefinedExercises =
    [
        new Exercise(Guid.Parse("9D8CE2F5-1A3A-4504-A5F7-CDC8C6AD2954"),"Pushups",60,5),
        new Exercise(Guid.Parse("EA72DE54-A3C7-45B1-A87F-7B7522E95BEF"),"Situps",60,4),
        new Exercise(Guid.Parse("2820DE0C-0A79-4E43-9EE9-4B0F31915767"),"Pullups",60, 5),
        new Exercise(Guid.Parse("E5C2668C-0C79-418B-8DA9-F0D965B797CB"),"Chinups",60,6),
        new Exercise(Guid.Parse("DF91F357-8BBB-4195-A42F-7032A58C5A18"),"Lunges",60,2),
        new Exercise(Guid.Parse("CD4B4583-4FAE-4C42-AD10-3D0FAF92052C"),"Punches",60,3)
    ];
}