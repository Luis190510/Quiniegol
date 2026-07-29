namespace Quiniegol.Views
{
    partial class FrmQuinielas
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
            grpCrearQuiniela = new GroupBox();
            btnCrearQuiniela = new Button();
            clbUsuarios = new CheckedListBox();
            lblUsuarios = new Label();
            txtDescripcion = new TextBox();
            lblDescripcion = new Label();
            txtNombre = new TextBox();
            lblNombre = new Label();
            grpIntegrantes = new GroupBox();
            dgvIntegrantes = new DataGridView();
            btnQuitarIntegrante = new Button();
            btnAgregarIntegrante = new Button();
            cmbUsuarioIntegrante = new ComboBox();
            lblUsuarioIntegrante = new Label();
            btnVerIntegrantes = new Button();
            cmbQuiniela = new ComboBox();
            lblQuiniela = new Label();
            btnCerrar = new Button();
            grpCrearQuiniela.SuspendLayout();
            grpIntegrantes.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvIntegrantes).BeginInit();
            SuspendLayout();
            // 
            // grpCrearQuiniela
            // 
            grpCrearQuiniela.Controls.Add(btnCrearQuiniela);
            grpCrearQuiniela.Controls.Add(clbUsuarios);
            grpCrearQuiniela.Controls.Add(lblUsuarios);
            grpCrearQuiniela.Controls.Add(txtDescripcion);
            grpCrearQuiniela.Controls.Add(lblDescripcion);
            grpCrearQuiniela.Controls.Add(txtNombre);
            grpCrearQuiniela.Controls.Add(lblNombre);
            grpCrearQuiniela.Location = new Point(18, 16);
            grpCrearQuiniela.Name = "grpCrearQuiniela";
            grpCrearQuiniela.Size = new Size(350, 521);
            grpCrearQuiniela.TabIndex = 0;
            grpCrearQuiniela.TabStop = false;
            grpCrearQuiniela.Text = "Crear quiniela privada";
            // 
            // btnCrearQuiniela
            // 
            btnCrearQuiniela.Location = new Point(96, 478);
            btnCrearQuiniela.Name = "btnCrearQuiniela";
            btnCrearQuiniela.Size = new Size(158, 27);
            btnCrearQuiniela.TabIndex = 6;
            btnCrearQuiniela.Text = "Crear quiniela";
            btnCrearQuiniela.UseVisualStyleBackColor = true;
            btnCrearQuiniela.Click += btnCrearQuiniela_Click;
            // 
            // clbUsuarios
            // 
            clbUsuarios.CheckOnClick = true;
            clbUsuarios.FormattingEnabled = true;
            clbUsuarios.Location = new Point(22, 249);
            clbUsuarios.Name = "clbUsuarios";
            clbUsuarios.Size = new Size(306, 220);
            clbUsuarios.TabIndex = 5;
            // 
            // lblUsuarios
            // 
            lblUsuarios.AutoSize = true;
            lblUsuarios.Location = new Point(22, 226);
            lblUsuarios.Name = "lblUsuarios";
            lblUsuarios.Size = new Size(171, 15);
            lblUsuarios.TabIndex = 4;
            lblUsuarios.Text = "Integrantes iniciales (opcional):";
            // 
            // txtDescripcion
            // 
            txtDescripcion.Location = new Point(22, 119);
            txtDescripcion.Multiline = true;
            txtDescripcion.Name = "txtDescripcion";
            txtDescripcion.ScrollBars = ScrollBars.Vertical;
            txtDescripcion.Size = new Size(306, 91);
            txtDescripcion.TabIndex = 3;
            // 
            // lblDescripcion
            // 
            lblDescripcion.AutoSize = true;
            lblDescripcion.Location = new Point(22, 96);
            lblDescripcion.Name = "lblDescripcion";
            lblDescripcion.Size = new Size(72, 15);
            lblDescripcion.TabIndex = 2;
            lblDescripcion.Text = "Descripción:";
            // 
            // txtNombre
            // 
            txtNombre.Location = new Point(22, 58);
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(306, 23);
            txtNombre.TabIndex = 1;
            // 
            // lblNombre
            // 
            lblNombre.AutoSize = true;
            lblNombre.Location = new Point(22, 35);
            lblNombre.Name = "lblNombre";
            lblNombre.Size = new Size(54, 15);
            lblNombre.TabIndex = 0;
            lblNombre.Text = "Nombre:";
            // 
            // grpIntegrantes
            // 
            grpIntegrantes.Controls.Add(dgvIntegrantes);
            grpIntegrantes.Controls.Add(btnQuitarIntegrante);
            grpIntegrantes.Controls.Add(btnAgregarIntegrante);
            grpIntegrantes.Controls.Add(cmbUsuarioIntegrante);
            grpIntegrantes.Controls.Add(lblUsuarioIntegrante);
            grpIntegrantes.Controls.Add(btnVerIntegrantes);
            grpIntegrantes.Controls.Add(cmbQuiniela);
            grpIntegrantes.Controls.Add(lblQuiniela);
            grpIntegrantes.Location = new Point(384, 16);
            grpIntegrantes.Name = "grpIntegrantes";
            grpIntegrantes.Size = new Size(577, 521);
            grpIntegrantes.TabIndex = 1;
            grpIntegrantes.TabStop = false;
            grpIntegrantes.Text = "Administrar integrantes";
            // 
            // dgvIntegrantes
            // 
            dgvIntegrantes.AllowUserToAddRows = false;
            dgvIntegrantes.AllowUserToDeleteRows = false;
            dgvIntegrantes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvIntegrantes.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvIntegrantes.Location = new Point(19, 151);
            dgvIntegrantes.MultiSelect = false;
            dgvIntegrantes.Name = "dgvIntegrantes";
            dgvIntegrantes.ReadOnly = true;
            dgvIntegrantes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvIntegrantes.Size = new Size(539, 354);
            dgvIntegrantes.TabIndex = 7;
            // 
            // btnQuitarIntegrante
            // 
            btnQuitarIntegrante.Location = new Point(421, 104);
            btnQuitarIntegrante.Name = "btnQuitarIntegrante";
            btnQuitarIntegrante.Size = new Size(137, 27);
            btnQuitarIntegrante.TabIndex = 6;
            btnQuitarIntegrante.Text = "Quitar integrante";
            btnQuitarIntegrante.UseVisualStyleBackColor = true;
            btnQuitarIntegrante.Click += btnQuitarIntegrante_Click;
            // 
            // btnAgregarIntegrante
            // 
            btnAgregarIntegrante.Location = new Point(269, 104);
            btnAgregarIntegrante.Name = "btnAgregarIntegrante";
            btnAgregarIntegrante.Size = new Size(137, 27);
            btnAgregarIntegrante.TabIndex = 5;
            btnAgregarIntegrante.Text = "Agregar integrante";
            btnAgregarIntegrante.UseVisualStyleBackColor = true;
            btnAgregarIntegrante.Click += btnAgregarIntegrante_Click;
            // 
            // cmbUsuarioIntegrante
            // 
            cmbUsuarioIntegrante.FormattingEnabled = true;
            cmbUsuarioIntegrante.Location = new Point(19, 107);
            cmbUsuarioIntegrante.Name = "cmbUsuarioIntegrante";
            cmbUsuarioIntegrante.Size = new Size(234, 23);
            cmbUsuarioIntegrante.TabIndex = 4;
            // 
            // lblUsuarioIntegrante
            // 
            lblUsuarioIntegrante.AutoSize = true;
            lblUsuarioIntegrante.Location = new Point(19, 86);
            lblUsuarioIntegrante.Name = "lblUsuarioIntegrante";
            lblUsuarioIntegrante.Size = new Size(50, 15);
            lblUsuarioIntegrante.TabIndex = 3;
            lblUsuarioIntegrante.Text = "Usuario:";
            // 
            // btnVerIntegrantes
            // 
            btnVerIntegrantes.Location = new Point(421, 46);
            btnVerIntegrantes.Name = "btnVerIntegrantes";
            btnVerIntegrantes.Size = new Size(137, 27);
            btnVerIntegrantes.TabIndex = 2;
            btnVerIntegrantes.Text = "Ver integrantes";
            btnVerIntegrantes.UseVisualStyleBackColor = true;
            btnVerIntegrantes.Click += btnVerIntegrantes_Click;
            // 
            // cmbQuiniela
            // 
            cmbQuiniela.FormattingEnabled = true;
            cmbQuiniela.Location = new Point(19, 49);
            cmbQuiniela.Name = "cmbQuiniela";
            cmbQuiniela.Size = new Size(387, 23);
            cmbQuiniela.TabIndex = 1;
            // 
            // lblQuiniela
            // 
            lblQuiniela.AutoSize = true;
            lblQuiniela.Location = new Point(19, 28);
            lblQuiniela.Name = "lblQuiniela";
            lblQuiniela.Size = new Size(54, 15);
            lblQuiniela.TabIndex = 0;
            lblQuiniela.Text = "Quiniela:";
            // 
            // btnCerrar
            // 
            btnCerrar.Location = new Point(812, 552);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(149, 29);
            btnCerrar.TabIndex = 2;
            btnCerrar.Text = "Cerrar";
            btnCerrar.UseVisualStyleBackColor = true;
            btnCerrar.Click += btnCerrar_Click;
            // 
            // FrmQuinielas
            // 
            AcceptButton = btnCrearQuiniela;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCerrar;
            ClientSize = new Size(979, 598);
            Controls.Add(btnCerrar);
            Controls.Add(grpIntegrantes);
            Controls.Add(grpCrearQuiniela);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmQuinielas";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Quinielas privadas";
            grpCrearQuiniela.ResumeLayout(false);
            grpCrearQuiniela.PerformLayout();
            grpIntegrantes.ResumeLayout(false);
            grpIntegrantes.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvIntegrantes).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox grpCrearQuiniela;
        private Button btnCrearQuiniela;
        private CheckedListBox clbUsuarios;
        private Label lblUsuarios;
        private TextBox txtDescripcion;
        private Label lblDescripcion;
        private TextBox txtNombre;
        private Label lblNombre;
        private GroupBox grpIntegrantes;
        private DataGridView dgvIntegrantes;
        private Button btnQuitarIntegrante;
        private Button btnAgregarIntegrante;
        private ComboBox cmbUsuarioIntegrante;
        private Label lblUsuarioIntegrante;
        private Button btnVerIntegrantes;
        private ComboBox cmbQuiniela;
        private Label lblQuiniela;
        private Button btnCerrar;
    }
}
