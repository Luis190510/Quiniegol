namespace Quiniegol.Models
{
    public class ClasificadoFaseFinalItem
    {
        public int Semilla { get; set; }

        public int SeleccionId { get; set; }

        public string Seleccion { get; set; } = "";

        public string Grupo { get; set; } = "";

        public string Origen { get; set; } = "";

        public int Puntos { get; set; }

        public int DiferenciaGoles { get; set; }

        public int GolesFavor { get; set; }
    }
}
