namespace TP_M2I_DOTNET.Services
{
    public class ApiConfiguration
    {
        public string BaseUrl { get; set; }

        public ApiConfiguration()
        {
            // Par défaut, utilisez l'URL de l'API depuis le fichier .env
            // Dans un vrai projet, vous utiliseriez la configuration de l'application
            BaseUrl = "https://api.tasks-collaboration.example/v1";
        }
    }
}
