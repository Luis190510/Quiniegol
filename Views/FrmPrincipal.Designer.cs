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
            btnSelecciones = new Button();
            btnPartidos = new Button();
            btnFechaSimulada = new Button();
            btnPronosticos = new Button();
            btnRanking = new Button();
            btnHistorialPronosticos = new Button();
            btnQuinielas = new Button();
            SuspendLayout();
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Location = new Point(322, 40);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(100, 15);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Sistema Quinegol";
            lblTitulo.Click += label1_Click;
            // 
            // btnUsuarios
            // 
            btnUsuarios.Location = new Point(375, 85);
            btnUsuarios.Name = "btnUsuarios";
            btnUsuarios.Size = new Size(103, 52);
            btnUsuarios.TabIndex = 1;
            btnUsuarios.Text = "Gestion de Usuarios";
            btnUsuarios.UseVisualStyleBackColor = true;
            btnUsuarios.Click += btnUsuarios_Click;
            // 
            // btnSelecciones
            // 
            btnSelecciones.Location = new Point(219, 95);
            btnSelecciones.Name = "btnSelecciones";
            btnSelecciones.Size = new Size(111, 32);
            btnSelecciones.TabIndex = 2;
            btnSelecciones.Text = "Ver selecciones";
            btnSelecciones.UseVisualStyleBackColor = true;
            btnSelecciones.Click += btnSelecciones_Click;
            // 
            // btnPartidos
            // 
            btnPartidos.Location = new Point(219, 186);
            btnPartidos.Name = "btnPartidos";
            btnPartidos.Size = new Size(130, 23);
            btnPartidos.TabIndex = 3;
            btnPartidos.Text = "Gestión de partidos";
            btnPartidos.UseVisualStyleBackColor = true;
            btnPartidos.Click += btnPartidos_Click;
            // 
            // btnFechaSimulada
            // 
            btnFechaSimulada.Location = new Point(375, 177);
            btnFechaSimulada.Name = "btnFechaSimulada";
            btnFechaSimulada.Size = new Size(130, 40);
            btnFechaSimulada.TabIndex = 4;
            btnFechaSimulada.Text = "Cambiar fecha simulada";
            btnFechaSimulada.UseVisualStyleBackColor = true;
            btnFechaSimulada.Click += btnFechaSimulada_Click;
            // 
            // btnPronosticos
            // 
            btnPronosticos.Location = new Point(292, 239);
            btnPronosticos.Name = "btnPronosticos";
            btnPronosticos.Size = new Size(130, 39);
            btnPronosticos.TabIndex = 5;
            btnPronosticos.Text = "Gestión de pronósticos";
            btnPronosticos.UseVisualStyleBackColor = true;
            btnPronosticos.Click += btnPronosticos_Click;
            // 
            // btnRanking
            // 
            btnRanking.Location = new Point(525, 95);
            btnRanking.Name = "btnRanking";
            btnRanking.Size = new Size(143, 36);
            btnRanking.TabIndex = 6;
            btnRanking.Text = "Ranking global";
            btnRanking.UseVisualStyleBackColor = true;
            btnRanking.Click += btnRanking_Click;
            // 
            // btnHistorialPronosticos
            // 
            btnHistorialPronosticos.Location = new Point(455, 240);
            btnHistorialPronosticos.Name = "btnHistorialPronosticos";
            btnHistorialPronosticos.Size = new Size(150, 38);
            btnHistorialPronosticos.TabIndex = 7;
            btnHistorialPronosticos.Text = "Historial de pronosticos";
            btnHistorialPronosticos.UseVisualStyleBackColor = true;
            btnHistorialPronosticos.Click += btnHistorialPronosticos_Click;
            //
            // btnQuinielas
            //
            btnQuinielas.Location = new Point(310, 316);
            btnQuinielas.Name = "btnQuinielas";
            btnQuinielas.Size = new Size(164, 38);
            btnQuinielas.TabIndex = 8;
            btnQuinielas.Text = "Quinielas privadas";
            btnQuinielas.UseVisualStyleBackColor = true;
            btnQuinielas.Click += btnQuinielas_Click;
            // 
            // FrmPrincipal
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(784, 411);
            Controls.Add(btnQuinielas);
            Controls.Add(btnHistorialPronosticos);
            Controls.Add(btnRanking);
            Controls.Add(btnPronosticos);
            Controls.Add(btnFechaSimulada);
            Controls.Add(btnPartidos);
            Controls.Add(btnSelecciones);
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
        private Button btnSelecciones;
        private Button btnPartidos;
        private Button btnFechaSimulada;
        private Button btnPronosticos;
        private Button btnRanking;
        private Button btnHistorialPronosticos;
        private Button btnQuinielas;
    }
}
