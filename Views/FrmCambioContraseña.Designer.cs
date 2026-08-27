namespace Quiniegol.Views
{
    partial class FrmCambioContraseña
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
            lblExplicacion = new Label();
            lblCuenta = new Label();
            lblNuevaContrasena = new Label();
            txtNuevaContrasena = new TextBox();
            lblConfirmacion = new Label();
            txtConfirmacion = new TextBox();
            lblAyuda = new Label();
            lblError = new Label();
            btnGuardar = new Button();
            btnCancelar = new Button();
            SuspendLayout();
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTitulo.Location = new Point(28, 22);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(242, 30);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Cambiar la contraseña";
            // 
            // lblExplicacion
            // 
            lblExplicacion.AutoSize = true;
            lblExplicacion.ForeColor = Color.DimGray;
            lblExplicacion.Location = new Point(30, 62);
            lblExplicacion.MaximumSize = new Size(420, 0);
            lblExplicacion.Name = "lblExplicacion";
            lblExplicacion.Size = new Size(389, 30);
            lblExplicacion.TabIndex = 1;
            lblExplicacion.Text = "La contraseña actual es temporal. Debe elegir una nueva antes de continuar.";
            // 
            // lblCuenta
            // 
            lblCuenta.AutoSize = true;
            lblCuenta.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblCuenta.Location = new Point(30, 103);
            lblCuenta.Name = "lblCuenta";
            lblCuenta.Size = new Size(49, 15);
            lblCuenta.TabIndex = 2;
            lblCuenta.Text = "Cuenta:";
            // 
            // lblNuevaContrasena
            // 
            lblNuevaContrasena.AutoSize = true;
            lblNuevaContrasena.Location = new Point(30, 135);
            lblNuevaContrasena.Name = "lblNuevaContrasena";
            lblNuevaContrasena.Size = new Size(105, 15);
            lblNuevaContrasena.TabIndex = 3;
            lblNuevaContrasena.Text = "Nueva contraseña:";
            // 
            // txtNuevaContrasena
            // 
            txtNuevaContrasena.Location = new Point(30, 153);
            txtNuevaContrasena.Name = "txtNuevaContrasena";
            txtNuevaContrasena.Size = new Size(420, 23);
            txtNuevaContrasena.TabIndex = 4;
            txtNuevaContrasena.UseSystemPasswordChar = true;
            // 
            // lblConfirmacion
            // 
            lblConfirmacion.AutoSize = true;
            lblConfirmacion.Location = new Point(30, 189);
            lblConfirmacion.Name = "lblConfirmacion";
            lblConfirmacion.Size = new Size(127, 15);
            lblConfirmacion.TabIndex = 5;
            lblConfirmacion.Text = "Confirmar contraseña:";
            // 
            // txtConfirmacion
            // 
            txtConfirmacion.Location = new Point(30, 207);
            txtConfirmacion.Name = "txtConfirmacion";
            txtConfirmacion.Size = new Size(420, 23);
            txtConfirmacion.TabIndex = 6;
            txtConfirmacion.UseSystemPasswordChar = true;
            // 
            // lblAyuda
            // 
            lblAyuda.AutoSize = true;
            lblAyuda.ForeColor = Color.DimGray;
            lblAyuda.Location = new Point(30, 239);
            lblAyuda.Name = "lblAyuda";
            lblAyuda.Size = new Size(357, 15);
            lblAyuda.TabIndex = 7;
            lblAyuda.Text = "Use al menos 8 caracteres y no repita la contraseña temporal.";
            // 
            // lblError
            // 
            lblError.AutoSize = true;
            lblError.ForeColor = Color.Firebrick;
            lblError.Location = new Point(30, 265);
            lblError.MaximumSize = new Size(420, 0);
            lblError.Name = "lblError";
            lblError.Size = new Size(0, 15);
            lblError.TabIndex = 8;
            // 
            // btnGuardar
            // 
            btnGuardar.Location = new Point(250, 300);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(96, 32);
            btnGuardar.TabIndex = 9;
            btnGuardar.Text = "Guardar";
            btnGuardar.UseVisualStyleBackColor = true;
            btnGuardar.Click += btnGuardar_Click;
            // 
            // btnCancelar
            // 
            btnCancelar.DialogResult = DialogResult.Cancel;
            btnCancelar.Location = new Point(354, 300);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(96, 32);
            btnCancelar.TabIndex = 10;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = true;
            btnCancelar.Click += btnCancelar_Click;
            // 
            // FrmCambioContraseña
            // 
            AcceptButton = btnGuardar;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancelar;
            ClientSize = new Size(482, 354);
            Controls.Add(btnCancelar);
            Controls.Add(btnGuardar);
            Controls.Add(lblError);
            Controls.Add(lblAyuda);
            Controls.Add(txtConfirmacion);
            Controls.Add(lblConfirmacion);
            Controls.Add(txtNuevaContrasena);
            Controls.Add(lblNuevaContrasena);
            Controls.Add(lblCuenta);
            Controls.Add(lblExplicacion);
            Controls.Add(lblTitulo);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmCambioContraseña";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Cambio obligatorio de contraseña";
            ResumeLayout(false);
            PerformLayout();
        }

        private Label lblTitulo = null!;
        private Label lblExplicacion = null!;
        private Label lblCuenta = null!;
        private Label lblNuevaContrasena = null!;
        private TextBox txtNuevaContrasena = null!;
        private Label lblConfirmacion = null!;
        private TextBox txtConfirmacion = null!;
        private Label lblAyuda = null!;
        private Label lblError = null!;
        private Button btnGuardar = null!;
        private Button btnCancelar = null!;
    }
}
