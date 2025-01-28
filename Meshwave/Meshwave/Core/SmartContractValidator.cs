using Enums;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Core;

public class SmartContractValidator
{
    Server _server { get; set; }
    Dictionary<Context, SmartContractContext> _contractContexts = new ();
    private readonly Dictionary<string, bool> _validationCache = new();

    public SmartContractValidator(Server server)
        => _server = server;
    
    private static readonly List<string> ProhibitedNamespaces = new List<string>
    {
        "System.IO",
        "System.Net",
        "System.Threading",
        "System.Diagnostics",
        "System.Environment",
        "System.Reflection"
    };

    public bool IsValid(string code, out string validationError)
    {
        validationError = string.Empty;

        // Verifica se o código já foi validado anteriormente
        if (_validationCache.TryGetValue(code, out var isValid))
        {
            return isValid;
        }

        // Realiza validação (com Roslyn ou abordagem simples)
        try
        {
            // Lógica de validação (por exemplo, análise de Roslyn)
            isValid = !ProhibitedNamespaces.Any(ns => code.Contains(ns));
            if (!isValid)
            {
                validationError = "Prohibited namespaces detected.";
            }
        }
        catch (Exception ex)
        {
            validationError = $"Validation failed: {ex.Message}";
            isValid = false;
        }

        // Armazena o resultado em cache
        _validationCache[code] = isValid;
        return isValid;
    }
}