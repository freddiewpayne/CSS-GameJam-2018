using UnityEngine.UI;
using UnityEngine;

public class SlotController : MonoBehaviour {

    public int slotNumber;
    public Image image;
    private Slot slot;

    // Update the slot sprite
    private void Update()
    {
        try
        {
            slot = InventoryController.Instance.slots[slotNumber];
            image.sprite = slot.sprite;
            image.gameObject.SetActive(true);
        }
        catch (System.Exception) { image.gameObject.SetActive(false); };
    }
}
