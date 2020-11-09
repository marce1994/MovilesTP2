using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchableObject : MonoBehaviour, IPointerClickHandler
{
    private ITouchable[] touchables;

    private void Awake()
    {
        var touchableObjects = new List<ITouchable>();
        touchableObjects.AddRange(gameObject.transform.parent.GetComponents<ITouchable>());
        if (gameObject.transform.parent != null)
            touchableObjects.AddRange(gameObject.transform.parent.GetComponents<ITouchable>());
        touchableObjects.AddRange(gameObject.GetComponents<ITouchable>());
        touchables = touchableObjects.ToArray();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        foreach (ITouchable item in touchables)
        {
            item.OnClick();
            Debug.Log($"Clicked: { item.GetType() }");
        }
    }
}
