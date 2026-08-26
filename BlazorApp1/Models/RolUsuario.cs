namespace Quiniegol.Models
{
    /// <summary>
    /// Define los permisos generales disponibles para un usuario.
    /// </summary>
    public enum RolUsuario
    {
        /// <summary>
        /// Puede consultar información y administrar sus propios pronósticos.
        /// </summary>
        Usuario = 0,

        /// <summary>
        /// Puede administrar usuarios, partidos, resultados y la fecha simulada.
        /// </summary>
        Administrador = 1
    }
}
