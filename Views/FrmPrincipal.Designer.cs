namespace Quiniegol.Views
{
    partial class FrmPrincipal
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
            btnUsuarios = new Button();
            SuspendLayout();
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Location = new Point(410, 53);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(100, 15);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Sistema Quinegol";
            lblTitulo.Click += label1_Click;
            // 
            // btnUsuarios
            // 
            btnUsuarios.Location = new Point(410, 95);
            btnUsuarios.Name = "btnUsuarios";
            btnUsuarios.Size = new Size(103, 52);
            btnUsuarios.TabIndex = 1;
            btnUsuarios.Text = "Gestion de Usuarios";
            btnUsuarios.UseVisualStyleBackColor = true;
            btnUsuarios.Click += btnUsuarios_Click;
            // 
            // FrmPrincipal
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(784, 411);
            Controls.Add(btnUsuarios);
            Controls.Add(lblTitulo);
            Name = "FrmPrincipal";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Quinegol- Menu Principal";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitulo;
        private Button btnUsuarios;
    }
}