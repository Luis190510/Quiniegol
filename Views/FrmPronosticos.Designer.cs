namespace Quiniegol.Views
{
    partial class FrmPronosticos
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
            lblPartido = new Label();
            lblGolesLocal = new Label();
            lblGolesVisitante = new Label();
            cmbUsuario = new ComboBox();
            cmbPartido = new ComboBox();
            nudGolesLocal = new NumericUpDown();
            nudGolesVisitante = new NumericUpDown();
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
            lblUsuario.Location = new Point(81, 29);
            lblUsuario.Name = "lblUsuario";
            lblUsuario.Size = new Size(50, 15);
            lblUsuario.TabIndex = 0;
            lblUsuario.Text = "Usuario:";
            // 
            // lblPartido
            // 
            lblPartido.AutoSize = true;
            lblPartido.Location = new Point(81, 58);
            lblPartido.Name = "lblPartido";
            lblPartido.Size = new Size(48, 15);
            lblPartido.TabIndex = 1;
            lblPartido.Text = "Partido:";
            // 
            // lblGolesLocal
            // 
            lblGolesLocal.AutoSize = true;
            lblGolesLocal.Location = new Point(79, 97);
            lblGolesLocal.Name = "lblGolesLocal";
            lblGolesLocal.Size = new Size(67, 15);
            lblGolesLocal.TabIndex = 2;
            lblGolesLocal.Text = "Goles local:";
            // 
            // lblGolesVisitante
            // 
            lblGolesVisitante.AutoSize = true;
            lblGolesVisitante.Location = new Point(81, 134);
            lblGolesVisitante.Name = "lblGolesVisitante";
            lblGolesVisitante.Size = new Size(86, 15);
            lblGolesVisitante.TabIndex = 3;
            lblGolesVisitante.Text = "Goles visitante:";
            // 
            // cmbUsuario
            // 
            cmbUsuario.FormattingEnabled = true;
            cmbUsuario.Location = new Point(229, 26);
            cmbUsuario.Name = "cmbUsuario";
            cmbUsuario.Size = new Size(121, 23);
            cmbUsuario.TabIndex = 4;
            // 
            // cmbPartido
            // 
            cmbPartido.FormattingEnabled = true;
            cmbPartido.Location = new Point(229, 55);
            cmbPartido.Name = "cmbPartido";
            cmbPartido.Size = new Size(121, 23);
            cmbPartido.TabIndex = 5;
            // 
            // nudGolesLocal
            // 
            nudGolesLocal.Location = new Point(230, 95);
            nudGolesLocal.Maximum = new decimal(new int[] { 20, 0, 0, 0 });
            nudGolesLocal.Name = "nudGolesLocal";
            nudGolesLocal.Size = new Size(120, 23);
            nudGolesLocal.TabIndex = 6;
            // 
            // nudGolesVisitante
            // 
            nudGolesVisitante.Location = new Point(229, 132);
            nudGolesVisitante.Maximum = new decimal(new int[] { 20, 0, 0, 0 });
            nudGolesVisitante.Name = "nudGolesVisitante";
            nudGolesVisitante.Size = new Size(120, 23);
            nudGolesVisitante.TabIndex = 7;
            // 
            // btnRegistrarPronostico
            // 
            btnRegistrarPronostico.Location = new Point(81, 186);
            btnRegistrarPronostico.Name = "btnRegistrarPronostico";
            btnRegistrarPronostico.Size = new Size(140, 23);
            btnRegistrarPronostico.TabIndex = 8;
            btnRegistrarPronostico.Text = "Registrar pronóstico";
            btnRegistrarPronostico.UseVisualStyleBackColor = true;
            btnRegistrarPronostico.Click += btnRegistrarPronostico_Click;
            // 
            // btnActualizar
            // 
            btnActualizar.Location = new Point(92, 391);
            btnActualizar.Name = "btnActualizar";
            btnActualizar.Size = new Size(75, 23);
            btnActualizar.TabIndex = 9;
            btnActualizar.Text = "Actualizar";
            btnActualizar.UseVisualStyleBackColor = true;
            btnActualizar.Click += btnActualizar_Click;
            // 
            // btnCerrar
            // 
            btnCerrar.Location = new Point(837, 526);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(75, 23);
            btnCerrar.TabIndex = 10;
            btnCerrar.Text = "Cerrar";
            btnCerrar.UseVisualStyleBackColor = true;
            btnCerrar.Click += btnCerrar_Click;
            // 
            // dgvPronosticos
            // 
            dgvPronosticos.AllowUserToAddRows = false;
            dgvPronosticos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPronosticos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvPronosticos.Location = new Point(229, 226);
            dgvPronosticos.Name = "dgvPronosticos";
            dgvPronosticos.ReadOnly = true;
            dgvPronosticos.Size = new Size(600, 150);
            dgvPronosticos.TabIndex = 11;
            // 
            // FrmPronosticos
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(934, 561);
            Controls.Add(dgvPronosticos);
            Controls.Add(btnCerrar);
            Controls.Add(btnActualizar);
            Controls.Add(btnRegistrarPronostico);
            Controls.Add(nudGolesVisitante);
            Controls.Add(nudGolesLocal);
            Controls.Add(cmbPartido);
            Controls.Add(cmbUsuario);
            Controls.Add(lblGolesVisitante);
            Controls.Add(lblGolesLocal);
            Controls.Add(lblPartido);
            Controls.Add(lblUsuario);
            Name = "FrmPronosticos";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Gestión de pronósticos";
            ((System.ComponentModel.ISupportInitialize)nudGolesLocal).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudGolesVisitante).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvPronosticos).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblUsuario;
        private Label lblPartido;
        private Label lblGolesLocal;
        private Label lblGolesVisitante;
        private ComboBox cmbUsuario;
        private ComboBox cmbPartido;
        private NumericUpDown nudGolesLocal;
        private NumericUpDown nudGolesVisitante;
        private Button btnRegistrarPronostico;
        private Button btnActualizar;
        private Button btnCerrar;
        private DataGridView dgvPronosticos;
    }
}