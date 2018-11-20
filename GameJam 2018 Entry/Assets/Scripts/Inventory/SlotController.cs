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

    public void UseItem()
    {
        if (slot == null) return;

        // Find the item type by its name 

        if (slot.itemName == "Torch")
        {
            // Change light size and colour
            PlayerController.Instance.gameObject.GetComponent<LightingSource2D>().lightSize = 3.2f;
            PlayerController.Instance.gameObject.GetComponent<LightingSource2D>().color = new Color( 0.7f, 0.7f, 0.2f );
            PlayerController.Instance.sword.SetActive(false);
        }

        else if ( slot.itemName == "Sword" )
        {
            PlayerController.Instance.sword.SetActive(true);
            // Reset light if needed 
            PlayerController.Instance.gameObject.GetComponent<LightingSource2D>().lightSize = 1.4f;
            PlayerController.Instance.gameObject.GetComponent<LightingSource2D>().color = new Color(0.5f, 0.5f, 0.5f);
        }         
    }
}
