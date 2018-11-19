using UnityEngine;

public class Slot {

    public Sprite sprite;
    public string itemName;

	public Slot( Sprite slotSprite, string objectName )
    {
        sprite = slotSprite;
        itemName = objectName;
    }
}
