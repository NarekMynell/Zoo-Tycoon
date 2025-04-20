using UnityEditor;
using UnityEngine;
using Scriptables;

[CustomEditor(typeof(NPCStates))]
public class NPCStatesEditor : Editor
{
    private NPCStates _npcStates;
    private SerializedProperty _statesProp;

    private const float MinBarHeight = 30f;
    private bool _isDragging = false;
    private int _draggingIndex = -1;

    private void OnEnable()
    {
        _npcStates = (NPCStates)target;
        _statesProp = serializedObject.FindProperty("_states");
        NormalizeIfNeeded();
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(_statesProp, true);
        if (EditorGUI.EndChangeCheck())
        {
            NormalizeIfNeeded();
        }

        EditorGUILayout.Space(20);

        if (_statesProp.arraySize > 0)
        {
            DrawVerticalBarChart();
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void NormalizeIfNeeded()
    {
        if (_statesProp.arraySize == 0) return;

        float total = 0f;
        bool needInit = false;

        for (int i = 0; i < _statesProp.arraySize; i++)
        {
            var coeff = _statesProp.GetArrayElementAtIndex(i).FindPropertyRelative("coefficient");
            total += coeff.floatValue;
            if (coeff.floatValue <= 0.0001f)
                needInit = true;
        }

        float defaultValue = 100f / _statesProp.arraySize;

        for (int i = 0; i < _statesProp.arraySize; i++)
        {
            var coeff = _statesProp.GetArrayElementAtIndex(i).FindPropertyRelative("coefficient");
            if (needInit)
                coeff.floatValue = defaultValue;
        }

        // Normalize
        total = 0f;
        for (int i = 0; i < _statesProp.arraySize; i++)
            total += _statesProp.GetArrayElementAtIndex(i).FindPropertyRelative("coefficient").floatValue;

        if (!Mathf.Approximately(total, 100f))
        {
            for (int i = 0; i < _statesProp.arraySize; i++)
            {
                var coeff = _statesProp.GetArrayElementAtIndex(i).FindPropertyRelative("coefficient");
                coeff.floatValue = coeff.floatValue / total * 100f;
            }
        }
    }

    private void DrawVerticalBarChart()
    {
        int count = _statesProp.arraySize;
        float chartHeight = Mathf.Max(MinBarHeight * count, 100);
        var areaRect = GUILayoutUtility.GetRect(EditorGUIUtility.currentViewWidth - 40, chartHeight);
        EditorGUI.DrawRect(areaRect, new Color(0.15f, 0.15f, 0.15f));

        float y = areaRect.y;
        float totalHeight = areaRect.height;

        for (int i = 0; i < count; i++)
        {
            var element = _statesProp.GetArrayElementAtIndex(i);
            var state = element.FindPropertyRelative("state").enumDisplayNames[element.FindPropertyRelative("state").enumValueIndex];
            var coeff = element.FindPropertyRelative("coefficient");

            float heightRatio = coeff.floatValue / 100f;
            float barHeight = heightRatio * totalHeight;

            var barRect = new Rect(areaRect.x, y, areaRect.width, barHeight);
            EditorGUI.DrawRect(barRect, GetColor(i));

            GUIStyle style = new GUIStyle(EditorStyles.label)
            {
                alignment = TextAnchor.MiddleLeft,
                normal = { textColor = Color.black },
                hover = { textColor = Color.black },
                fontSize = 12,
                padding = new RectOffset(6, 0, 0, 0)
            };
            GUI.Label(barRect, $"{state} - {coeff.floatValue:F1}%", style);

            if (i < count - 1)
            {
                var handleRect = new Rect(barRect.x, barRect.yMax - 3, barRect.width, 6);
                EditorGUIUtility.AddCursorRect(handleRect, MouseCursor.ResizeVertical);

                if (Event.current.type == EventType.MouseDown && handleRect.Contains(Event.current.mousePosition))
                {
                    _isDragging = true;
                    _draggingIndex = i;
                    Event.current.Use();
                }

                if (_isDragging && _draggingIndex == i && Event.current.type == EventType.MouseDrag)
                {
                    float deltaPercent = Event.current.delta.y / totalHeight * 100f;
                    AdjustPairCoefficients(i, deltaPercent);
                    Event.current.Use();
                }

                if (Event.current.type == EventType.MouseUp && _isDragging)
                {
                    _isDragging = false;
                    _draggingIndex = -1;
                    Event.current.Use();
                }
            }

            y += barHeight;
        }
    }

    private void AdjustPairCoefficients(int index, float delta)
    {
        var a = _statesProp.GetArrayElementAtIndex(index).FindPropertyRelative("coefficient");
        var b = _statesProp.GetArrayElementAtIndex(index + 1).FindPropertyRelative("coefficient");

        float aVal = a.floatValue;
        float bVal = b.floatValue;

        float newA = Mathf.Clamp(aVal + delta, 0f, aVal + bVal);
        float newB = (aVal + bVal) - newA;

        a.floatValue = newA;
        b.floatValue = newB;
    }

    private Color GetColor(int index)
    {
        Color[] colors = new Color[]
        {
            new Color32(255, 99, 132, 255),
            new Color32(54, 162, 235, 255),
            new Color32(255, 206, 86, 255),
            new Color32(75, 192, 192, 255),
            new Color32(153, 102, 255, 255),
            new Color32(255, 159, 64, 255),
            new Color32(199, 199, 199, 255),
            new Color32(255, 99, 255, 255),
            new Color32(100, 255, 218, 255),
            new Color32(240, 128, 128, 255)
        };

        return colors[index % colors.Length];
    }
}
