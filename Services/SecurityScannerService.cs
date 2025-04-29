using System.IO;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using BackEnd_Server.Models;
using System.Reflection;

namespace BackEnd_Server.Services{

    public class SecurityScannerService
    {
        private readonly List<ScanRule> _rules;

        public SecurityScannerService()
        {
            // 1. Obtener el assembly donde está incrustado el recurso
            var assembly = Assembly.GetExecutingAssembly();

            // 2. Nombre completo del recurso: <DefaultNamespace>.<Archivo>
            //    Ajusta "BackEnd_Server" si tu espacio de nombres raíz es distinto
            var resourceName = "BackEnd_Server.scanning-rules.json";

            // 3. Abrir stream y leer JSON
            using var stream = assembly.GetManifestResourceStream(resourceName)
                            ?? throw new InvalidOperationException($"No se encontró el recurso {resourceName}");
            using var reader = new StreamReader(stream);
            var json = reader.ReadToEnd();

            // 4. Deserializar con System.Text.Json
            _rules = JsonSerializer.Deserialize<List<ScanRule>>(json)
                    ?? throw new InvalidOperationException("No se pudieron cargar las reglas de escaneo");

            // 5. Compilar cada expresión regular
            foreach (var rule in _rules)
                rule.Compiled = new Regex(rule.Pattern, RegexOptions.Compiled);
        }

public List<ScanResult> ScanDirectory(string rootPath)
{
    var results = new List<ScanResult>();
    var allowedExt = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        { ".cs", ".js", ".ts", ".env", ".config", ".json", ".xml" };

    foreach (var file in Directory.GetFiles(rootPath, "*.*", SearchOption.AllDirectories))
    {
        // 1) Saltar todo lo de Migrations
        if (file.Contains(Path.DirectorySeparatorChar + "Migrations" + Path.DirectorySeparatorChar))
            continue;

        // 2) Extensiones relevantes
        if (!allowedExt.Contains(Path.GetExtension(file)))
            continue;

        int lineNum = 0;
        foreach (var line in File.ReadLines(file))
        {
            lineNum++;
            var trimmed = line.Trim();

            // 3) Pre-filtrado de decoradores y comentarios
            if (trimmed.StartsWith("[") || trimmed.StartsWith("//")
             || trimmed.StartsWith("/*") || trimmed.EndsWith("*/"))
                continue;

            // 4) Aplicar reglas
            foreach (var rule in _rules)
            {
                var m = rule.Compiled?.Match(trimmed);
                if (m != null && m.Success)
                {
                    results.Add(new ScanResult {
                        FilePath    = file,
                        LineNumber  = lineNum,
                        Snippet     = trimmed,
                        RuleId      = rule.RuleId,
                        Description = rule.Description
                    });
                }
            }
        }
    }

    return results;
}

    }
}