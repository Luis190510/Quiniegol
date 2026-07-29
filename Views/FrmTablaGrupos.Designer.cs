namespace Quiniegol.Views
{
    partial class FrmTablaGrupos
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
            lblGrupo = new Label();
            cmbGrupo = new ComboBox();
            btnCalcular = new Button();
            dgvTabla = new DataGridView();
            btnCerrar = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvTabla).BeginInit();
            SuspendLayout();
            //
            // lblGrupo
            //
            lblGrupo.AutoSize = true;
            lblGrupo.Location = new Point(22, 26);
            lblGrupo.Name = "lblGrupo";
            lblGrupo.Size = new Size(43, 15);
            lblGrupo.TabIndex = 0;
            lblGrupo.Text = "Grupo:";
            //
            // cmbGrupo
            //
            cmbGrupo.FormattingEnabled = true;
            cmbGrupo.Location = new Point(71, 22);
            cmbGrupo.Name = "cmbGrupo";
            cmbGrupo.Size = new Size(184, 23);
            cmbGrupo.TabIndex = 1;
            //
            // btnCalcular
            //
            btnCalcular.Location = new Point(274, 19);
            btnCalcular.Name = "btnCalcular";
            btnCalcular.Size = new Size(170, 29);
            btnCalcular.TabIndex = 2;
            btnCalcular.Text = "Calcular tabla";
            btnCalcular.UseVisualStyleBackColor = true;
            btnCalcular.Click += btnCalcular_Click;
            //
            // dgvTabla
            //
            dgvTabla.AllowUserToAddRows = false;
            dgvTabla.AllowUserToDeleteRows = false;
            dgvTabla.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvTabla.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvTabla.Location = new Point(22, 67);
            dgvTabla.MultiSelect = false;
            dgvTabla.Name = "dgvTabla";
            dgvTabla.ReadOnly = true;
            dgvTabla.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTabla.Size = new Size(956, 366);
            dgvTabla.TabIndex = 3;
            //
            // btnCerrar
            //
            btnCerrar.Location = new Point(808, 450);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(170, 30);
            btnCerrar.TabIndex = 4;
            btnCerrar.Text = "Cerrar";
            btnCerrar.UseVisualStyleBackColor = true;
            btnCerrar.Click += btnCerrar_Click;
            //
            // FrmTablaGrupos
            //
            AcceptButton = btnCalcular;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCerrar;
            ClientSize = new Size(1000, 499);
            Controls.Add(btnCerrar);
            Controls.Add(dgvTabla);
            Controls.Add(btnCalcular);
            Controls.Add(cmbGrupo);
            Controls.Add(lblGrupo);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmTablaGrupos";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Tabla de grupos";
            ((System.ComponentModel.ISupportInitialize)dgvTabla).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblGrupo;
        private ComboBox cmbGrupo;
        private Button btnCalcular;
        private DataGridView dgvTabla;
        private Button btnCerrar;
    }
}
