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
    //protected override void OnAfterRender(bool firstRender)
    //{
    //    if (firstRender)
    //    {
    //        Exercises.AddRandomExercises();
    //        Exercises.Add(new ExerciseModel("Test",5,5,5));
    //    }
    //}
}