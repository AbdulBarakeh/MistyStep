namespace MistyStep.Models;
[AutoInterfaceAttributes.AutoInterface]
public class ExerciseService : IExerciseService
{
    public async Task<bool> CreateNewExercise(ExerciseModel model)
    {
        var temp = model;
        if (temp is not null)
        {
            return true;
        }
        return false;
    }
}
