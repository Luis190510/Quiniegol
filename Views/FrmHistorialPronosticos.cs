using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System;
using System.Linq;
using System.Windows.Forms;
using Quiniegol.Controllers;
using Quiniegol.Models;

using Quiniegol.Services;

namespace Quiniegol.Views
{
    public partial class FrmHistorialPronosticos : Form
    {
        private readonly UsuarioController
            _usuarioController;

        private readonly HistorialPronosticoController
            _historialController;

        public FrmHistorialPronosticos()
        {
            InitializeComponent();

            _usuarioController =
                new UsuarioController();

            _historialController =
                new HistorialPronosticoController();

            CargarUsuarios();
        }

        private void CargarUsuarios()
        {
            var usuarios = SesionUsuarioService.EsAdministrador
                ? _usuarioController
                    .ObtenerUsuarios()
                    .OrderBy(usuario => usuario.Nombre)
                    .ToList()
                : new List<Usuario>
                {
                    SesionUsuarioService.UsuarioActual
                };

            cmbUsuario.DataSource = null;
            cmbUsuario.DataSource = usuarios;
            cmbUsuario.DisplayMember = "Nombre";
            cmbUsuario.ValueMember = "Id";
            cmbUsuario.DropDownStyle =
                ComboBoxStyle.DropDownList;

            cmbUsuario.Enabled =
                SesionUsuarioService.EsAdministrador;
        }

        private void btnConsultar_Click(
            object sender,
            EventArgs e)
        {
            try
            {
                if (cmbUsuario.SelectedItem
                    is not Usuario usuario)
                {
                    MessageBox.Show(
                        "Debe seleccionar un usuario.",
                        "Dato requerido",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );

                    return;
                }

                var historial =
                    _historialController
                        .ObtenerPorUsuario(
                            usuario.Id
                        );

                dgvHistorial.DataSource = null;
                dgvHistorial.DataSource = historial;

                ConfigurarColumnas();

                if (historial.Count == 0)
                {
                    MessageBox.Show(
                        "El usuario seleccionado no tiene pronósticos.",
                        "Sin pronósticos",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void ConfigurarColumnas()
        {
            if (dgvHistorial.Columns["PronosticoId"]
                is DataGridViewColumn columnaId)
            {
                columnaId.Visible = false;
            }

            CambiarTitulo("FechaRegistro", "Fecha del pronóstico");
            CambiarTitulo("Partido", "Partido");
            CambiarTitulo("MarcadorPronosticado", "Pronóstico");
            CambiarTitulo("GoleadoresPronosticados", "Goleadores elegidos");
            CambiarTitulo("ResultadoReal", "Resultado real");
            CambiarTitulo("Estado", "Estado");
            CambiarTitulo("Puntos", "Puntos");
        }

        private void CambiarTitulo(string nombre, string titulo)
        {
            if (dgvHistorial.Columns[nombre]
                is DataGridViewColumn columna)
            {
                columna.HeaderText = titulo;
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
