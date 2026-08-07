using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;


namespace TrainingApp.Models;

public partial class Activity
{
    public Guid ActivityId { get; set; }

    public Guid? ExerciseId { get; set; }

    public DateOnly DateActivity { get; set; }

    public int? Duration { get; set; }

    public string? Note { get; set; }

    public bool ExerciseLocked { get; set; }

    public virtual Exercise? Exercise { get; set; }
}
