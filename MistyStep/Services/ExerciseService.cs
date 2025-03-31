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

    public async Task<bool> CreateNewExerciseRecord(ExerciseRecord record)
    {
        var temp = record;
        if (temp is not null)
        {
            ExerciseTestList.ExerciseRecordsList.Add(record);
            return true;
        }
        return false;
    }
}

public static class ExerciseTestList
{
    public static List<Exercise> ExerciseList { get; set; } = [
        new Exercise(Guid.Parse("9D8CE2F5-1A3A-4504-A5F7-CDC8C6AD2954"),"Pushups",60,5),
        new Exercise(Guid.Parse("EA72DE54-A3C7-45B1-A87F-7B7522E95BEF"),"Situps",60,4),
        new Exercise(Guid.Parse("2820DE0C-0A79-4E43-9EE9-4B0F31915767"),"Pullups",60, 5),
        new Exercise(Guid.Parse("E5C2668C-0C79-418B-8DA9-F0D965B797CB"),"Chinups",60,6),
        new Exercise(Guid.Parse("DF91F357-8BBB-4195-A42F-7032A58C5A18"),"Lunges",60,2),
        new Exercise(Guid.Parse("CD4B4583-4FAE-4C42-AD10-3D0FAF92052C"),"Punches",60,3)
    ];
    public static List<ExerciseRecord> ExerciseRecordsList { get; set; } = [
        new ExerciseRecord(Guid.Parse("02715457-AC30-428F-BA5C-4539BCD9FEF5"),DateTime.UtcNow,Guid.Parse("9D8CE2F5-1A3A-4504-A5F7-CDC8C6AD2954"),Guid.Parse("9D8CE2F5-1A3A-4504-A5F7-CDC8C6AD2954"),60),
        new ExerciseRecord(Guid.Parse("39CB98C6-38DD-43CC-9A41-D16C129FE82E"),DateTime.UtcNow,Guid.Parse("EA72DE54-A3C7-45B1-A87F-7B7522E95BEF"),Guid.Parse("EA72DE54-A3C7-45B1-A87F-7B7522E95BEF"),60),
        new ExerciseRecord(Guid.Parse("14DBA489-5E6B-4FEE-8B9D-092D25317100"),DateTime.UtcNow,Guid.Parse("2820DE0C-0A79-4E43-9EE9-4B0F31915767"),Guid.Parse("8A1174B9-DDA8-4A85-A7B0-FBC94DDF8F79"),60),
        new ExerciseRecord(Guid.Parse("D429CAFB-4EAE-40D7-AE63-D3B9D52F6AF1"),DateTime.UtcNow,Guid.Parse("E5C2668C-0C79-418B-8DA9-F0D965B797CB"),Guid.Parse("8A1174B9-DDA8-4A85-A7B0-FBC94DDF8F79"),60),
        new ExerciseRecord(Guid.Parse("7348D384-2CF6-479D-A51F-8A0528585C12"),DateTime.UtcNow,Guid.Parse("DF91F357-8BBB-4195-A42F-7032A58C5A18"),Guid.Parse("8A1174B9-DDA8-4A85-A7B0-FBC94DDF8F79"),60),
        new ExerciseRecord(Guid.Parse("B8ADA4F9-E95F-4D52-A16B-1B41B799A69D"),DateTime.UtcNow,Guid.Parse("CD4B4583-4FAE-4C42-AD10-3D0FAF92052C"),Guid.Parse("8A1174B9-DDA8-4A85-A7B0-FBC94DDF8F79"),60)
];
    public static List<ExerciseProgram> ExerciseProgramList { get; set; } = [
        new ExerciseProgram(Guid.Parse("88846CDE-50DA-4BE0-813C-93B9D8749652"),"Program 1", [
        new Exercise(Guid.Parse("9D8CE2F5-1A3A-4504-A5F7-CDC8C6AD2954"),"Pushups",60,5),
        new Exercise(Guid.Parse("EA72DE54-A3C7-45B1-A87F-7B7522E95BEF"),"Situps",60,4),
        new Exercise(Guid.Parse("2820DE0C-0A79-4E43-9EE9-4B0F31915767"),"Pullups",60, 5),
        new Exercise(Guid.Parse("E5C2668C-0C79-418B-8DA9-F0D965B797CB"),"Chinups",60,6),
        new Exercise(Guid.Parse("DF91F357-8BBB-4195-A42F-7032A58C5A18"),"Lunges",60,2),
        new Exercise(Guid.Parse("CD4B4583-4FAE-4C42-AD10-3D0FAF92052C"),"Punches",60,3)
        ],60),
        new ExerciseProgram(Guid.Parse("6DF9B852-FC35-49DF-BA69-9138419B735E"),"Program 2",[
        new Exercise(Guid.Parse("2820DE0C-0A79-4E43-9EE9-4B0F31915767"),"Pullups",60,5),
        new Exercise(Guid.Parse("E5C2668C-0C79-418B-8DA9-F0D965B797CB"),"Chinups",60,4),
        new Exercise(Guid.Parse("DF91F357-8BBB-4195-A42F-7032A58C5A18"),"Lunges",60, 5),
        new Exercise(Guid.Parse("CD4B4583-4FAE-4C42-AD10-3D0FAF92052C"),"Punches",60,6)
        ],60),
        new ExerciseProgram(Guid.Parse("8A1174B9-DDA8-4A85-A7B0-FBC94DDF8F79"),"Program 3",[
        new Exercise(Guid.Parse("2820DE0C-0A79-4E43-9EE9-4B0F31915767"),"Pullups",5,5),
        new Exercise(Guid.Parse("E5C2668C-0C79-418B-8DA9-F0D965B797CB"),"Chinups",10,4),
        new Exercise(Guid.Parse("DF91F357-8BBB-4195-A42F-7032A58C5A18"),"Lunges",15, 5),
        new Exercise(Guid.Parse("CD4B4583-4FAE-4C42-AD10-3D0FAF92052C"),"Punches",20,3)
        ],60),
        ];
}
