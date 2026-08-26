using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Quiniegol.Models
{
    /// <summary>
    /// Representa una cuenta que participa en Quiniegol.
    /// </summary>
    public class Usuario
    {
        /// <summary>Obtiene o establece el identificador único.</summary>
        public int Id { get; set; }

        /// <summary>Obtiene o establece el nombre visible.</summary>
        public string Nombre { get; set; } = "";

        /// <summary>Obtiene o establece el nombre utilizado para iniciar sesión.</summary>
        public string NombreUsuario { get; set; } = "";

        /// <summary>Obtiene o establece el correo que también identifica la cuenta.</summary>
        public string Correo { get; set; } = "";

        /// <summary>Obtiene o establece el hash seguro de la contraseña.</summary>
        public string ContrasenaHash { get; set; } = "";

        /// <summary>Obtiene o establece el rol autorizado.</summary>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public RolUsuario Rol { get; set; } = RolUsuario.Usuario;

        /// <summary>Indica si la cuenta puede iniciar sesión.</summary>
        public bool Activo { get; set; } = true;

        /// <summary>Indica si la cuenta conserva una contraseña temporal.</summary>
        public bool DebeCambiarContrasena { get; set; }

        /// <summary>Obtiene o establece la selección preferida.</summary>
        public string PaisPreferido { get; set; } = "";

        /// <summary>Obtiene o establece los puntos calculados.</summary>
        public int Puntos { get; set; }

        /// <summary>Obtiene o establece las insignias obtenidas.</summary>
        public List<string> Insignias { get; set; } = new();
    }
}
