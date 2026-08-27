namespace Quiniegol.Views
{
    partial class FrmLogin
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
            lblIdentificador = new Label();
            txtIdentificador = new TextBox();
            lblContrasena = new Label();
            txtContrasena = new TextBox();
            btnIngresar = new Button();
            btnRegistrar = new Button();
            btnSalir = new Button();
            lblAyuda = new Label();
            lblError = new Label();
            SuspendLayout();
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTitulo.Location = new Point(74, 24);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(292, 32);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Inicio de Sesión Quinegol";
            // 
            // lblIdentificador
            // 
            lblIdentificador.AutoSize = true;
            lblIdentificador.Location = new Point(48, 87);
            lblIdentificador.Name = "lblIdentificador";
            lblIdentificador.Size = new Size(107, 15);
            lblIdentificador.TabIndex = 1;
            lblIdentificador.Text = "Usuario o correo:";
            // 
            // txtIdentificador
            // 
            txtIdentificador.Location = new Point(48, 105);
            txtIdentificador.Name = "txtIdentificador";
            txtIdentificador.PlaceholderText = "Ejemplo: ejemplo@gmail.com";
            txtIdentificador.Size = new Size(344, 23);
            txtIdentificador.TabIndex = 2;
            // 
            // lblContrasena
            // 
            lblContrasena.AutoSize = true;
            lblContrasena.Location = new Point(48, 143);
            lblContrasena.Name = "lblContrasena";
            lblContrasena.Size = new Size(70, 15);
            lblContrasena.TabIndex = 3;
            lblContrasena.Text = "Contraseña:";
            // 
            // txtContrasena
            // 
            txtContrasena.Location = new Point(48, 161);
            txtContrasena.Name = "txtContrasena";
            txtContrasena.Size = new Size(344, 23);
            txtContrasena.TabIndex = 4;
            txtContrasena.UseSystemPasswordChar = true;
            // 
            // btnIngresar
            // 
            btnIngresar.Location = new Point(214, 238);
            btnIngresar.Name = "btnIngresar";
            btnIngresar.Size = new Size(86, 31);
            btnIngresar.TabIndex = 8;
            btnIngresar.Text = "Ingresar";
            btnIngresar.UseVisualStyleBackColor = true;
            btnIngresar.Click += btnIngresar_Click;
            //
            // btnRegistrar
            //
            btnRegistrar.Location = new Point(48, 238);
            btnRegistrar.Name = "btnRegistrar";
            btnRegistrar.Size = new Size(150, 31);
            btnRegistrar.TabIndex = 7;
            btnRegistrar.Text = "Crear una cuenta";
            btnRegistrar.UseVisualStyleBackColor = true;
            btnRegistrar.Click += btnRegistrar_Click;
            // 
            // btnSalir
            // 
            btnSalir.DialogResult = DialogResult.Cancel;
            btnSalir.Location = new Point(306, 238);
            btnSalir.Name = "btnSalir";
            btnSalir.Size = new Size(86, 31);
            btnSalir.TabIndex = 9;
            btnSalir.Text = "Salir";
            btnSalir.UseVisualStyleBackColor = true;
            btnSalir.Click += btnSalir_Click;
            // 
            // lblAyuda
            // 
            lblAyuda.AutoSize = true;
            lblAyuda.ForeColor = Color.DimGray;
            lblAyuda.Location = new Point(48, 199);
            lblAyuda.Name = "lblAyuda";
            lblAyuda.Size = new Size(302, 15);
            lblAyuda.TabIndex = 5;
            lblAyuda.Text = "La contraseña distingue mayúsculas de minúsculas.";
            // 
            // lblError
            // 
            lblError.AutoSize = true;
            lblError.ForeColor = Color.Firebrick;
            lblError.Location = new Point(48, 278);
            lblError.MaximumSize = new Size(344, 0);
            lblError.Name = "lblError";
            lblError.Size = new Size(0, 15);
            lblError.TabIndex = 6;
            // 
            // FrmLogin
            // 
            AcceptButton = btnIngresar;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnSalir;
            ClientSize = new Size(440, 320);
            Controls.Add(lblError);
            Controls.Add(lblAyuda);
            Controls.Add(btnSalir);
            Controls.Add(btnIngresar);
            Controls.Add(btnRegistrar);
            Controls.Add(txtContrasena);
            Controls.Add(lblContrasena);
            Controls.Add(txtIdentificador);
            Controls.Add(lblIdentificador);
            Controls.Add(lblTitulo);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmLogin";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Iniciar sesión";
            ResumeLayout(false);
            PerformLayout();
        }

        private Label lblTitulo = null!;
        private Label lblIdentificador = null!;
        private TextBox txtIdentificador = null!;
        private Label lblContrasena = null!;
        private TextBox txtContrasena = null!;
        private Button btnIngresar = null!;
        private Button btnRegistrar = null!;
        private Button btnSalir = null!;
        private Label lblAyuda = null!;
        private Label lblError = null!;
    }
}
