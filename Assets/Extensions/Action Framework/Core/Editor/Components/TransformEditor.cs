using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(Transform), true)]
public class TransformEditor : Editor
{
    private SerializedProperty position;
    private SerializedProperty scale;
    private SerializedProperty rotation;
    private GUIStyle style = null;

    private void OnEnable()
    {
        position = serializedObject.FindProperty("m_LocalPosition");
        rotation = serializedObject.FindProperty("m_LocalRotation");
        scale = serializedObject.FindProperty("m_LocalScale");
    }

    public override void OnInspectorGUI()
    {
        if (style == null)
            style = new GUIStyle(GUI.skin.button)
            {
                fontStyle = FontStyle.Bold,
                fixedWidth = 20f,
                fixedHeight = 16f
            };

        serializedObject.Update();
        GUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("P", style)) position.vector3Value = Vector3.zero;
            else position.vector3Value = EditorGUILayout.Vector3Field("Position", position.vector3Value);
        }
        GUILayout.EndHorizontal();

        DrawRotation();

        GUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("S", style)) scale.vector3Value = Vector3.one;
            else scale.vector3Value = EditorGUILayout.Vector3Field("Scale", scale.vector3Value);
        }
        GUILayout.EndHorizontal();
        serializedObject.ApplyModifiedProperties();
    }

    //  NGUI Transform Editor
    #region Rotation is ugly as hell... since there is no native support for quaternion property drawing
    enum Axes : int
    {
        None = 0,
        X = 1,
        Y = 2,
        Z = 4,
        All = 7,
    }

    bool FloatField(ref float value, float axis)
    {
        if (value != axis && Differs(axis, value))
        {
            value = axis;
            return true;
        }

        return false;
    }

    bool Differs(float a, float b) { return Mathf.Abs(a - b) > 0.0001f; }
    void RegisterUndo(string name, params Object[] objects)
    {
        if (objects != null && objects.Length > 0) UnityEditor.Undo.RecordObjects(objects, name);
    }

    float WrapAngle(float angle)
    {
        while (angle > 180f) angle -= 360f;
        while (angle < -180f) angle += 360f;
        return angle;
    }

    void DrawRotation()
    {
        GUILayout.BeginHorizontal();
        {
            bool reset = GUILayout.Button("R", style);

            Vector3 visible = (serializedObject.targetObject as Transform).localEulerAngles;

            visible.x = WrapAngle(visible.x);
            visible.y = WrapAngle(visible.y);
            visible.z = WrapAngle(visible.z);

            Axes altered = Axes.None;
            Vector3 newVector = EditorGUILayout.Vector3Field("Rotation", visible);

            if (FloatField(ref visible.x, newVector.x)) altered |= Axes.X;
            if (FloatField(ref visible.y, newVector.y)) altered |= Axes.Y;
            if (FloatField(ref visible.z, newVector.z)) altered |= Axes.Z;

            if (reset)
            {
                rotation.quaternionValue = Quaternion.identity;
            }
            else if (altered != Axes.None)
            {
                RegisterUndo("Change Rotation", serializedObject.targetObjects);

                foreach (Object obj in serializedObject.targetObjects)
                {
                    Transform t = obj as Transform;
                    Vector3 v = t.localEulerAngles;

                    if ((altered & Axes.X) != 0) v.x = visible.x;
                    if ((altered & Axes.Y) != 0) v.y = visible.y;
                    if ((altered & Axes.Z) != 0) v.z = visible.z;

                    t.localEulerAngles = v;
                }
            }
        }
        GUILayout.EndHorizontal();
    }
    #endregion

}
