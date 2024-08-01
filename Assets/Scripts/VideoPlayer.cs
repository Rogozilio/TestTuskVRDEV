using System.Collections.Generic;
using DG.Tweening;
using RenderHeads.Media.AVProVideo;
using TMPro;
using UnityEditor;
using UnityEngine;

public class VideoPlayer : MonoBehaviour
{
    public MediaPlayer mediaPlayer;
    public VideoData data;

    public Transform playList;
    public GameObject prefabPreview;

    public TextMeshPro name;

    public UIButton uiButton;
    public TextMeshPro play;
    public TextMeshPro stop;

    private byte _indexActiveVideo;
    private List<Preview> _previews;
    private List<MediaPath> _mediaReferences;

    private void Awake()
    {
        InitPreview();
        InitVideo();
        InitPlayButton();

        AddVideoAndName(_indexActiveVideo);

        mediaPlayer.Events.AddListener(HandleEventFinishedPlaying);
    }

    private void InitPreview()
    {
        _previews = new List<Preview>();

        foreach (var urlPrevire in data.urlPreview)
        {
            var goPreview = Instantiate(prefabPreview, playList, false);
            var preview = goPreview.GetComponent<Preview>();
            preview.SetImage(urlPrevire);
            _previews.Add(preview);
        }

        for (byte i = 0; i < _previews.Count; i++)
        {
            byte index = i;
            _previews[i].SetActionOnClick(() =>
            {
                if (_indexActiveVideo == index)
                    return;

                _indexActiveVideo = index;
                AddVideoAndName(index);
                FadePlayStop(false);
            });
        }
    }

    private void InitVideo()
    {
        _mediaReferences = new List<MediaPath>();

        foreach (var urlVideo in data.urlVideo)
        {
            var mediaPath = new MediaPath(urlVideo, MediaPathType.AbsolutePathOrURL);

            _mediaReferences.Add(mediaPath);
        }
    }

    private void InitPlayButton()
    {
        uiButton.onClick += () =>
        {
            if (mediaPlayer.Control.IsFinished() || mediaPlayer.Control.IsPaused())
            {
                mediaPlayer.Play();
                FadePlayStop(true);
            }
            else if (mediaPlayer.Control.IsPlaying())
            {
                mediaPlayer.Pause();
                FadePlayStop(false);
            }
        };
    }

    private void FadePlayStop(bool isPlay)
    {
        play.DOFade(isPlay ? 0 : 1, 0.5f);
        stop.DOFade(isPlay ? 1 : 0, 0.5f);
    }

    private void AddVideoAndName(byte index)
    {
        name.text = data.name[index];
        mediaPlayer.OpenMedia(_mediaReferences[index], false);
    }

    private void HandleEventFinishedPlaying(MediaPlayer mp, MediaPlayerEvent.EventType eventType, ErrorCode code)
    {
        if (eventType == MediaPlayerEvent.EventType.FinishedPlaying)
        {
            FadePlayStop(false);
        }
    }
}

#region Editor

#if(UNITY_EDITOR)
[CustomEditor(typeof(VideoPlayer))]
public class VideoPlayerEditor : Editor
{
    private SerializedProperty _mediaPlayer;
    private SerializedProperty _data;
    private SerializedProperty _playList;
    private SerializedProperty _prefabPreview;
    private SerializedProperty _name;
    private SerializedProperty _uiButton;
    private SerializedProperty _play;
    private SerializedProperty _stop;

    private int _indexToolbar;

    private void OnEnable()
    {
        _mediaPlayer = serializedObject.FindProperty("mediaPlayer");
        _data = serializedObject.FindProperty("data");
        _playList = serializedObject.FindProperty("playList");
        _prefabPreview = serializedObject.FindProperty("prefabPreview");
        _name = serializedObject.FindProperty("name");
        _uiButton = serializedObject.FindProperty("uiButton");
        _play = serializedObject.FindProperty("play");
        _stop = serializedObject.FindProperty("stop");
    }

    public override void OnInspectorGUI()
    {
        _indexToolbar = GUILayout.Toolbar(_indexToolbar,
            new[] { "Main", "ListVideoPreview", "Name", "Button play" });

        EditorGUILayout.Space();
        EditorGUI.indentLevel++;

        switch (_indexToolbar)
        {
            case 0:
                EditorGUILayout.PropertyField(_mediaPlayer);
                EditorGUILayout.PropertyField(_data);
                break;
            case 1:
                EditorGUILayout.PropertyField(_playList);
                EditorGUILayout.PropertyField(_prefabPreview);
                break;
            case 2:
                EditorGUILayout.PropertyField(_name);
                break;
            case 3:
                EditorGUILayout.PropertyField(_uiButton);
                EditorGUILayout.PropertyField(_play);
                EditorGUILayout.PropertyField(_stop);
                break;
        }

        EditorGUI.indentLevel--;
        serializedObject.ApplyModifiedProperties();
    }
}
#endif

#endregion