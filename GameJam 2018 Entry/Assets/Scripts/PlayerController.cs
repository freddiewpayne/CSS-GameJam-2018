using UnityEngine;
using UnityEngine.UI;
using System;

// Controls all player functions
public class PlayerController : MonoBehaviour {

    public float speed;
    public float hp;
    public GameObject shield;
    public float shieldOffset;
    public float rotSpeed;
    private float angleLastFrame;

    public Slider lifebar;

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
        hp = 10f;
    }

    // Player and Shield Movement and update hp slider
    void FixedUpdate () {

        // Player WASD movement 
        float deltaX = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        float deltaY = Input.GetAxis("Vertical") * speed * Time.deltaTime;

        gameObject.transform.position += new Vector3( deltaX, deltaY );


        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10;

        Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);
        mousePos.x = mousePos.x - objectPos.x;
        mousePos.y = mousePos.y - objectPos.y;

        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        shield.transform.RotateAround(gameObject.transform.position, new Vector3(0, 0, 1), angle - angleLastFrame);
        angleLastFrame = angle;

        /*/ Shield Movement with mouse  
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 shieldDirection = gameObject.transform.position - mousePos;

        // Shield rotation 
        float shieldAngle = (float) Math.Acos( shieldDirection.normalized.x );
        if (shieldDirection.y < 0)
            shieldAngle = 2 * ( (float) Math.PI ) - shieldAngle;
 
        shield.transform.RotateAround(gameObject.transform.position, new Vector3(0, 0, 1), (shieldAngle - angleLastFrame) * rotSpeed );
        angleLastFrame = shieldAngle;*/

        // Update lifebar
        lifebar.value = hp;

	}
}
