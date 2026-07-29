namespace Quiniegol.Views
{
    partial class FrmInformacionPartidos
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
            lblFechaSimulada = new Label();
            tabPartidos = new TabControl();
            tabUltimos = new TabPage();
            dgvUltimos = new DataGridView();
            tabProximos = new TabPage();
            dgvProximos = new DataGridView();
            btnActualizar = new Button();
            btnCerrar = new Button();
            tabPartidos.SuspendLayout();
            tabUltimos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvUltimos).BeginInit();
            tabProximos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvProximos).BeginInit();
            SuspendLayout();
            //
            // lblFechaSimulada
            //
            lblFechaSimulada.AutoSize = true;
            lblFechaSimulada.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblFechaSimulada.Location = new Point(20, 18);
            lblFechaSimulada.Name = "lblFechaSimulada";
            lblFechaSimulada.Size = new Size(112, 19);
            lblFechaSimulada.TabIndex = 0;
            lblFechaSimulada.Text = "Fecha simulada:";
            //
            // tabPartidos
            //
            tabPartidos.Controls.Add(tabUltimos);
            tabPartidos.Controls.Add(tabProximos);
            tabPartidos.Location = new Point(20, 52);
            tabPartidos.Name = "tabPartidos";
            tabPartidos.SelectedIndex = 0;
            tabPartidos.Size = new Size(810, 400);
            tabPartidos.TabIndex = 1;
            //
            // tabUltimos
            //
            tabUltimos.Controls.Add(dgvUltimos);
            tabUltimos.Location = new Point(4, 24);
            tabUltimos.Name = "tabUltimos";
            tabUltimos.Padding = new Padding(3);
            tabUltimos.Size = new Size(802, 372);
            tabUltimos.TabIndex = 0;
            tabUltimos.Text = "Últimos 5 partidos";
            tabUltimos.UseVisualStyleBackColor = true;
            //
            // dgvUltimos
            //
            dgvUltimos.AllowUserToAddRows = false;
            dgvUltimos.AllowUserToDeleteRows = false;
            dgvUltimos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvUltimos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvUltimos.Dock = DockStyle.Fill;
            dgvUltimos.Location = new Point(3, 3);
            dgvUltimos.MultiSelect = false;
            dgvUltimos.Name = "dgvUltimos";
            dgvUltimos.ReadOnly = true;
            dgvUltimos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUltimos.Size = new Size(796, 366);
            dgvUltimos.TabIndex = 0;
            //
            // tabProximos
            //
            tabProximos.Controls.Add(dgvProximos);
            tabProximos.Location = new Point(4, 24);
            tabProximos.Name = "tabProximos";
            tabProximos.Padding = new Padding(3);
            tabProximos.Size = new Size(802, 372);
            tabProximos.TabIndex = 1;
            tabProximos.Text = "Próximas 24 horas";
            tabProximos.UseVisualStyleBackColor = true;
            //
            // dgvProximos
            //
            dgvProximos.AllowUserToAddRows = false;
            dgvProximos.AllowUserToDeleteRows = false;
            dgvProximos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvProximos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvProximos.Dock = DockStyle.Fill;
            dgvProximos.Location = new Point(3, 3);
            dgvProximos.MultiSelect = false;
            dgvProximos.Name = "dgvProximos";
            dgvProximos.ReadOnly = true;
            dgvProximos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProximos.Size = new Size(796, 366);
            dgvProximos.TabIndex = 0;
            //
            // btnActualizar
            //
            btnActualizar.Location = new Point(20, 470);
            btnActualizar.Name = "btnActualizar";
            btnActualizar.Size = new Size(154, 30);
            btnActualizar.TabIndex = 2;
            btnActualizar.Text = "Actualizar";
            btnActualizar.UseVisualStyleBackColor = true;
            btnActualizar.Click += btnActualizar_Click;
            //
            // btnCerrar
            //
            btnCerrar.Location = new Point(676, 470);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(154, 30);
            btnCerrar.TabIndex = 3;
            btnCerrar.Text = "Cerrar";
            btnCerrar.UseVisualStyleBackColor = true;
            btnCerrar.Click += btnCerrar_Click;
            //
            // FrmInformacionPartidos
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCerrar;
            ClientSize = new Size(850, 518);
            Controls.Add(btnCerrar);
            Controls.Add(btnActualizar);
            Controls.Add(tabPartidos);
            Controls.Add(lblFechaSimulada);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmInformacionPartidos";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Información de partidos";
            tabPartidos.ResumeLayout(false);
            tabUltimos.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvUltimos).EndInit();
            tabProximos.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvProximos).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblFechaSimulada;
        private TabControl tabPartidos;
        private TabPage tabUltimos;
        private DataGridView dgvUltimos;
        private TabPage tabProximos;
        private DataGridView dgvProximos;
        private Button btnActualizar;
        private Button btnCerrar;
    }
}
