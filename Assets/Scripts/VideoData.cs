using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "VideoDatas", menuName = "ScriptableObjects/VideoData", order = 1)]
public class VideoData : ScriptableObject
{
    public List<string> name;
    public List<string> urlPreview;
    public List<string> urlVideo;
}
#if(UNITY_EDITOR)
[CustomEditor(typeof(VideoData))]
public class SODataEditor : Editor
{
    private SerializedProperty _name;
    private SerializedProperty _urlPreview;
    private SerializedProperty _urlVideo;

    private void OnEnable()
    {
        _name = serializedObject.FindProperty("name");
        _urlPreview = serializedObject.FindProperty("urlPreview");
        _urlVideo = serializedObject.FindProperty("urlVideo");

        _name.arraySize = _urlVideo.arraySize = _urlPreview.arraySize;
    }

    public override void OnInspectorGUI()
    {
        for (var i = 0; i < _urlVideo.arraySize; i++)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Vedeo Data " + i);
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("X"))
            {
                _name.DeleteArrayElementAtIndex(i);
                _urlPreview.DeleteArrayElementAtIndex(i);
                _urlVideo.DeleteArrayElementAtIndex(i);
                break;
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(_name.GetArrayElementAtIndex(i), new GUIContent("Name"));
            EditorGUILayout.PropertyField(_urlPreview.GetArrayElementAtIndex(i), new GUIContent("URL Preview"));
            EditorGUILayout.PropertyField(_urlVideo.GetArrayElementAtIndex(i), new GUIContent("URL Video"));
            EditorGUI.indentLevel--;
            EditorGUILayout.Space();
        }
        EditorGUILayout.Space();
        if (GUILayout.Button("Add video"))
        {
            _name.arraySize++;
            _urlPreview.arraySize++;
            _urlVideo.arraySize++;
        }

        serializedObject.ApplyModifiedProperties();
    }
}
#endif
