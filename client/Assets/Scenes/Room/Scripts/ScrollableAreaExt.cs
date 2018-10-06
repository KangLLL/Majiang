using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[RequireComponent (typeof(tk2dUIScrollableArea))]
public class ScrollableAreaExt : MonoBehaviour {
    [SerializeField] tk2dUIScrollableArea scrollableArea;

    void OnEnable()
    {
        scrollableArea.OnScroll += OnScroll;
    }
    void OnDisable()
    {
        scrollableArea.OnScroll -= OnScroll;
    }
    void OnScroll(tk2dUIScrollableArea scrollableArea)
    {
        UpdateListGraphics();
    }
    void UpdateListGraphics()
    {
        //float previousOffset = scrollableArea.Value * (scrollableArea.ContentLength - scrollableArea.VisibleAreaLength);
        //int firstVisibleItem = Mathf.FloorToInt(previousOffset / itemStride);
        //float newContentLength = allItems.Count * itemStride;
        
    }
    public void Initial(List<Object> oList)
    {

    }
}
