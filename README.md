# Application de Gestion de Tâches - TP-M2I-DOTNET

Une application MAUI moderne pour la gestion de tâches, avec prise en charge des modes API et simulation.

## Fonctionnalités

- **Gestion complète des tâches**
  - Affichage de la liste des tâches
  - Filtrage par statut (À faire, En cours, Terminé)
  - Visualisation des détails d'une tâche
  - Création de nouvelles tâches
  - Modification des tâches existantes
  - Suppression de tâches

- **Double mode de fonctionnement**
  - Mode API : connexion à une API REST externe
  - Mode Simulation : fonctionnement avec des données locales pour le développement et les tests

- **Architecture robuste**
  - Modèle MVVM (Model-View-ViewModel)
  - Injection de dépendances
  - Services abstraits avec implémentations concrètes
  - Gestion des erreurs

## Configuration

### Configuration de l'API

L'application utilise un fichier `.env` pour configurer l'URL de l'API. Créez ce fichier à la racine du projet avec le contenu suivant :

```
API_BASE_URL=https://votre-api-url.com/v1
```

Remplacez `https://votre-api-url.com/v1` par l'URL de votre API réelle.

## Structure du projet

- **Models/** : Définition des modèles de données
  - `TodoTask.cs` : Modèle de tâche
  - `TaskStatus.cs` : Énumération des statuts possibles
  - `TaskPriority.cs` : Énumération des priorités

- **Views/** : Pages de l'interface utilisateur
  - `TasksPage.xaml` : Page principale affichant la liste des tâches (mode API)
  - `SimulationTasksPage.xaml` : Page affichant la liste des tâches simulées
  - `TaskDetailPage.xaml` : Page de détail et d'édition d'une tâche

- **ViewModels/** : Logique de présentation
  - `TasksViewModel.cs` : ViewModel pour la liste des tâches (mode API)
  - `SimulationTasksViewModel.cs` : ViewModel pour la liste des tâches simulées
  - `TaskDetailViewModel.cs` : ViewModel pour les détails d'une tâche

- **Services/** : Services d'accès aux données
  - `ITaskService.cs` : Interface définissant les opérations sur les tâches
  - `ApiTaskService.cs` : Implémentation utilisant une API REST
  - `SimulationTaskService.cs` : Implémentation utilisant des données locales
  - `ApiConfiguration.cs` : Configuration de l'API

- **Converters/** : Convertisseurs pour l'interface utilisateur
  - `TaskStatusConverter.cs` : Convertit les statuts en texte lisible
  - `BoolToStringConverter.cs` : Convertit les booléens en texte
  - `InverseBoolConverter.cs` : Inverse les valeurs booléennes

## Tests

Le projet inclut une suite de tests unitaires dans le projet `TP-M2I-DOTNET.Tests` :

- **SimulationTaskServiceTests.cs** : Tests du service de simulation
- **TasksViewModelTests.cs** : Tests du ViewModel principal
- **TaskDetailViewModelTests.cs** : Tests du ViewModel de détail
- **TodoTaskTests.cs** : Tests du modèle de tâche

Pour exécuter les tests :

```bash
dotnet test TP-M2I-DOTNET.Tests
```

## Développement

### Prérequis

- .NET 9.0 SDK
- Visual Studio 2022 ou Visual Studio Code
- MAUI Workload installé

### Compilation et exécution

```bash
dotnet build
dotnet run --project TP-M2I-DOTNET
```

## Licence

Ce projet est sous licence MIT. Voir le fichier LICENSE pour plus de détails.