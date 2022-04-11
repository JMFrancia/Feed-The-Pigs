using UnityEngine;
using UnityEngine.EventSystems;

/*
* For use with draggable; simple component with callback for dropping draggable UI
*/
public class DropZone : MonoBehaviour, IDropHandler
{
    public delegate void OnDropCallback(GameObject obj, PointerEventData data);
    public OnDropCallback OnDrop;

    void IDropHandler.OnDrop(PointerEventData eventData)
    {
        if (OnDrop != null)
        {
            OnDrop(gameObject, eventData);
        }
    }
}
