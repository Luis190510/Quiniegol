namespace Quiniegol.Views
{
    partial class FrmSelecciones
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lblTitulo = new Label();
            dgvSelecciones = new DataGridView();
            btnCerrar = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvSelecciones).BeginInit();
            SuspendLayout();
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Location = new Point(251, 75);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(139, 15);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Selecciones participantes";
            // 
            // dgvSelecciones
            // 
            dgvSelecciones.AllowUserToAddRows = false;
            dgvSelecciones.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvSelecciones.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvSelecciones.Location = new Point(440, 48);
            dgvSelecciones.Name = "dgvSelecciones";
            dgvSelecciones.ReadOnly = true;
            dgvSelecciones.Size = new Size(240, 150);
            dgvSelecciones.TabIndex = 1;
            // 
            // btnCerrar
            // 
            btnCerrar.Location = new Point(273, 122);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(75, 23);
            btnCerrar.TabIndex = 2;
            btnCerrar.Text = "Cerrar";
            btnCerrar.UseVisualStyleBackColor = true;
            btnCerrar.Click += btnCerrar_Click_1;
            // 
            // FrmSelecciones
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnCerrar);
            Controls.Add(dgvSelecciones);
            Controls.Add(lblTitulo);
            Name = "FrmSelecciones";
            Text = "FrmSelecciones";
            ((System.ComponentModel.ISupportInitialize)dgvSelecciones).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitulo;
        private DataGridView dgvSelecciones;
        private Button btnCerrar;
    }
}