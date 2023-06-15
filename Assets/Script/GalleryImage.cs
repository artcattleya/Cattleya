using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GalleryImage : MonoBehaviour, IPointerClickHandler, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    private Gallery _gallery;
    private ScrollRect _scroll;
    public int id;

    private bool _isWasDrag = false;

    private void Awake()
    {
        _gallery = transform.parent.GetComponent<Gallery>();
        _scroll = GameObject.Find("Canvas/ScrollView").transform.GetComponent<ScrollRect>();
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!_isWasDrag)
        _gallery.OnClickImage(id);

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _scroll.OnBeginDrag(eventData);
        _isWasDrag = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _scroll.OnDrag(eventData);
        _isWasDrag = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _scroll.OnEndDrag(eventData);
        _isWasDrag = false;
    }

}
