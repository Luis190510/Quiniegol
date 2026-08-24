using Quiniegol.Models;

namespace Quiniegol.Services
{
    /// <summary>
    /// Calcula los clasificados y los cruces de la primera ronda eliminatoria.
    /// </summary>
    public class CrucesFaseFinalService
    {
        private const int CantidadGrupos = 12;
        private const int MejoresTercerosRequeridos = 8;
        private const int MejoresSegundosCabeza = 4;

        private readonly TablaPosicionesService _tablaService;

        public CrucesFaseFinalService()
        {
            _tablaService = new TablaPosicionesService();
        }

        /// <summary>
        /// Obtiene los dos primeros lugares de cada grupo y los ocho mejores terceros.
        /// </summary>
        public List<ClasificadoFaseFinalItem> ObtenerClasificados()
        {
            List<string> grupos = _tablaService.ObtenerGrupos();
            if (grupos.Count < CantidadGrupos)
            {
                throw new InvalidOperationException(
                    $"Deben existir los {CantidadGrupos} grupos antes de calcular la fase final.");
            }

            var primeros = new List<ClasificadoFaseFinalItem>();
            var segundos = new List<ClasificadoFaseFinalItem>();
            var terceros = new List<ClasificadoFaseFinalItem>();

            foreach (string grupo in grupos)
            {
                List<PosicionGrupoItem> tabla = _tablaService.CalcularTabla(grupo);
                ValidarGrupoTerminado(grupo, tabla);

                primeros.Add(Convertir(tabla[0], $"1.º del grupo {grupo}"));
                segundos.Add(Convertir(tabla[1], $"2.º del grupo {grupo}"));
                terceros.Add(Convertir(tabla[2], $"3.º del grupo {grupo}"));
            }

            List<ClasificadoFaseFinalItem> clasificados = OrdenarPorRendimiento(primeros)
                .Concat(OrdenarPorRendimiento(segundos))
                .Concat(OrdenarPorRendimiento(terceros).Take(MejoresTercerosRequeridos))
                .ToList();

            for (int indice = 0; indice < clasificados.Count; indice++)
            {
                clasificados[indice].PosicionClasificacion = indice + 1;
            }

            return clasificados;
        }

        /// <summary>
        /// Forma los cruces procurando que dos selecciones del mismo grupo no se enfrenten.
        /// </summary>
        public List<CruceFaseFinalItem> CalcularCruces()
        {
            List<ClasificadoFaseFinalItem> clasificados = ObtenerClasificados();
            List<ClasificadoFaseFinalItem> primeros = FiltrarPorOrigen(clasificados, "1.º");
            List<ClasificadoFaseFinalItem> segundos = FiltrarPorOrigen(clasificados, "2.º");
            List<ClasificadoFaseFinalItem> terceros = FiltrarPorOrigen(clasificados, "3.º");

            List<ClasificadoFaseFinalItem> mejoresSegundos = OrdenarPorRendimiento(segundos)
                .Take(MejoresSegundosCabeza)
                .ToList();
            HashSet<int> idsCabezasAdicionales = mejoresSegundos
                .Select(clasificado => clasificado.SeleccionId)
                .ToHashSet();

            List<ClasificadoFaseFinalItem> cabezas = OrdenarPorRendimiento(
                primeros.Concat(mejoresSegundos));
            List<ClasificadoFaseFinalItem> disponibles = OrdenarPorRendimiento(
                segundos.Where(segundo => !idsCabezasAdicionales.Contains(segundo.SeleccionId))
                    .Concat(terceros));

            var cruces = new List<CruceFaseFinalItem>();
            foreach (ClasificadoFaseFinalItem cabeza in cabezas)
            {
                int indiceRival = BuscarRival(disponibles, cabeza.Grupo);
                ClasificadoFaseFinalItem rival = disponibles[indiceRival];
                disponibles.RemoveAt(indiceRival);

                cruces.Add(new CruceFaseFinalItem
                {
                    NumeroPartido = cruces.Count + 1,
                    Local = cabeza.Seleccion,
                    OrigenLocal = cabeza.Origen,
                    Visitante = rival.Seleccion,
                    OrigenVisitante = rival.Origen
                });
            }

            return cruces;
        }

        private static void ValidarGrupoTerminado(string grupo, List<PosicionGrupoItem> tabla)
        {
            bool grupoTerminado = tabla.Count >= 4 &&
                tabla.All(fila => fila.PartidosJugados >= 3);
            if (!grupoTerminado)
            {
                throw new InvalidOperationException($"El grupo {grupo} todavía no ha terminado.");
            }
        }

        private static List<ClasificadoFaseFinalItem> FiltrarPorOrigen(
            IEnumerable<ClasificadoFaseFinalItem> clasificados,
            string prefijo)
        {
            return clasificados
                .Where(clasificado => clasificado.Origen.StartsWith(prefijo))
                .ToList();
        }

        private static int BuscarRival(
            List<ClasificadoFaseFinalItem> disponibles,
            string grupoCabeza)
        {
            int indice = disponibles.FindLastIndex(rival => rival.Grupo != grupoCabeza);
            return indice >= 0 ? indice : disponibles.Count - 1;
        }

        private static ClasificadoFaseFinalItem Convertir(
            PosicionGrupoItem fila,
            string origen)
        {
            return new ClasificadoFaseFinalItem
            {
                SeleccionId = fila.SeleccionId,
                Seleccion = fila.Seleccion,
                Grupo = fila.Grupo,
                Origen = origen,
                Puntos = fila.Puntos,
                DiferenciaGoles = fila.DiferenciaGoles,
                GolesFavor = fila.GolesFavor
            };
        }

        private static List<ClasificadoFaseFinalItem> OrdenarPorRendimiento(
            IEnumerable<ClasificadoFaseFinalItem> clasificados)
        {
            return clasificados
                .OrderByDescending(clasificado => clasificado.Puntos)
                .ThenByDescending(clasificado => clasificado.DiferenciaGoles)
                .ThenByDescending(clasificado => clasificado.GolesFavor)
                .ThenBy(clasificado => clasificado.Seleccion)
                .ToList();
        }
    }
}
