using System;
using System.Collections.Generic;
using DG.Tweening;
using RenderHeads.Media.AVProVideo;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class VideoPlayer : MonoBehaviour
{
    public Transform playList;

    public GameObject prefabPreview;

    public VideoData Datas;
    public MediaPlayer mediaPlayer;
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

        for (byte i = 0; i < _previews.Count; i++)
        {
            byte index = i;
            _previews[i].SetActionOnClick( () =>
            {
                if(_indexActiveVideo == index)
                    return;
                    
                _indexActiveVideo = index;
                name.text = Datas.name[index];
                mediaPlayer.OpenMedia(_mediaReferences[index], false);
                FadePlayStop(false);
            });
        }

        name.text = Datas.name[0];
        mediaPlayer.MediaReference.MediaPath = _mediaReferences[0];

        mediaPlayer.OpenMedia(false);
        
        mediaPlayer.Events.AddListener(HandleEventFinishedPlaying);
    }

    private void InitPreview()
    {
        _previews = new List<Preview>();

        foreach (var urlPrevire in Datas.urlPreview)
        {
            var goPreview = Instantiate(prefabPreview, playList, false);
            var preview = goPreview.GetComponent<Preview>();
            preview.SetImage(urlPrevire);
            _previews.Add(preview);
        }
    }

    private void InitVideo()
    {
        _mediaReferences = new List<MediaPath>();

        foreach (var urlVideo in Datas.urlVideo)
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
    
    private void HandleEventFinishedPlaying(MediaPlayer mp, MediaPlayerEvent.EventType eventType, ErrorCode code)
    {
        Debug.Log("MediaPlayer " + mp.name + " generated event: " + eventType.ToString());
        if (eventType == MediaPlayerEvent.EventType.FinishedPlaying)
        {
            FadePlayStop(false);
        }
    }
}