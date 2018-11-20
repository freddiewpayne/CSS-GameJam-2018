using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour {

    public Vector3 offset;

    private void FixedUpdate()
    {
        transform.position = PlayerController.Instance.transform.position + offset;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       if (collision.gameObject.tag == "Enemy")
        {
            Destroy(collision.gameObject);
            PlayerController.Instance.enemiesKilled += 1;
        }
    }
}
