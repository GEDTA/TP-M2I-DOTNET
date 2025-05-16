using System.Globalization;
using TP_M2I_DOTNET.Models;

namespace TP_M2I_DOTNET.Converters
{
    public class TaskStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Models.TaskStatus status)
            {
                return status switch
                {
                    Models.TaskStatus.Todo => "À faire",
                    Models.TaskStatus.InProgress => "En cours",
                    Models.TaskStatus.Done => "Terminé",
                    _ => "Inconnu"
                };
            }

            return "Inconnu";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string statusText)
            {
                return statusText switch
                {
                    "À faire" => Models.TaskStatus.Todo,
                    "En cours" => Models.TaskStatus.InProgress,
                    "Terminé" => Models.TaskStatus.Done,
                    _ => Models.TaskStatus.Todo
                };
            }

            return Models.TaskStatus.Todo;
        }
    }
}
