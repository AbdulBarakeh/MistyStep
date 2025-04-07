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

    public async Task<Result<ExerciseProgram>> GetExerciseProgramById(Guid programId)
    {
        var program = await DbService.GetProgramByIdAsync(programId);
        if (program is null)
        {
            return new Result<ExerciseProgram>(false, null, "program was not found");
        }
        return program;
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

    public async Task<Result<List<Exercise>>> GetExercises()
    {
        var result = await DbService.GetExercisesAsync();
        if (result is null || result.Count == 0)
        {
            return new Result<List<Exercise>>(false, null, "exercises were not found");
        }
        return result;
    }

    public async Task<Result<List<Exercise>>> GetExercisesByIds(List<Guid> ids)
    {
        var exercises = await DbService.GetExercisesAsync();
        var result = exercises
            .Where(x => ids.Contains(x.Id))
            .ToList();

        if (result is null || result.Count == 0)
        {
            return new Result<List<Exercise>>(false, null, "exercises were not found");
        }

        return result;
    }

    public async Task<Result<List<ExerciseProgram>>> GetExercisePrograms()
    {
        var exercisePrograms = await DbService.GetProgramsAsync();
        if (exercisePrograms is null || exercisePrograms.Count == 0)
        {
            return new Result<List<ExerciseProgram>>(false, null, "exercise programs were not found");
        }
        return exercisePrograms;
    }

    public async Task<List<ExerciseRecord>> GetExerciseRecords()
    {
        return await DbService.GetRecordsAsync();
    }

    public async Task<Result<ExerciseRecord>> GetExerciseRecordById(Guid exerciseId, Guid programId)
    {
        var records = await DbService.GetRecordsAsync();
        var record = records
            .OrderBy(x => x.RecordSet)
            .FirstOrDefault(x => x.ExerciseId == exerciseId && x.ExerciseProgramId == programId);

        if (record is null)
        {
            return new Result<ExerciseRecord>(false, null, "record was not found");
        }

        return record;
    }
    
    public async Task<Result<List<ExerciseRecord>>> GetExerciseRecordsByProgramId(Guid programId)
    {
        var records = await DbService.GetRecordsAsync();
        var record = records
            .OrderBy(x => x.RecordSet)
            .Where(x => x.ExerciseProgramId == programId).ToList();

        if (record is null)
        {
            return new Result<List<ExerciseRecord>>(false, null, "record was not found");
        }

        return record;
    }
}
