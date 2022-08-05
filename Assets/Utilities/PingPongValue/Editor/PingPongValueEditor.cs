using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public abstract class PingPongValueEditor : Editor
{
    private SerializedProperty _targetValue;

    protected virtual void OnEnable()
    {
        _targetValue = serializedObject.FindProperty("targetValue");
    }

    protected void OnInspectorGUI<T>(PingPongValue<T> pingPongValue)
    {
        GUILayout.Space(5);

        serializedObject.Update();

        EditorGUILayout.PropertyField(_targetValue);

        pingPongValue.pingPongAmount = EditorGUILayout.IntField("Ping Pong Amount", pingPongValue.pingPongAmount);

        pingPongValue.pingPongDuration = EditorGUILayout.FloatField("Ping Pong Duration", pingPongValue.pingPongDuration);

        pingPongValue.loop = EditorGUILayout.Toggle("Loop", pingPongValue.loop);

        if (pingPongValue.loop)
            pingPongValue.loopInterval = EditorGUILayout.FloatField("Loop Interval", pingPongValue.loopInterval);

        serializedObject.ApplyModifiedProperties();

        if (GUI.changed)
        {
            EditorUtility.SetDirty(pingPongValue);
            EditorSceneManager.MarkSceneDirty(pingPongValue.gameObject.scene);
        }

        GUILayout.Space(5);
    }
}