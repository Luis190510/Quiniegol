namespace Quiniegol.Views
{
    partial class FrmPrincipal
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
            lblSesion = new Label();
            lblFechaSimulada = new Label();
            btnCerrarSesion = new Button();
            grpAdministracion = new GroupBox();
            flpAdministracion = new FlowLayoutPanel();
            btnUsuarios = new Button();
            btnPartidos = new Button();
            btnFechaSimulada = new Button();
            grpParticipacion = new GroupBox();
            flpParticipacion = new FlowLayoutPanel();
            btnPronosticos = new Button();
            btnHistorialPronosticos = new Button();
            btnRanking = new Button();
            btnQuinielas = new Button();
            btnRankingPrivado = new Button();
            btnTimeline = new Button();
            grpInsignias = new GroupBox();
            txtInsignias = new TextBox();
            grpTorneo = new GroupBox();
            flpTorneo = new FlowLayoutPanel();
            btnSelecciones = new Button();
            btnInformacionPartidos = new Button();
            btnDetallePartido = new Button();
            btnEstadisticas = new Button();
            btnTablaGrupos = new Button();
            btnFaseFinal = new Button();
            grpAdministracion.SuspendLayout();
            flpAdministracion.SuspendLayout();
            grpParticipacion.SuspendLayout();
            flpParticipacion.SuspendLayout();
            grpInsignias.SuspendLayout();
            grpTorneo.SuspendLayout();
            flpTorneo.SuspendLayout();
            SuspendLayout();
            //
            // lblTitulo
            //
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTitulo.Location = new Point(24, 18);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(214, 32);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Sistema Quinegol";
            //
            // lblSesion
            //
            lblSesion.AutoSize = true;
            lblSesion.ForeColor = Color.DimGray;
            lblSesion.Location = new Point(26, 58);
            lblSesion.Name = "lblSesion";
            lblSesion.Size = new Size(50, 15);
            lblSesion.TabIndex = 1;
            lblSesion.Text = "Usuario:";
            //
            // lblFechaSimulada
            //
            lblFechaSimulada.AutoSize = true;
            lblFechaSimulada.ForeColor = Color.DimGray;
            lblFechaSimulada.Location = new Point(350, 58);
            lblFechaSimulada.Name = "lblFechaSimulada";
            lblFechaSimulada.Size = new Size(92, 15);
            lblFechaSimulada.TabIndex = 2;
            lblFechaSimulada.Text = "Fecha simulada:";
            //
            // btnCerrarSesion
            //
            btnCerrarSesion.Location = new Point(756, 26);
            btnCerrarSesion.Name = "btnCerrarSesion";
            btnCerrarSesion.Size = new Size(140, 34);
            btnCerrarSesion.TabIndex = 2;
            btnCerrarSesion.Text = "Cerrar sesión";
            btnCerrarSesion.UseVisualStyleBackColor = true;
            btnCerrarSesion.Click += btnCerrarSesion_Click;
            //
            // grpAdministracion
            //
            grpAdministracion.Controls.Add(flpAdministracion);
            grpAdministracion.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            grpAdministracion.Location = new Point(24, 92);
            grpAdministracion.Name = "grpAdministracion";
            grpAdministracion.Size = new Size(872, 82);
            grpAdministracion.TabIndex = 3;
            grpAdministracion.TabStop = false;
            grpAdministracion.Text = "Administración";
            //
            // flpAdministracion
            //
            flpAdministracion.Controls.Add(btnUsuarios);
            flpAdministracion.Controls.Add(btnPartidos);
            flpAdministracion.Dock = DockStyle.Fill;
            flpAdministracion.Location = new Point(3, 19);
            flpAdministracion.Name = "flpAdministracion";
            flpAdministracion.Padding = new Padding(8, 4, 8, 4);
            flpAdministracion.Size = new Size(866, 60);
            flpAdministracion.TabIndex = 0;
            //
            // btnUsuarios
            //
            btnUsuarios.Font = new Font("Segoe UI", 9F);
            btnUsuarios.Location = new Point(15, 11);
            btnUsuarios.Margin = new Padding(7);
            btnUsuarios.Name = "btnUsuarios";
            btnUsuarios.Size = new Size(190, 40);
            btnUsuarios.TabIndex = 0;
            btnUsuarios.Text = "Gestión de usuarios";
            btnUsuarios.UseVisualStyleBackColor = true;
            btnUsuarios.Click += btnUsuarios_Click;
            //
            // btnPartidos
            //
            btnPartidos.Font = new Font("Segoe UI", 9F);
            btnPartidos.Location = new Point(219, 11);
            btnPartidos.Margin = new Padding(7);
            btnPartidos.Name = "btnPartidos";
            btnPartidos.Size = new Size(190, 40);
            btnPartidos.TabIndex = 1;
            btnPartidos.Text = "Gestión de partidos";
            btnPartidos.UseVisualStyleBackColor = true;
            btnPartidos.Click += btnPartidos_Click;
            //
            // btnFechaSimulada
            //
            btnFechaSimulada.Font = new Font("Segoe UI", 9F);
            btnFechaSimulada.Location = new Point(15, 11);
            btnFechaSimulada.Margin = new Padding(7);
            btnFechaSimulada.Name = "btnFechaSimulada";
            btnFechaSimulada.Size = new Size(190, 40);
            btnFechaSimulada.TabIndex = 2;
            btnFechaSimulada.Text = "Ajustar fecha (pasado/futuro)";
            btnFechaSimulada.UseVisualStyleBackColor = true;
            btnFechaSimulada.Click += btnFechaSimulada_Click;
            //
            // grpParticipacion
            //
            grpParticipacion.Controls.Add(flpParticipacion);
            grpParticipacion.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            grpParticipacion.Location = new Point(24, 188);
            grpParticipacion.Name = "grpParticipacion";
            grpParticipacion.Size = new Size(872, 132);
            grpParticipacion.TabIndex = 4;
            grpParticipacion.TabStop = false;
            grpParticipacion.Text = "Participación y fecha simulada";
            //
            // flpParticipacion
            //
            flpParticipacion.Controls.Add(btnFechaSimulada);
            flpParticipacion.Controls.Add(btnPronosticos);
            flpParticipacion.Controls.Add(btnHistorialPronosticos);
            flpParticipacion.Controls.Add(btnRanking);
            flpParticipacion.Controls.Add(btnQuinielas);
            flpParticipacion.Controls.Add(btnRankingPrivado);
            flpParticipacion.Controls.Add(btnTimeline);
            flpParticipacion.Dock = DockStyle.Fill;
            flpParticipacion.Location = new Point(3, 19);
            flpParticipacion.Name = "flpParticipacion";
            flpParticipacion.Padding = new Padding(8, 4, 8, 4);
            flpParticipacion.Size = new Size(866, 110);
            flpParticipacion.TabIndex = 0;
            //
            // btnPronosticos
            //
            btnPronosticos.Font = new Font("Segoe UI", 9F);
            btnPronosticos.Location = new Point(219, 11);
            btnPronosticos.Margin = new Padding(7);
            btnPronosticos.Name = "btnPronosticos";
            btnPronosticos.Size = new Size(190, 40);
            btnPronosticos.TabIndex = 0;
            btnPronosticos.Text = "Registrar pronóstico";
            btnPronosticos.UseVisualStyleBackColor = true;
            btnPronosticos.Click += btnPronosticos_Click;
            //
            // btnHistorialPronosticos
            //
            btnHistorialPronosticos.Font = new Font("Segoe UI", 9F);
            btnHistorialPronosticos.Location = new Point(423, 11);
            btnHistorialPronosticos.Margin = new Padding(7);
            btnHistorialPronosticos.Name = "btnHistorialPronosticos";
            btnHistorialPronosticos.Size = new Size(190, 40);
            btnHistorialPronosticos.TabIndex = 1;
            btnHistorialPronosticos.Text = "Historial de pronósticos";
            btnHistorialPronosticos.UseVisualStyleBackColor = true;
            btnHistorialPronosticos.Click += btnHistorialPronosticos_Click;
            //
            // btnRanking
            //
            btnRanking.Font = new Font("Segoe UI", 9F);
            btnRanking.Location = new Point(627, 11);
            btnRanking.Margin = new Padding(7);
            btnRanking.Name = "btnRanking";
            btnRanking.Size = new Size(190, 40);
            btnRanking.TabIndex = 2;
            btnRanking.Text = "Ranking global";
            btnRanking.UseVisualStyleBackColor = true;
            btnRanking.Click += btnRanking_Click;
            //
            // btnQuinielas
            //
            btnQuinielas.Font = new Font("Segoe UI", 9F);
            btnQuinielas.Location = new Point(15, 65);
            btnQuinielas.Margin = new Padding(7);
            btnQuinielas.Name = "btnQuinielas";
            btnQuinielas.Size = new Size(190, 40);
            btnQuinielas.TabIndex = 3;
            btnQuinielas.Text = "Quinielas privadas";
            btnQuinielas.UseVisualStyleBackColor = true;
            btnQuinielas.Click += btnQuinielas_Click;
            //
            // btnRankingPrivado
            //
            btnRankingPrivado.Font = new Font("Segoe UI", 9F);
            btnRankingPrivado.Location = new Point(219, 65);
            btnRankingPrivado.Margin = new Padding(7);
            btnRankingPrivado.Name = "btnRankingPrivado";
            btnRankingPrivado.Size = new Size(190, 40);
            btnRankingPrivado.TabIndex = 4;
            btnRankingPrivado.Text = "Ranking privado";
            btnRankingPrivado.UseVisualStyleBackColor = true;
            btnRankingPrivado.Click += btnRankingPrivado_Click;
            //
            // btnTimeline
            //
            btnTimeline.Font = new Font("Segoe UI", 9F);
            btnTimeline.Location = new Point(423, 65);
            btnTimeline.Margin = new Padding(7);
            btnTimeline.Name = "btnTimeline";
            btnTimeline.Size = new Size(190, 40);
            btnTimeline.TabIndex = 5;
            btnTimeline.Text = "Actividad de quiniela";
            btnTimeline.UseVisualStyleBackColor = true;
            btnTimeline.Click += btnTimeline_Click;
            //
            // grpInsignias
            //
            grpInsignias.Controls.Add(txtInsignias);
            grpInsignias.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            grpInsignias.Location = new Point(24, 334);
            grpInsignias.Name = "grpInsignias";
            grpInsignias.Size = new Size(872, 110);
            grpInsignias.TabIndex = 5;
            grpInsignias.TabStop = false;
            grpInsignias.Text = "Mis insignias";
            //
            // txtInsignias
            //
            txtInsignias.BackColor = SystemColors.Control;
            txtInsignias.BorderStyle = BorderStyle.None;
            txtInsignias.Font = new Font("Segoe UI", 9F);
            txtInsignias.Location = new Point(16, 25);
            txtInsignias.Multiline = true;
            txtInsignias.Name = "txtInsignias";
            txtInsignias.ReadOnly = true;
            txtInsignias.ScrollBars = ScrollBars.Vertical;
            txtInsignias.Size = new Size(840, 70);
            txtInsignias.TabIndex = 0;
            //
            // grpTorneo
            //
            grpTorneo.Controls.Add(flpTorneo);
            grpTorneo.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            grpTorneo.Location = new Point(24, 458);
            grpTorneo.Name = "grpTorneo";
            grpTorneo.Size = new Size(872, 132);
            grpTorneo.TabIndex = 5;
            grpTorneo.TabStop = false;
            grpTorneo.Text = "Información del Mundial 2026";
            //
            // flpTorneo
            //
            flpTorneo.Controls.Add(btnSelecciones);
            flpTorneo.Controls.Add(btnInformacionPartidos);
            flpTorneo.Controls.Add(btnDetallePartido);
            flpTorneo.Controls.Add(btnEstadisticas);
            flpTorneo.Controls.Add(btnTablaGrupos);
            flpTorneo.Controls.Add(btnFaseFinal);
            flpTorneo.Dock = DockStyle.Fill;
            flpTorneo.Location = new Point(3, 19);
            flpTorneo.Name = "flpTorneo";
            flpTorneo.Padding = new Padding(8, 4, 8, 4);
            flpTorneo.Size = new Size(866, 110);
            flpTorneo.TabIndex = 0;
            //
            // btnSelecciones
            //
            btnSelecciones.Font = new Font("Segoe UI", 9F);
            btnSelecciones.Location = new Point(15, 11);
            btnSelecciones.Margin = new Padding(7);
            btnSelecciones.Name = "btnSelecciones";
            btnSelecciones.Size = new Size(190, 40);
            btnSelecciones.TabIndex = 0;
            btnSelecciones.Text = "Ver selecciones";
            btnSelecciones.UseVisualStyleBackColor = true;
            btnSelecciones.Click += btnSelecciones_Click;
            //
            // btnInformacionPartidos
            //
            btnInformacionPartidos.Font = new Font("Segoe UI", 9F);
            btnInformacionPartidos.Location = new Point(219, 11);
            btnInformacionPartidos.Margin = new Padding(7);
            btnInformacionPartidos.Name = "btnInformacionPartidos";
            btnInformacionPartidos.Size = new Size(190, 40);
            btnInformacionPartidos.TabIndex = 1;
            btnInformacionPartidos.Text = "Información de partidos";
            btnInformacionPartidos.UseVisualStyleBackColor = true;
            btnInformacionPartidos.Click += btnInformacionPartidos_Click;
            //
            // btnDetallePartido
            //
            btnDetallePartido.Font = new Font("Segoe UI", 9F);
            btnDetallePartido.Location = new Point(423, 11);
            btnDetallePartido.Margin = new Padding(7);
            btnDetallePartido.Name = "btnDetallePartido";
            btnDetallePartido.Size = new Size(190, 40);
            btnDetallePartido.TabIndex = 2;
            btnDetallePartido.Text = "Detalle de partidos";
            btnDetallePartido.UseVisualStyleBackColor = true;
            btnDetallePartido.Click += btnDetallePartido_Click;
            //
            // btnEstadisticas
            //
            btnEstadisticas.Font = new Font("Segoe UI", 9F);
            btnEstadisticas.Location = new Point(627, 11);
            btnEstadisticas.Margin = new Padding(7);
            btnEstadisticas.Name = "btnEstadisticas";
            btnEstadisticas.Size = new Size(190, 40);
            btnEstadisticas.TabIndex = 3;
            btnEstadisticas.Text = "Reportes";
            btnEstadisticas.UseVisualStyleBackColor = true;
            btnEstadisticas.Click += btnEstadisticas_Click;
            //
            // btnTablaGrupos
            //
            btnTablaGrupos.Font = new Font("Segoe UI", 9F);
            btnTablaGrupos.Location = new Point(15, 65);
            btnTablaGrupos.Margin = new Padding(7);
            btnTablaGrupos.Name = "btnTablaGrupos";
            btnTablaGrupos.Size = new Size(190, 40);
            btnTablaGrupos.TabIndex = 4;
            btnTablaGrupos.Text = "Tabla de grupos";
            btnTablaGrupos.UseVisualStyleBackColor = true;
            btnTablaGrupos.Click += btnTablaGrupos_Click;
            //
            // btnFaseFinal
            //
            btnFaseFinal.Font = new Font("Segoe UI", 9F);
            btnFaseFinal.Location = new Point(219, 65);
            btnFaseFinal.Margin = new Padding(7);
            btnFaseFinal.Name = "btnFaseFinal";
            btnFaseFinal.Size = new Size(190, 40);
            btnFaseFinal.TabIndex = 5;
            btnFaseFinal.Text = "Cruces de fase final";
            btnFaseFinal.UseVisualStyleBackColor = true;
            btnFaseFinal.Click += btnFaseFinal_Click;
            //
            // FrmPrincipal
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(920, 616);
            Controls.Add(grpTorneo);
            Controls.Add(grpInsignias);
            Controls.Add(grpParticipacion);
            Controls.Add(grpAdministracion);
            Controls.Add(btnCerrarSesion);
            Controls.Add(lblFechaSimulada);
            Controls.Add(lblSesion);
            Controls.Add(lblTitulo);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "FrmPrincipal";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Quinegol - Menú principal";
            grpAdministracion.ResumeLayout(false);
            flpAdministracion.ResumeLayout(false);
            grpParticipacion.ResumeLayout(false);
            flpParticipacion.ResumeLayout(false);
            grpInsignias.ResumeLayout(false);
            grpInsignias.PerformLayout();
            grpTorneo.ResumeLayout(false);
            flpTorneo.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        private Label lblTitulo = null!;
        private Label lblSesion = null!;
        private Label lblFechaSimulada = null!;
        private Button btnCerrarSesion = null!;
        private GroupBox grpAdministracion = null!;
        private FlowLayoutPanel flpAdministracion = null!;
        private Button btnUsuarios = null!;
        private Button btnPartidos = null!;
        private Button btnFechaSimulada = null!;
        private GroupBox grpParticipacion = null!;
        private FlowLayoutPanel flpParticipacion = null!;
        private Button btnPronosticos = null!;
        private Button btnHistorialPronosticos = null!;
        private Button btnRanking = null!;
        private Button btnQuinielas = null!;
        private Button btnRankingPrivado = null!;
        private Button btnTimeline = null!;
        private GroupBox grpInsignias = null!;
        private TextBox txtInsignias = null!;
        private GroupBox grpTorneo = null!;
        private FlowLayoutPanel flpTorneo = null!;
        private Button btnSelecciones = null!;
        private Button btnInformacionPartidos = null!;
        private Button btnDetallePartido = null!;
        private Button btnEstadisticas = null!;
        private Button btnTablaGrupos = null!;
        private Button btnFaseFinal = null!;
    }
}
