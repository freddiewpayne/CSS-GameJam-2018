using UnityEngine;

// Controls all player functions
public class PlayerController : MonoBehaviour {

    public float speed;

	// Player Movement
	void FixedUpdate () {

        float deltaX = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        float deltaY = Input.GetAxis("Vertical") * speed * Time.deltaTime;

        gameObject.transform.position += new Vector3( deltaX, deltaY );

	}
}
