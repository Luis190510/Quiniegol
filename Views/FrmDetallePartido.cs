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

            dgvAnotadores.DataSource = null;

            dgvAnotadores.DataSource =
                _detalleActual.Anotadores;

            ConfigurarColumnasAnotadores();

            cmbSeleccionAnotador.DataSource =
                new[]
                {
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

            txtJugador.Enabled =
                partidoFinalizado;

            nudMinuto.Enabled =
                partidoFinalizado;

            btnAgregarAnotador.Enabled =
                partidoFinalizado;

            btnEliminarAnotador.Enabled =
                partidoFinalizado;
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
            if (dgvAnotadores.Columns["AnotadorId"] != null)
            {
                dgvAnotadores.Columns["AnotadorId"]
                    .Visible = false;
            }

            if (dgvAnotadores.Columns["Jugador"] != null)
            {
                dgvAnotadores.Columns["Jugador"]
                    .HeaderText = "Jugador";
            }

            if (dgvAnotadores.Columns["Seleccion"] != null)
            {
                dgvAnotadores.Columns["Seleccion"]
                    .HeaderText = "Selección";
            }

            if (dgvAnotadores.Columns["Minuto"] != null)
            {
                dgvAnotadores.Columns["Minuto"]
                    .HeaderText = "Minuto";
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

        private void btnAgregarAnotador_Click(
            object sender,
            EventArgs e)
        {
            try
            {
                if (_detalleActual == null)
                {
                    throw new InvalidOperationException(
                        "Primero debe cargar un partido."
                    );
                }

                if (cmbSeleccionAnotador.SelectedItem
                    is not SeleccionOpcionItem seleccion)
                {
                    throw new InvalidOperationException(
                        "Debe seleccionar una selección."
                    );
                }

                _detalleController.AgregarAnotador(
                    _detalleActual.PartidoId,
                    seleccion.SeleccionId,
                    txtJugador.Text,
                    (int)nudMinuto.Value
                );

                MessageBox.Show(
                    "El anotador fue registrado.",
                    "Registro correcto",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                txtJugador.Clear();
                nudMinuto.Value = 1;

                CargarDetalle();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "No se pudo agregar",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void btnEliminarAnotador_Click(
            object sender,
            EventArgs e)
        {
            try
            {
                if (_detalleActual == null)
                {
                    throw new InvalidOperationException(
                        "Primero debe cargar un partido."
                    );
                }

                if (dgvAnotadores.CurrentRow?.DataBoundItem
                    is not AnotadorVistaItem anotador)
                {
                    throw new InvalidOperationException(
                        "Debe seleccionar un anotador."
                    );
                }

                _detalleController.EliminarAnotador(
                    _detalleActual.PartidoId,
                    anotador.AnotadorId
                );

                MessageBox.Show(
                    "El anotador fue eliminado.",
                    "Registro eliminado",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                CargarDetalle();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "No se pudo eliminar",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
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
