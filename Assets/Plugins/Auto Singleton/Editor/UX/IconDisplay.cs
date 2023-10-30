using AutoSingleton;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace AutoSingletonEditor
{
    static class IconDisplay
    {
        const string IconPath = "Icons/SingletonIcon.png";

        const float MaxHeight = 64f;

        static Texture2D icon;
        static HashSet<string> singletonGuids;

        [InitializeOnLoadMethod]
        static void Initialize()
        {
            EditorApplication.projectWindowItemOnGUI -= ItemOnGUI;
            EditorApplication.projectWindowItemOnGUI += ItemOnGUI;

            icon = AssetDatabase.LoadAssetAtPath<Texture2D>(Path.Combine(ToolUtility.RelativePath, IconPath));

            IEnumerable<Object> monoBehaviours = SingletonCatalogue.MonoBehaviours.Select(mb => (mb.value as MonoBehaviour))
                                                                                  .Where(mb => mb != null)
                                                                                  .Select(mb => mb.gameObject);
            IEnumerable<Object> scriptableObjects = SingletonCatalogue.ScriptableObjects.Select(so => so.value);

            IEnumerable<string> guids = monoBehaviours.Concat(scriptableObjects)
                                                      .Select(o => (o != null && AssetDatabase.TryGetGUIDAndLocalFileIdentifier(o, out string guid, out long _) ? guid : null))
                                                      .Where(guid => guid != null);
            singletonGuids = new HashSet<string>(guids);
        }

        static void ItemOnGUI(string guid, Rect rect)
        {
            if (Options.ProjectIcons == false)
                return;

            if (singletonGuids == null || singletonGuids.Contains(guid) == false)
                return;

            ColumnLayout columnLayout = ProjectWindowUtility.ColumnLayout;
            if (columnLayout == ColumnLayout.Unknown)
                return;

            if (columnLayout == ColumnLayout.One)
                rect.width = rect.height;
            else // (columnLayout == ColumnLayout.Two)
            {
                float gridSize = ProjectWindowUtility.GridSize;

                if (rect.width > rect.height)
                    rect.width = rect.height;

                if (gridSize > 18f)
                {
                    rect.y -= (gridSize) / 3f;

                    float extraHeight = rect.height - MaxHeight;
                    if (extraHeight > 0f)
                    {
                        rect.x += extraHeight / 2f;
                        rect.y += extraHeight;

                        rect.width -= extraHeight;
                        rect.height -= extraHeight;
                    }
                }
                else
                    rect.x += 3f;

                rect.y += (rect.height - rect.width);
                rect.height = rect.width;
            }

            GUI.DrawTexture(rect, icon);
        }
    }
}
