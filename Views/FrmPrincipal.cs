using Quiniegol.Models;
using Quiniegol.Services;

namespace Quiniegol.Views
{
    /// <summary>Presenta las opciones permitidas para la sesión actual.</summary>
    public partial class FrmPrincipal : Form
    {
        /// <summary>Indica que la persona eligió volver a la pantalla de acceso.</summary>
        public bool SolicitoCerrarSesion { get; private set; }

        public FrmPrincipal()
        {
            InitializeComponent();
            ConfigurarSesion();
        }

        private void ConfigurarSesion()
        {
            Usuario usuario = SesionUsuarioService.UsuarioActual;
            bool esAdministrador = SesionUsuarioService.EsAdministrador;

            lblSesion.Text = esAdministrador
                ? "Usuario Administrador"
                : $"Usuario: {usuario.Nombre}";
            lblFechaSimulada.Text =
                $"Fecha simulada: " +
                $"{FechaSimuladaService.Instancia.FechaActual:dd/MM/yyyy HH:mm}";
            grpAdministracion.Visible = esAdministrador;
            btnPronosticos.Visible = !esAdministrador;

            ReorganizarSecciones();
        }

        private void ReorganizarSecciones()
        {
            const int margenSuperior = 92;
            const int separacion = 14;
            int siguientePosicion = margenSuperior;

            if (grpAdministracion.Visible)
            {
                grpAdministracion.Top = siguientePosicion;
                siguientePosicion = grpAdministracion.Bottom + separacion;
            }

            grpParticipacion.Top = siguientePosicion;
            grpTorneo.Top = grpParticipacion.Bottom + separacion;
            ClientSize = new Size(920, grpTorneo.Bottom + 24);
        }

        private void btnUsuarios_Click(object sender, EventArgs e)
        {
            using FrmUsuarios formulario = new();
            formulario.ShowDialog();
        }

        private void btnSelecciones_Click(object sender, EventArgs e)
        {
            using FrmSelecciones formulario = new();
            formulario.ShowDialog();
        }

        private void btnPartidos_Click(object sender, EventArgs e)
        {
            using FrmPartidos formulario = new();
            formulario.ShowDialog();
        }

        private void btnFechaSimulada_Click(object sender, EventArgs e)
        {
            using FrmFechaSimulada formulario = new();
            formulario.ShowDialog();

            ConfigurarSesion();
        }

        private void btnPronosticos_Click(object sender, EventArgs e)
        {
            using FrmPronosticos formulario = new();
            formulario.ShowDialog();
        }

        private void btnRanking_Click(object sender, EventArgs e)
        {
            using FrmRanking formulario = new();
            formulario.ShowDialog();
        }

        private void btnHistorialPronosticos_Click(object sender, EventArgs e)
        {
            using FrmHistorialPronosticos formulario = new();
            formulario.ShowDialog();
        }

        private void btnQuinielas_Click(object sender, EventArgs e)
        {
            using FrmQuinielas formulario = new();
            formulario.ShowDialog();
        }

        private void btnRankingPrivado_Click(object sender, EventArgs e)
        {
            using FrmRankingPrivado formulario = new();
            formulario.ShowDialog();
        }

        private void btnEstadisticas_Click(object sender, EventArgs e)
        {
            using FrmEstadisticas formulario = new();
            formulario.ShowDialog();
        }

        private void btnInformacionPartidos_Click(object sender, EventArgs e)
        {
            using FrmInformacionPartidos formulario = new();
            formulario.ShowDialog();
        }

        private void btnDetallePartido_Click(object sender, EventArgs e)
        {
            using FrmDetallePartido formulario = new();
            formulario.ShowDialog();
        }

        private void btnTablaGrupos_Click(object sender, EventArgs e)
        {
            using FrmTablaGrupos formulario = new();
            formulario.ShowDialog();
        }

        private void btnFaseFinal_Click(object sender, EventArgs e)
        {
            using FrmFaseFinal formulario = new();
            formulario.ShowDialog();
        }

        private void btnTimeline_Click(object sender, EventArgs e)
        {
            using FrmTimeline formulario = new();
            formulario.ShowDialog();
        }

        private void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            SolicitoCerrarSesion = true;
            Close();
        }
    }
}
