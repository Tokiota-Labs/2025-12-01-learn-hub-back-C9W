using System;
using System.Collections.Generic;

public class Instructor
{
    public Guid Id { get; set; }
    public string Nombre { get; set; }
    public string Biografia { get; set; }

    public List<Curso> Cursos { get; set; } = new List<Curso>();
}