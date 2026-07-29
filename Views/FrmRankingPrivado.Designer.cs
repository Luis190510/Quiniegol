namespace Quiniegol.Views
{
    partial class FrmRankingPrivado
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
            lblQuiniela = new Label();
            cmbQuiniela = new ComboBox();
            btnConsultar = new Button();
            dgvRanking = new DataGridView();
            btnCerrar = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvRanking).BeginInit();
            SuspendLayout();
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblTitulo.Location = new Point(315, 18);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(142, 21);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Ranking privado";
            // 
            // lblQuiniela
            // 
            lblQuiniela.AutoSize = true;
            lblQuiniela.Location = new Point(24, 70);
            lblQuiniela.Name = "lblQuiniela";
            lblQuiniela.Size = new Size(55, 15);
            lblQuiniela.TabIndex = 1;
            lblQuiniela.Text = "Quiniela:";
            // 
            // cmbQuiniela
            // 
            cmbQuiniela.FormattingEnabled = true;
            cmbQuiniela.Location = new Point(85, 67);
            cmbQuiniela.Name = "cmbQuiniela";
            cmbQuiniela.Size = new Size(478, 23);
            cmbQuiniela.TabIndex = 2;
            // 
            // btnConsultar
            // 
            btnConsultar.Location = new Point(582, 65);
            btnConsultar.Name = "btnConsultar";
            btnConsultar.Size = new Size(182, 27);
            btnConsultar.TabIndex = 3;
            btnConsultar.Text = "Consultar ranking";
            btnConsultar.UseVisualStyleBackColor = true;
            btnConsultar.Click += btnConsultar_Click;
            // 
            // dgvRanking
            // 
            dgvRanking.AllowUserToAddRows = false;
            dgvRanking.AllowUserToDeleteRows = false;
            dgvRanking.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvRanking.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvRanking.Location = new Point(24, 112);
            dgvRanking.MultiSelect = false;
            dgvRanking.Name = "dgvRanking";
            dgvRanking.ReadOnly = true;
            dgvRanking.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvRanking.Size = new Size(740, 278);
            dgvRanking.TabIndex = 4;
            // 
            // btnCerrar
            // 
            btnCerrar.Location = new Point(621, 410);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(143, 28);
            btnCerrar.TabIndex = 5;
            btnCerrar.Text = "Cerrar";
            btnCerrar.UseVisualStyleBackColor = true;
            btnCerrar.Click += btnCerrar_Click;
            // 
            // FrmRankingPrivado
            // 
            AcceptButton = btnConsultar;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCerrar;
            ClientSize = new Size(788, 455);
            Controls.Add(btnCerrar);
            Controls.Add(dgvRanking);
            Controls.Add(btnConsultar);
            Controls.Add(cmbQuiniela);
            Controls.Add(lblQuiniela);
            Controls.Add(lblTitulo);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmRankingPrivado";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Ranking privado";
            ((System.ComponentModel.ISupportInitialize)dgvRanking).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitulo;
        private Label lblQuiniela;
        private ComboBox cmbQuiniela;
        private Button btnConsultar;
        private DataGridView dgvRanking;
        private Button btnCerrar;
    }
}
