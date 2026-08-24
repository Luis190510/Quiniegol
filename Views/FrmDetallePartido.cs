using Quiniegol.Controllers;
using Quiniegol.Models;
using Quiniegol.Services;

namespace Quiniegol.Views
{
    /// <summary>
    /// Muestra marcador, banderas y goleadores oficiales de un encuentro.
    /// </summary>
    public partial class FrmDetallePartido : Form
    {
        private readonly DetallePartidoController _detalleController;
        private PartidoDetalleItem? _detalleActual;

        public FrmDetallePartido()
        {
            InitializeComponent();
            _detalleController = new DetallePartidoController();
            CargarListaPartidos();
        }

        private void CargarListaPartidos()
        {
            cmbPartido.DataSource = _detalleController.ObtenerOpcionesPartidos();
            cmbPartido.DisplayMember = nameof(PartidoOpcionItem.Descripcion);
            cmbPartido.ValueMember = nameof(PartidoOpcionItem.PartidoId);
            cmbPartido.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void CargarDetalle()
        {
            if (cmbPartido.SelectedItem is not PartidoOpcionItem opcion)
            {
                MessageBox.Show(
                    "Debe seleccionar un partido.",
                    "Dato requerido",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            _detalleActual = _detalleController.ObtenerDetalle(opcion.PartidoId);
            lblNombreLocal.Text = _detalleActual.Local;
            lblNombreVisitante.Text = _detalleActual.Visitante;
            lblMarcador.Text = $"Marcador: {_detalleActual.Marcador}";
            lblEstado.Text = $"Estado: {_detalleActual.Estado}";
            lblFecha.Text = $"Fecha: {_detalleActual.FechaHora:dd/MM/yyyy HH:mm}";
            CargarBandera(picLocal, _detalleActual.RutaBanderaLocal);
            CargarBandera(picVisitante, _detalleActual.RutaBanderaVisitante);
            CargarFiltroSelecciones();

            bool partidoFinalizado = _detalleActual.Estado == "Finalizado";
            cmbSeleccionAnotador.Enabled = partidoFinalizado;
            lblGoleadores.Text = partidoFinalizado
                ? "Goleadores oficiales del partido"
                : "Los goleadores aparecerán cuando el partido finalice " +
                  "según la fecha simulada.";
            MostrarAnotadores(0);
        }

        private void CargarFiltroSelecciones()
        {
            if (_detalleActual == null)
            {
                return;
            }

            cmbSeleccionAnotador.DataSource = new[]
            {
                new SeleccionOpcionItem
                {
                    SeleccionId = 0,
                    Nombre = "Todas las selecciones"
                },
                new SeleccionOpcionItem
                {
                    SeleccionId = _detalleActual.SeleccionLocalId,
                    Nombre = _detalleActual.Local
                },
                new SeleccionOpcionItem
                {
                    SeleccionId = _detalleActual.SeleccionVisitanteId,
                    Nombre = _detalleActual.Visitante
                }
            };
            cmbSeleccionAnotador.DisplayMember = nameof(SeleccionOpcionItem.Nombre);
            cmbSeleccionAnotador.ValueMember = nameof(SeleccionOpcionItem.SeleccionId);
            cmbSeleccionAnotador.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void MostrarAnotadores(int seleccionId)
        {
            if (_detalleActual == null)
            {
                return;
            }

            dgvAnotadores.DataSource = _detalleActual.Anotadores
                .Where(anotador => seleccionId == 0 || anotador.SeleccionId == seleccionId)
                .ToList();
            ConfigurarColumnasAnotadores();
        }

        private static void CargarBandera(PictureBox pictureBox, string rutaRelativa)
        {
            pictureBox.Image?.Dispose();
            pictureBox.Image = null;
            if (string.IsNullOrWhiteSpace(rutaRelativa))
            {
                return;
            }

            string rutaCompleta = RutaDatosService.ObtenerRutaRecurso(rutaRelativa);
            if (!File.Exists(rutaCompleta))
            {
                return;
            }

            using Image imagenOriginal = Image.FromFile(rutaCompleta);
            pictureBox.Image = new Bitmap(imagenOriginal);
        }

        private void ConfigurarColumnasAnotadores()
        {
            if (dgvAnotadores.Columns[nameof(AnotadorVistaItem.SeleccionId)]
                is DataGridViewColumn columnaId)
            {
                columnaId.Visible = false;
            }

            CambiarTituloAnotador(nameof(AnotadorVistaItem.Jugador), "Jugador");
            CambiarTituloAnotador(nameof(AnotadorVistaItem.Seleccion), "Selección");
            CambiarTituloAnotador(nameof(AnotadorVistaItem.Minuto), "Minuto");
        }

        private void CambiarTituloAnotador(string nombre, string titulo)
        {
            if (dgvAnotadores.Columns[nombre] is DataGridViewColumn columna)
            {
                columna.HeaderText = titulo;
            }
        }

        private void btnCargar_Click(object sender, EventArgs e)
        {
            try
            {
                CargarDetalle();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Error al cargar el partido",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void cmbSeleccionAnotador_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbSeleccionAnotador.SelectedItem is SeleccionOpcionItem seleccion)
            {
                MostrarAnotadores(seleccion.SeleccionId);
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
