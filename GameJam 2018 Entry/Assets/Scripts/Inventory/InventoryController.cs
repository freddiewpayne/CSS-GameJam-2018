using UnityEngine;

public class InventoryController : MonoBehaviour {

    // Inventory will be a singleton
    public static InventoryController Instance;
    public Slot[] slots = new Slot[5];

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }

    // Adding to inventory 
    public void add( Sprite objectSprite, string objectName )
    {
        for (int i = 0; i < 5; i++)
        {
            if (slots[i] == null)
            {
                slots[i] = new Slot(objectSprite, objectName);
                return;
            }
        }
    }
}
