namespace Quiniegol.Views
{
    partial class FrmPartidos
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
            lblLocal = new Label();
            lblVisitante = new Label();
            lblFechaHora = new Label();
            cmbLocal = new ComboBox();
            cmbVisitante = new ComboBox();
            dtpFechaHora = new DateTimePicker();
            btnRegistrarPartido = new Button();
            dgvPartidos = new DataGridView();
            lblGolesLocal = new Label();
            lblGolesVisitante = new Label();
            nudGolesLocal = new NumericUpDown();
            nudGolesVisitante = new NumericUpDown();
            btnGuardarResultado = new Button();
            btnCerrar = new Button();
            btnEliminarPartido = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvPartidos).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudGolesLocal).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudGolesVisitante).BeginInit();
            SuspendLayout();
            // 
            // lblLocal
            // 
            lblLocal.AutoSize = true;
            lblLocal.Location = new Point(151, 72);
            lblLocal.Name = "lblLocal";
            lblLocal.Size = new Size(88, 15);
            lblLocal.TabIndex = 0;
            lblLocal.Text = "Selección local:";
            // 
            // lblVisitante
            // 
            lblVisitante.AutoSize = true;
            lblVisitante.Location = new Point(383, 72);
            lblVisitante.Name = "lblVisitante";
            lblVisitante.Size = new Size(107, 15);
            lblVisitante.TabIndex = 1;
            lblVisitante.Text = "Selección visitante:";
            // 
            // lblFechaHora
            // 
            lblFechaHora.AutoSize = true;
            lblFechaHora.Location = new Point(616, 72);
            lblFechaHora.Name = "lblFechaHora";
            lblFechaHora.Size = new Size(77, 15);
            lblFechaHora.TabIndex = 2;
            lblFechaHora.Text = "Fecha y hora:";
            // 
            // cmbLocal
            // 
            cmbLocal.FormattingEnabled = true;
            cmbLocal.Location = new Point(151, 102);
            cmbLocal.Name = "cmbLocal";
            cmbLocal.Size = new Size(121, 23);
            cmbLocal.TabIndex = 3;
            // 
            // cmbVisitante
            // 
            cmbVisitante.FormattingEnabled = true;
            cmbVisitante.Location = new Point(383, 102);
            cmbVisitante.Name = "cmbVisitante";
            cmbVisitante.Size = new Size(121, 23);
            cmbVisitante.TabIndex = 4;
            // 
            // dtpFechaHora
            // 
            dtpFechaHora.CausesValidation = false;
            dtpFechaHora.CustomFormat = "dd/MM/yyyy HH:mm";
            dtpFechaHora.Format = DateTimePickerFormat.Custom;
            dtpFechaHora.Location = new Point(616, 102);
            dtpFechaHora.Name = "dtpFechaHora";
            dtpFechaHora.ShowUpDown = true;
            dtpFechaHora.Size = new Size(200, 23);
            dtpFechaHora.TabIndex = 5;
            // 
            // btnRegistrarPartido
            // 
            btnRegistrarPartido.Location = new Point(616, 152);
            btnRegistrarPartido.Name = "btnRegistrarPartido";
            btnRegistrarPartido.Size = new Size(200, 23);
            btnRegistrarPartido.TabIndex = 6;
            btnRegistrarPartido.Text = "Registrar partido";
            btnRegistrarPartido.UseVisualStyleBackColor = true;
            btnRegistrarPartido.Click += btnRegistrarPartido_Click;
            // 
            // dgvPartidos
            // 
            dgvPartidos.AllowUserToAddRows = false;
            dgvPartidos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPartidos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvPartidos.Location = new Point(151, 188);
            dgvPartidos.MultiSelect = false;
            dgvPartidos.Name = "dgvPartidos";
            dgvPartidos.ReadOnly = true;
            dgvPartidos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPartidos.Size = new Size(665, 196);
            dgvPartidos.TabIndex = 7;
            // 
            // lblGolesLocal
            // 
            lblGolesLocal.AutoSize = true;
            lblGolesLocal.Location = new Point(151, 421);
            lblGolesLocal.Name = "lblGolesLocal";
            lblGolesLocal.Size = new Size(67, 15);
            lblGolesLocal.TabIndex = 8;
            lblGolesLocal.Text = "Goles local:";
            // 
            // lblGolesVisitante
            // 
            lblGolesVisitante.AutoSize = true;
            lblGolesVisitante.Location = new Point(383, 421);
            lblGolesVisitante.Name = "lblGolesVisitante";
            lblGolesVisitante.Size = new Size(86, 15);
            lblGolesVisitante.TabIndex = 9;
            lblGolesVisitante.Text = "Goles visitante:";
            // 
            // nudGolesLocal
            // 
            nudGolesLocal.Location = new Point(151, 448);
            nudGolesLocal.Maximum = new decimal(new int[] { 20, 0, 0, 0 });
            nudGolesLocal.Name = "nudGolesLocal";
            nudGolesLocal.Size = new Size(120, 23);
            nudGolesLocal.TabIndex = 10;
            // 
            // nudGolesVisitante
            // 
            nudGolesVisitante.Location = new Point(383, 448);
            nudGolesVisitante.Maximum = new decimal(new int[] { 20, 0, 0, 0 });
            nudGolesVisitante.Name = "nudGolesVisitante";
            nudGolesVisitante.Size = new Size(120, 23);
            nudGolesVisitante.TabIndex = 11;
            // 
            // btnGuardarResultado
            // 
            btnGuardarResultado.Location = new Point(616, 448);
            btnGuardarResultado.Name = "btnGuardarResultado";
            btnGuardarResultado.Size = new Size(200, 23);
            btnGuardarResultado.TabIndex = 12;
            btnGuardarResultado.Text = "Guardar resultado";
            btnGuardarResultado.UseVisualStyleBackColor = true;
            btnGuardarResultado.Click += btnGuardarResultado_Click;
            // 
            // btnCerrar
            // 
            btnCerrar.Location = new Point(741, 511);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(75, 23);
            btnCerrar.TabIndex = 13;
            btnCerrar.Text = "Cerrar";
            btnCerrar.UseVisualStyleBackColor = true;
            btnCerrar.Click += btnCerrar_Click;
            // 
            // btnEliminarPartido
            // 
            btnEliminarPartido.Location = new Point(372, 511);
            btnEliminarPartido.Name = "btnEliminarPartido";
            btnEliminarPartido.Size = new Size(161, 23);
            btnEliminarPartido.TabIndex = 14;
            btnEliminarPartido.Text = "Eliminar partido";
            btnEliminarPartido.UseVisualStyleBackColor = true;
            btnEliminarPartido.Click += btnEliminarPartido_Click;
            // 
            // FrmPartidos
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(884, 561);
            Controls.Add(btnEliminarPartido);
            Controls.Add(btnCerrar);
            Controls.Add(btnGuardarResultado);
            Controls.Add(nudGolesVisitante);
            Controls.Add(nudGolesLocal);
            Controls.Add(lblGolesVisitante);
            Controls.Add(lblGolesLocal);
            Controls.Add(dgvPartidos);
            Controls.Add(btnRegistrarPartido);
            Controls.Add(dtpFechaHora);
            Controls.Add(cmbVisitante);
            Controls.Add(cmbLocal);
            Controls.Add(lblFechaHora);
            Controls.Add(lblVisitante);
            Controls.Add(lblLocal);
            Name = "FrmPartidos";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Gestión de partidos";
            ((System.ComponentModel.ISupportInitialize)dgvPartidos).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudGolesLocal).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudGolesVisitante).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblLocal;
        private Label lblVisitante;
        private Label lblFechaHora;
        private ComboBox cmbLocal;
        private ComboBox cmbVisitante;
        private DateTimePicker dtpFechaHora;
        private Button btnRegistrarPartido;
        private DataGridView dgvPartidos;
        private Label lblGolesLocal;
        private Label lblGolesVisitante;
        private NumericUpDown nudGolesLocal;
        private NumericUpDown nudGolesVisitante;
        private Button btnGuardarResultado;
        private Button btnCerrar;
        private Button btnEliminarPartido;
    }
}
