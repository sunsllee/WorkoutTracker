using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TrainingApp.Models;

public partial class AllActivity
{
    public Guid ActivityId { get; set; }

    public string ExerciseTitle { get; set; } = null!;

    public DateOnly DateActivity { get; set; }

    public int? Duration { get; set; }

    public string? Note { get; set; }

    public bool ExerciseLocked { get; set; }

    public bool IsExerciseActive { get; set; }
}
