using System;
using System.IO;

namespace TP_M2I_DOTNET.Services
{
    public class ApiConfiguration
    {
        public string BaseUrl { get; set; }

        public ApiConfiguration()
        {
            try
            {
                // Chemin vers le fichier .env


                // J'ai pas reussi a installer correctement le package DotNetEnv
                // Donc j'ai opté pour une approche simple avec un fichier .env
                string envFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ".env");
                
                if (File.Exists(envFilePath))
                {
                    string[] lines = File.ReadAllLines(envFilePath);
                    foreach (string line in lines)
                    {
                        if (line.StartsWith("API_BASE_URL="))
                        {
                            BaseUrl = line.Substring("API_BASE_URL=".Length).Trim();
                            break;
                        }
                    }
                }
                
            
            }
            catch (Exception)
            {
              
            }
        }
    }
}
