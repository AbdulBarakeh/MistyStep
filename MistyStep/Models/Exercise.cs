namespace MistyStep.Models;

public record Exercise(Guid Id, string Name, TimeSpan ExerciseDuration);

public record ExerciseProgram(Guid Id, string Name, List<Exercise> Exercises, TimeSpan PauseDuration);

public record ExerciseRecord(Guid Id, DateTime RecordSet, Guid ExerciseId, Guid ExerciseProgramId, double Sets);