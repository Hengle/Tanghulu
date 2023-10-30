using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AutoSingleton
{
    static class SingletonContainer
    {
        const string RootParentName = "Auto Singleton";
        const RuntimeInitializeLoadType InitializeTiming = RuntimeInitializeLoadType.BeforeSceneLoad;

        static Dictionary<Type, Object> collection = new Dictionary<Type, Object>();

        public static IReadOnlyDictionary<Type, Object> Collection => collection;

        [RuntimeInitializeOnLoadMethod(InitializeTiming)]
        static void InstantiateSingletons()
        {
            bool anyNull = false;

            InstantiateScriptableObjects(ref anyNull);

            GameObject rootParent = new GameObject(RootParentName);
            Object.DontDestroyOnLoad(rootParent);

            InstantiateMonoBehaviours(rootParent.transform, ref anyNull);

            if (anyNull)
                Debug.LogWarning($"Found an empty entry in {SingletonCatalogue.AssetName}.");
        }

        static void InstantiateScriptableObjects(ref bool anyNull)
        {
            foreach (ToggleableSingleton<Object> soToggleSingleton in SingletonCatalogue.ScriptableObjects)
            {
                if (soToggleSingleton.enabled == false)
                    continue;

                Object soSingleton = soToggleSingleton.value;
                if (soSingleton != null)
                    collection.Add(soSingleton.GetType(), soSingleton);
                else
                    anyNull = true;
            }
        }

        static void InstantiateMonoBehaviours(Transform parent, ref bool anyNull)
        {
            foreach (ToggleableSingleton<Object> mbToggleSingleton in SingletonCatalogue.MonoBehaviours)
            {
                if (mbToggleSingleton.enabled == false)
                    continue;

                Object mbSingleton = mbToggleSingleton.value;
                if (mbSingleton != null)
                {
                    GameObject prefab = (mbSingleton as MonoBehaviour).gameObject;

                    GameObject instance = Object.Instantiate(prefab, parent);
                    instance.name = prefab.name;

                    MonoBehaviour mb = instance.GetComponent(mbSingleton.GetType()) as MonoBehaviour;
                    if (mb == null)
                        throw new InvalidOperationException($"Could not get the MonoBehaviour of type '{mbSingleton.GetType().Name}'.");

                    collection.Add(mbSingleton.GetType(), mb);
                }
                else
                    anyNull = true;
            }
        }
    }
}
