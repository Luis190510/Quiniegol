namespace Quiniegol.Models
{
    public class Anotador
    {
        public int Id { get; set; }

        public int SeleccionId { get; set; }

        public string NombreJugador { get; set; } = "";

        public int Minuto { get; set; }
    }
}
