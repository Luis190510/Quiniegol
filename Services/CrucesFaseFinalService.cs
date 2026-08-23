using System;
using System.Collections.Generic;
using System.Linq;
using Quiniegol.Models;

namespace Quiniegol.Services
{
    public class CrucesFaseFinalService
    {
        private readonly TablaPosicionesService
            _tablaService;

        public CrucesFaseFinalService()
        {
            _tablaService =
                new TablaPosicionesService();
        }

        public List<ClasificadoFaseFinalItem>
            ObtenerClasificados()
        {
            List<string> grupos =
                _tablaService.ObtenerGrupos();

            if (grupos.Count < 12)
            {
                throw new InvalidOperationException(
                    "Deben existir los 12 grupos " +
                    "antes de calcular la fase final."
                );
            }

            List<ClasificadoFaseFinalItem> primeros =
                new List<ClasificadoFaseFinalItem>();

            List<ClasificadoFaseFinalItem> segundos =
                new List<ClasificadoFaseFinalItem>();

            List<ClasificadoFaseFinalItem> terceros =
                new List<ClasificadoFaseFinalItem>();

            foreach (string grupo in grupos)
            {
                List<PosicionGrupoItem> tabla =
                    _tablaService.CalcularTabla(
                        grupo
                    );

                bool grupoTerminado =
                    tabla.Count >= 4 &&
                    tabla.All(fila =>
                        fila.PartidosJugados >= 3
                    );

                if (!grupoTerminado)
                {
                    throw new InvalidOperationException(
                        $"El grupo {grupo} todavía " +
                        "no ha terminado."
                    );
                }

                primeros.Add(
                    Convertir(
                        tabla[0],
                        $"1.º del grupo {grupo}"
                    )
                );

                segundos.Add(
                    Convertir(
                        tabla[1],
                        $"2.º del grupo {grupo}"
                    )
                );

                terceros.Add(
                    Convertir(
                        tabla[2],
                        $"3.º del grupo {grupo}"
                    )
                );
            }

            List<ClasificadoFaseFinalItem>
                mejoresTerceros =
                    OrdenarPorRendimiento(
                        terceros
                    )
                    .Take(8)
                    .ToList();

            List<ClasificadoFaseFinalItem>
                clasificados =
                    new List<ClasificadoFaseFinalItem>();

            clasificados.AddRange(
                OrdenarPorRendimiento(
                    primeros
                )
            );

            clasificados.AddRange(
                OrdenarPorRendimiento(
                    segundos
                )
            );

            clasificados.AddRange(
                OrdenarPorRendimiento(
                    mejoresTerceros
                )
            );

            for (int indice = 0;
                 indice < clasificados.Count;
                 indice++)
            {
                clasificados[indice].PosicionClasificacion =
                    indice + 1;
            }

            return clasificados;
        }

        public List<CruceFaseFinalItem>
            CalcularCruces()
        {
            List<ClasificadoFaseFinalItem>
                clasificados =
                    ObtenerClasificados();

            List<ClasificadoFaseFinalItem>
                primeros =
                    clasificados
                        .Where(elemento =>
                            elemento.Origen
                                .StartsWith(
                                    "1.º"
                                )
                        )
                        .ToList();

            List<ClasificadoFaseFinalItem>
                segundos =
                    clasificados
                        .Where(elemento =>
                            elemento.Origen
                                .StartsWith(
                                    "2.º"
                                )
                        )
                        .ToList();

            List<ClasificadoFaseFinalItem>
                terceros =
                    clasificados
                        .Where(elemento =>
                            elemento.Origen
                                .StartsWith(
                                    "3.º"
                                )
                        )
                        .ToList();

            List<ClasificadoFaseFinalItem>
                mejoresSegundos =
                    OrdenarPorRendimiento(
                        segundos
                    )
                    .Take(4)
                    .ToList();

            List<ClasificadoFaseFinalItem>
                cabezas =
                    OrdenarPorRendimiento(
                        primeros
                            .Concat(
                                mejoresSegundos
                            )
                            .ToList()
                    );

            List<int> idsMejoresSegundos =
                mejoresSegundos
                    .Select(elemento =>
                        elemento.SeleccionId
                    )
                    .ToList();

            List<ClasificadoFaseFinalItem>
                noCabezas =
                    OrdenarPorRendimiento(
                        segundos
                            .Where(elemento =>
                                !idsMejoresSegundos
                                    .Contains(
                                        elemento
                                            .SeleccionId
                                    )
                            )
                            .Concat(terceros)
                            .ToList()
                    );

            List<ClasificadoFaseFinalItem>
                disponibles =
                    noCabezas
                        .OrderByDescending(
                            elemento =>
                                elemento.Puntos
                        )
                        .ThenByDescending(
                            elemento =>
                                elemento
                                    .DiferenciaGoles
                        )
                        .ThenByDescending(
                            elemento =>
                                elemento.GolesFavor
                        )
                        .ToList();

            List<CruceFaseFinalItem> cruces =
                new List<CruceFaseFinalItem>();

            int numeroPartido = 1;

            foreach (ClasificadoFaseFinalItem cabeza
                     in cabezas)
            {
                int indiceRival =
                    disponibles.FindLastIndex(
                        rival =>
                            rival.Grupo !=
                            cabeza.Grupo
                    );

                if (indiceRival < 0)
                {
                    indiceRival =
                        disponibles.Count - 1;
                }

                ClasificadoFaseFinalItem rival =
                    disponibles[indiceRival];

                disponibles.RemoveAt(
                    indiceRival
                );

                cruces.Add(
                    new CruceFaseFinalItem
                    {
                        NumeroPartido =
                            numeroPartido,
                        Local =
                            cabeza.Seleccion,
                        OrigenLocal =
                            cabeza.Origen,
                        Visitante =
                            rival.Seleccion,
                        OrigenVisitante =
                            rival.Origen
                    }
                );

                numeroPartido++;
            }

            return cruces;
        }

        private ClasificadoFaseFinalItem Convertir(
            PosicionGrupoItem fila,
            string origen)
        {
            return new ClasificadoFaseFinalItem
            {
                SeleccionId =
                    fila.SeleccionId,
                Seleccion =
                    fila.Seleccion,
                Grupo =
                    fila.Grupo,
                Origen =
                    origen,
                Puntos =
                    fila.Puntos,
                DiferenciaGoles =
                    fila.DiferenciaGoles,
                GolesFavor =
                    fila.GolesFavor
            };
        }

        private List<ClasificadoFaseFinalItem>
            OrdenarPorRendimiento(
                IEnumerable<ClasificadoFaseFinalItem>
                    elementos)
        {
            return elementos
                .OrderByDescending(elemento =>
                    elemento.Puntos
                )
                .ThenByDescending(elemento =>
                    elemento.DiferenciaGoles
                )
                .ThenByDescending(elemento =>
                    elemento.GolesFavor
                )
                .ThenBy(elemento =>
                    elemento.Seleccion
                )
                .ToList();
        }
    }
}
