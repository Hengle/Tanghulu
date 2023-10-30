using AutoSingleton;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace AutoSingletonEditor
{
    static class TopBarEntries
    {
        const string MenuPrefix = "Tools/Auto Singleton/";

        const string ForceRefreshMenu = MenuPrefix + "Force Refresh";
        const string OpenSingletonCatalogueMenu = MenuPrefix + SingletonCatalogue.AssetName;
        const string ToggleAutomaticRefreshMenu = MenuPrefix + "Options/Automatic Refresh";
        const string ToggleLogChangesMenu = MenuPrefix + "Options/Log Changes";
        const string ToggleProjectIconsMenu = MenuPrefix + "Options/Project Icons";
        const string OpenDocumentationMenu = MenuPrefix + "Documentation";
        const string OpenContactMenu = MenuPrefix + "Contact";

        const string DocumentationPath = "Documentation Auto Singleton.pdf";
        const string ContactURL = "https://justetools.com/contact/";

        [MenuItem(ForceRefreshMenu, priority = 0)]
        static void ForceRefresh()
        {
            if (CompilationTracker.HasAnyCompilationError)
                Debug.LogWarning("Cannot refresh the singleton lists when they are compilation errors.");
            else
            {
                ProcessorsManager.ExecuteAllProcessors();
                Debug.Log("Singleton force refresh done.");
            }
        }

        [MenuItem(OpenSingletonCatalogueMenu, priority = 0)]
        static void OpenSingletonCatalogue()
        {
            Selection.activeObject = SingletonCatalogue.Asset;
        }

        [MenuItem(ToggleAutomaticRefreshMenu, priority = 0)]
        static void ToggleAutomaticRefresh()
        {
            Options.AutomaticRefresh = !Options.AutomaticRefresh;
        }
        [MenuItem(ToggleAutomaticRefreshMenu, true)]
        static bool ValidateAutomaticRefresh()
        {
            Menu.SetChecked(ToggleAutomaticRefreshMenu, Options.AutomaticRefresh);
            return true;
        }

        [MenuItem(ToggleLogChangesMenu, priority = 0)]
        static void ToggleLogChanges()
        {
            Options.LogChanges = !Options.LogChanges;
        }
        [MenuItem(ToggleLogChangesMenu, true)]
        static bool ValidateLogChanges()
        {
            Menu.SetChecked(ToggleLogChangesMenu, Options.LogChanges);
            return true;
        }

        [MenuItem(ToggleProjectIconsMenu, priority = 0)]
        static void ToggleProjectIcons()
        {
            Options.ProjectIcons = !Options.ProjectIcons;
        }
        [MenuItem(ToggleProjectIconsMenu, true)]
        static bool ValidateProjectIcons()
        {
            Menu.SetChecked(ToggleProjectIconsMenu, Options.ProjectIcons);
            return true;
        }

        [MenuItem(OpenDocumentationMenu, priority = 50)]
        static void OpenDocumentation()
        {
            Application.OpenURL($"file:{Path.Combine(ToolUtility.AbsolutePath, DocumentationPath)}");
        }

        [MenuItem(OpenContactMenu, priority = 51)]
        static void OpenContact()
        {
            Application.OpenURL(ContactURL);
        }
    }
}
