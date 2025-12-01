using System.ComponentModel.DataAnnotations;

namespace LearnHub.Back.Application.DTOs
{
    /// <summary>
    /// Representa un alumno en el sistema
    /// </summary>
    public class AlumnoDto
    {
        /// <summary>
        /// Identificador único del alumno
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Nombre del alumno
        /// </summary>
        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 50 caracteres")]
        public string Nombre { get; set; }

        /// <summary>
        /// Apellido del alumno
        /// </summary>
        [Required(ErrorMessage = "El apellido es requerido")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "El apellido debe tener entre 2 y 50 caracteres")]
        public string Apellido { get; set; }

        /// <summary>
        /// Fecha de nacimiento del alumno
        /// </summary>
        [Required(ErrorMessage = "La fecha de nacimiento es requerida")]
        [DataType(DataType.Date)]
        public DateTime FechaNacimiento { get; set; }

        /// <summary>
        /// Correo electrónico del alumno
        /// </summary>
        [Required(ErrorMessage = "El email es requerido")]
        [EmailAddress(ErrorMessage = "El formato del email no es válido")]
        [StringLength(100, ErrorMessage = "El email no puede exceder los 100 caracteres")]
        public string Email { get; set; }
    }
}