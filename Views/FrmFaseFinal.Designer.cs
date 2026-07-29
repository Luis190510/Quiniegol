namespace Quiniegol.Views
{
    partial class FrmFaseFinal
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
            lblAviso = new Label();
            btnCalcular = new Button();
            dgvClasificados = new DataGridView();
            dgvCruces = new DataGridView();
            btnCerrar = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvClasificados).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvCruces).BeginInit();
            SuspendLayout();
            //
            // lblAviso
            //
            lblAviso.AutoSize = true;
            lblAviso.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblAviso.Location = new Point(22, 25);
            lblAviso.Name = "lblAviso";
            lblAviso.Size = new Size(386, 19);
            lblAviso.TabIndex = 0;
            lblAviso.Text = "Cruces calculados con la regla definida para Quiniegol";
            //
            // btnCalcular
            //
            btnCalcular.Location = new Point(866, 19);
            btnCalcular.Name = "btnCalcular";
            btnCalcular.Size = new Size(212, 31);
            btnCalcular.TabIndex = 1;
            btnCalcular.Text = "Calcular fase final";
            btnCalcular.UseVisualStyleBackColor = true;
            btnCalcular.Click += btnCalcular_Click;
            //
            // dgvClasificados
            //
            dgvClasificados.AllowUserToAddRows = false;
            dgvClasificados.AllowUserToDeleteRows = false;
            dgvClasificados.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvClasificados.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvClasificados.Location = new Point(22, 68);
            dgvClasificados.MultiSelect = false;
            dgvClasificados.Name = "dgvClasificados";
            dgvClasificados.ReadOnly = true;
            dgvClasificados.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvClasificados.Size = new Size(1056, 266);
            dgvClasificados.TabIndex = 2;
            //
            // dgvCruces
            //
            dgvCruces.AllowUserToAddRows = false;
            dgvCruces.AllowUserToDeleteRows = false;
            dgvCruces.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvCruces.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvCruces.Location = new Point(22, 354);
            dgvCruces.MultiSelect = false;
            dgvCruces.Name = "dgvCruces";
            dgvCruces.ReadOnly = true;
            dgvCruces.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCruces.Size = new Size(1056, 244);
            dgvCruces.TabIndex = 3;
            //
            // btnCerrar
            //
            btnCerrar.Location = new Point(866, 617);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(212, 31);
            btnCerrar.TabIndex = 4;
            btnCerrar.Text = "Cerrar";
            btnCerrar.UseVisualStyleBackColor = true;
            btnCerrar.Click += btnCerrar_Click;
            //
            // FrmFaseFinal
            //
            AcceptButton = btnCalcular;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCerrar;
            ClientSize = new Size(1100, 667);
            Controls.Add(btnCerrar);
            Controls.Add(dgvCruces);
            Controls.Add(dgvClasificados);
            Controls.Add(btnCalcular);
            Controls.Add(lblAviso);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmFaseFinal";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Cruces de fase final";
            ((System.ComponentModel.ISupportInitialize)dgvClasificados).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvCruces).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblAviso;
        private Button btnCalcular;
        private DataGridView dgvClasificados;
        private DataGridView dgvCruces;
        private Button btnCerrar;
    }
}
