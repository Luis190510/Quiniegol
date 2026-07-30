# Quiniegol

Quiniegol es una aplicación de escritorio desarrollada en **C# con Windows Forms** para gestionar una quiniela mundialista. La primera iteración utiliza archivos **JSON** como medio de persistencia y organiza el código mediante una arquitectura **Modelo-Vista-Controlador (MVC)**.

> Proyecto académico del curso **Técnicas de Programación** de la Universidad Politécnica Internacional.

## Funcionalidades

- Registro y consulta de usuarios.
- Carga de selecciones y partidos desde JSON.
- Fecha simulada para controlar partidos pendientes, en curso y finalizados.
- Registro de pronósticos únicamente antes del inicio del partido.
- Resultados reales separados del calendario para evitar mostrarlos anticipadamente.
- Cálculo de puntajes:
  - **5 puntos** por marcador exacto.
  - **2 puntos** por acertar ganador o empate.
  - **0 puntos** cuando el resultado es incorrecto.
- Historial de pronósticos por usuario.
- Ranking global.
- Creación y gestión de quinielas privadas.
- Ranking interno por quiniela.
- Insignias positivas y de vergüenza.
- Timeline de notificaciones por quiniela.
- Estadísticas por rango de fechas.
- Consulta de últimos partidos y próximos encuentros.
- Banderas y anotadores.
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

3. Abre `Quiniegol.sln` en Visual Studio.
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
├── pronosticos.json
├── quinielas.json
└── timeline.json
```

Los archivos JSON se configuran para trabajar directamente desde la carpeta del proyecto y no desde una copia temporal de `bin/Debug`.

## Uso básico

1. Define una fecha desde **Fecha simulada**.
2. Registra o selecciona un usuario.
3. Abre **Pronósticos** y selecciona un partido pendiente.
4. Guarda el marcador pronosticado.
5. Adelanta la fecha simulada hasta después del partido.
6. Abre el **Ranking global** para recalcular los puntos.
7. Consulta el historial, las quinielas privadas, el ranking privado y las estadísticas.

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

## Gestión del proyecto

- **Repositorio:** https://github.com/Luis190510/Quiniegol.git
- **Tablero de Jira:** https://trello.com/confirm?confirmationToken=&idMember=6a558b8ef8889788a64f7b8a&returnUrl=%2Fb%2FTTIKgDjL%2Fmy-trello-board
- **Rama final:** `master`

## Integrantes

- Luis Alonso Espinoza Bonilla
- Dannel Roberto Quesada Solano