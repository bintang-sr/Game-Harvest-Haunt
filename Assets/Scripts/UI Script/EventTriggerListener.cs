using UnityEngine;
using UnityEngine.EventSystems;

public class EventTriggerListener : MonoBehaviour, IPointerEnterHandler
{
    public System.Action onEnter;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (onEnter != null) onEnter.Invoke();
    }
}
