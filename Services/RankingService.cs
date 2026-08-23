using Quiniegol.Models;

namespace Quiniegol.Services
{
    /// <summary>Convierte participantes ordenados en filas de ranking.</summary>
    public static class RankingService
    {
        /// <summary>
        /// Construye posiciones compartidas cuando dos personas tienen igual puntaje.
        /// </summary>
        public static List<RankingItem> Crear(
            IEnumerable<Usuario> usuarios,
            Func<Usuario, IEnumerable<string>> seleccionarInsignias)
        {
            ArgumentNullException.ThrowIfNull(usuarios);
            ArgumentNullException.ThrowIfNull(seleccionarInsignias);

            List<Usuario> ordenados = usuarios
                .OrderByDescending(usuario => usuario.Puntos)
                .ThenBy(usuario => usuario.Nombre)
                .ToList();
            List<RankingItem> ranking = new();
            int posicion = 0;
            int? puntosAnteriores = null;

            for (int indice = 0; indice < ordenados.Count; indice++)
            {
                Usuario usuario = ordenados[indice];

                if (puntosAnteriores != usuario.Puntos)
                {
                    posicion = indice + 1;
                }

                ranking.Add(new RankingItem
                {
                    Posicion = posicion,
                    UsuarioId = usuario.Id,
                    Nombre = usuario.Nombre,
                    PaisPreferido = usuario.PaisPreferido,
                    Puntos = usuario.Puntos,
                    Insignias = string.Join(
                        ", ",
                        seleccionarInsignias(usuario)
                    )
                });

                puntosAnteriores = usuario.Puntos;
            }

            return ranking;
        }
    }
}
