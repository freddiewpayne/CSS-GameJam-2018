using UnityEngine;

public class PickupController : MonoBehaviour {

    public Sprite UISprite;
    public string objectName;
    public bool picked = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && !picked)
        {   
            InventoryController.Instance.add( UISprite, objectName );
            Destroy(gameObject);
        }
    }
}
