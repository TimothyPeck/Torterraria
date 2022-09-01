using UnityEngine;
using UnityEngine.EventSystems;

public class SlotClick : MonoBehaviour, IPointerClickHandler
{
    public GameObject slot;

    public GameObject gameObject;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            gameObject.GetComponent<Inventory>().moveToCraft(slot);
        }
    }
}
