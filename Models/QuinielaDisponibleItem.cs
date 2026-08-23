namespace Quiniegol.Models
{
    /// <summary>
    /// Identificación mínima de una quiniela disponible para inscripción.
    /// No expone descripción, integrantes, ranking ni actividad.
    /// </summary>
    public class QuinielaDisponibleItem
    {
        public int QuinielaId { get; set; }

        public string Nombre { get; set; } = "";
    }
}
