using UnityEditor;
using UnityEngine;
using AutoSingleton;
using static UnityEditor.EditorGUILayout;

namespace AutoSingletonEditor
{
    [CustomEditor(typeof(SingletonCatalogue))]
    class SingletonCatalogueEditor : Editor
    {
        const string MonoBehaviourTitle = "Mono Behaviour";
        const string ScriptableObjectTitle = "Scriptable Object";
        const string NoneText = "(none)";

        const float ToggleWidth = 14f;

        SerializedProperty monoBehavioursProp;
        SerializedProperty scriptableObjectsProp;

        void OnEnable()
        {
            monoBehavioursProp = serializedObject.FindProperty(nameof(SingletonCatalogue.monoBehaviours));
            scriptableObjectsProp = serializedObject.FindProperty(nameof(SingletonCatalogue.scriptableObjects));
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            LabelField(MonoBehaviourTitle, EditorStyles.boldLabel);

            ListGUI(monoBehavioursProp);

            Space();

            LabelField(ScriptableObjectTitle, EditorStyles.boldLabel);

            ListGUI(scriptableObjectsProp);

            serializedObject.ApplyModifiedProperties();
        }

        void ListGUI(SerializedProperty listProp)
        {
            if (listProp.arraySize == 0)
            {
                using (new GUIBlock.ReadOnly())
                    LabelField(NoneText);

                return;
            }

            for (int i = 0; i < listProp.arraySize; i++)
            {
                SerializedProperty enabledProp = listProp.GetArrayElementAtIndex(i).FindPropertyRelative(nameof(ToggleableSingleton<object>.enabled));
                SerializedProperty valueProp = listProp.GetArrayElementAtIndex(i).FindPropertyRelative(nameof(ToggleableSingleton<object>.value));

                using (new HorizontalScope())
                {
                    using (new GUIBlock.ReadOnly(Application.isPlaying))
                        PropertyField(enabledProp, GUIContent.none, GUILayout.Width(ToggleWidth));

                    using (new GUIBlock.ReadOnly(halfAlpha: (enabledProp.boolValue == false)))
                        PropertyField(valueProp, GUIContent.none);
                }
            }
        }
    }
}
