using AutoSingleton;
using System.Collections.Generic;
using UnityEditor;

namespace AutoSingletonEditor
{
    [InitializeOnLoad]
    static class ProcessorsManager
    {
        static readonly IReadOnlyCollection<AssetProcessor> AssetsProcessors = new AssetProcessor[]
        {
            new ScriptableObjectProcessor(),
            new MonoBehaviourProcessor(),
        };

        static ProcessorsManager()
        {
            if (Options.AutomaticRefresh && CompilationTracker.HasAnyCompilationError == false)
                EditorApplication.delayCall += ExecuteAllProcessors;
        }

        public static void ExecuteAllProcessors()
        {
            foreach (AssetProcessor processor in AssetsProcessors)
                processor.Execute();

            AssetDatabase.SaveAssetIfDirty(SingletonCatalogue.Asset);
            AssetDatabase.Refresh();
        }
    }
}
