namespace Quiniegol.Views
{
    partial class FrmUsuarios
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
            btnRegistrar = new Button();
            dgvUsuarios = new DataGridView();
            btnRestablecer = new Button();
            btnCambiarEstado = new Button();
            btnCerrar = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvUsuarios).BeginInit();
            SuspendLayout();
            //
            // lblTitulo
            //
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
            lblTitulo.Location = new Point(24, 22);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(203, 28);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Usuarios registrados";
            //
            // btnRegistrar
            //
            btnRegistrar.Location = new Point(719, 22);
            btnRegistrar.Name = "btnRegistrar";
            btnRegistrar.Size = new Size(178, 32);
            btnRegistrar.TabIndex = 1;
            btnRegistrar.Text = "Registrar nuevo usuario";
            btnRegistrar.UseVisualStyleBackColor = true;
            btnRegistrar.Click += btnRegistrar_Click;
            //
            // dgvUsuarios
            //
            dgvUsuarios.AllowUserToAddRows = false;
            dgvUsuarios.AllowUserToDeleteRows = false;
            dgvUsuarios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvUsuarios.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvUsuarios.Location = new Point(24, 72);
            dgvUsuarios.MultiSelect = false;
            dgvUsuarios.Name = "dgvUsuarios";
            dgvUsuarios.ReadOnly = true;
            dgvUsuarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUsuarios.Size = new Size(873, 372);
            dgvUsuarios.TabIndex = 2;
            dgvUsuarios.SelectionChanged += dgvUsuarios_SelectionChanged;
            //
            // btnRestablecer
            //
            btnRestablecer.Enabled = false;
            btnRestablecer.Location = new Point(24, 459);
            btnRestablecer.Name = "btnRestablecer";
            btnRestablecer.Size = new Size(200, 30);
            btnRestablecer.TabIndex = 3;
            btnRestablecer.Text = "Restablecer contraseña";
            btnRestablecer.UseVisualStyleBackColor = true;
            btnRestablecer.Click += btnRestablecer_Click;
            //
            // btnCambiarEstado
            //
            btnCambiarEstado.Enabled = false;
            btnCambiarEstado.Location = new Point(236, 459);
            btnCambiarEstado.Name = "btnCambiarEstado";
            btnCambiarEstado.Size = new Size(174, 30);
            btnCambiarEstado.TabIndex = 4;
            btnCambiarEstado.Text = "Desactivar cuenta";
            btnCambiarEstado.UseVisualStyleBackColor = true;
            btnCambiarEstado.Click += btnCambiarEstado_Click;
            //
            // btnCerrar
            //
            btnCerrar.Location = new Point(793, 459);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(104, 30);
            btnCerrar.TabIndex = 5;
            btnCerrar.Text = "Cerrar";
            btnCerrar.UseVisualStyleBackColor = true;
            btnCerrar.Click += btnCerrar_Click;
            //
            // FrmUsuarios
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(922, 511);
            Controls.Add(btnCerrar);
            Controls.Add(btnCambiarEstado);
            Controls.Add(btnRestablecer);
            Controls.Add(dgvUsuarios);
            Controls.Add(btnRegistrar);
            Controls.Add(lblTitulo);
            Name = "FrmUsuarios";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Gestión de usuarios";
            ((System.ComponentModel.ISupportInitialize)dgvUsuarios).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private Label lblTitulo = null!;
        private Button btnRegistrar = null!;
        private DataGridView dgvUsuarios = null!;
        private Button btnRestablecer = null!;
        private Button btnCambiarEstado = null!;
        private Button btnCerrar = null!;
    }
}
