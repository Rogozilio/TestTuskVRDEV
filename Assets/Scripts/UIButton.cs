using System;
using UnityEngine;
using UnityEngine.EventSystems;


public class UIButton : MonoBehaviour, IPointerClickHandler
{
    public Action onClick;
    
    public void OnPointerClick(PointerEventData eventData)
    {
        onClick?.Invoke();
    }
}