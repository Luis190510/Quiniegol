using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System;
using System.Windows.Forms;
using Quiniegol.Controllers;
using Quiniegol.Services;

namespace Quiniegol.Views
{
    public partial class FrmRanking : Form
    {
        private readonly PuntajeController
            _puntajeController;

        private readonly InsigniaService
            _insigniaService;

        public FrmRanking()
        {
            InitializeComponent();

            _puntajeController =
                new PuntajeController();

            _insigniaService =
                new InsigniaService();

            CargarRanking();
        }

        private void CargarRanking()
        {
            try
            {
                _insigniaService
                    .RecalcularInsignias();

                dgvRanking.DataSource = null;

                dgvRanking.DataSource =
                    _puntajeController
                        .ObtenerRanking();

                ConfigurarColumnas();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Error al cargar el ranking",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void ConfigurarColumnas()
        {
            CambiarTitulo("Posicion", "Posición");
            CambiarTitulo("UsuarioId", "ID");
            CambiarTitulo("Nombre", "Usuario");
            CambiarTitulo("PaisPreferido", "País preferido");
            CambiarTitulo("Puntos", "Puntos");
            CambiarTitulo("Insignias", "Insignias");
        }

        private void CambiarTitulo(string nombre, string titulo)
        {
            if (dgvRanking.Columns[nombre] is DataGridViewColumn columna)
            {
                columna.HeaderText = titulo;
            }
        }

        private void btnActualizarRanking_Click(
            object sender,
            EventArgs e)
        {
            CargarRanking();

            MessageBox.Show(
                "Los puntajes fueron actualizados correctamente.",
                "Ranking actualizado",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        private void btnCerrar_Click(
            object sender,
            EventArgs e)
        {
            Close();
        }
    }
}
