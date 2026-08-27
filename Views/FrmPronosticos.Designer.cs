namespace Quiniegol.Views
{
    partial class FrmPronosticos
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
            lblUsuario = new Label();
            lblPartido = new Label();
            lblGolesLocal = new Label();
            lblGolesVisitante = new Label();
            lblGoleadoresLocal = new Label();
            lblGoleadoresVisitante = new Label();
            lblAyudaGoleadores = new Label();
            cmbUsuario = new ComboBox();
            cmbPartido = new ComboBox();
            nudGolesLocal = new NumericUpDown();
            nudGolesVisitante = new NumericUpDown();
            txtGoleadoresLocal = new TextBox();
            txtGoleadoresVisitante = new TextBox();
            btnRegistrarPronostico = new Button();
            btnActualizar = new Button();
            btnCerrar = new Button();
            dgvPronosticos = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)nudGolesLocal).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudGolesVisitante).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvPronosticos).BeginInit();
            SuspendLayout();
            //
            // lblUsuario
            //
            lblUsuario.AutoSize = true;
            lblUsuario.Location = new Point(22, 24);
            lblUsuario.Name = "lblUsuario";
            lblUsuario.Size = new Size(50, 15);
            lblUsuario.TabIndex = 0;
            lblUsuario.Text = "Usuario:";
            //
            // lblPartido
            //
            lblPartido.AutoSize = true;
            lblPartido.Location = new Point(22, 58);
            lblPartido.Name = "lblPartido";
            lblPartido.Size = new Size(48, 15);
            lblPartido.TabIndex = 1;
            lblPartido.Text = "Partido:";
            //
            // lblGolesLocal
            //
            lblGolesLocal.AutoSize = true;
            lblGolesLocal.Location = new Point(22, 99);
            lblGolesLocal.Name = "lblGolesLocal";
            lblGolesLocal.Size = new Size(67, 15);
            lblGolesLocal.TabIndex = 2;
            lblGolesLocal.Text = "Goles local:";
            //
            // lblGolesVisitante
            //
            lblGolesVisitante.AutoSize = true;
            lblGolesVisitante.Location = new Point(334, 99);
            lblGolesVisitante.Name = "lblGolesVisitante";
            lblGolesVisitante.Size = new Size(86, 15);
            lblGolesVisitante.TabIndex = 3;
            lblGolesVisitante.Text = "Goles visitante:";
            //
            // lblGoleadoresLocal
            //
            lblGoleadoresLocal.AutoSize = true;
            lblGoleadoresLocal.Location = new Point(22, 137);
            lblGoleadoresLocal.Name = "lblGoleadoresLocal";
            lblGoleadoresLocal.Size = new Size(212, 15);
            lblGoleadoresLocal.TabIndex = 4;
            lblGoleadoresLocal.Text = "Posibles goleadores del equipo local:";
            //
            // lblGoleadoresVisitante
            //
            lblGoleadoresVisitante.AutoSize = true;
            lblGoleadoresVisitante.Location = new Point(550, 137);
            lblGoleadoresVisitante.Name = "lblGoleadoresVisitante";
            lblGoleadoresVisitante.Size = new Size(231, 15);
            lblGoleadoresVisitante.TabIndex = 5;
            lblGoleadoresVisitante.Text = "Posibles goleadores del equipo visitante:";
            //
            // lblAyudaGoleadores
            //
            lblAyudaGoleadores.AutoSize = true;
            lblAyudaGoleadores.ForeColor = Color.DimGray;
            lblAyudaGoleadores.Location = new Point(22, 260);
            lblAyudaGoleadores.Name = "lblAyudaGoleadores";
            lblAyudaGoleadores.Size = new Size(443, 15);
            lblAyudaGoleadores.TabIndex = 6;
            lblAyudaGoleadores.Text = "Opcional: escriba uno o varios nombres, separados por línea, coma o punto y coma.";
            //
            // cmbUsuario
            //
            cmbUsuario.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbUsuario.FormattingEnabled = true;
            cmbUsuario.Location = new Point(183, 21);
            cmbUsuario.Name = "cmbUsuario";
            cmbUsuario.Size = new Size(350, 23);
            cmbUsuario.TabIndex = 7;
            //
            // cmbPartido
            //
            cmbPartido.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbPartido.FormattingEnabled = true;
            cmbPartido.Location = new Point(183, 55);
            cmbPartido.Name = "cmbPartido";
            cmbPartido.Size = new Size(895, 23);
            cmbPartido.TabIndex = 8;
            cmbPartido.SelectedIndexChanged += cmbPartido_SelectedIndexChanged;
            //
            // nudGolesLocal
            //
            nudGolesLocal.Location = new Point(183, 97);
            nudGolesLocal.Maximum = new decimal(new int[] { 20, 0, 0, 0 });
            nudGolesLocal.Name = "nudGolesLocal";
            nudGolesLocal.Size = new Size(120, 23);
            nudGolesLocal.TabIndex = 9;
            //
            // nudGolesVisitante
            //
            nudGolesVisitante.Location = new Point(430, 97);
            nudGolesVisitante.Maximum = new decimal(new int[] { 20, 0, 0, 0 });
            nudGolesVisitante.Name = "nudGolesVisitante";
            nudGolesVisitante.Size = new Size(120, 23);
            nudGolesVisitante.TabIndex = 10;
            //
            // txtGoleadoresLocal
            //
            txtGoleadoresLocal.Location = new Point(22, 158);
            txtGoleadoresLocal.Multiline = true;
            txtGoleadoresLocal.Name = "txtGoleadoresLocal";
            txtGoleadoresLocal.ScrollBars = ScrollBars.Vertical;
            txtGoleadoresLocal.Size = new Size(508, 92);
            txtGoleadoresLocal.TabIndex = 11;
            //
            // txtGoleadoresVisitante
            //
            txtGoleadoresVisitante.Location = new Point(550, 158);
            txtGoleadoresVisitante.Multiline = true;
            txtGoleadoresVisitante.Name = "txtGoleadoresVisitante";
            txtGoleadoresVisitante.ScrollBars = ScrollBars.Vertical;
            txtGoleadoresVisitante.Size = new Size(528, 92);
            txtGoleadoresVisitante.TabIndex = 12;
            //
            // btnRegistrarPronostico
            //
            btnRegistrarPronostico.Location = new Point(22, 289);
            btnRegistrarPronostico.Name = "btnRegistrarPronostico";
            btnRegistrarPronostico.Size = new Size(190, 30);
            btnRegistrarPronostico.TabIndex = 13;
            btnRegistrarPronostico.Text = "Registrar pronóstico";
            btnRegistrarPronostico.UseVisualStyleBackColor = true;
            btnRegistrarPronostico.Click += btnRegistrarPronostico_Click;
            //
            // btnActualizar
            //
            btnActualizar.Location = new Point(226, 289);
            btnActualizar.Name = "btnActualizar";
            btnActualizar.Size = new Size(135, 30);
            btnActualizar.TabIndex = 14;
            btnActualizar.Text = "Actualizar";
            btnActualizar.UseVisualStyleBackColor = true;
            btnActualizar.Click += btnActualizar_Click;
            //
            // dgvPronosticos
            //
            dgvPronosticos.AllowUserToAddRows = false;
            dgvPronosticos.AllowUserToDeleteRows = false;
            dgvPronosticos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPronosticos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvPronosticos.Location = new Point(22, 334);
            dgvPronosticos.Name = "dgvPronosticos";
            dgvPronosticos.ReadOnly = true;
            dgvPronosticos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPronosticos.Size = new Size(1056, 268);
            dgvPronosticos.TabIndex = 15;
            //
            // btnCerrar
            //
            btnCerrar.Location = new Point(923, 617);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(155, 30);
            btnCerrar.TabIndex = 16;
            btnCerrar.Text = "Cerrar";
            btnCerrar.UseVisualStyleBackColor = true;
            btnCerrar.Click += btnCerrar_Click;
            //
            // FrmPronosticos
            //
            AcceptButton = btnRegistrarPronostico;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCerrar;
            ClientSize = new Size(1100, 665);
            Controls.Add(btnCerrar);
            Controls.Add(dgvPronosticos);
            Controls.Add(btnActualizar);
            Controls.Add(btnRegistrarPronostico);
            Controls.Add(txtGoleadoresVisitante);
            Controls.Add(txtGoleadoresLocal);
            Controls.Add(nudGolesVisitante);
            Controls.Add(nudGolesLocal);
            Controls.Add(cmbPartido);
            Controls.Add(cmbUsuario);
            Controls.Add(lblAyudaGoleadores);
            Controls.Add(lblGoleadoresVisitante);
            Controls.Add(lblGoleadoresLocal);
            Controls.Add(lblGolesVisitante);
            Controls.Add(lblGolesLocal);
            Controls.Add(lblPartido);
            Controls.Add(lblUsuario);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmPronosticos";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Registrar pronóstico";
            ((System.ComponentModel.ISupportInitialize)nudGolesLocal).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudGolesVisitante).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvPronosticos).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private Label lblUsuario = null!;
        private Label lblPartido = null!;
        private Label lblGolesLocal = null!;
        private Label lblGolesVisitante = null!;
        private Label lblGoleadoresLocal = null!;
        private Label lblGoleadoresVisitante = null!;
        private Label lblAyudaGoleadores = null!;
        private ComboBox cmbUsuario = null!;
        private ComboBox cmbPartido = null!;
        private NumericUpDown nudGolesLocal = null!;
        private NumericUpDown nudGolesVisitante = null!;
        private TextBox txtGoleadoresLocal = null!;
        private TextBox txtGoleadoresVisitante = null!;
        private Button btnRegistrarPronostico = null!;
        private Button btnActualizar = null!;
        private Button btnCerrar = null!;
        private DataGridView dgvPronosticos = null!;
    }
}
