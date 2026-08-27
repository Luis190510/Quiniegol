namespace Quiniegol.Views
{
    partial class FrmTimeline
    {
        private System.ComponentModel.IContainer components = null!;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                components?.Dispose();
            }

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            lblTitulo = new Label();
            cmbQuiniela = new ComboBox();
            btnConsultar = new Button();
            dgvActividad = new DataGridView();
            btnCerrar = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvActividad).BeginInit();
            SuspendLayout();
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTitulo.Location = new Point(28, 24);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(202, 25);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Actividad de quiniela";
            // 
            // cmbQuiniela
            // 
            cmbQuiniela.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbQuiniela.FormattingEnabled = true;
            cmbQuiniela.Location = new Point(28, 67);
            cmbQuiniela.Name = "cmbQuiniela";
            cmbQuiniela.Size = new Size(360, 23);
            cmbQuiniela.TabIndex = 1;
            // 
            // btnConsultar
            // 
            btnConsultar.Location = new Point(404, 66);
            btnConsultar.Name = "btnConsultar";
            btnConsultar.Size = new Size(108, 25);
            btnConsultar.TabIndex = 2;
            btnConsultar.Text = "Consultar";
            btnConsultar.UseVisualStyleBackColor = true;
            btnConsultar.Click += btnConsultar_Click;
            // 
            // dgvActividad
            // 
            dgvActividad.AllowUserToAddRows = false;
            dgvActividad.AllowUserToDeleteRows = false;
            dgvActividad.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            dgvActividad.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvActividad.Location = new Point(28, 112);
            dgvActividad.Name = "dgvActividad";
            dgvActividad.ReadOnly = true;
            dgvActividad.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvActividad.Size = new Size(704, 310);
            dgvActividad.TabIndex = 3;
            // 
            // btnCerrar
            // 
            btnCerrar.Location = new Point(624, 439);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(108, 29);
            btnCerrar.TabIndex = 4;
            btnCerrar.Text = "Cerrar";
            btnCerrar.UseVisualStyleBackColor = true;
            btnCerrar.Click += btnCerrar_Click;
            // 
            // FrmTimeline
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(760, 490);
            Controls.Add(btnCerrar);
            Controls.Add(dgvActividad);
            Controls.Add(btnConsultar);
            Controls.Add(cmbQuiniela);
            Controls.Add(lblTitulo);
            Name = "FrmTimeline";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Actividad privada";
            ((System.ComponentModel.ISupportInitialize)dgvActividad).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private Label lblTitulo = null!;
        private ComboBox cmbQuiniela = null!;
        private Button btnConsultar = null!;
        private DataGridView dgvActividad = null!;
        private Button btnCerrar = null!;
    }
}
