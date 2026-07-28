using Quiniegol.Controllers;

namespace Quiniegol.Views
{
    public partial class FrmPronosticos : Form
    {
        private readonly PronosticoController
            _pronosticoController;

        private readonly UsuarioController
            _usuarioController;

        private readonly PartidoController
            _partidoController;

        private readonly SeleccionController
            _seleccionController;

        public FrmPronosticos()
        {
            InitializeComponent();

            _pronosticoController =
                new PronosticoController();

            _usuarioController =
                new UsuarioController();

            _partidoController =
                new PartidoController();

            _seleccionController =
                new SeleccionController();

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
            var usuarios =
                _usuarioController.ObtenerUsuarios();

            cmbUsuario.DataSource = null;
            cmbUsuario.DataSource = usuarios;
            cmbUsuario.DisplayMember = "Nombre";
            cmbUsuario.ValueMember = "Id";
            cmbUsuario.SelectedIndex = -1;
        }

        private void CargarPartidos()
        {
            var selecciones =
                _seleccionController
                    .ObtenerSelecciones();

            var partidos =
                _partidoController
                    .ObtenerPartidosPendientes();

            var opciones = partidos.Select(
                partido => new
                {
                    partido.Id,

                    Descripcion =
                        $"{ObtenerNombreSeleccion(
                            partido.SeleccionLocalId,
                            selecciones)} vs " +
                        $"{ObtenerNombreSeleccion(
                            partido.SeleccionVisitanteId,
                            selecciones)} - " +
                        $"{partido.FechaHora:dd/MM/yyyy HH:mm}"
                }
            ).ToList();

            cmbPartido.DataSource = null;
            cmbPartido.DataSource = opciones;
            cmbPartido.DisplayMember = "Descripcion";
            cmbPartido.ValueMember = "Id";
            cmbPartido.SelectedIndex = -1;
        }

        private string ObtenerNombreSeleccion(
            int seleccionId,
            IEnumerable<Models.Seleccion> selecciones)
        {
            return selecciones
                .FirstOrDefault(
                    seleccion =>
                        seleccion.Id == seleccionId
                )?.Nombre ?? "Desconocida";
        }

        private void CargarPronosticos()
        {
            var pronosticos =
                _pronosticoController
                    .ObtenerPronosticos();

            var usuarios =
                _usuarioController.ObtenerUsuarios();

            var partidos =
                _partidoController.ObtenerPartidos();

            var selecciones =
                _seleccionController
                    .ObtenerSelecciones();

            var filas = pronosticos.Select(
                pronostico =>
                {
                    var usuario =
                        usuarios.FirstOrDefault(
                            usuarioActual =>
                                usuarioActual.Id ==
                                pronostico.UsuarioId
                        );

                    var partido =
                        partidos.FirstOrDefault(
                            partidoActual =>
                                partidoActual.Id ==
                                pronostico.PartidoId
                        );

                    string descripcionPartido =
                        partido == null
                            ? "Partido no encontrado"
                            : $"{ObtenerNombreSeleccion(
                                partido.SeleccionLocalId,
                                selecciones)} vs " +
                              $"{ObtenerNombreSeleccion(
                                partido.SeleccionVisitanteId,
                                selecciones)}";

                    return new
                    {
                        pronostico.Id,

                        Usuario =
                            usuario?.Nombre ??
                            "Usuario no encontrado",

                        Partido =
                            descripcionPartido,

                        Marcador =
                            $"{pronostico.GolesLocalPronosticados}" +
                            $" - " +
                            $"{pronostico.GolesVisitantePronosticados}",

                        Fecha =
                            pronostico.FechaRegistro,

                        Puntos =
                            pronostico.PuntosObtenidos
                    };
                }
            ).ToList();

            dgvPronosticos.DataSource = null;
            dgvPronosticos.DataSource = filas;
        }

        private void btnRegistrarPronostico_Click(
            object sender,
            EventArgs e)
        {
            try
            {
                if (cmbUsuario.SelectedValue == null)
                {
                    throw new InvalidOperationException(
                        "Debe seleccionar un usuario."
                    );
                }

                if (cmbPartido.SelectedValue == null)
                {
                    throw new InvalidOperationException(
                        "Debe seleccionar un partido."
                    );
                }

                int usuarioId =
                    Convert.ToInt32(
                        cmbUsuario.SelectedValue
                    );

                int partidoId =
                    Convert.ToInt32(
                        cmbPartido.SelectedValue
                    );

                int golesLocal =
                    Convert.ToInt32(
                        nudGolesLocal.Value
                    );

                int golesVisitante =
                    Convert.ToInt32(
                        nudGolesVisitante.Value
                    );

                _pronosticoController
                    .RegistrarPronostico(
                        usuarioId,
                        partidoId,
                        golesLocal,
                        golesVisitante
                    );

                MessageBox.Show(
                    "Pronóstico registrado correctamente.",
                    "Registro exitoso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                nudGolesLocal.Value = 0;
                nudGolesVisitante.Value = 0;

                CargarDatos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "No fue posible registrar el pronóstico",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
            }
        }

        private void btnActualizar_Click(
            object sender,
            EventArgs e)
        {
            CargarDatos();
        }

        private void btnCerrar_Click(
            object sender,
            EventArgs e)
        {
            Close();
        }
    }
}