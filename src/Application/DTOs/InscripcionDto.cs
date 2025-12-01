using System;
using System.ComponentModel.DataAnnotations;

namespace LearnHub.Back.Application.DTOs
{
    /// <summary>
    /// Representa una inscripción a un curso
    /// </summary>
    public class InscripcionDto
    {
        /// <summary>
        /// Identificador único de la inscripción
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// ID del alumno inscrito
        /// </summary>
        [Required(ErrorMessage = "El ID del alumno es requerido")]
        public Guid AlumnoId { get; set; }

        /// <summary>
        /// ID del curso al que se inscribe
        /// </summary>
        [Required(ErrorMessage = "El ID del curso es requerido")]
        public Guid CursoId { get; set; }

        /// <summary>
        /// Fecha en que se realiza la inscripción
        /// </summary>
        [Required(ErrorMessage = "La fecha de inscripción es requerida")]
        public DateTime FechaInscripcion { get; set; }

        /// <summary>
        /// Estado actual de la inscripción
        /// </summary>
        [Required(ErrorMessage = "El estado es requerido")]
        [RegularExpression("^(Aprobada|Rechazada|Pendiente)$", 
            ErrorMessage = "El estado debe ser 'Aprobada', 'Rechazada' o 'Pendiente'")]
        public string Estado { get; set; }

        /// <summary>
        /// Preferencia de horario del alumno
        /// </summary>
        [Required(ErrorMessage = "La preferencia de horario es requerida")]
        [StringLength(50, ErrorMessage = "La preferencia de horario no puede exceder los 50 caracteres")]
        public string PreferenciaHorario { get; set; }

        /// <summary>
        /// ID del pago asociado a la inscripción
        /// </summary>
        public Guid? PagoId { get; set; }

        // Propiedades de navegación para la API
        public AlumnoDto Alumno { get; set; }
        public CursoDto Curso { get; set; }
    }
}