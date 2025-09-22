using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Users
{
    public class RegistroDTO
    {
        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido.")]
        public string CorreoElectronico { get; set; }

        [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        public string NombreUsuario { get; set; }

        [Required(ErrorMessage = "La clave es obligatoria.")]
        public string Clave { get; set; }

        [Required(ErrorMessage = "El rol es obligatorio.")]
        public int RolId { get; set; }
    }

    public class LoginDTO
    {
        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido.")]
        public string CorreoElectronico { get; set; }

        [Required(ErrorMessage = "La clave es obligatoria.")]
        public string Clave { get; set; }
    }

    public class LoginResponseDTO
    {
        public string Token { get; set; }
    }

    public class ForgotPasswordDTO
    {
        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido.")]
        public string CorreoElectronico { get; set; }
    }

    public class ResetPasswordDTO
    {
        [Required(ErrorMessage = "El token es obligatorio.")]
        public string Token { get; set; }

        [Required(ErrorMessage = "La nueva clave es obligatoria.")]
        public string NuevaClave { get; set; }
    }
}
