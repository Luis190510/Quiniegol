using Quiniegol.Models;

namespace Quiniegol.Services
{
    /// <summary>
    /// Construye únicamente los reportes permitidos para el rol solicitante.
    /// </summary>
    public static class ReportePorRolService
    {
        /// <summary>
        /// Distribuye los indicadores administrativos o personales según el rol.
        /// </summary>
        public static List<EstadisticaItem> CrearReporte(
            Usuario solicitante,
            IEnumerable<Pronostico> pronosticos,
            IEnumerable<Partido> partidos,
            IEnumerable<Usuario> usuarios,
            IEnumerable<Seleccion> selecciones)
        {
            ArgumentNullException.ThrowIfNull(solicitante);

            List<Pronostico> listaPronosticos = pronosticos.ToList();
            List<Partido> listaPartidos = partidos.ToList();
            List<Seleccion> listaSelecciones = selecciones.ToList();
            Dictionary<int, Partido> partidosPorId = listaPartidos
                .ToDictionary(partido => partido.Id);
            Dictionary<int, string> seleccionesPorId = listaSelecciones
                .ToDictionary(seleccion => seleccion.Id, seleccion => seleccion.Nombre);
            Dictionary<int, string> usuariosPorId = usuarios
                .ToDictionary(usuario => usuario.Id, usuario => usuario.Nombre);

            List<EstadisticaItem> reporte = solicitante.Rol == RolUsuario.Administrador
                ? CrearReporteAdministrador(
                    listaPronosticos,
                    listaPartidos,
                    partidosPorId,
                    seleccionesPorId,
                    usuariosPorId)
                : CrearReporteUsuario(
                    solicitante.Id,
                    listaPronosticos,
                    listaPartidos,
                    partidosPorId,
                    seleccionesPorId);

            AgregarEstadisticasCompartidas(reporte, listaPartidos, listaSelecciones);
            return reporte;
        }

        private static List<EstadisticaItem> CrearReporteAdministrador(
            List<Pronostico> pronosticos,
            List<Partido> partidos,
            IReadOnlyDictionary<int, Partido> partidosPorId,
            IReadOnlyDictionary<int, string> selecciones,
            IReadOnlyDictionary<int, string> usuarios)
        {
            return new List<EstadisticaItem>
            {
                CrearItem("Resultado más repetido", ObtenerResultadoMasRepetido(pronosticos)),
                CrearItem("Partido con más aciertos", ObtenerPartidoConMasAciertos(
                    pronosticos, partidosPorId, selecciones)),
                CrearItem("Usuarios con más aciertos (Top 1)", ObtenerTopUsuarios(
                    pronosticos, usuarios, 1)),
                CrearItem("Usuarios con más aciertos (Top 3)", ObtenerTopUsuarios(
                    pronosticos, usuarios, 3)),
                CrearItem("Usuarios con más aciertos (Top 5)", ObtenerTopUsuarios(
                    pronosticos, usuarios, 5)),
                CrearItem("Partido con más pronósticos", ObtenerPartidoConMasPronosticos(
                    pronosticos, partidosPorId, selecciones)),
                CrearItem("Promedio de goles", ObtenerPromedioGoles(partidos)),
                CrearItem("Partidos sin aciertos", ObtenerPartidosSinAciertos(
                    pronosticos, partidos, selecciones))
            };
        }

        private static List<EstadisticaItem> CrearReporteUsuario(
            int usuarioId,
            List<Pronostico> pronosticos,
            List<Partido> partidos,
            IReadOnlyDictionary<int, Partido> partidosPorId,
            IReadOnlyDictionary<int, string> selecciones)
        {
            List<Pronostico> pronosticosEvaluados = pronosticos
                .Where(pronostico =>
                    pronostico.UsuarioId == usuarioId &&
                    pronostico.PuntosObtenidos.HasValue)
                .ToList();
            int aciertos = pronosticosEvaluados.Count(
                pronostico => pronostico.PuntosObtenidos > 0);

            return new List<EstadisticaItem>
            {
                CrearItem("Equipo más apostado", ObtenerEquipoMasApostado(
                    pronosticos, partidosPorId, selecciones)),
                CrearItem("Equipo sorpresa (resultado y estadística)", ObtenerEquipoSorpresa(
                    pronosticos, partidos, selecciones)),
                CrearItem("Pronósticos anteriores evaluados",
                    pronosticosEvaluados.Count.ToString()),
                CrearItem("Aciertos obtenidos", aciertos.ToString()),
                CrearItem("Probabilidad histórica de acierto",
                    ObtenerProbabilidadAcierto(aciertos, pronosticosEvaluados.Count))
            };
        }

        private static void AgregarEstadisticasCompartidas(
            ICollection<EstadisticaItem> reporte,
            List<Partido> partidos,
            List<Seleccion> selecciones)
        {
            reporte.Add(CrearItem(
                "Equipo(s) con más goles",
                EstadisticasGolesService.ObtenerConMasGoles(partidos, selecciones)));
            reporte.Add(CrearItem(
                "Equipo(s) con menos goles",
                EstadisticasGolesService.ObtenerConMenosGoles(partidos, selecciones)));
        }

        private static EstadisticaItem CrearItem(string nombre, string resultado)
        {
            return new EstadisticaItem { Estadistica = nombre, Resultado = resultado };
        }

        private static string ObtenerResultadoMasRepetido(IEnumerable<Pronostico> pronosticos)
        {
            var resultado = pronosticos
                .GroupBy(pronostico => (
                    pronostico.GolesLocalPronosticados,
                    pronostico.GolesVisitantePronosticados))
                .OrderByDescending(grupo => grupo.Count())
                .ThenBy(grupo => grupo.Key.GolesLocalPronosticados)
                .ThenBy(grupo => grupo.Key.GolesVisitantePronosticados)
                .FirstOrDefault();

            return resultado == null
                ? "Sin datos"
                : $"{resultado.Key.GolesLocalPronosticados} - " +
                  $"{resultado.Key.GolesVisitantePronosticados} " +
                  $"({resultado.Count()} veces)";
        }

        private static string ObtenerPartidoConMasAciertos(
            IEnumerable<Pronostico> pronosticos,
            IReadOnlyDictionary<int, Partido> partidos,
            IReadOnlyDictionary<int, string> selecciones)
        {
            var resultado = pronosticos
                .Where(pronostico => pronostico.PuntosObtenidos > 0)
                .GroupBy(pronostico => pronostico.PartidoId)
                .OrderByDescending(grupo => grupo.Count())
                .ThenBy(grupo => grupo.Key)
                .FirstOrDefault();

            return resultado == null
                ? "Sin datos"
                : $"{ObtenerNombrePartido(partidos, selecciones, resultado.Key)} " +
                  $"({resultado.Count()} aciertos)";
        }

        private static string ObtenerTopUsuarios(
            IEnumerable<Pronostico> pronosticos,
            IReadOnlyDictionary<int, string> usuarios,
            int cantidad)
        {
            List<string> posiciones = pronosticos
                .Where(pronostico => pronostico.PuntosObtenidos > 0)
                .GroupBy(pronostico => pronostico.UsuarioId)
                .Select(grupo => new { UsuarioId = grupo.Key, Aciertos = grupo.Count() })
                .OrderByDescending(resultado => resultado.Aciertos)
                .ThenBy(resultado => usuarios.GetValueOrDefault(
                    resultado.UsuarioId,
                    "Usuario desconocido"))
                .Take(cantidad)
                .Select((resultado, indice) =>
                    $"{indice + 1}. " +
                    $"{usuarios.GetValueOrDefault(resultado.UsuarioId, "Usuario desconocido")} " +
                    $"({resultado.Aciertos} aciertos)")
                .ToList();

            return posiciones.Count == 0 ? "Sin datos" : string.Join("; ", posiciones);
        }

        private static string ObtenerPartidoConMasPronosticos(
            IEnumerable<Pronostico> pronosticos,
            IReadOnlyDictionary<int, Partido> partidos,
            IReadOnlyDictionary<int, string> selecciones)
        {
            var resultado = pronosticos
                .GroupBy(pronostico => pronostico.PartidoId)
                .OrderByDescending(grupo => grupo.Count())
                .ThenBy(grupo => grupo.Key)
                .FirstOrDefault();

            return resultado == null
                ? "Sin datos"
                : $"{ObtenerNombrePartido(partidos, selecciones, resultado.Key)} " +
                  $"({resultado.Count()} pronósticos)";
        }

        private static string ObtenerPromedioGoles(IEnumerable<Partido> partidos)
        {
            List<Partido> finalizados = partidos.Where(PartidoTieneResultado).ToList();
            if (finalizados.Count == 0)
            {
                return "Sin partidos finalizados";
            }

            double goles = finalizados.Sum(
                partido => partido.GolesLocal!.Value + partido.GolesVisitante!.Value);
            return $"{goles / finalizados.Count:0.00} goles por partido";
        }

        private static string ObtenerPartidosSinAciertos(
            IEnumerable<Pronostico> pronosticos,
            IEnumerable<Partido> partidos,
            IReadOnlyDictionary<int, string> selecciones)
        {
            Dictionary<int, List<Pronostico>> pronosticosPorPartido = pronosticos
                .Where(pronostico => pronostico.PuntosObtenidos.HasValue)
                .GroupBy(pronostico => pronostico.PartidoId)
                .ToDictionary(grupo => grupo.Key, grupo => grupo.ToList());

            List<string> sinAciertos = partidos
                .Where(PartidoTieneResultado)
                .Where(partido =>
                    pronosticosPorPartido.TryGetValue(partido.Id, out List<Pronostico>? apuestas) &&
                    apuestas.Count > 0 &&
                    apuestas.All(pronostico => pronostico.PuntosObtenidos <= 0))
                .OrderBy(partido => partido.FechaHora)
                .Select(partido =>
                    $"{ObtenerNombrePartido(partido, selecciones)} " +
                    $"({pronosticosPorPartido[partido.Id].Count} pronósticos)")
                .ToList();

            return sinAciertos.Count == 0
                ? "No se encontraron partidos sin aciertos"
                : string.Join("; ", sinAciertos);
        }

        private static string ObtenerEquipoMasApostado(
            IEnumerable<Pronostico> pronosticos,
            IReadOnlyDictionary<int, Partido> partidos,
            IReadOnlyDictionary<int, string> selecciones)
        {
            List<IGrouping<int, int>> equipos = pronosticos
                .Select(pronostico => partidos.TryGetValue(
                    pronostico.PartidoId,
                    out Partido? partido)
                    ? ObtenerGanadorPronosticado(pronostico, partido)
                    : null)
                .Where(seleccionId => seleccionId.HasValue)
                .Select(seleccionId => seleccionId!.Value)
                .GroupBy(seleccionId => seleccionId)
                .ToList();
            if (equipos.Count == 0)
            {
                return "Sin datos";
            }

            int mayorCantidad = equipos.Max(grupo => grupo.Count());
            string nombres = string.Join(
                ", ",
                equipos
                    .Where(grupo => grupo.Count() == mayorCantidad)
                    .Select(grupo => ObtenerNombreSeleccion(selecciones, grupo.Key))
                    .OrderBy(nombre => nombre));
            return $"{nombres} ({mayorCantidad} apuestas)";
        }

        private static string ObtenerEquipoSorpresa(
            IEnumerable<Pronostico> pronosticos,
            IEnumerable<Partido> partidos,
            IReadOnlyDictionary<int, string> selecciones)
        {
            Dictionary<int, List<Pronostico>> pronosticosPorPartido = pronosticos
                .GroupBy(pronostico => pronostico.PartidoId)
                .ToDictionary(grupo => grupo.Key, grupo => grupo.ToList());

            var sorpresas = new List<(Partido Partido, int GanadorId, int RivalId,
                int EnContra, int Total, double Porcentaje)>();
            foreach (Partido partido in partidos.Where(PartidoTieneResultado))
            {
                (int? ganadorId, int? rivalId) = ObtenerGanadorYRival(partido);
                if (!ganadorId.HasValue ||
                    !rivalId.HasValue ||
                    !pronosticosPorPartido.TryGetValue(
                        partido.Id,
                        out List<Pronostico>? apuestas) ||
                    apuestas.Count == 0)
                {
                    continue;
                }

                int enContra = apuestas.Count(
                    pronostico => ObtenerGanadorPronosticado(pronostico, partido) == rivalId);
                double porcentaje = (double)enContra / apuestas.Count;
                if (porcentaje >= 0.60)
                {
                    sorpresas.Add((
                        partido,
                        ganadorId.Value,
                        rivalId.Value,
                        enContra,
                        apuestas.Count,
                        porcentaje));
                }
            }

            var sorpresa = sorpresas
                .OrderByDescending(resultado => resultado.Porcentaje)
                .ThenByDescending(resultado => resultado.Total)
                .FirstOrDefault();
            if (sorpresa.Partido == null)
            {
                return "No se encontró un equipo sorpresa en el rango";
            }

            string ganador = ObtenerNombreSeleccion(selecciones, sorpresa.GanadorId);
            string rival = ObtenerNombreSeleccion(selecciones, sorpresa.RivalId);
            return $"{ObtenerNombrePartido(sorpresa.Partido, selecciones)} terminó " +
                   $"{sorpresa.Partido.GolesLocal} - {sorpresa.Partido.GolesVisitante}; " +
                   $"ganó {ganador} y {sorpresa.EnContra} de {sorpresa.Total} " +
                   $"pronósticos ({sorpresa.Porcentaje:P2}) apostaron por {rival}.";
        }

        private static string ObtenerProbabilidadAcierto(int aciertos, int evaluados)
        {
            if (evaluados == 0)
            {
                return "Sin pronósticos finalizados en el rango";
            }

            double probabilidad = (double)aciertos / evaluados;
            return $"{probabilidad:P2} ({aciertos} de {evaluados})";
        }

        private static bool PartidoTieneResultado(Partido partido)
        {
            return partido.Estado == "Finalizado" &&
                partido.GolesLocal.HasValue &&
                partido.GolesVisitante.HasValue;
        }

        private static (int? GanadorId, int? RivalId) ObtenerGanadorYRival(Partido partido)
        {
            if (partido.GolesLocal > partido.GolesVisitante)
            {
                return (partido.SeleccionLocalId, partido.SeleccionVisitanteId);
            }

            if (partido.GolesVisitante > partido.GolesLocal)
            {
                return (partido.SeleccionVisitanteId, partido.SeleccionLocalId);
            }

            return (null, null);
        }

        private static int? ObtenerGanadorPronosticado(Pronostico pronostico, Partido partido)
        {
            if (pronostico.GolesLocalPronosticados > pronostico.GolesVisitantePronosticados)
            {
                return partido.SeleccionLocalId;
            }

            if (pronostico.GolesVisitantePronosticados > pronostico.GolesLocalPronosticados)
            {
                return partido.SeleccionVisitanteId;
            }

            return null;
        }

        private static string ObtenerNombrePartido(
            IReadOnlyDictionary<int, Partido> partidos,
            IReadOnlyDictionary<int, string> selecciones,
            int partidoId)
        {
            return partidos.TryGetValue(partidoId, out Partido? partido)
                ? ObtenerNombrePartido(partido, selecciones)
                : "Partido desconocido";
        }

        private static string ObtenerNombrePartido(
            Partido partido,
            IReadOnlyDictionary<int, string> selecciones)
        {
            string local = ObtenerNombreSeleccion(selecciones, partido.SeleccionLocalId);
            string visitante = ObtenerNombreSeleccion(selecciones, partido.SeleccionVisitanteId);
            return $"{local} vs {visitante}";
        }

        private static string ObtenerNombreSeleccion(
            IReadOnlyDictionary<int, string> selecciones,
            int seleccionId)
        {
            return selecciones.GetValueOrDefault(seleccionId, $"Selección {seleccionId}");
        }
    }
}
