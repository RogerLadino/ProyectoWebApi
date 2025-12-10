using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Users
{
    public class RegistroDto
    {
        [Required(ErrorMessage = "El primer nombre es obligatorio.")]
        public required string PrimerNombre { get; set; }

        public required string SegundoNombre { get; set; }

        [Required(ErrorMessage = "El primer apellido es obligatorio.")]
        public required string PrimerApellido { get; set; }

        public required string SegundoApellido { get; set; }

        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido.")]
        public required string CorreoElectronico { get; set; }

        [Required(ErrorMessage = "La clave es obligatoria.")]
        public required string Clave { get; set; }

        [Required(ErrorMessage = "El rol es obligatorio.")]
        public required int RolId { get; set; }
    }

    public class LoginDto
    {
        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido.")]
        public required string CorreoElectronico { get; set; }

        [Required(ErrorMessage = "La clave es obligatoria.")]
        public required string Clave { get; set; }
    }

    public class LoginResponseDto
    {
        public required string Token { get; set; }
    }

    public class ForgotPasswordDto
    {
        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido.")]
        public required string CorreoElectronico { get; set; }
    }

    public class ResetPasswordDto
    {
        [Required(ErrorMessage = "El token es obligatorio.")]
        public required string Token { get; set; }

        [Required(ErrorMessage = "La nueva clave es obligatoria.")]
        public required string NuevaClave { get; set; }
    }
}
