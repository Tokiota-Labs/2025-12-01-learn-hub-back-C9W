using System.ComponentModel.DataAnnotations;

namespace LearnHub.Back.Application.DTOs
{
    /// <summary>
    /// Representa un curso en el sistema
    /// </summary>
    public class CursoDto
    {
        /// <summary>
        /// Identificador único del curso
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Nombre del curso
        /// </summary>
        [Required(ErrorMessage = "El nombre del curso es requerido")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 100 caracteres")]
        public string Nombre { get; set; }

        /// <summary>
        /// Descripción detallada del curso
        /// </summary>
        [Required(ErrorMessage = "La descripción del curso es requerida")]
        [StringLength(500, ErrorMessage = "La descripción no puede exceder los 500 caracteres")]
        public string Descripcion { get; set; }

        /// <summary>
        /// Fecha de inicio del curso
        /// </summary>
        [Required(ErrorMessage = "La fecha de inicio es requerida")]
        public DateTime FechaInicio { get; set; }

        /// <summary>
        /// Fecha de finalización del curso
        /// </summary>
        [Required(ErrorMessage = "La fecha de finalización es requerida")]
        public DateTime FechaFin { get; set; }
    }
}