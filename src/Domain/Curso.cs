using System;
using System.Collections.Generic;

public class Curso
{
    public Guid Id { get; set; }
    public string Titulo { get; set; }
    public string Descripcion { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public int Duracion { get; set; } // en horas o semanas
    public decimal Precio { get; set; }
    public string RequisitosPrevios { get; set; }
    public Guid InstructorId { get; set; }
    public Instructor Instructor { get; set; }
    public string Modalidad { get; set; }
    public string MaterialesIncluidos { get; set; } 
    public string Certificacion { get; set; }
    public int NumeroPlazas { get; set; }
    public string Ubicacion { get; set; }
    public string Categoria { get; set; }
    public List<string> Horarios { get; set; } = new List<string>();
    public List<Inscripcion> Inscripciones { get; set; } = new List<Inscripcion>();
}