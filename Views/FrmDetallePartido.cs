using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Quiniegol.Controllers;
using Quiniegol.Models;
using Quiniegol.Services;

namespace Quiniegol.Views
{
    public partial class FrmDetallePartido : Form
    {
        private readonly DetallePartidoController
            _detalleController;

        private PartidoDetalleItem?
            _detalleActual;

        public FrmDetallePartido()
        {
            InitializeComponent();

            _detalleController =
                new DetallePartidoController();

            CargarListaPartidos();
        }

        private void CargarListaPartidos()
        {
            var partidos =
                _detalleController
                    .ObtenerOpcionesPartidos();

            cmbPartido.DataSource = null;
            cmbPartido.DataSource = partidos;
            cmbPartido.DisplayMember =
                "Descripcion";
            cmbPartido.ValueMember =
                "PartidoId";

            cmbPartido.DropDownStyle =
                ComboBoxStyle.DropDownList;
        }

        private void CargarDetalle()
        {
            if (cmbPartido.SelectedItem
                is not PartidoOpcionItem opcion)
            {
                MessageBox.Show(
                    "Debe seleccionar un partido.",
                    "Dato requerido",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                return;
            }

            _detalleActual =
                _detalleController
                    .ObtenerDetalle(
                        opcion.PartidoId
                    );

            lblNombreLocal.Text =
                _detalleActual.Local;

            lblNombreVisitante.Text =
                _detalleActual.Visitante;

            lblMarcador.Text =
                "Marcador: " +
                _detalleActual.Marcador;

            lblEstado.Text =
                "Estado: " +
                _detalleActual.Estado;

            lblFecha.Text =
                "Fecha: " +
                _detalleActual.FechaHora
                    .ToString(
                        "dd/MM/yyyy HH:mm"
                    );

            CargarBandera(
                picLocal,
                _detalleActual.RutaBanderaLocal
            );

            CargarBandera(
                picVisitante,
                _detalleActual.RutaBanderaVisitante
            );

            cmbSeleccionAnotador.DataSource =
                new[]
                {
                    new SeleccionOpcionItem
                    {
                        SeleccionId = 0,
                        Nombre = "Todas las selecciones"
                    },
                    new SeleccionOpcionItem
                    {
                        SeleccionId =
                            _detalleActual
                                .SeleccionLocalId,
                        Nombre =
                            _detalleActual.Local
                    },
                    new SeleccionOpcionItem
                    {
                        SeleccionId =
                            _detalleActual
                                .SeleccionVisitanteId,
                        Nombre =
                            _detalleActual.Visitante
                    }
                };

            cmbSeleccionAnotador.DisplayMember =
                "Nombre";

            cmbSeleccionAnotador.ValueMember =
                "SeleccionId";

            cmbSeleccionAnotador.DropDownStyle =
                ComboBoxStyle.DropDownList;

            bool partidoFinalizado =
                _detalleActual.Estado ==
                "Finalizado";

            cmbSeleccionAnotador.Enabled =
                partidoFinalizado;

            lblGoleadores.Text = partidoFinalizado
                ? "Goleadores oficiales del partido"
                : "Los goleadores aparecerán cuando el partido finalice " +
                  "según la fecha simulada.";

            MostrarAnotadores(0);
        }

        private void MostrarAnotadores(int seleccionId)
        {
            if (_detalleActual == null)
            {
                return;
            }

            var anotadores = _detalleActual.Anotadores
                .Where(anotador =>
                    seleccionId == 0 ||
                    anotador.SeleccionId == seleccionId)
                .ToList();

            dgvAnotadores.DataSource = null;
            dgvAnotadores.DataSource = anotadores;
            ConfigurarColumnasAnotadores();
        }

        private void CargarBandera(
            PictureBox pictureBox,
            string rutaRelativa)
        {
            pictureBox.Image?.Dispose();
            pictureBox.Image = null;

            if (string.IsNullOrWhiteSpace(
                rutaRelativa
            ))
            {
                return;
            }

            string rutaCompleta =
                RutaDatosService
                    .ObtenerRutaRecurso(
                        rutaRelativa
                    );

            if (!File.Exists(rutaCompleta))
            {
                return;
            }

            using Image imagenOriginal =
                Image.FromFile(
                    rutaCompleta
                );

            pictureBox.Image =
                new Bitmap(
                    imagenOriginal
                );
        }

        private void ConfigurarColumnasAnotadores()
        {
            if (dgvAnotadores.Columns["SeleccionId"]
                is DataGridViewColumn columnaId)
            {
                columnaId.Visible = false;
            }

            CambiarTituloAnotador("Jugador", "Jugador");
            CambiarTituloAnotador("Seleccion", "Selección");
            CambiarTituloAnotador("Minuto", "Minuto");
        }

        private void CambiarTituloAnotador(string nombre, string titulo)
        {
            if (dgvAnotadores.Columns[nombre]
                is DataGridViewColumn columna)
            {
                columna.HeaderText = titulo;
            }
        }

        private void btnCargar_Click(
            object sender,
            EventArgs e)
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
                    MessageBoxIcon.Error
                );
            }
        }

        private void cmbSeleccionAnotador_SelectedIndexChanged(
            object sender,
            EventArgs e)
        {
            if (cmbSeleccionAnotador.SelectedItem
                is SeleccionOpcionItem seleccion)
            {
                MostrarAnotadores(seleccion.SeleccionId);
            }
        }

        private void btnCerrar_Click(
            object sender,
            EventArgs e)
        {
            Close();
        }
    }
}
