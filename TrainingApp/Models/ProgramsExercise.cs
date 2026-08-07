using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TrainingApp.Models;

public partial class ProgramsExercise
{
    public Guid ProgramId { get; set; }

    public Guid ExerciseId { get; set; }

    public int? SetsCount { get; set; }

    public int? RepsCount { get; set; }

    public virtual Exercise Exercise { get; set; } = null!;

    public virtual Program Program { get; set; } = null!;
}
