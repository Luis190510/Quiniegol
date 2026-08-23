using Quiniegol.Services;

namespace Quiniegol.Views
{
    public partial class FrmFechaSimulada : Form
    {
        private readonly FechaSimuladaService _fechaService;

        public FrmFechaSimulada()
        {
            InitializeComponent();

            _fechaService = FechaSimuladaService.Instancia;

            dtpFechaSimulada.Value = _fechaService.FechaActual;

            MostrarFechaActual();
        }

        private void MostrarFechaActual()
        {
            lblFechaActual.Text =
                $"Fecha actual: {_fechaService.FechaActual:dd/MM/yyyy HH:mm}";
        }

        private void btnAplicarFecha_Click(
            object sender,
            EventArgs e)
        {
            _fechaService.CambiarFecha(
                dtpFechaSimulada.Value
            );

            new InsigniaService().RecalcularInsignias();

            MostrarFechaActual();

            MessageBox.Show(
                "La fecha simulada fue actualizada.",
                "Fecha actualizada",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        private void btnCerrar_Click(
            object sender,
            EventArgs e)
        {
            Close();
        }
    }
}
