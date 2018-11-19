using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword_enemy_move : MonoBehaviour {

    Transform player;
    public Rigidbody2D self;
    public float chase_distance = 3;

    // Update is called once per frame
    void Update () {

        player = GameObject.FindGameObjectWithTag("Player").transform;
        float distance = Mathf.Sqrt(Mathf.Pow(player.position.x - transform.position.x, 2)
                                           + Mathf.Pow(player.position.y - transform.position.y, 2));
        Vector3 playerRotation = player.position - transform.position;
        transform.rotation = Quaternion.LookRotation(Vector3.forward, playerRotation);

        if (Mathf.Abs(distance) < chase_distance)
        {
            self.velocity = transform.TransformDirection(Vector3.up * Mathf.Sign(distance));
        }
        else
        {
            self.velocity = transform.TransformDirection(Vector3.zero);
        }
    }

}
