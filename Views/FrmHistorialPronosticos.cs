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
            var usuarios =
                _usuarioController
                    .ObtenerUsuarios()
                    .OrderBy(usuario =>
                        usuario.Nombre
                    )
                    .ToList();

            cmbUsuario.DataSource = null;
            cmbUsuario.DataSource = usuarios;
            cmbUsuario.DisplayMember = "Nombre";
            cmbUsuario.ValueMember = "Id";
            cmbUsuario.DropDownStyle =
                ComboBoxStyle.DropDownList;
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
            if (dgvHistorial.Columns["PronosticoId"] != null)
            {
                dgvHistorial.Columns["PronosticoId"]
                    .Visible = false;
            }

            if (dgvHistorial.Columns["FechaRegistro"] != null)
            {
                dgvHistorial.Columns["FechaRegistro"]
                    .HeaderText = "Fecha del pronóstico";
            }

            if (dgvHistorial.Columns["Partido"] != null)
            {
                dgvHistorial.Columns["Partido"]
                    .HeaderText = "Partido";
            }

            if (dgvHistorial.Columns["MarcadorPronosticado"] != null)
            {
                dgvHistorial.Columns["MarcadorPronosticado"]
                    .HeaderText = "Pronóstico";
            }

            if (dgvHistorial.Columns["ResultadoReal"] != null)
            {
                dgvHistorial.Columns["ResultadoReal"]
                    .HeaderText = "Resultado real";
            }

            if (dgvHistorial.Columns["Estado"] != null)
            {
                dgvHistorial.Columns["Estado"]
                    .HeaderText = "Estado";
            }

            if (dgvHistorial.Columns["Puntos"] != null)
            {
                dgvHistorial.Columns["Puntos"]
                    .HeaderText = "Puntos";
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