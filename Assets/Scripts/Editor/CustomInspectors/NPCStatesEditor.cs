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

    private int _previousSize = -1;

    private void OnEnable()
    {
        _npcStates = (NPCStates)target;
        _statesProp = serializedObject.FindProperty("_states");
        _previousSize = _statesProp.arraySize;
        NormalizeAndInitializeIfNeeded();
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(_statesProp, true);
        if (EditorGUI.EndChangeCheck())
        {
            HandleStateInitialization();
            NormalizeAndInitializeIfNeeded();
        }

        EditorGUILayout.Space(20);

        if (_statesProp.arraySize > 0)
        {
            DrawVerticalBarChart();
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void HandleStateInitialization()
    {
        var enumValues = System.Enum.GetValues(typeof(NPCState));
        int newSize = _statesProp.arraySize;

        if (_previousSize < 0)
        {
            _previousSize = 0;
        }

        if (newSize > _previousSize)
        {
            int nextEnumIndex = 0;

            if (_previousSize > 0 && _statesProp.arraySize > _previousSize)
            {
                var lastElement = _statesProp.GetArrayElementAtIndex(_previousSize - 1);
                var lastEnumIndex = lastElement.FindPropertyRelative("state").enumValueIndex;
                nextEnumIndex = (lastEnumIndex + 1) % enumValues.Length;
            }

            for (int i = _previousSize; i < newSize; i++)
            {
                var newElement = _statesProp.GetArrayElementAtIndex(i);
                newElement.FindPropertyRelative("state").enumValueIndex = nextEnumIndex;
                newElement.FindPropertyRelative("coefficient").floatValue = 100f / newSize;

                nextEnumIndex = (nextEnumIndex + 1) % enumValues.Length;
            }
        }

        _previousSize = newSize;
    }

    private void NormalizeAndInitializeIfNeeded()
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
                fontSize = 12,
                padding = new RectOffset(6, 0, 0, 0)
            };
            GUI.Label(barRect, $"{state} - {coeff.floatValue:F1}%", style);

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
                AdjustCoefficients(i, deltaPercent);
                Event.current.Use();
            }

            if (Event.current.type == EventType.MouseUp && _isDragging)
            {
                _isDragging = false;
                _draggingIndex = -1;
                Event.current.Use();
            }

            y += barHeight;
        }
    }

    private void AdjustCoefficients(int index, float delta)
    {
        var coeff = _statesProp.GetArrayElementAtIndex(index).FindPropertyRelative("coefficient");
        float newVal = Mathf.Clamp(coeff.floatValue + delta, 1f, 99f);
        float deltaApplied = newVal - coeff.floatValue;

        float totalOthers = 0f;
        for (int i = 0; i < _statesProp.arraySize; i++)
        {
            if (i == index) continue;
            totalOthers += _statesProp.GetArrayElementAtIndex(i).FindPropertyRelative("coefficient").floatValue;
        }

        if (Mathf.Approximately(totalOthers, 0f)) return;

        coeff.floatValue = newVal;

        float scale = (100f - newVal) / totalOthers;

        for (int i = 0; i < _statesProp.arraySize; i++)
        {
            if (i == index) continue;
            var other = _statesProp.GetArrayElementAtIndex(i).FindPropertyRelative("coefficient");
            other.floatValue = Mathf.Clamp(other.floatValue * scale, 1f, 99f);
        }

        NormalizeAndInitializeIfNeeded();
    }

    private Color GetColor(int index)
    {
        Color[] colors = new Color[]
        {
            new Color32(255, 105, 97, 255),    // Coral Red
            new Color32(97, 189, 255, 255),    // Sky Blue
            new Color32(255, 203, 112, 255),   // Apricot
            new Color32(121, 237, 187, 255),   // Mint Green
            new Color32(178, 127, 255, 255),   // Lavender
            new Color32(255, 153, 102, 255),   // Peach
            new Color32(222, 222, 222, 255),   // Soft Grey
            new Color32(255, 128, 191, 255),   // Bubblegum Pink
            new Color32(102, 255, 218, 255),   // Aqua
            new Color32(255, 182, 193, 255)    // Light Pink
        };

        return colors[index % colors.Length];
    }
}
