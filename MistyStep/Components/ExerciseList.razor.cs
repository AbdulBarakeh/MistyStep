using MistyStep.Extensions;
using MistyStep.Models;
namespace MistyStep.Components;
public partial class ExerciseList
{

    public List<Exercise> Exercises { get; set; } = [];

    protected override void OnInitialized()
    {
        Exercises.AddRandomExercises();
    }

}