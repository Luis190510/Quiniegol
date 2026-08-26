using System;
using System.Collections.Generic;
using System.Text;

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Quiniegol.Repositories
{
    public class JsonRepository<T>
    {
        private readonly string _rutaArchivo;

        private readonly JsonSerializerOptions _opcionesJson = new()
        {
            WriteIndented = true
        };

        public JsonRepository(string rutaArchivo)
        {
            _rutaArchivo = rutaArchivo;
        }

        public List<T> ObtenerTodos()
        {
            try
            {
                if (!File.Exists(_rutaArchivo))
                {
                    return new List<T>();
                }

                string contenidoJson = File.ReadAllText(_rutaArchivo);

                if (string.IsNullOrWhiteSpace(contenidoJson))
                {
                    return new List<T>();
                }

                return JsonSerializer.Deserialize<List<T>>(
                    contenidoJson,
                    _opcionesJson
                ) ?? new List<T>();
            }
            catch (JsonException ex)
            {
                throw new InvalidOperationException(
                    "El archivo JSON contiene información inválida.",
                    ex
                );
            }
            catch (IOException ex)
            {
                throw new InvalidOperationException(
                    "No fue posible leer el archivo JSON.",
                    ex
                );
            }
        }

        public void GuardarTodos(List<T> datos)
        {
            try
            {
                string? carpeta = Path.GetDirectoryName(_rutaArchivo);

                if (!string.IsNullOrWhiteSpace(carpeta))
                {
                    Directory.CreateDirectory(carpeta);
                }

                string contenidoJson = JsonSerializer.Serialize(
                    datos,
                    _opcionesJson
                );

                File.WriteAllText(_rutaArchivo, contenidoJson);
            }
            catch (IOException ex)
            {
                throw new InvalidOperationException(
                    "No fue posible guardar el archivo JSON.",
                    ex
                );
            }
        }
    }
}