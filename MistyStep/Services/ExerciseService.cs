using MistyStep.Models;

namespace MistyStep.Services;
[AutoInterfaceAttributes.AutoInterface]
public class ExerciseService(IIndexedDbService DbService) : IExerciseService
{
    public async Task<bool> CreateNewExercise(Exercise exercise)
    {
        var temp = exercise;
        if (temp is not null)
        {
            await DbService.AddExerciseAsync(exercise);
            return true;
        }
        return false;
    }

    public async Task<bool> CreateNewExerciseProgram(ExerciseProgram program)
    {
        var temp = program;
        if (temp is not null)
        {
            await DbService.AddProgramAsync(program);
            return true;
        }
        return false;
    }

    public async Task<bool> CreateNewExerciseRecord(ExerciseRecord record)
    {
        var temp = record;
        if (temp is not null)
        {
            await DbService.AddRecordAsync(record);
            return true;
        }
        return false;
    }

    public async Task<List<Exercise>> GetExercises()
    {
        return await DbService.GetExercisesAsync();
    }

    public async Task<List<ExerciseProgram>> GetExercisePrograms()
    {
        return await DbService.GetProgramsAsync();
    }

    public async Task<List<ExerciseRecord>> GetExerciseRecords()
    {
        return await DbService.GetRecordsAsync();
    }
}
