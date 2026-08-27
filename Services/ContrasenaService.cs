using System.Security.Cryptography;

namespace Quiniegol.Services
{
    /// <summary>
    /// Crea y verifica hashes PBKDF2 para las contraseñas de Quiniegol.
    /// </summary>
    public static class ContrasenaService
    {
        private const int Iteraciones = 100_000;
        private const int TamanoSal = 16;
        private const int TamanoHash = 32;
        private const string Algoritmo = "PBKDF2-SHA256";

        /// <summary>
        /// Genera una representación segura y salada de una contraseña.
        /// </summary>
        /// <param name="contrasena">Contraseña proporcionada por el usuario.</param>
        /// <returns>Cadena que contiene algoritmo, iteraciones, sal y hash.</returns>
        public static string CrearHash(string contrasena)
        {
            if (string.IsNullOrWhiteSpace(contrasena))
            {
                throw new ArgumentException(
                    "La contraseña no puede estar vacía.",
                    nameof(contrasena)
                );
            }

            byte[] sal = RandomNumberGenerator.GetBytes(TamanoSal);
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
                contrasena,
                sal,
                Iteraciones,
                HashAlgorithmName.SHA256,
                TamanoHash
            );

            return string.Join(
                '$',
                Algoritmo,
                Iteraciones,
                Convert.ToBase64String(sal),
                Convert.ToBase64String(hash)
            );
        }

        /// <summary>
        /// Comprueba una contraseña sin exponer el valor almacenado.
        /// </summary>
        /// <param name="contrasena">Contraseña a comprobar.</param>
        /// <param name="hashAlmacenado">Hash generado por <see cref="CrearHash"/>.</param>
        /// <returns><see langword="true"/> cuando la contraseña coincide.</returns>
        public static bool Verificar(
            string contrasena,
            string hashAlmacenado)
        {
            if (string.IsNullOrEmpty(contrasena) ||
                string.IsNullOrWhiteSpace(hashAlmacenado))
            {
                return false;
            }

            string[] partes = hashAlmacenado.Split('$');

            if (partes.Length != 4 ||
                !partes[0].Equals(
                    Algoritmo,
                    StringComparison.Ordinal) ||
                !int.TryParse(partes[1], out int iteraciones))
            {
                return false;
            }

            try
            {
                byte[] sal = Convert.FromBase64String(partes[2]);
                byte[] hashEsperado = Convert.FromBase64String(partes[3]);
                byte[] hashCalculado = Rfc2898DeriveBytes.Pbkdf2(
                    contrasena,
                    sal,
                    iteraciones,
                    HashAlgorithmName.SHA256,
                    hashEsperado.Length
                );

                return CryptographicOperations.FixedTimeEquals(
                    hashCalculado,
                    hashEsperado
                );
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
