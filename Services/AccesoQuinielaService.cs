using Quiniegol.Models;

namespace Quiniegol.Services
{
    /// <summary>Centraliza las reglas de privacidad de las quinielas.</summary>
    public static class AccesoQuinielaService
    {
        /// <summary>
        /// Indica si una cuenta puede consultar integrantes, ranking y actividad.
        /// </summary>
        public static bool PuedeConsultar(Quiniela quiniela, Usuario usuario)
        {
            ArgumentNullException.ThrowIfNull(quiniela);
            ArgumentNullException.ThrowIfNull(usuario);

            return usuario.Rol == RolUsuario.Administrador ||
                   quiniela.IntegrantesIds.Contains(usuario.Id);
        }

        /// <summary>Indica si una cuenta puede modificar una quiniela.</summary>
        public static bool PuedeAdministrar(Quiniela quiniela, Usuario usuario)
        {
            ArgumentNullException.ThrowIfNull(quiniela);
            ArgumentNullException.ThrowIfNull(usuario);

            return usuario.Rol == RolUsuario.Administrador ||
                   quiniela.CreadorUsuarioId == usuario.Id;
        }

        /// <summary>
        /// Permite que un participante se una por nombre exacto sin revelar
        /// previamente la información de la quiniela.
        /// </summary>
        public static bool PuedeUnirse(Quiniela quiniela, Usuario usuario)
        {
            ArgumentNullException.ThrowIfNull(quiniela);
            ArgumentNullException.ThrowIfNull(usuario);

            return usuario.Rol == RolUsuario.Usuario &&
                   !quiniela.IntegrantesIds.Contains(usuario.Id);
        }

        /// <summary>Rechaza la consulta de una quiniela ajena.</summary>
        public static void ExigirConsulta(Quiniela quiniela, Usuario usuario)
        {
            if (!PuedeConsultar(quiniela, usuario))
            {
                throw new UnauthorizedAccessException(
                    "No tiene acceso a esta quiniela privada."
                );
            }
        }

        /// <summary>Rechaza cambios hechos por alguien que no sea creador o administrador.</summary>
        public static void ExigirAdministracion(
            Quiniela quiniela,
            Usuario usuario)
        {
            if (!PuedeAdministrar(quiniela, usuario))
            {
                throw new UnauthorizedAccessException(
                    "Solo el creador puede administrar esta quiniela."
                );
            }
        }
    }
}
