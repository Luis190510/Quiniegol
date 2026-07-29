namespace Quiniegol.Views
{
    partial class FrmEstadisticas
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
            lblDesde = new Label();
            dtpDesde = new DateTimePicker();
            lblHasta = new Label();
            dtpHasta = new DateTimePicker();
            btnCalcular = new Button();
            dgvEstadisticas = new DataGridView();
            btnCerrar = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvEstadisticas).BeginInit();
            SuspendLayout();
            //
            // lblTitulo
            //
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblTitulo.Location = new Point(314, 18);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(191, 21);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Estadísticas por fechas";
            //
            // lblDesde
            //
            lblDesde.AutoSize = true;
            lblDesde.Location = new Point(25, 69);
            lblDesde.Name = "lblDesde";
            lblDesde.Size = new Size(43, 15);
            lblDesde.TabIndex = 1;
            lblDesde.Text = "Desde:";
            //
            // dtpDesde
            //
            dtpDesde.Format = DateTimePickerFormat.Short;
            dtpDesde.Location = new Point(74, 65);
            dtpDesde.Name = "dtpDesde";
            dtpDesde.Size = new Size(137, 23);
            dtpDesde.TabIndex = 2;
            //
            // lblHasta
            //
            lblHasta.AutoSize = true;
            lblHasta.Location = new Point(238, 69);
            lblHasta.Name = "lblHasta";
            lblHasta.Size = new Size(40, 15);
            lblHasta.TabIndex = 3;
            lblHasta.Text = "Hasta:";
            //
            // dtpHasta
            //
            dtpHasta.Format = DateTimePickerFormat.Short;
            dtpHasta.Location = new Point(284, 65);
            dtpHasta.Name = "dtpHasta";
            dtpHasta.Size = new Size(137, 23);
            dtpHasta.TabIndex = 4;
            //
            // btnCalcular
            //
            btnCalcular.Location = new Point(584, 62);
            btnCalcular.Name = "btnCalcular";
            btnCalcular.Size = new Size(190, 29);
            btnCalcular.TabIndex = 5;
            btnCalcular.Text = "Calcular estadísticas";
            btnCalcular.UseVisualStyleBackColor = true;
            btnCalcular.Click += btnCalcular_Click;
            //
            // dgvEstadisticas
            //
            dgvEstadisticas.AllowUserToAddRows = false;
            dgvEstadisticas.AllowUserToDeleteRows = false;
            dgvEstadisticas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvEstadisticas.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvEstadisticas.Location = new Point(25, 111);
            dgvEstadisticas.MultiSelect = false;
            dgvEstadisticas.Name = "dgvEstadisticas";
            dgvEstadisticas.ReadOnly = true;
            dgvEstadisticas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvEstadisticas.Size = new Size(749, 302);
            dgvEstadisticas.TabIndex = 6;
            //
            // btnCerrar
            //
            btnCerrar.Location = new Point(631, 430);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(143, 29);
            btnCerrar.TabIndex = 7;
            btnCerrar.Text = "Cerrar";
            btnCerrar.UseVisualStyleBackColor = true;
            btnCerrar.Click += btnCerrar_Click;
            //
            // FrmEstadisticas
            //
            AcceptButton = btnCalcular;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCerrar;
            ClientSize = new Size(800, 477);
            Controls.Add(btnCerrar);
            Controls.Add(dgvEstadisticas);
            Controls.Add(btnCalcular);
            Controls.Add(dtpHasta);
            Controls.Add(lblHasta);
            Controls.Add(dtpDesde);
            Controls.Add(lblDesde);
            Controls.Add(lblTitulo);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmEstadisticas";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Estadísticas";
            ((System.ComponentModel.ISupportInitialize)dgvEstadisticas).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitulo;
        private Label lblDesde;
        private DateTimePicker dtpDesde;
        private Label lblHasta;
        private DateTimePicker dtpHasta;
        private Button btnCalcular;
        private DataGridView dgvEstadisticas;
        private Button btnCerrar;
    }
}
