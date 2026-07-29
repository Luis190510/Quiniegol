namespace Quiniegol.Views
{
    partial class FrmRanking
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
            dgvRanking = new DataGridView();
            btnActualizarRanking = new Button();
            btnCerrar = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvRanking).BeginInit();
            SuspendLayout();
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Location = new Point(344, 9);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(86, 15);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Ranking global";
            // 
            // dgvRanking
            // 
            dgvRanking.AllowUserToAddRows = false;
            dgvRanking.AllowUserToDeleteRows = false;
            dgvRanking.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvRanking.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvRanking.Location = new Point(23, 54);
            dgvRanking.MultiSelect = false;
            dgvRanking.Name = "dgvRanking";
            dgvRanking.ReadOnly = true;
            dgvRanking.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvRanking.Size = new Size(765, 237);
            dgvRanking.TabIndex = 1;
            // 
            // btnActualizarRanking
            // 
            btnActualizarRanking.Location = new Point(176, 361);
            btnActualizarRanking.Name = "btnActualizarRanking";
            btnActualizarRanking.Size = new Size(158, 23);
            btnActualizarRanking.TabIndex = 2;
            btnActualizarRanking.Text = "Actualizar ranking";
            btnActualizarRanking.UseVisualStyleBackColor = true;
            btnActualizarRanking.Click += btnActualizarRanking_Click;
            // 
            // btnCerrar
            // 
            btnCerrar.Location = new Point(509, 361);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(75, 23);
            btnCerrar.TabIndex = 3;
            btnCerrar.Text = "Cerrar";
            btnCerrar.UseVisualStyleBackColor = true;
            btnCerrar.Click += btnCerrar_Click;
            // 
            // FrmRanking
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnCerrar);
            Controls.Add(btnActualizarRanking);
            Controls.Add(dgvRanking);
            Controls.Add(lblTitulo);
            Name = "FrmRanking";
            Text = "FrmRanking";
            ((System.ComponentModel.ISupportInitialize)dgvRanking).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitulo;
        private DataGridView dgvRanking;
        private Button btnActualizarRanking;
        private Button btnCerrar;
    }
}