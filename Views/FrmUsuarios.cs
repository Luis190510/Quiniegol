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
    public partial class FrmUsuarios : Form
    {
        private readonly UsuarioController _usuarioController;

        public FrmUsuarios()
        {
            InitializeComponent();

            _usuarioController = new UsuarioController();

            cmbPais.Items.AddRange(new object[]
            {
                "Argentina",
                "Australia",
                "Austria",
                "Bélgica",
                "Bosnia y Herzegovina",
                "Brasil",
                "Cabo Verde",
                "Canadá",
                "Colombia",
                "Congo DR",
                "Corea del Sur",
                "Costa de Marfil",
                "Croacia",
                "Curazao",
                "Ecuador",
                "Egipto",
                "Escocia",
                "Eslovaquia",
                "España",
                "Estados Unidos",
                "Francia",
                "Gales",
                "Ghana",
                "Haití",
                "Inglaterra",
                "Irán",
                "Irak",
                "Japón",
                "Jordania",
                "Marruecos",
                "México",
                "Noruega",
                "Nueva Zelanda",
                "Países Bajos",
                "Panamá",
                "Paraguay",
                "Portugal",
                "Qatar",
                "República Checa",
                "Senegal",
                "Sudáfrica",
                "Suecia",
                "Suiza",
                "Túnez",
                "Turquía",
                "Uruguay",
                "Uzbekistán"
            });

            CargarUsuarios();
        }

        private void CargarUsuarios()
        {
            dgvUsuarios.DataSource = null;
            dgvUsuarios.DataSource =
                _usuarioController.ObtenerUsuarios();
        }
    }
}