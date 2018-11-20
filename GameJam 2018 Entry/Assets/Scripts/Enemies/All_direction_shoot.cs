using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class All_direction_shoot : MonoBehaviour {

    Transform player;
    float timer = 0;
    public float shoot_timer;
    public GameObject projectile;
    private Rigidbody2D projectileRB;
    public int n_projectile = 10;
    public float bullets_speed = 2;

    // Update is called once per frame
    void Update () {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        timer += Time.deltaTime;
        Vector3 playerRotation = player.position - transform.position;
        transform.rotation = Quaternion.LookRotation(Vector3.forward, playerRotation);

        if (timer > shoot_timer)
        {
            timer = 0;
            shoot_all_directions();


        }

    }

    void shoot_all_directions()
    {
        float angle = 360 / n_projectile;

        for(int i = 0; i < n_projectile; i++)
        {
            Transform direction = transform;
            direction.rotation = Quaternion.AngleAxis(angle * i, Vector3.forward);

            GameObject clone;
            clone = Instantiate(projectile, direction.position, direction.rotation);
            projectileRB = clone.GetComponent<Rigidbody2D>();
            projectileRB.velocity = transform.TransformDirection(Vector3.up * bullets_speed);
        }


    }
}
