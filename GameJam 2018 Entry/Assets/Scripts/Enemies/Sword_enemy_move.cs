using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword_enemy_move : MonoBehaviour {

    Transform player;
    public Rigidbody2D self;
    public sword_timer = 1;
    float timer = 0;

	// Update is called once per frame
	void Update () {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        float distance = Mathf.Sqrt(Mathf.Pow(player.position.x - transform.position.x, 2)
                                           + Mathf.Pow(player.position.y - transform.position.y, 2));

        if (Mathf.Abs(distance) < 2)
        {
            if(timer > sword_timer)
            {

            }
        }

    }
}
