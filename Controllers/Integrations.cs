using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEnd_Server.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace BackEnd_Server.Controllers
{
    [Route("[controller]")]
    public class Integrations : Controller
    {
        private readonly SecurityScannerService _scanner;
        public Integrations(SecurityScannerService scanner) => _scanner = scanner;

        [HttpPost("AnalizeProject")]
        public async Task<IActionResult> AnalizeProject()
        {
            // 1. Definir carpeta raíz única
            var tempRoot = Path.Combine(Path.GetTempPath(), "ProjectScan", Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempRoot);

            // 2. Guardar cada archivo respetando su ruta relativa
            var formCollection = await Request.ReadFormAsync();
            foreach (var file in formCollection.Files)
            {
                // file.FileName contiene la ruta relativa (ej. "src/app/app.component.ts")
                var fullPath = Path.Combine(tempRoot, file.FileName);

                // Asegurar que exista el directorio padre
                var dir = Path.GetDirectoryName(fullPath);
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                // Guardar el archivo
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }

            // 3. Recorrer y mostrar en consola la estructura
            Console.WriteLine("Estructura de archivos recibidos:");
            PrintTree(tempRoot, "");
            var findings = _scanner.ScanDirectory(tempRoot);

            // Mostrar en consola (o guardarlo en logs)
            Console.WriteLine("Resultados del escaneo de secretos:");
            foreach (var f in findings)
            {
                Console.WriteLine(
                $"{f.FilePath} (línea {f.LineNumber}): {f.RuleId} → {f.Snippet}"
                );
            }

            // Y devolver al cliente un resumen
            return Ok(new
            {
                Message  = "Archivos analizados",
                Secrets  = findings
            });
        }

        // Método auxiliar para imprimir árbol de directorios
        private void PrintTree(string directoryPath, string indent)
        {
            // Mostrar carpetas
            foreach (var dir in Directory.GetDirectories(directoryPath))
            {
                Console.WriteLine($"{indent}{Path.GetFileName(dir)}/");
                PrintTree(dir, indent + "  ");
            }
            // Mostrar archivos
            foreach (var file in Directory.GetFiles(directoryPath))
            {
                Console.WriteLine($"{indent}{Path.GetFileName(file)}");
            }
        }
    }
}