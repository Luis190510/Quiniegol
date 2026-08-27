namespace Quiniegol.Views
{
    partial class FrmDetallePartido
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
            cmbPartido = new ComboBox();
            btnCargar = new Button();
            picLocal = new PictureBox();
            picVisitante = new PictureBox();
            lblNombreLocal = new Label();
            lblNombreVisitante = new Label();
            lblMarcador = new Label();
            lblEstado = new Label();
            lblFecha = new Label();
            lblGoleadores = new Label();
            dgvAnotadores = new DataGridView();
            lblSeleccionAnotador = new Label();
            cmbSeleccionAnotador = new ComboBox();
            btnCerrar = new Button();
            ((System.ComponentModel.ISupportInitialize)picLocal).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picVisitante).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvAnotadores).BeginInit();
            SuspendLayout();
            //
            // cmbPartido
            //
            cmbPartido.FormattingEnabled = true;
            cmbPartido.Location = new Point(20, 20);
            cmbPartido.Name = "cmbPartido";
            cmbPartido.Size = new Size(788, 23);
            cmbPartido.TabIndex = 0;
            //
            // btnCargar
            //
            btnCargar.Location = new Point(825, 17);
            btnCargar.Name = "btnCargar";
            btnCargar.Size = new Size(196, 29);
            btnCargar.TabIndex = 1;
            btnCargar.Text = "Cargar detalle";
            btnCargar.UseVisualStyleBackColor = true;
            btnCargar.Click += btnCargar_Click;
            //
            // picLocal
            //
            picLocal.BorderStyle = BorderStyle.FixedSingle;
            picLocal.Location = new Point(80, 69);
            picLocal.Name = "picLocal";
            picLocal.Size = new Size(190, 112);
            picLocal.SizeMode = PictureBoxSizeMode.Zoom;
            picLocal.TabIndex = 2;
            picLocal.TabStop = false;
            //
            // picVisitante
            //
            picVisitante.BorderStyle = BorderStyle.FixedSingle;
            picVisitante.Location = new Point(771, 69);
            picVisitante.Name = "picVisitante";
            picVisitante.Size = new Size(190, 112);
            picVisitante.SizeMode = PictureBoxSizeMode.Zoom;
            picVisitante.TabIndex = 3;
            picVisitante.TabStop = false;
            //
            // lblNombreLocal
            //
            lblNombreLocal.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblNombreLocal.Location = new Point(45, 188);
            lblNombreLocal.Name = "lblNombreLocal";
            lblNombreLocal.Size = new Size(260, 23);
            lblNombreLocal.TabIndex = 4;
            lblNombreLocal.Text = "Local";
            lblNombreLocal.TextAlign = ContentAlignment.MiddleCenter;
            //
            // lblNombreVisitante
            //
            lblNombreVisitante.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblNombreVisitante.Location = new Point(736, 188);
            lblNombreVisitante.Name = "lblNombreVisitante";
            lblNombreVisitante.Size = new Size(260, 23);
            lblNombreVisitante.TabIndex = 5;
            lblNombreVisitante.Text = "Visitante";
            lblNombreVisitante.TextAlign = ContentAlignment.MiddleCenter;
            //
            // lblMarcador
            //
            lblMarcador.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblMarcador.Location = new Point(390, 84);
            lblMarcador.Name = "lblMarcador";
            lblMarcador.Size = new Size(260, 32);
            lblMarcador.TabIndex = 6;
            lblMarcador.Text = "Marcador";
            lblMarcador.TextAlign = ContentAlignment.MiddleCenter;
            //
            // lblEstado
            //
            lblEstado.Location = new Point(390, 127);
            lblEstado.Name = "lblEstado";
            lblEstado.Size = new Size(260, 23);
            lblEstado.TabIndex = 7;
            lblEstado.Text = "Estado";
            lblEstado.TextAlign = ContentAlignment.MiddleCenter;
            //
            // lblFecha
            //
            lblFecha.Location = new Point(390, 158);
            lblFecha.Name = "lblFecha";
            lblFecha.Size = new Size(260, 23);
            lblFecha.TabIndex = 8;
            lblFecha.Text = "Fecha";
            lblFecha.TextAlign = ContentAlignment.MiddleCenter;
            //
            // lblGoleadores
            //
            lblGoleadores.AutoSize = true;
            lblGoleadores.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblGoleadores.Location = new Point(20, 220);
            lblGoleadores.Name = "lblGoleadores";
            lblGoleadores.Size = new Size(190, 15);
            lblGoleadores.TabIndex = 9;
            lblGoleadores.Text = "Goleadores oficiales del partido";
            //
            // dgvAnotadores
            //
            dgvAnotadores.AllowUserToAddRows = false;
            dgvAnotadores.AllowUserToDeleteRows = false;
            dgvAnotadores.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvAnotadores.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvAnotadores.Location = new Point(20, 242);
            dgvAnotadores.MultiSelect = false;
            dgvAnotadores.Name = "dgvAnotadores";
            dgvAnotadores.ReadOnly = true;
            dgvAnotadores.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAnotadores.Size = new Size(1001, 258);
            dgvAnotadores.TabIndex = 10;
            //
            // lblSeleccionAnotador
            //
            lblSeleccionAnotador.AutoSize = true;
            lblSeleccionAnotador.Location = new Point(20, 520);
            lblSeleccionAnotador.Name = "lblSeleccionAnotador";
            lblSeleccionAnotador.Size = new Size(61, 15);
            lblSeleccionAnotador.TabIndex = 11;
            lblSeleccionAnotador.Text = "Filtrar goleadores por selección:";
            //
            // cmbSeleccionAnotador
            //
            cmbSeleccionAnotador.FormattingEnabled = true;
            cmbSeleccionAnotador.Location = new Point(20, 541);
            cmbSeleccionAnotador.Name = "cmbSeleccionAnotador";
            cmbSeleccionAnotador.Size = new Size(256, 23);
            cmbSeleccionAnotador.TabIndex = 12;
            cmbSeleccionAnotador.SelectedIndexChanged += cmbSeleccionAnotador_SelectedIndexChanged;
            //
            // btnCerrar
            //
            btnCerrar.Location = new Point(862, 532);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(159, 32);
            btnCerrar.TabIndex = 13;
            btnCerrar.Text = "Cerrar";
            btnCerrar.UseVisualStyleBackColor = true;
            btnCerrar.Click += btnCerrar_Click;
            //
            // FrmDetallePartido
            //
            AcceptButton = btnCargar;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCerrar;
            ClientSize = new Size(1041, 584);
            Controls.Add(btnCerrar);
            Controls.Add(cmbSeleccionAnotador);
            Controls.Add(lblSeleccionAnotador);
            Controls.Add(dgvAnotadores);
            Controls.Add(lblGoleadores);
            Controls.Add(lblFecha);
            Controls.Add(lblEstado);
            Controls.Add(lblMarcador);
            Controls.Add(lblNombreVisitante);
            Controls.Add(lblNombreLocal);
            Controls.Add(picVisitante);
            Controls.Add(picLocal);
            Controls.Add(btnCargar);
            Controls.Add(cmbPartido);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmDetallePartido";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Detalle de partidos";
            ((System.ComponentModel.ISupportInitialize)picLocal).EndInit();
            ((System.ComponentModel.ISupportInitialize)picVisitante).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvAnotadores).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ComboBox cmbPartido;
        private Button btnCargar;
        private PictureBox picLocal;
        private PictureBox picVisitante;
        private Label lblNombreLocal;
        private Label lblNombreVisitante;
        private Label lblMarcador;
        private Label lblEstado;
        private Label lblFecha;
        private Label lblGoleadores;
        private DataGridView dgvAnotadores;
        private Label lblSeleccionAnotador;
        private ComboBox cmbSeleccionAnotador;
        private Button btnCerrar;
    }
}
