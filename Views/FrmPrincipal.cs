using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Quiniegol.Views;

namespace Quiniegol.Views
{
    public partial class FrmPrincipal : Form
    {
        public FrmPrincipal()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void btnUsuarios_Click(object sender, EventArgs e)
        {
            using (FrmUsuarios formulario = new FrmUsuarios())
            {
                formulario.ShowDialog();
            }
        }

        private void btnSelecciones_Click(object sender, EventArgs e)
        {
            using (FrmSelecciones formulario = new FrmSelecciones())
            {
                formulario.ShowDialog();
            }
        }

        private void btnPartidos_Click(object sender, EventArgs e)
        {
            using (FrmPartidos formulario = new FrmPartidos())
            {
                formulario.ShowDialog();
            }
        }

        private void btnFechaSimulada_Click(object sender, EventArgs e)
        {
            using (
       FrmFechaSimulada formulario =
           new FrmFechaSimulada()
        )
            {
                formulario.ShowDialog();
            }
        }

        private void btnPronosticos_Click(object sender, EventArgs e)
        {
            using (
                FrmPronosticos formulario =
                    new FrmPronosticos()
            )
            {
                formulario.ShowDialog();
            }
        }

        private void btnRanking_Click(object sender, EventArgs e)
        {
            using FrmRanking formulario =
                new FrmRanking();

            formulario.ShowDialog();
        }
    }
}