﻿using UnityEngine;

public class BulletController : MonoBehaviour {

    private int bounces = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerController.Instance.hp -= 1;
            Destroy(gameObject);
        }

        else if (collision.gameObject.tag != "Enemy" && bounces == 3)
            Destroy(gameObject);

        else
            bounces += 1;
    }
}