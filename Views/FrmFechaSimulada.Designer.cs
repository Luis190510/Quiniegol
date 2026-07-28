namespace Quiniegol.Views
{
    partial class FrmFechaSimulada
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
            lblFechaActual = new Label();
            dtpFechaSimulada = new DateTimePicker();
            btnAplicarFecha = new Button();
            btnCerrar = new Button();
            SuspendLayout();
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Location = new Point(41, 28);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(135, 15);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Cambiar fecha simulada";
            // 
            // lblFechaActual
            // 
            lblFechaActual.AutoSize = true;
            lblFechaActual.Location = new Point(41, 64);
            lblFechaActual.Name = "lblFechaActual";
            lblFechaActual.Size = new Size(76, 15);
            lblFechaActual.TabIndex = 1;
            lblFechaActual.Text = "Fecha actual:";
            // 
            // dtpFechaSimulada
            // 
            dtpFechaSimulada.CustomFormat = "dd/MM/yyyy HH:mm";
            dtpFechaSimulada.Format = DateTimePickerFormat.Custom;
            dtpFechaSimulada.Location = new Point(41, 92);
            dtpFechaSimulada.Name = "dtpFechaSimulada";
            dtpFechaSimulada.ShowUpDown = true;
            dtpFechaSimulada.Size = new Size(200, 23);
            dtpFechaSimulada.TabIndex = 2;
            // 
            // btnAplicarFecha
            // 
            btnAplicarFecha.Location = new Point(41, 136);
            btnAplicarFecha.Name = "btnAplicarFecha";
            btnAplicarFecha.Size = new Size(135, 23);
            btnAplicarFecha.TabIndex = 3;
            btnAplicarFecha.Text = "Aplicar fecha";
            btnAplicarFecha.UseVisualStyleBackColor = true;
            btnAplicarFecha.Click += btnAplicarFecha_Click;
            // 
            // btnCerrar
            // 
            btnCerrar.Location = new Point(382, 226);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(75, 23);
            btnCerrar.TabIndex = 4;
            btnCerrar.Text = "Cerrar";
            btnCerrar.UseVisualStyleBackColor = true;
            btnCerrar.Click += btnCerrar_Click;
            // 
            // FrmFechaSimulada
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(484, 261);
            Controls.Add(btnCerrar);
            Controls.Add(btnAplicarFecha);
            Controls.Add(dtpFechaSimulada);
            Controls.Add(lblFechaActual);
            Controls.Add(lblTitulo);
            Name = "FrmFechaSimulada";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Fecha simulada del sistema";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitulo;
        private Label lblFechaActual;
        private DateTimePicker dtpFechaSimulada;
        private Button btnAplicarFecha;
        private Button btnCerrar;
    }
}