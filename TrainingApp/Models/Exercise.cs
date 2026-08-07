using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TrainingApp.Models;

public partial class Exercise
{
    public Guid ExerciseId { get; set; }

    public string ExerciseTitle { get; set; } = null!;

    public bool Active { get; set; }

    public virtual ICollection<Activity> Activities { get; set; } = new List<Activity>();

    public virtual ICollection<ProgramsExercise> ProgramsExercises { get; set; } = new List<ProgramsExercise>();
}
