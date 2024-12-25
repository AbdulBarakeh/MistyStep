using System;
using System.Collections.Generic;
using MistyStep.Models;
namespace MistyStep.Extensions;
public static class ExerciseExtensions
{
    public static List<Exercise> AddRandomExercises(this List<Exercise> exercises)
    {
        var possibleNum = Enum.GetNames(typeof(CommonExercises)).Length;
        var random = new Random();

        exercises ??= [];
        for (int i = 0; i < 20; i++)
        {
            exercises.Add(new Exercise(

                 Enum.GetName(typeof(CommonExercises), random.Next(0, possibleNum)))
            );
        }

        return exercises;
    }
}

public enum CommonExercises
{
    Pushups,
    Situps,
    Pullups,
    Chinups,
    Lunges,
    Punches
}