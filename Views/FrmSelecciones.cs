using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Quiniegol.Controllers;

namespace Quiniegol.Views
{
    public partial class FrmSelecciones : Form
    {
        private readonly SeleccionController _seleccionController;

        public FrmSelecciones()
        {
            InitializeComponent();

            _seleccionController = new SeleccionController();

            CargarSelecciones();
        }

        private void CargarSelecciones()
        {
            try
            {
                dgvSelecciones.DataSource = null;
                dgvSelecciones.DataSource =
                    _seleccionController.ObtenerSelecciones();

                if (dgvSelecciones.Columns["RutaBandera"] != null)
                {
                    dgvSelecciones.Columns["RutaBandera"].Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "No fue posible cargar las selecciones",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnCerrar_Click_1(object sender, EventArgs e)
        {

        }
    }
}