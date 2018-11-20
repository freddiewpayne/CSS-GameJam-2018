using UnityEngine;

public class PickupController : MonoBehaviour {

    public Sprite UISprite;
    public string objectName;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            InventoryController.Instance.add( UISprite, objectName );
            Destroy(gameObject);
        }
    }
}
