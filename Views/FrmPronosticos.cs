using Quiniegol.Controllers;
using Quiniegol.Models;
using Quiniegol.Services;

namespace Quiniegol.Views
{
    /// <summary>
    /// Pantalla para registrar y consultar los pronósticos del participante.
    /// </summary>
    public partial class FrmPronosticos : Form
    {
        private readonly PronosticoController _pronosticoController;
        private readonly UsuarioController _usuarioController;
        private readonly PartidoController _partidoController;
        private readonly SeleccionController _seleccionController;

        private Dictionary<int, Partido> _partidosPendientesPorId = new();
        private Dictionary<int, string> _seleccionesPorId = new();

        public FrmPronosticos()
        {
            InitializeComponent();
            _pronosticoController = new PronosticoController();
            _usuarioController = new UsuarioController();
            _partidoController = new PartidoController();
            _seleccionController = new SeleccionController();
            CargarDatos();
        }

        private void CargarDatos()
        {
            CargarUsuarios();
            CargarPartidos();
            CargarPronosticos();
        }

        private void CargarUsuarios()
        {
            cmbUsuario.DataSource = new[] { SesionUsuarioService.UsuarioActual };
            cmbUsuario.DisplayMember = nameof(Usuario.Nombre);
            cmbUsuario.ValueMember = nameof(Usuario.Id);
            cmbUsuario.SelectedIndex = 0;
            cmbUsuario.Enabled = false;
        }

        private void CargarPartidos()
        {
            _seleccionesPorId = _seleccionController.ObtenerSelecciones()
                .ToDictionary(seleccion => seleccion.Id, seleccion => seleccion.Nombre);
            _partidosPendientesPorId = _partidoController.ObtenerPartidosPendientes()
                .ToDictionary(partido => partido.Id);

            var opciones = _partidosPendientesPorId.Values
                .OrderBy(partido => partido.FechaHora)
                .Select(partido => new
                {
                    partido.Id,
                    Descripcion = $"{ObtenerNombreSeleccion(partido.SeleccionLocalId)} vs " +
                        $"{ObtenerNombreSeleccion(partido.SeleccionVisitanteId)} - " +
                        $"{partido.FechaHora:dd/MM/yyyy HH:mm}"
                })
                .ToList();

            cmbPartido.DataSource = opciones;
            cmbPartido.DisplayMember = "Descripcion";
            cmbPartido.ValueMember = nameof(Partido.Id);
            cmbPartido.SelectedIndex = -1;
        }

        private void CargarPronosticos()
        {
            Dictionary<int, string> usuariosPorId = _usuarioController.ObtenerUsuarios()
                .ToDictionary(usuario => usuario.Id, usuario => usuario.Nombre);
            Dictionary<int, Partido> partidosPorId = _partidoController.ObtenerPartidos()
                .ToDictionary(partido => partido.Id);

            var filas = _pronosticoController.ObtenerPronosticos()
                .Select(pronostico => CrearFilaPronostico(
                    pronostico,
                    usuariosPorId,
                    partidosPorId))
                .ToList();

            dgvPronosticos.DataSource = filas;
        }

        private object CrearFilaPronostico(
            Pronostico pronostico,
            IReadOnlyDictionary<int, string> usuarios,
            IReadOnlyDictionary<int, Partido> partidos)
        {
            usuarios.TryGetValue(pronostico.UsuarioId, out string? usuario);
            partidos.TryGetValue(pronostico.PartidoId, out Partido? partido);

            string local = partido == null
                ? "Local"
                : ObtenerNombreSeleccion(partido.SeleccionLocalId);
            string visitante = partido == null
                ? "Visitante"
                : ObtenerNombreSeleccion(partido.SeleccionVisitanteId);
            string descripcion = partido == null
                ? "Partido no encontrado"
                : $"{local} vs {visitante}";

            return new
            {
                pronostico.Id,
                Usuario = usuario ?? "Usuario no encontrado",
                Partido = descripcion,
                Marcador =
                    $"{pronostico.GolesLocalPronosticados} - " +
                    $"{pronostico.GolesVisitantePronosticados}",
                Goleadores = GoleadoresPronosticoService.Formatear(
                    pronostico,
                    local,
                    visitante),
                Fecha = pronostico.FechaRegistro,
                Puntos = pronostico.PuntosObtenidos
            };
        }

        private string ObtenerNombreSeleccion(int seleccionId)
        {
            return _seleccionesPorId.GetValueOrDefault(seleccionId, "Desconocida");
        }

        private void btnRegistrarPronostico_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbUsuario.SelectedValue == null)
                {
                    throw new InvalidOperationException("Debe seleccionar un usuario.");
                }

                if (cmbPartido.SelectedValue == null)
                {
                    throw new InvalidOperationException("Debe seleccionar un partido.");
                }

                _pronosticoController.RegistrarPronostico(
                    Convert.ToInt32(cmbUsuario.SelectedValue),
                    Convert.ToInt32(cmbPartido.SelectedValue),
                    Convert.ToInt32(nudGolesLocal.Value),
                    Convert.ToInt32(nudGolesVisitante.Value),
                    SepararGoleadores(txtGoleadoresLocal.Text),
                    SepararGoleadores(txtGoleadoresVisitante.Text));

                MessageBox.Show(
                    "Pronóstico registrado correctamente.",
                    "Registro exitoso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                LimpiarFormulario();
                CargarDatos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "No fue posible registrar el pronóstico",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        private void LimpiarFormulario()
        {
            nudGolesLocal.Value = 0;
            nudGolesVisitante.Value = 0;
            txtGoleadoresLocal.Clear();
            txtGoleadoresVisitante.Clear();
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            CargarDatos();
        }

        private static IEnumerable<string> SepararGoleadores(string texto)
        {
            return (texto ?? string.Empty).Split(
                new[] { '\r', '\n', ',', ';' },
                StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        }

        private void cmbPartido_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbPartido.SelectedValue == null ||
                !int.TryParse(cmbPartido.SelectedValue.ToString(), out int partidoId) ||
                !_partidosPendientesPorId.TryGetValue(partidoId, out Partido? partido))
            {
                lblGoleadoresLocal.Text = "Posibles goleadores del equipo local:";
                lblGoleadoresVisitante.Text = "Posibles goleadores del equipo visitante:";
                return;
            }

            lblGoleadoresLocal.Text =
                $"Posibles goleadores de {ObtenerNombreSeleccion(partido.SeleccionLocalId)}:";
            lblGoleadoresVisitante.Text =
                $"Posibles goleadores de {ObtenerNombreSeleccion(partido.SeleccionVisitanteId)}:";
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
