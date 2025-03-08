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
        new Exercise("Pushups"),
        new Exercise("Situps"),
        new Exercise("Pullups"),
        new Exercise("Chinups"),
        new Exercise("Lunges"),
        new Exercise("Punches")
    ];

    public static List<ExerciseProgram> ExerciseProgramList { get; set; } = [
        new ExerciseProgram("Program 1", [
            new("Pushups"),
            new("Situps"),
            new("Pullups"),
            new("Chinups"),
            new("Lunges"),
            new("Punches")
        ]),
        new ExerciseProgram("Program 2",[
            new("Pushups"),
            new("Situps"),
            new("Pullups"),
            new("Punches")
        ]),
        new ExerciseProgram("Program 3",[
            new("Lunges"),
            new("Punches")
        ]),
        ];
}
