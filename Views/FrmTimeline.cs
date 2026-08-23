using Quiniegol.Controllers;
using Quiniegol.Models;
using Quiniegol.Services;

namespace Quiniegol.Views
{
    /// <summary>Muestra la actividad privada de una quiniela accesible.</summary>
    public partial class FrmTimeline : Form
    {
        private readonly QuinielaController _quinielaController;
        private readonly TimelineService _timelineService;

        public FrmTimeline()
        {
            InitializeComponent();
            _quinielaController = new QuinielaController();
            _timelineService = new TimelineService();
            CargarQuinielas();
        }

        private void CargarQuinielas()
        {
            cmbQuiniela.DataSource =
                _quinielaController.ObtenerQuinielas();
            cmbQuiniela.DisplayMember = "Nombre";
            cmbQuiniela.ValueMember = "Id";
        }

        private void btnConsultar_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbQuiniela.SelectedItem is not Quiniela quiniela)
                {
                    throw new InvalidOperationException(
                        "Debe seleccionar una quiniela."
                    );
                }

                dgvActividad.DataSource = _timelineService
                    .ObtenerPorQuiniela(quiniela.Id)
                    .Select(notificacion => new
                    {
                        notificacion.Fecha,
                        Actividad = notificacion.Mensaje
                    })
                    .ToList();

                dgvActividad.Columns["Fecha"]!.HeaderText = "Fecha";
                dgvActividad.Columns["Actividad"]!.AutoSizeMode =
                    DataGridViewAutoSizeColumnMode.Fill;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "No se pudo cargar la actividad",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
