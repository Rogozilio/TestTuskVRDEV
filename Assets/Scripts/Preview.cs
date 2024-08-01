using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Preview : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private UIButton _UIbutton;

    public async void SetImage(string url)
    {
        if (url == string.Empty) return;
        
        _image.sprite = await Load.FromWebOrDisk(url);
        
        if(_image.TryGetComponent<Animator>(out var animator))
            animator.enabled = false;
    }

    public void SetActionOnClick(Action action)
    {
        _UIbutton.onClick += action;
    }
}