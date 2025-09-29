using System;

namespace Core.Exceptions
{
    // Excepción para cuando las credenciales (correo/clave) no son válidas
    public class InvalidCredentialsException : Exception
    {
        public InvalidCredentialsException(string message) : base(message) { }
    }

    // Excepción para cuando se intenta registrar un usuario que ya existe
    public class UserAlreadyExistsException : Exception
    {
        public UserAlreadyExistsException(string message) : base(message) { }
    }

    // Excepción para tokens de recuperación de contraseña inválidos o expirados
    public class InvalidTokenException : Exception
    {
        public InvalidTokenException(string message) : base(message) { }
    }
}