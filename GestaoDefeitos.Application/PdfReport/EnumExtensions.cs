using System.ComponentModel;
using System.Reflection;

namespace GestaoDefeitos.Application.PdfReport
{
    public static class EnumExtensions
    {
        public static string GetDescription(this System.Enum value)
        {
            if (value is null)
                return string.Empty;

            var type = value.GetType();
            var name = System.Enum.GetName(type, value);
            if (string.IsNullOrEmpty(name))
                return value.ToString();

            var field = type.GetField(name, BindingFlags.Public | BindingFlags.Static);
            if (field is null)
                return name;

            var attribute = field.GetCustomAttribute<DescriptionAttribute>();
            return attribute?.Description ?? name;
        }

        public static string GetPortugueseDescription(this System.Enum value)
        {
            var englishDescription = value.GetDescription();

            return englishDescription switch
            {
                // DefectStatus
                "Resolved" => "Resolvido",
                "Invalid" => "Inválido",
                "Reopened" => "Reaberto",
                "In Progress" => "Em Andamento",
                "Waiting for User" => "Aguardando Usuário",
                "New" => "Novo",

                // DefectPriority
                "P1 - Too high" => "P1 - Altíssima",
                "P2 - High" => "P2 - Alta",
                "P3 - Medium" => "P3 - Média",
                "P4 - Low" => "P4 - Baixa",
                "P5 - Too low" => "P5 - Baixíssima",

                // DefectSeverity
                "Very high" => "Gravíssima",
                "High" => "Grave",
                "Medium" => "Média",
                "Low" => "Leve",
                "Very low" => "Muito Leve",

                // DefectCategory
                "Functional" => "Funcional",
                "Interface" => "Interface",
                "Performance" => "Performance",
                "Improvement" => "Melhoria",

                _ => value.ToString()
            };
        }
    }
}