namespace Quiniegol.Views
{
    partial class FrmRegistroUsuario
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
            lblNombre = new Label();
            txtNombre = new TextBox();
            lblPais = new Label();
            cmbPais = new ComboBox();
            lblNombreUsuario = new Label();
            txtNombreUsuario = new TextBox();
            lblCorreo = new Label();
            txtCorreo = new TextBox();
            lblContrasena = new Label();
            txtContrasena = new TextBox();
            lblConfirmacion = new Label();
            txtConfirmacion = new TextBox();
            lblAyuda = new Label();
            lblError = new Label();
            btnRegistrar = new Button();
            btnCancelar = new Button();
            SuspendLayout();
            //
            // lblTitulo
            //
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTitulo.Location = new Point(34, 24);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(227, 30);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Crear cuenta Quinegol";
            //
            // lblNombre
            //
            lblNombre.AutoSize = true;
            lblNombre.Location = new Point(34, 78);
            lblNombre.Name = "lblNombre";
            lblNombre.Size = new Size(106, 15);
            lblNombre.TabIndex = 1;
            lblNombre.Text = "Nombre completo:";
            //
            // txtNombre
            //
            txtNombre.Location = new Point(34, 96);
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(492, 23);
            txtNombre.TabIndex = 2;
            //
            // lblPais
            //
            lblPais.AutoSize = true;
            lblPais.Location = new Point(34, 134);
            lblPais.Name = "lblPais";
            lblPais.Size = new Size(78, 15);
            lblPais.TabIndex = 3;
            lblPais.Text = "País favorito:";
            //
            // cmbPais
            //
            cmbPais.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbPais.FormattingEnabled = true;
            cmbPais.Location = new Point(34, 152);
            cmbPais.Name = "cmbPais";
            cmbPais.Size = new Size(492, 23);
            cmbPais.TabIndex = 4;
            //
            // lblNombreUsuario
            //
            lblNombreUsuario.AutoSize = true;
            lblNombreUsuario.Location = new Point(34, 190);
            lblNombreUsuario.Name = "lblNombreUsuario";
            lblNombreUsuario.Size = new Size(110, 15);
            lblNombreUsuario.TabIndex = 5;
            lblNombreUsuario.Text = "Nombre de usuario:";
            //
            // txtNombreUsuario
            //
            txtNombreUsuario.Location = new Point(34, 208);
            txtNombreUsuario.Name = "txtNombreUsuario";
            txtNombreUsuario.PlaceholderText = "Ejemplo: maria.rojas";
            txtNombreUsuario.Size = new Size(492, 23);
            txtNombreUsuario.TabIndex = 6;
            //
            // lblCorreo
            //
            lblCorreo.AutoSize = true;
            lblCorreo.Location = new Point(34, 246);
            lblCorreo.Name = "lblCorreo";
            lblCorreo.Size = new Size(107, 15);
            lblCorreo.TabIndex = 7;
            lblCorreo.Text = "Correo electrónico:";
            //
            // txtCorreo
            //
            txtCorreo.Location = new Point(34, 264);
            txtCorreo.Name = "txtCorreo";
            txtCorreo.Size = new Size(492, 23);
            txtCorreo.TabIndex = 8;
            //
            // lblContrasena
            //
            lblContrasena.AutoSize = true;
            lblContrasena.Location = new Point(34, 302);
            lblContrasena.Name = "lblContrasena";
            lblContrasena.Size = new Size(70, 15);
            lblContrasena.TabIndex = 9;
            lblContrasena.Text = "Contraseña:";
            //
            // txtContrasena
            //
            txtContrasena.Location = new Point(34, 320);
            txtContrasena.Name = "txtContrasena";
            txtContrasena.Size = new Size(238, 23);
            txtContrasena.TabIndex = 10;
            txtContrasena.UseSystemPasswordChar = true;
            //
            // lblConfirmacion
            //
            lblConfirmacion.AutoSize = true;
            lblConfirmacion.Location = new Point(288, 302);
            lblConfirmacion.Name = "lblConfirmacion";
            lblConfirmacion.Size = new Size(127, 15);
            lblConfirmacion.TabIndex = 11;
            lblConfirmacion.Text = "Confirmar contraseña:";
            //
            // txtConfirmacion
            //
            txtConfirmacion.Location = new Point(288, 320);
            txtConfirmacion.Name = "txtConfirmacion";
            txtConfirmacion.Size = new Size(238, 23);
            txtConfirmacion.TabIndex = 12;
            txtConfirmacion.UseSystemPasswordChar = true;
            //
            // lblAyuda
            //
            lblAyuda.AutoSize = true;
            lblAyuda.ForeColor = Color.DimGray;
            lblAyuda.Location = new Point(34, 352);
            lblAyuda.Name = "lblAyuda";
            lblAyuda.Size = new Size(237, 15);
            lblAyuda.TabIndex = 13;
            lblAyuda.Text = "La contraseña debe tener al menos 8 caracteres.";
            //
            // lblError
            //
            lblError.AutoSize = true;
            lblError.ForeColor = Color.Firebrick;
            lblError.Location = new Point(34, 381);
            lblError.MaximumSize = new Size(492, 0);
            lblError.Name = "lblError";
            lblError.Size = new Size(0, 15);
            lblError.TabIndex = 14;
            //
            // btnRegistrar
            //
            btnRegistrar.Location = new Point(330, 428);
            btnRegistrar.Name = "btnRegistrar";
            btnRegistrar.Size = new Size(94, 32);
            btnRegistrar.TabIndex = 15;
            btnRegistrar.Text = "Registrar";
            btnRegistrar.UseVisualStyleBackColor = true;
            btnRegistrar.Click += btnRegistrar_Click;
            //
            // btnCancelar
            //
            btnCancelar.DialogResult = DialogResult.Cancel;
            btnCancelar.Location = new Point(432, 428);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(94, 32);
            btnCancelar.TabIndex = 16;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = true;
            btnCancelar.Click += btnCancelar_Click;
            //
            // FrmRegistroUsuario
            //
            AcceptButton = btnRegistrar;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancelar;
            ClientSize = new Size(562, 484);
            Controls.Add(btnCancelar);
            Controls.Add(btnRegistrar);
            Controls.Add(lblError);
            Controls.Add(lblAyuda);
            Controls.Add(txtConfirmacion);
            Controls.Add(lblConfirmacion);
            Controls.Add(txtContrasena);
            Controls.Add(lblContrasena);
            Controls.Add(txtCorreo);
            Controls.Add(lblCorreo);
            Controls.Add(txtNombreUsuario);
            Controls.Add(lblNombreUsuario);
            Controls.Add(cmbPais);
            Controls.Add(lblPais);
            Controls.Add(txtNombre);
            Controls.Add(lblNombre);
            Controls.Add(lblTitulo);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmRegistroUsuario";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Registrar participante";
            ResumeLayout(false);
            PerformLayout();
        }

        private Label lblTitulo = null!;
        private Label lblNombre = null!;
        private TextBox txtNombre = null!;
        private Label lblPais = null!;
        private ComboBox cmbPais = null!;
        private Label lblNombreUsuario = null!;
        private TextBox txtNombreUsuario = null!;
        private Label lblCorreo = null!;
        private TextBox txtCorreo = null!;
        private Label lblContrasena = null!;
        private TextBox txtContrasena = null!;
        private Label lblConfirmacion = null!;
        private TextBox txtConfirmacion = null!;
        private Label lblAyuda = null!;
        private Label lblError = null!;
        private Button btnRegistrar = null!;
        private Button btnCancelar = null!;
    }
}
