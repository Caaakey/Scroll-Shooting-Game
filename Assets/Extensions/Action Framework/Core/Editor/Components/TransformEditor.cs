using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Transform))]
public class TransformEditor : Editor
{
    private Transform transform = null;
    private GUIStyle style = null;

    private void OnEnable()
        => transform = target as Transform;

    public override void OnInspectorGUI()
    {
        if (style == null)
            style = new GUIStyle(GUI.skin.button)
            {
                fontStyle = FontStyle.Bold,
                fixedWidth = 20f,
                fixedHeight = 16f
            };

        GUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("P", style)) transform.localPosition = Vector3.zero;
            transform.localPosition = EditorGUILayout.Vector3Field("Position", transform.localPosition);
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("R", style)) transform.localRotation = Quaternion.identity;
            transform.eulerAngles = EditorGUILayout.Vector3Field("Rotation", transform.eulerAngles);
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("S", style)) transform.localScale = Vector3.one;
            transform.localScale = EditorGUILayout.Vector3Field("Scale", transform.localScale);
        }
        GUILayout.EndHorizontal();
    }


}
