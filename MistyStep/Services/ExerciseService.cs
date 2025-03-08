using MistyStep.Models;

namespace MistyStep.Services;
[AutoInterfaceAttributes.AutoInterface]
public class ExerciseService : IExerciseService
{
    public async Task<bool> CreateNewExercise(Exercise exercise)
    {
        var temp = exercise;
        if (temp is not null)
        {
            ExerciseTestList.ExerciseList.Add(exercise);
            return true;
        }
        return false;
    }

    public async Task<bool> CreateNewExerciseProgram(ExerciseProgram program)
    {
        var temp = program;
        if (temp is not null)
        {
            ExerciseTestList.ExerciseProgramList.Add(program);
            return true;
        }
        return false;
    }
}

public static class ExerciseTestList
{
    public static List<Exercise> ExerciseList { get; set; } = [
        new Exercise(Guid.NewGuid(),"Pushups",TimeSpan.FromSeconds(60)),
        new Exercise(Guid.NewGuid(),"Situps",TimeSpan.FromSeconds(60)),
        new Exercise(Guid.NewGuid(),"Pullups",TimeSpan.FromSeconds(60)),
        new Exercise(Guid.NewGuid(),"Chinups",TimeSpan.FromSeconds(60)),
        new Exercise(Guid.NewGuid(),"Lunges",TimeSpan.FromSeconds(60)),
        new Exercise(Guid.NewGuid(),"Punches",TimeSpan.FromSeconds(60))
    ];

    public static List<ExerciseProgram> ExerciseProgramList { get; set; } = [
        new ExerciseProgram(Guid.NewGuid(),"Program 1", [
            new(Guid.NewGuid(),"Pushups",TimeSpan.FromSeconds(60)),
            new(Guid.NewGuid(),"Situps",TimeSpan.FromSeconds(60)),
            new(Guid.NewGuid(),"Pullups",TimeSpan.FromSeconds(60)),
            new(Guid.NewGuid(),"Chinups",TimeSpan.FromSeconds(60)),
            new(Guid.NewGuid(),"Lunges",TimeSpan.FromSeconds(60)),
            new(Guid.NewGuid(),"Punches",TimeSpan.FromSeconds(60)),
        ],TimeSpan.FromSeconds(60)),
        new ExerciseProgram(Guid.NewGuid(),"Program 2",[
            new(Guid.NewGuid(),"Pushups",TimeSpan.FromSeconds(60)),
            new(Guid.NewGuid(),"Situps",TimeSpan.FromSeconds(60)),
            new(Guid.NewGuid(),"Pullups",TimeSpan.FromSeconds(60)),
            new(Guid.NewGuid(),"Punches",TimeSpan.FromSeconds(60))
        ],TimeSpan.FromSeconds(60)),
        new ExerciseProgram(Guid.NewGuid(),"Program 3",[
            new(Guid.NewGuid(),"Lunges", TimeSpan.FromSeconds(60)),
            new(Guid.NewGuid(),"Punches", TimeSpan.FromSeconds(60))
        ],TimeSpan.FromSeconds(60)),
        ];
}
