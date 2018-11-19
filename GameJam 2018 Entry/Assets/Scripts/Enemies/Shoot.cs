using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Shoot : MonoBehaviour {

    Transform player;
    float timer = 0;
    public float shoot_timer;
    public GameObject projectile;
    private Rigidbody2D projectileRB;

    private void Start()
    {
        projectileRB = projectile.GetComponent<Rigidbody2D>();
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
            GameObject clone;
            clone = Instantiate(projectile, transform.position, transform.rotation);
            projectileRB = clone.GetComponent<Rigidbody2D>();
            projectileRB.velocity = transform.TransformDirection(Vector3.up * 10);
        }

    }
}
