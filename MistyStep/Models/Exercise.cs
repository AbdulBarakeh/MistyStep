namespace MistyStep.Models;

public class Exercise
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public int? ExerciseDurationInSeconds { get; set; }
    public double? PointPerRep { get; set; }

    public Exercise(string Name, int? ExerciseDurationInSeconds, double PointPerRep)
    {
        this.Id = Guid.NewGuid();
        this.Name = Name;
        this.ExerciseDurationInSeconds = ExerciseDurationInSeconds;
        this.PointPerRep = PointPerRep;
    }

    public Exercise(Guid Id, string Name, int? ExerciseDurationInSeconds, double PointPerRep)
    {
        this.Id = Id;
        this.Name = Name;
        this.ExerciseDurationInSeconds = ExerciseDurationInSeconds;
        this.PointPerRep = PointPerRep;
    }

    public Exercise()
    {
        this.Id = Guid.NewGuid();
    }
}

public class ExerciseProgram
{
    public Guid Id { get; set; }
    public string Name { get; set; } = "";
    public List<Guid> ExerciseIds { get; set; } = [];
    public int? PauseDurationInSeconds { get; set; }

    public ExerciseProgram(Guid Id, string Name, List<Guid> ExerciseIds, int? PauseDurationInSeconds)
    {
        this.Id = Id;
        this.Name = Name;
        this.ExerciseIds = ExerciseIds;
        this.PauseDurationInSeconds = PauseDurationInSeconds;
    }
    public ExerciseProgram( string Name, List<Guid> ExerciseIds, int? PauseDurationInSeconds)
    {
        this.Id = Guid.NewGuid();
        this.Name = Name;
        this.ExerciseIds = ExerciseIds;
        this.PauseDurationInSeconds = PauseDurationInSeconds;
    }

    public ExerciseProgram()
    {
        this.Id = Guid.NewGuid();
    }
}

public record ExerciseRecord(Guid Id, DateTime RecordSet, Guid ExerciseId, Guid ExerciseProgramId, double Reps);

public struct Result<T>
{
    public bool IsSuccess { get; set; }
    public T? Data { get; set; }
    public string? ErrorMessage { get; set; }

    public Result(bool success, T? data, string? errorMessage)
    {
        IsSuccess = success;
        Data = data;
        ErrorMessage = errorMessage;
    }

    public static implicit operator Result<T>(T data) => new(true, data, null);
}