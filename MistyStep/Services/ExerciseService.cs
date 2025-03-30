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
        new Exercise(Guid.NewGuid(),"Pushups",60,5),
        new Exercise(Guid.NewGuid(),"Situps",60,4),
        new Exercise(Guid.NewGuid(),"Pullups",60, 5),
        new Exercise(Guid.NewGuid(),"Chinups",60,6),
        new Exercise(Guid.NewGuid(),"Lunges",60,2),
        new Exercise(Guid.NewGuid(),"Punches",60,3)
    ];

    public static List<ExerciseProgram> ExerciseProgramList { get; set; } = [
        new ExerciseProgram(Guid.NewGuid(),"Program 1", [
        new Exercise(Guid.NewGuid(),"Pushups",60,5),
        new Exercise(Guid.NewGuid(),"Situps",60,4),
        new Exercise(Guid.NewGuid(),"Pullups",60, 5),
        new Exercise(Guid.NewGuid(),"Chinups",60,6),
        new Exercise(Guid.NewGuid(),"Lunges",60,2),
        new Exercise(Guid.NewGuid(),"Punches",60,3)
        ],60),
        new ExerciseProgram(Guid.NewGuid(),"Program 2",[
                    new Exercise(Guid.NewGuid(),"Pushups",60,5),
        new Exercise(Guid.NewGuid(),"Situps",60,4),
        new Exercise(Guid.NewGuid(),"Pullups",60, 5),
        new Exercise(Guid.NewGuid(),"Chinups",60,6)
        ],60),
        new ExerciseProgram(Guid.NewGuid(),"Program 3",[
        new Exercise(Guid.NewGuid(),"Pushups",60,5),
        new Exercise(Guid.NewGuid(),"Situps",60,4),
        new Exercise(Guid.NewGuid(),"Pullups",60, 5),
        new Exercise(Guid.NewGuid(),"Punches",60,3)
        ],60),
        ];
}
