using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_move : MonoBehaviour {

    Transform player;
    float move_time = 0;
    public float time_of_move = 1;
    public Rigidbody2D self;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        if (move_time > 0) move();
        else chose_move();
    }

    void chose_move()
    {
        float distance = Mathf.Sqrt(Mathf.Pow(player.position.x - transform.position.x, 2)
                                           + Mathf.Pow(player.position.y - transform.position.y, 2));

        if (Mathf.Abs(distance) < 5)
        {
            System.Random rand = new System.Random();
            if (rand.Next(0, 100) > 50)
            {
                self.velocity = transform.TransformDirection(Vector3.down * Mathf.Sign(distance));
                move_time = time_of_move;
            }
        }
        else
        {
            System.Random rand = new System.Random();
            if (rand.Next(0, 100) > 50)
            {
                Vector3 v = new Vector3(1.0f, 0.0f, 0.0f);
                switch (rand.Next(0, 4))
                {
                    case 0: v = new Vector3(1.0f, 0.0f, 0.0f); break;
                    case 1: v = new Vector3(0.0f, 1.0f, 0.0f); break;
                    case 2: v = new Vector3(-1.0f, 0.0f, 0.0f); break;
                    case 3: v = new Vector3(0.0f, -1.0f, 0.0f); break;
                }
                self.velocity = transform.TransformDirection(v);
                move_time = time_of_move;
            }
        }
    }

    void move()
    {
        move_time -= Time.deltaTime;
        if (move_time < 0)
        {
            move_time = 0;
            self.velocity = transform.TransformDirection(Vector3.zero);
        }
    }
}
