using MistyStep.Models;
using MistyStep.Models.Extensions;
namespace MistyStep.Components;
public partial class ExerciseList
{

    public List<ExerciseModel> Exercises { get; set; } = [];

    protected override void OnInitialized()
    {
        Exercises.AddRandomExercises();
    }

}