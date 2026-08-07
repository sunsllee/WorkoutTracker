using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;


namespace TrainingApp.Models;

public partial class Program
{
    public Guid ProgramId { get; set; }

    public string ProgramTitle { get; set; } = null!;

    public string TypeProgram { get; set; } = null!;

    public bool Active { get; set; }

    public virtual ICollection<ProgramsExercise> ProgramsExercises { get; set; } = new List<ProgramsExercise>();
}
