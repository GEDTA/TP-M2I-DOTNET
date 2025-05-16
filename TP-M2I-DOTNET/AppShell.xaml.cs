namespace TP_M2I_DOTNET
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            
            // Enregistrer les routes
            Routing.RegisterRoute("taskdetail", typeof(Views.TaskDetailPage));
        }
    }
}
