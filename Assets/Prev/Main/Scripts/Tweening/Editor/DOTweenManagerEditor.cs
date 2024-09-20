using DG.Tweening;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DOTweenManager))]
public class DOTweenManagerEditor : Editor
{
    private const int maxBehaviors = 3;
    private SerializedProperty behaviorsProperty;
    private string[] animationTypes;
    private int selectedAnimationTypeIndex = 0;
    private int selectedBehaviorIndex = -1;
    private bool[] foldouts;

    private void OnEnable()
    {
        behaviorsProperty = serializedObject.FindProperty("behaviors");
        animationTypes = System.Enum.GetNames(typeof(DOTweenManager.AnimationType));
        foldouts = new bool[behaviorsProperty.arraySize];
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        DOTweenManager manager = (DOTweenManager)target;

        EditorGUILayout.LabelField("DOTween Manager", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        if (behaviorsProperty.arraySize < maxBehaviors)
        {
            EditorGUILayout.BeginHorizontal();
            selectedAnimationTypeIndex = EditorGUILayout.Popup("Select Animation", selectedAnimationTypeIndex, animationTypes);

            if (GUILayout.Button("Add Behavior"))
            {
                behaviorsProperty.arraySize++;
                SerializedProperty newBehaviorProperty = behaviorsProperty.GetArrayElementAtIndex(behaviorsProperty.arraySize - 1);
                newBehaviorProperty.FindPropertyRelative("animationType").enumValueIndex = selectedAnimationTypeIndex;
                selectedBehaviorIndex = behaviorsProperty.arraySize - 1; // Select currently added behavior (or new)
                System.Array.Resize(ref foldouts, behaviorsProperty.arraySize);
            }
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Behaviors", EditorStyles.boldLabel);

        for (int i = 0; i < behaviorsProperty.arraySize; i++)
        {
            SerializedProperty behaviorProperty = behaviorsProperty.GetArrayElementAtIndex(i);

            EditorGUILayout.BeginVertical("box");
            foldouts[i] = EditorGUILayout.Foldout(foldouts[i], $"{(DOTweenManager.AnimationType)behaviorProperty.FindPropertyRelative("animationType").enumValueIndex} Behavior", true);

            if (foldouts[i])
            {
                EditorGUILayout.BeginHorizontal();

                if (GUILayout.Button("Edit", GUILayout.Width(80)))
                {
                    selectedBehaviorIndex = i;
                    AdvancedSettingsWindow.ShowWindow(behaviorProperty);
                }

                if (GUILayout.Button("Remove", GUILayout.Width(80)))
                {
                    behaviorsProperty.DeleteArrayElementAtIndex(i);
                    if (selectedBehaviorIndex >= behaviorsProperty.arraySize) selectedBehaviorIndex = -1;
                    System.Array.Resize(ref foldouts, behaviorsProperty.arraySize);
                    break;
                }

                EditorGUILayout.EndHorizontal();

                DrawBehaviorBasicSettings(behaviorProperty);
            }

            EditorGUILayout.EndVertical();
        }

        if (GUI.changed)
        {
            serializedObject.ApplyModifiedProperties();
        }
    }


    private void DrawBehaviorBasicSettings(SerializedProperty behaviorProperty)
    {
        EditorGUILayout.LabelField("Basic Settings", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        EditorGUI.indentLevel++;
        SerializedProperty animationType = behaviorProperty.FindPropertyRelative("animationType");

        switch ((DOTweenManager.AnimationType)animationType.enumValueIndex)
        {
            case DOTweenManager.AnimationType.Position:
                EditorGUILayout.PropertyField(behaviorProperty.FindPropertyRelative("targetPosition"), new GUIContent("Target Position"));
                EditorGUILayout.PropertyField(behaviorProperty.FindPropertyRelative("positionDuration"), new GUIContent("Duration"));
                break;

            case DOTweenManager.AnimationType.Rotation:
                EditorGUILayout.PropertyField(behaviorProperty.FindPropertyRelative("targetRotation"), new GUIContent("Target Rotation"));
                EditorGUILayout.PropertyField(behaviorProperty.FindPropertyRelative("rotationDuration"), new GUIContent("Duration"));
                break;

            case DOTweenManager.AnimationType.Scale:
                EditorGUILayout.PropertyField(behaviorProperty.FindPropertyRelative("targetScale"), new GUIContent("Target Scale"));
                EditorGUILayout.PropertyField(behaviorProperty.FindPropertyRelative("scaleDuration"), new GUIContent("Duration"));
                break;

            case DOTweenManager.AnimationType.Fade:
                EditorGUILayout.PropertyField(behaviorProperty.FindPropertyRelative("targetAlpha"), new GUIContent("Target Alpha"));
                EditorGUILayout.PropertyField(behaviorProperty.FindPropertyRelative("fadeDuration"), new GUIContent("Duration"));
                break;

            case DOTweenManager.AnimationType.Color:
                EditorGUILayout.PropertyField(behaviorProperty.FindPropertyRelative("targetColor"), new GUIContent("Target Color"));
                EditorGUILayout.PropertyField(behaviorProperty.FindPropertyRelative("colorDuration"), new GUIContent("Duration"));
                break;

            case DOTweenManager.AnimationType.ShakePosition:
                EditorGUILayout.PropertyField(behaviorProperty.FindPropertyRelative("shakeStrength"), new GUIContent("Shake Strength"));
                EditorGUILayout.PropertyField(behaviorProperty.FindPropertyRelative("shakeDuration"), new GUIContent("Shake Duration"));
                break;

            case DOTweenManager.AnimationType.ShakeRotation:
                EditorGUILayout.PropertyField(behaviorProperty.FindPropertyRelative("shakeStrength"), new GUIContent("Shake Rotation Strength"));
                EditorGUILayout.PropertyField(behaviorProperty.FindPropertyRelative("shakeDuration"), new GUIContent("Shake Duration"));
                break;

            case DOTweenManager.AnimationType.ShakeScale:
                EditorGUILayout.PropertyField(behaviorProperty.FindPropertyRelative("shakeStrength"), new GUIContent("Shake Scale Strength"));
                EditorGUILayout.PropertyField(behaviorProperty.FindPropertyRelative("shakeDuration"), new GUIContent("Shake Duration"));
                break;

            case DOTweenManager.AnimationType.PunchPosition:
                EditorGUILayout.PropertyField(behaviorProperty.FindPropertyRelative("punchStrength"), new GUIContent("Punch Strength"));
                EditorGUILayout.PropertyField(behaviorProperty.FindPropertyRelative("punchDuration"), new GUIContent("Punch Duration"));
                break;

            case DOTweenManager.AnimationType.PunchRotation:
                EditorGUILayout.PropertyField(behaviorProperty.FindPropertyRelative("punchStrength"), new GUIContent("Punch Rotation Strength"));
                EditorGUILayout.PropertyField(behaviorProperty.FindPropertyRelative("punchDuration"), new GUIContent("Punch Duration"));
                break;

            case DOTweenManager.AnimationType.PunchScale:
                EditorGUILayout.PropertyField(behaviorProperty.FindPropertyRelative("punchStrength"), new GUIContent("Punch Scale Strength"));
                EditorGUILayout.PropertyField(behaviorProperty.FindPropertyRelative("punchDuration"), new GUIContent("Punch Duration"));
                break;

            case DOTweenManager.AnimationType.Path:
                EditorGUILayout.PropertyField(behaviorProperty.FindPropertyRelative("pathDuration"), new GUIContent("Duration"));
                break;
        }

        EditorGUI.indentLevel--;
    }
}


public class AdvancedSettingsWindow : EditorWindow
{
    private SerializedObject serializedObject;
    private SerializedProperty behaviorProperty;
    private DOTweenManager.AnimationType? currentAnimationType;

    public static void ShowWindow(SerializedProperty behaviorProperty)
    {
        var window = GetWindow<AdvancedSettingsWindow>("Advanced Settings");
        window.serializedObject = behaviorProperty.serializedObject;
        window.behaviorProperty = behaviorProperty.Copy();
        window.currentAnimationType = (DOTweenManager.AnimationType)window.behaviorProperty.FindPropertyRelative("animationType").enumValueIndex;
        window.Show();
        window.Repaint();

    }

    private void OnGUI()
    {
        if (behaviorProperty == null || serializedObject == null)
        {
            Close();
            return;
        }

        serializedObject.Update();
        EditorGUILayout.LabelField("Advanced Settings", EditorStyles.boldLabel);

        SerializedProperty animationType = behaviorProperty.FindPropertyRelative("animationType");
        DOTweenManager.AnimationType animationTypeEnum = (DOTweenManager.AnimationType)animationType.enumValueIndex;

        if (animationTypeEnum != currentAnimationType)
        {
            currentAnimationType = animationTypeEnum;
            Repaint(); 
        }

        switch (animationTypeEnum)
        {
            case DOTweenManager.AnimationType.Position:
                EditorGUILayout.PropertyField(behaviorProperty.FindPropertyRelative("positionEase"), new GUIContent("Advanced Ease"));
                EditorGUILayout.PropertyField(behaviorProperty.FindPropertyRelative("loop"), new GUIContent("Loop"));
                EditorGUILayout.PropertyField(behaviorProperty.FindPropertyRelative("loopCount"), new GUIContent("Loop Count"));
                EditorGUILayout.PropertyField(behaviorProperty.FindPropertyRelative("startAtBeginning"), new GUIContent("Start Initially"));
                break;

            case DOTweenManager.AnimationType.Rotation:
                EditorGUILayout.PropertyField(behaviorProperty.FindPropertyRelative("rotationEase"), new GUIContent("Advanced Ease"));
                EditorGUILayout.PropertyField(behaviorProperty.FindPropertyRelative("startAtBeginning"), new GUIContent("Start Initially"));
                break;

            case DOTweenManager.AnimationType.Scale:
                EditorGUILayout.PropertyField(behaviorProperty.FindPropertyRelative("scaleEase"), new GUIContent("Advanced Ease"));
                EditorGUILayout.PropertyField(behaviorProperty.FindPropertyRelative("loop"), new GUIContent("Loop"));
                EditorGUILayout.PropertyField(behaviorProperty.FindPropertyRelative("loopCount"), new GUIContent("Loop Count"));
                EditorGUILayout.PropertyField(behaviorProperty.FindPropertyRelative("startAtBeginning"), new GUIContent("Start Initially"));
                break;

            case DOTweenManager.AnimationType.Fade:
                EditorGUILayout.PropertyField(behaviorProperty.FindPropertyRelative("fadeEase"), new GUIContent("Advanced Ease"));
                EditorGUILayout.PropertyField(behaviorProperty.FindPropertyRelative("loop"), new GUIContent("Loop"));
                EditorGUILayout.PropertyField(behaviorProperty.FindPropertyRelative("reverseFade"), new GUIContent("Re-Fade"));
                EditorGUILayout.PropertyField(behaviorProperty.FindPropertyRelative("loopCount"), new GUIContent("Loop Count"));
                EditorGUILayout.PropertyField(behaviorProperty.FindPropertyRelative("startAtBeginning"), new GUIContent("Start Initially"));
                break;

            case DOTweenManager.AnimationType.Color:
                EditorGUILayout.PropertyField(behaviorProperty.FindPropertyRelative("colorEase"), new GUIContent("Advanced Ease"));
                EditorGUILayout.PropertyField(behaviorProperty.FindPropertyRelative("loop"), new GUIContent("Loop"));
                EditorGUILayout.PropertyField(behaviorProperty.FindPropertyRelative("loopCount"), new GUIContent("Loop Count"));
                EditorGUILayout.PropertyField(behaviorProperty.FindPropertyRelative("startAtBeginning"), new GUIContent("Start Initially"));
                break;

            case DOTweenManager.AnimationType.ShakePosition:
                EditorGUILayout.PropertyField(behaviorProperty.FindPropertyRelative("vibrato"), new GUIContent("Advanced Vibrato"));
                EditorGUILayout.PropertyField(behaviorProperty.FindPropertyRelative("randomness"), new GUIContent("Advanced Randomness"));
                EditorGUILayout.PropertyField(behaviorProperty.FindPropertyRelative("loop"), new GUIContent("Loop"));
                EditorGUILayout.PropertyField(behaviorProperty.FindPropertyRelative("loopCount"), new GUIContent("Loop Count"));
                EditorGUILayout.PropertyField(behaviorProperty.FindPropertyRelative("startAtBeginning"), new GUIContent("Start Initially"));
                break;

            case DOTweenManager.AnimationType.ShakeRotation:
                EditorGUILayout.PropertyField(behaviorProperty.FindPropertyRelative("vibrato"), new GUIContent("Advanced Vibrato"));
                EditorGUILayout.PropertyField(behaviorProperty.FindPropertyRelative("randomness"), new GUIContent("Advanced Randomness"));
                EditorGUILayout.PropertyField(behaviorProperty.FindPropertyRelative("loop"), new GUIContent("Loop"));
                EditorGUILayout.PropertyField(behaviorProperty.FindPropertyRelative("loopCount"), new GUIContent("Loop Count"));
                EditorGUILayout.PropertyField(behaviorProperty.FindPropertyRelative("startAtBeginning"), new GUIContent("Start Initially"));
                break;

            case DOTweenManager.AnimationType.ShakeScale:
                EditorGUILayout.PropertyField(behaviorProperty.FindPropertyRelative("vibrato"), new GUIContent("Advanced Vibrato"));
                EditorGUILayout.PropertyField(behaviorProperty.FindPropertyRelative("randomness"), new GUIContent("Advanced Randomness"));
                EditorGUILayout.PropertyField(behaviorProperty.FindPropertyRelative("loop"), new GUIContent("Loop"));
                EditorGUILayout.PropertyField(behaviorProperty.FindPropertyRelative("loopCount"), new GUIContent("Loop Count"));
                EditorGUILayout.PropertyField(behaviorProperty.FindPropertyRelative("startAtBeginning"), new GUIContent("Start Initially"));
                break;

            case DOTweenManager.AnimationType.PunchPosition:
                EditorGUILayout.PropertyField(behaviorProperty.FindPropertyRelative("punchVibrato"), new GUIContent("Advanced Punch Vibrato"));
                EditorGUILayout.PropertyField(behaviorProperty.FindPropertyRelative("punchElasticity"), new GUIContent("Advanced Punch Elasticity"));
                EditorGUILayout.PropertyField(behaviorProperty.FindPropertyRelative("loop"), new GUIContent("Loop"));
                EditorGUILayout.PropertyField(behaviorProperty.FindPropertyRelative("loopCount"), new GUIContent("Loop Count"));
                EditorGUILayout.PropertyField(behaviorProperty.FindPropertyRelative("startAtBeginning"), new GUIContent("Start Initially"));
                break;

            case DOTweenManager.AnimationType.PunchRotation:
                EditorGUILayout.PropertyField(behaviorProperty.FindPropertyRelative("punchVibrato"), new GUIContent("Advanced Punch Vibrato"));
                EditorGUILayout.PropertyField(behaviorProperty.FindPropertyRelative("punchElasticity"), new GUIContent("Advanced Punch Elasticity"));
                EditorGUILayout.PropertyField(behaviorProperty.FindPropertyRelative("loop"), new GUIContent("Loop"));
                EditorGUILayout.PropertyField(behaviorProperty.FindPropertyRelative("loopCount"), new GUIContent("Loop Count"));
                EditorGUILayout.PropertyField(behaviorProperty.FindPropertyRelative("startAtBeginning"), new GUIContent("Start Initially"));
                break;

            case DOTweenManager.AnimationType.PunchScale:
                EditorGUILayout.PropertyField(behaviorProperty.FindPropertyRelative("punchVibrato"), new GUIContent("Advanced Punch Vibrato"));
                EditorGUILayout.PropertyField(behaviorProperty.FindPropertyRelative("punchElasticity"), new GUIContent("Advanced Punch Elasticity"));
                EditorGUILayout.PropertyField(behaviorProperty.FindPropertyRelative("loop"), new GUIContent("Loop"));
                EditorGUILayout.PropertyField(behaviorProperty.FindPropertyRelative("loopCount"), new GUIContent("Loop Count"));
                EditorGUILayout.PropertyField(behaviorProperty.FindPropertyRelative("startAtBeginning"), new GUIContent("Start Initially"));
                break;

            case DOTweenManager.AnimationType.Path:
                EditorGUILayout.PropertyField(behaviorProperty.FindPropertyRelative("pathPoints"), new GUIContent("Advanced Path Points"), true);
                EditorGUILayout.PropertyField(behaviorProperty.FindPropertyRelative("pathType"), new GUIContent("Advanced Path Type"));
                EditorGUILayout.PropertyField(behaviorProperty.FindPropertyRelative("pathMode"), new GUIContent("Advanced Path Mode"));
                EditorGUILayout.PropertyField(behaviorProperty.FindPropertyRelative("loop"), new GUIContent("Loop"));
                EditorGUILayout.PropertyField(behaviorProperty.FindPropertyRelative("loopCount"), new GUIContent("Loop Count"));
                EditorGUILayout.PropertyField(behaviorProperty.FindPropertyRelative("startAtBeginning"), new GUIContent("Start Initially"));
                break;
        }

        SerializedProperty onStartEventProp = behaviorProperty.FindPropertyRelative("OnStartEvent");
        SerializedProperty onCompleteEventProp = behaviorProperty.FindPropertyRelative("OnCompleteEvent");

        if (onStartEventProp != null)
            EditorGUILayout.PropertyField(onStartEventProp, new GUIContent("On Start Event"));

        if (onCompleteEventProp != null)
            EditorGUILayout.PropertyField(onCompleteEventProp, new GUIContent("On Complete Event"));

        serializedObject.ApplyModifiedProperties();
    }
}
