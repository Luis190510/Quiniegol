namespace Quiniegol.Views
{
    partial class FrmHistorialPronosticos
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
            lblUsuario = new Label();
            cmbUsuario = new ComboBox();
            btnConsultar = new Button();
            dgvHistorial = new DataGridView();
            btnCerrar = new Button();
            lblHistorial = new Label();
            ((System.ComponentModel.ISupportInitialize)dgvHistorial).BeginInit();
            SuspendLayout();
            // 
            // lblUsuario
            // 
            lblUsuario.AutoSize = true;
            lblUsuario.Location = new Point(46, 76);
            lblUsuario.Name = "lblUsuario";
            lblUsuario.Size = new Size(50, 15);
            lblUsuario.TabIndex = 0;
            lblUsuario.Text = "Usuario:";
            // 
            // cmbUsuario
            // 
            cmbUsuario.FormattingEnabled = true;
            cmbUsuario.Location = new Point(155, 73);
            cmbUsuario.Name = "cmbUsuario";
            cmbUsuario.Size = new Size(121, 23);
            cmbUsuario.TabIndex = 1;
            // 
            // btnConsultar
            // 
            btnConsultar.Location = new Point(334, 73);
            btnConsultar.Name = "btnConsultar";
            btnConsultar.Size = new Size(132, 23);
            btnConsultar.TabIndex = 2;
            btnConsultar.Text = "Consultar historial";
            btnConsultar.UseVisualStyleBackColor = true;
            btnConsultar.Click += btnConsultar_Click;
            // 
            // dgvHistorial
            // 
            dgvHistorial.AllowUserToAddRows = false;
            dgvHistorial.AllowUserToDeleteRows = false;
            dgvHistorial.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvHistorial.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvHistorial.Location = new Point(26, 117);
            dgvHistorial.MultiSelect = false;
            dgvHistorial.Name = "dgvHistorial";
            dgvHistorial.ReadOnly = true;
            dgvHistorial.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvHistorial.Size = new Size(731, 150);
            dgvHistorial.TabIndex = 3;
            // 
            // btnCerrar
            // 
            btnCerrar.Location = new Point(46, 308);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(75, 23);
            btnCerrar.TabIndex = 4;
            btnCerrar.Text = "Cerrar";
            btnCerrar.UseVisualStyleBackColor = true;
            btnCerrar.Click += btnCerrar_Click;
            // 
            // lblHistorial
            // 
            lblHistorial.AutoSize = true;
            lblHistorial.Location = new Point(26, 9);
            lblHistorial.Name = "lblHistorial";
            lblHistorial.Size = new Size(132, 15);
            lblHistorial.TabIndex = 5;
            lblHistorial.Text = "Historial de pronosticos";
            // 
            // FrmHistorialPronosticos
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(lblHistorial);
            Controls.Add(btnCerrar);
            Controls.Add(dgvHistorial);
            Controls.Add(btnConsultar);
            Controls.Add(cmbUsuario);
            Controls.Add(lblUsuario);
            Name = "FrmHistorialPronosticos";
            Text = "FrmHistorialPronosticos";
            ((System.ComponentModel.ISupportInitialize)dgvHistorial).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblUsuario;
        private ComboBox cmbUsuario;
        private Button btnConsultar;
        private DataGridView dgvHistorial;
        private Button btnCerrar;
        private Label lblHistorial;
    }
}