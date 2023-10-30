using AutoSingleton;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AutoSingletonEditor
{
    class MonoBehaviourProcessor : AssetProcessor
    {
        protected override string DisplayTitle => "Mono Behaviour";

        protected override Type TargetType => typeof(MonoBehaviour);

        protected override string DefaultFolder => "Singleton/Mono Behaviour";

        protected override string FileExtension => "prefab";

        protected override List<ToggleableSingleton<Object>> CatalogueList => SingletonCatalogue.MonoBehaviours;

        protected override Object CreateAndSaveToDisk(Type type, string path, string name)
        {
            GameObject go = new GameObject(name);

            GameObject prefab = PrefabUtility.SaveAsPrefabAsset(go, path);
            Object mb = prefab.AddComponent(type);

            Object.DestroyImmediate(go);

            return mb;
        }
    }
}
