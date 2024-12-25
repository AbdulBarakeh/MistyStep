using MistyStep.Models;
using MistyStep.Extensions;
namespace MistyStep.Components;
public partial class ProgramList
{

    public List<Models.ExerciseProgram> Programs { get; set; } = [];

    protected override void OnInitialized()
    {
    }

}