namespace MistyStep.Models;

public class Exercise
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public TimeSpan? ExerciseDuration { get; set; }

    public Exercise(string Name, TimeSpan ExerciseDuration)
    {
        this.Id = Guid.NewGuid();
        this.Name = Name;
        this.ExerciseDuration = ExerciseDuration;
    }

    public Exercise(Guid Id, string Name, TimeSpan ExerciseDuration)
    {
        this.Id = Id;
        this.Name = Name;
        this.ExerciseDuration = ExerciseDuration;
    }

    public Exercise()
    {
        this.Id = Guid.NewGuid();
    }
}

public record ExerciseProgram(Guid Id, string Name, List<Exercise> Exercises, TimeSpan PauseDuration);

public record ExerciseRecord(Guid Id, DateTime RecordSet, Guid ExerciseId, Guid ExerciseProgramId, double Sets);