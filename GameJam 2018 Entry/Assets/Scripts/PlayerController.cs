using UnityEngine;
using System;

// Controls all player functions
public class PlayerController : MonoBehaviour {

    public float speed;
    public GameObject shield;
    public float shieldOffset;
    public float rotSpeed;
    private float angleLastFrame;

    public static PlayerController Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }

    private void Start()
    {
        angleLastFrame = 0;
    }

    // Player and Shield Movement
    void FixedUpdate () {

        // Player WASD movement 
        float deltaX = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        float deltaY = Input.GetAxis("Vertical") * speed * Time.deltaTime;

        gameObject.transform.position += new Vector3( deltaX, deltaY );

        // Shield Movement with mouse  
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 shieldDirection = gameObject.transform.position - mousePos;

        // Shield rotation 
        float shieldAngle = (float) Math.Acos( shieldDirection.normalized.x );
        if (shieldDirection.y < 0)
            shieldAngle = 2 * ( (float) Math.PI ) - shieldAngle;
 
        shield.transform.RotateAround(gameObject.transform.position, new Vector3(0, 0, 1), (shieldAngle - angleLastFrame) * rotSpeed );
        angleLastFrame = shieldAngle;
	}
}
