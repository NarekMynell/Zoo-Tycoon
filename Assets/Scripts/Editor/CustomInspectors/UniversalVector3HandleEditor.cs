using UnityEditor;
using UnityEngine;
using System.Reflection;

[CustomEditor(typeof(MonoBehaviour), true)]
public class UniversalVector3HandleEditor : Editor
{
    void OnSceneGUI()
    {
        var targetObject = target as MonoBehaviour;
        DrawVector3Handles(targetObject, targetObject, targetObject.GetType(), targetObject.transform);
    }

    void DrawVector3Handles(object rootObject, object currentObject, System.Type type, Transform contextTransform, string path = "")
    {
        if (currentObject == null || type == null)
            return;

        var fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        foreach (var field in fields)
        {
            object fieldValue = field.GetValue(currentObject);
            var fieldType = field.FieldType;
            string fullPath = string.IsNullOrEmpty(path) ? field.Name : path + "." + field.Name;

            bool isWorld = field.GetCustomAttribute<WorldVector>() != null;
            bool isLocal = field.GetCustomAttribute<LocalVector>() != null;

            if ((isWorld || isLocal) && fieldType == typeof(Vector3))
            {
                Vector3 value = (Vector3)fieldValue;
                Vector3 handlePosition = value;

                if (isLocal)
                    handlePosition = contextTransform.TransformPoint(value);

                EditorGUI.BeginChangeCheck();
                Handles.color = isWorld ? Color.cyan : Color.yellow;
                Vector3 newWorldPosition = Handles.PositionHandle(handlePosition, Quaternion.identity);
                Handles.Label(newWorldPosition + Vector3.up * 0.2f, fullPath, EditorStyles.boldLabel);

                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject((Object)rootObject, $"Move {fullPath}");

                    Vector3 newValue = newWorldPosition;
                    if (isLocal)
                        newValue = contextTransform.InverseTransformPoint(newWorldPosition);

                    field.SetValue(currentObject, newValue);
                    EditorUtility.SetDirty((Object)rootObject);
                }
            }

            // Recursively process serializable classes
            else if (fieldValue != null && !fieldType.IsPrimitive && fieldType != typeof(string) && !typeof(Object).IsAssignableFrom(fieldType))
            {
                if (fieldType.IsClass || fieldType.IsValueType)
                {
                    DrawVector3Handles(rootObject, fieldValue, fieldType, contextTransform, fullPath);
                }
            }
        }
    }
}
