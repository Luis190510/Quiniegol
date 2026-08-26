namespace Quiniegol.Models
{
    public class PosicionGrupoItem
    {
        public int Posicion { get; set; }

        public int SeleccionId { get; set; }

        public string Seleccion { get; set; } = "";

        public string Grupo { get; set; } = "";

        public int PartidosJugados { get; set; }

        public int PartidosGanados { get; set; }

        public int PartidosEmpatados { get; set; }

        public int PartidosPerdidos { get; set; }

        public int GolesFavor { get; set; }

        public int GolesContra { get; set; }

        public int DiferenciaGoles =>
            GolesFavor - GolesContra;

        public int Puntos { get; set; }
    }
}
