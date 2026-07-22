using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

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
    }
    }