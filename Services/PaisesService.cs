namespace Quiniegol.Services
{
    /// <summary>Proporciona el catálogo compartido de selecciones favoritas.</summary>
    public static class PaisesService
    {
        private static readonly string[] Paises =
        {
            "Argentina", "Arabia Saudita", "Australia", "Austria",
            "Bélgica", "Bosnia y Herzegovina", "Brasil", "Cabo Verde",
            "Canadá", "Colombia", "Congo DR", "Corea del Sur",
            "Costa de Marfil", "Croacia", "Curazao", "Ecuador", "Egipto",
            "Escocia", "Eslovaquia", "España", "Estados Unidos", "Francia",
            "Gales", "Ghana", "Haití", "Inglaterra", "Irán", "Irak",
            "Japón", "Jordania", "Marruecos", "México", "Noruega",
            "Nueva Zelanda", "Países Bajos", "Panamá", "Paraguay",
            "Portugal", "Qatar", "República Checa", "Senegal", "Sudáfrica",
            "Suecia", "Suiza", "Túnez", "Turquía", "Uruguay", "Uzbekistán"
        };

        /// <summary>Obtiene los países ordenados para mostrarlos en formularios.</summary>
        public static IReadOnlyList<string> ObtenerTodos()
        {
            return Paises;
        }
    }
}
