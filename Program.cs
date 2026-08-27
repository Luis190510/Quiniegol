using Quiniegol.Controllers;
using Quiniegol.Services;
using Quiniegol.Views;

namespace Quiniegol
{
    /// <summary>Punto de entrada de la aplicación de escritorio.</summary>
    internal static class Program
    {
        /// <summary>Solicita autenticación antes de abrir el menú principal.</summary>
        [STAThread]
        private static void Main()
        {
            ApplicationConfiguration.Initialize();

            DatosPronosticosService datosPronosticos = new();
            datosPronosticos.CompletarCoberturaDelTorneo();
            datosPronosticos.CompletarGoleadoresHistoricos();

            bool volverAlLogin;

            do
            {
                SesionUsuarioService.CerrarSesion();

                using FrmLogin login = new(new LoginController());

                if (login.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                new InsigniaService().RecalcularInsignias();

                using FrmPrincipal principal = new();
                Application.Run(principal);
                volverAlLogin = principal.SolicitoCerrarSesion;
            }
            while (volverAlLogin);

            SesionUsuarioService.CerrarSesion();
        }
    }
}
