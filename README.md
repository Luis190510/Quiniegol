# Quiniegol

Quiniegol es una aplicación de escritorio desarrollada en **C# con Windows Forms** para gestionar una quiniela mundialista. La primera iteración utiliza archivos **JSON** como medio de persistencia y organiza el código mediante una arquitectura **Modelo-Vista-Controlador (MVC)**.

> Proyecto académico del curso **Técnicas de Programación** de la Universidad Politécnica Internacional.

## Funcionalidades

- Registro y consulta de usuarios.
- Carga de selecciones y partidos desde JSON.
- Fecha simulada para controlar partidos pendientes, en curso y finalizados.
- Registro de marcador y posibles goleadores únicamente antes del inicio del partido.
- Resultados reales separados del calendario para evitar mostrarlos anticipadamente.
- Cálculo de puntajes:
  - **5 puntos** por marcador exacto.
  - **2 puntos** por acertar ganador o empate.
  - **0 puntos** cuando el resultado es incorrecto.
- Historial de pronósticos por usuario.
- Ranking global.
- Creación, autoinscripción por nombre exacto y gestión de quinielas privadas.
- Ranking interno por quiniela.
- Insignias positivas y de vergüenza.
- Dashboard con insignias globales y privadas en secciones separadas.
- Timeline de notificaciones por quiniela.
- Estadísticas por rango de fechas.
- Consulta de últimos partidos y próximos encuentros.
- Banderas y goleadores oficiales, protegidos por la fecha simulada.
- Tablas de grupos y cruces de fase final.

## Se utilizo:

- C#
- Windows Forms
- .NET para escritorio de Windows
- JSON
- Visual Studio Community
- Git y GitHub
- Jira
- SonarQube for Visual Studio / Sonar Analyzer

## Arquitectura

El proyecto utiliza MVC y separa responsabilidades en carpetas adicionales:

```text
Quiniegol/
├── Controllers/     Casos de uso y coordinación
├── Data/            Archivos JSON persistentes
├── Images/          Banderas y recursos visuales
├── Models/          Entidades y objetos de presentación
├── Repositories/    Lectura y escritura genérica de JSON
├── Services/        Lógica especializada
├── Strategies/      Reglas intercambiables de puntaje
├── Views/           Formularios de Windows Forms
├── Documentacion/   PDF, pruebas y evidencias
└── README.md
```

### Decisiones principales

- `JsonRepository<T>` evita duplicar la lógica de lectura y escritura.
- `RutaDatosService` mantiene los archivos editables en la carpeta `Data` de la raíz del proyecto.
- `FechaSimuladaService` comparte una misma fecha entre los módulos.
- `partidos.json` contiene el calendario y `resultados2026.json` contiene los marcadores reales.
- `goleadores2026.json` contiene una copia local de los goles oficiales del torneo.
- El patrón **Strategy** permite separar las reglas de 5, 2 y 0 puntos.

## Requisitos

- Windows 10 u 11.
- Visual Studio Community 2022 o una edición compatible.
- Carga de trabajo **Desarrollo de escritorio de .NET**.
- Git, cuando se desea clonar el repositorio.

El framework exacto se encuentra en el archivo `.csproj` de la solución.

## Instalación y ejecución

1. Clona el repositorio:

```bash
git clone https://github.com/Luis190510/Quiniegol.git
```

2. Cambia a la rama final:

```bash
git checkout master
```

3. Abre `Quiniegol.slnx` en Visual Studio.
4. Selecciona **Compilar > Recompilar solución** o usa `Ctrl + Shift + B`.
5. Confirma que la solución compile sin errores.
6. Ejecuta con `F5`.

## Archivos de datos

La aplicación utiliza los archivos de la carpeta `Data`:

```text
Data/
├── usuarios.json
├── selecciones.json
├── partidos.json
├── resultados2026.json
├── goleadores2026.json
├── pronosticos.json
└── quinielas.json
```

Los archivos JSON se configuran para trabajar directamente desde la carpeta del proyecto y no desde una copia temporal de `bin/Debug`.

## Uso básico

1. Inicia sesión con una cuenta existente.
2. Ajusta la fecha de prueba desde **Ajustar fecha (pasado/futuro)** cuando
   necesites simular el momento anterior o posterior a un partido.
3. Si eres participante, abre **Pronósticos** y selecciona un partido pendiente.
4. Guarda el marcador y, opcionalmente, los posibles goleadores.
5. Adelanta la fecha simulada hasta después del partido.
6. Abre el **Ranking global** para recalcular los puntos.
7. Consulta el historial, la actividad de tus quinielas, el ranking privado y las estadísticas.

### Acceso inicial

Al abrir por primera vez los datos de la Parte 1, la aplicación completa las
credenciales que faltaban sin guardar contraseñas en texto plano:

- Administrador: `admin` / `Admin123!`
- Usuarios existentes: nombre normalizado (por ejemplo, `alberto.porras`) /
  `Quiniegol123!`

La contraseña anterior solo se asigna durante la migración de usuarios de la
Parte 1. Toda cuenta nueva debe indicar nombre completo, país favorito, nombre
de usuario, correo y una contraseña propia desde **Crear una cuenta** en la
pantalla de inicio de sesión. El registro público siempre crea participantes y
nunca cuentas administrativas.

El administrador puede gestionar usuarios, partidos y resultados. La fecha
simulada es una herramienta de prueba disponible para todas las cuentas. Cada
participante solo puede crear pronósticos con su propia
identidad y consultar las quinielas privadas donde figure como integrante. El
administrador puede consultar cualquier quiniela para labores de soporte.

### Reglas de insignias

Las insignias globales se comparan entre todos los participantes y solo se
muestran en el dashboard y el ranking global. Las insignias privadas se
calculan de manera independiente entre los integrantes de cada quiniela y solo
aparecen en el dashboard del participante y en el ranking de esa quiniela.

- **Líder global / Líder de quiniela:** mayor puntaje del ámbito correspondiente.
- **Peor del ranking / Peor de quiniela:** menor puntaje cuando compiten al menos
  dos participantes.
- **Rey de los empates:** mayor cantidad de empates acertados globalmente.
- **Racha de 10 aciertos:** al menos diez pronósticos acertados consecutivos.
- **Precisión goleadora:** mayor cantidad de goles locales o visitantes
  acertados exactamente. Un marcador totalmente exacto aporta dos aciertos.
- **Cazagoleadores:** mayor cantidad de jugadores goleadores pronosticados
  correctamente; cada jugador cuenta una vez por partido.

Las reglas **Precisión goleadora** y **Cazagoleadores** también se otorgan dentro
de cada quiniela privada, comparando únicamente a sus integrantes. Si existe un
empate por el mejor valor, todos los participantes empatados reciben la insignia.

Para unirse, el participante puede seleccionar el nombre de una quiniela
disponible. Antes de inscribirse, la aplicación no revela su descripción,
integrantes, ranking, pronósticos ni actividad.

### Fuente de los goleadores del Mundial 2026

La copia local de `goleadores2026.json` fue obtenida el 23 de agosto de 2026
del servicio oficial de FIFA. Los partidos se asociaron por sus dos selecciones,
no por el número de partido, y se validaron los 308 goles de los 104 encuentros
contra los marcadores de `resultados2026.json`. Se incluyeron goles normales,
penales durante el partido y autogoles; se excluyeron los penales de desempate.

- Calendario oficial: `https://api.fifa.com/api/v3/calendar/matches?idCompetition=17&idSeason=285023&language=en&count=500`
- Cronología oficial: `https://api.fifa.com/api/v3/timelines/{idPartidoFifa}?language=en`

La pantalla **Detalle de partidos** solo consulta esta información cuando el
estado calculado con la fecha simulada es **Finalizado**. Antes de ese momento
no expone marcador ni goleadores futuros.

## Calidad del código

La solución aplica:

- Programación orientada a objetos.
- Encapsulamiento.
- Herencia y polimorfismo en las reglas de puntaje.
- Arquitectura MVC.
- Principios SOLID, especialmente separación de responsabilidades.
- Patrón Singleton para la fecha simulada.
- Patrón Strategy para el cálculo de puntos.
- Manejo de excepciones y validaciones.
- Revisión mediante Sonar.

## Documentación

El código nuevo utiliza comentarios XML compatibles con Doxygen. Para generar
la documentación HTML, instala Doxygen y ejecuta desde la raíz:

```bash
doxygen Doxyfile
```

El resultado se genera en `docs/doxygen/html` y no debe editarse manualmente.

La documentación de entrega se encuentra en:

```text
Documentacion/
├── Quiniegol_Documentacion_Tecnica.pdf
├── Entregable_1.pdf
├── Registro_Pruebas.txt
└── Evidencias/
```

## Gestión del proyecto

- **Repositorio:** https://github.com/Luis190510/Quiniegol.git
- **Tablero de Jira:** https://trello.com/confirm?confirmationToken=&idMember=6a558b8ef8889788a64f7b8a&returnUrl=%2Fb%2FTTIKgDjL%2Fmy-trello-board
- **Rama final:** `master`

## Integrantes

- Luis Alonso Espinoza Bonilla
- Dannel Roberto Quesada Solano
