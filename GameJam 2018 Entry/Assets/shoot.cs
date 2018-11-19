using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class shoot : MonoBehaviour {

    Transform player;
    float timer = 0;
    public float shoot_timer;
    public Rigidbody2D projectile;

    // Use this for initialization
    void Start () {
        
    }

    // Update is called once per frame
    void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        timer += Time.deltaTime;
        Vector3 playerRotation = player.position - transform.position;
        transform.rotation = Quaternion.LookRotation(Vector3.forward, playerRotation);

        if (timer > shoot_timer)
        {
            timer = 0;
            Rigidbody2D clone;
            clone = Instantiate(projectile, transform.position, transform.rotation);
            clone.velocity = transform.TransformDirection(Vector3.up * 10);
        }

    }
}
