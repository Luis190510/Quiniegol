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
            lblDescripcion = new Label();
            lblDesde = new Label();
            dtpDesde = new DateTimePicker();
            lblHasta = new Label();
            dtpHasta = new DateTimePicker();
            btnCalcular = new Button();
            dgvEstadisticas = new DataGridView();
            btnDescargarCsv = new Button();
            btnDescargarTxt = new Button();
            btnDescargarPdf = new Button();
            btnCerrar = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvEstadisticas).BeginInit();
            SuspendLayout();
            //
            // lblTitulo
            //
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblTitulo.Location = new Point(25, 18);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(191, 21);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Reportes por rol";
            //
            // lblDescripcion
            //
            lblDescripcion.AutoSize = true;
            lblDescripcion.ForeColor = Color.DimGray;
            lblDescripcion.Location = new Point(27, 49);
            lblDescripcion.Name = "lblDescripcion";
            lblDescripcion.Size = new Size(272, 15);
            lblDescripcion.TabIndex = 1;
            lblDescripcion.Text = "Contenido disponible para la sesión autenticada.";
            //
            // lblDesde
            //
            lblDesde.AutoSize = true;
            lblDesde.Location = new Point(25, 91);
            lblDesde.Name = "lblDesde";
            lblDesde.Size = new Size(43, 15);
            lblDesde.TabIndex = 1;
            lblDesde.Text = "Desde:";
            //
            // dtpDesde
            //
            dtpDesde.CustomFormat = "dd/MM/yyyy";
            dtpDesde.Format = DateTimePickerFormat.Custom;
            dtpDesde.Location = new Point(74, 87);
            dtpDesde.Name = "dtpDesde";
            dtpDesde.Size = new Size(137, 23);
            dtpDesde.TabIndex = 2;
            dtpDesde.ValueChanged += dtpFecha_ValueChanged;
            //
            // lblHasta
            //
            lblHasta.AutoSize = true;
            lblHasta.Location = new Point(238, 91);
            lblHasta.Name = "lblHasta";
            lblHasta.Size = new Size(40, 15);
            lblHasta.TabIndex = 3;
            lblHasta.Text = "Hasta:";
            //
            // dtpHasta
            //
            dtpHasta.CustomFormat = "dd/MM/yyyy";
            dtpHasta.Format = DateTimePickerFormat.Custom;
            dtpHasta.Location = new Point(284, 87);
            dtpHasta.Name = "dtpHasta";
            dtpHasta.Size = new Size(137, 23);
            dtpHasta.TabIndex = 4;
            dtpHasta.ValueChanged += dtpFecha_ValueChanged;
            //
            // btnCalcular
            //
            btnCalcular.Location = new Point(725, 84);
            btnCalcular.Name = "btnCalcular";
            btnCalcular.Size = new Size(190, 29);
            btnCalcular.TabIndex = 5;
            btnCalcular.Text = "Generar reporte";
            btnCalcular.UseVisualStyleBackColor = true;
            btnCalcular.Click += btnCalcular_Click;
            //
            // dgvEstadisticas
            //
            dgvEstadisticas.AllowUserToAddRows = false;
            dgvEstadisticas.AllowUserToDeleteRows = false;
            dgvEstadisticas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvEstadisticas.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvEstadisticas.Location = new Point(25, 130);
            dgvEstadisticas.MultiSelect = false;
            dgvEstadisticas.Name = "dgvEstadisticas";
            dgvEstadisticas.ReadOnly = true;
            dgvEstadisticas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvEstadisticas.Size = new Size(890, 370);
            dgvEstadisticas.TabIndex = 6;
            //
            // btnDescargarCsv
            //
            btnDescargarCsv.Enabled = false;
            btnDescargarCsv.Location = new Point(25, 515);
            btnDescargarCsv.Name = "btnDescargarCsv";
            btnDescargarCsv.Size = new Size(150, 29);
            btnDescargarCsv.TabIndex = 7;
            btnDescargarCsv.Text = "Descargar CSV";
            btnDescargarCsv.UseVisualStyleBackColor = true;
            btnDescargarCsv.Click += btnDescargarCsv_Click;
            //
            // btnDescargarTxt
            //
            btnDescargarTxt.Enabled = false;
            btnDescargarTxt.Location = new Point(187, 515);
            btnDescargarTxt.Name = "btnDescargarTxt";
            btnDescargarTxt.Size = new Size(150, 29);
            btnDescargarTxt.TabIndex = 8;
            btnDescargarTxt.Text = "Descargar TXT";
            btnDescargarTxt.UseVisualStyleBackColor = true;
            btnDescargarTxt.Click += btnDescargarTxt_Click;
            //
            // btnDescargarPdf
            //
            btnDescargarPdf.Enabled = false;
            btnDescargarPdf.Location = new Point(349, 515);
            btnDescargarPdf.Name = "btnDescargarPdf";
            btnDescargarPdf.Size = new Size(150, 29);
            btnDescargarPdf.TabIndex = 9;
            btnDescargarPdf.Text = "Descargar PDF";
            btnDescargarPdf.UseVisualStyleBackColor = true;
            btnDescargarPdf.Click += btnDescargarPdf_Click;
            //
            // btnCerrar
            //
            btnCerrar.Location = new Point(772, 515);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(143, 29);
            btnCerrar.TabIndex = 10;
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
            ClientSize = new Size(940, 560);
            Controls.Add(btnCerrar);
            Controls.Add(btnDescargarPdf);
            Controls.Add(btnDescargarTxt);
            Controls.Add(btnDescargarCsv);
            Controls.Add(dgvEstadisticas);
            Controls.Add(btnCalcular);
            Controls.Add(dtpHasta);
            Controls.Add(lblHasta);
            Controls.Add(dtpDesde);
            Controls.Add(lblDesde);
            Controls.Add(lblTitulo);
            Controls.Add(lblDescripcion);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmEstadisticas";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Reportes por rol";
            ((System.ComponentModel.ISupportInitialize)dgvEstadisticas).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitulo;
        private Label lblDescripcion;
        private Label lblDesde;
        private DateTimePicker dtpDesde;
        private Label lblHasta;
        private DateTimePicker dtpHasta;
        private Button btnCalcular;
        private DataGridView dgvEstadisticas;
        private Button btnDescargarCsv;
        private Button btnDescargarTxt;
        private Button btnDescargarPdf;
        private Button btnCerrar;
    }
}
