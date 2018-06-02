using UnityEngine;
using UnityEditor;
using UnityEditor.UI;

namespace Komugi.UI
{
    [CanEditMultipleObjects, CustomEditor(typeof(ButtonEx), true)]
    public class ButtonExEditor : ButtonEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("clickSe"), true);
            serializedObject.ApplyModifiedProperties();
        }
    }
}