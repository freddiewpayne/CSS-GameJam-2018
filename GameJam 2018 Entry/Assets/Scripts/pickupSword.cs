using UnityEngine;
using System;
using System.Collections.Generic;
public class pickupSword : MonoBehaviour
{

    public Sprite UISprite;
    public string objectName;


    private void Start()
    {
        System.Random ran = new System.Random();
        double randomNo = (ran.Next(0, 1000) / (double)1000);
        for (int i = 0; i < 50; i++)
        {
            randomNo = (ran.Next(0, 1000) / (double)1000);
        }
        int pX = (2 * ((Int16)(randomNo * 9) + 1)) + 1;
        transform.position = new Vector3((float)(pX + 0.5), (float)(pX + 0.35), 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            InventoryController.Instance.add(UISprite, objectName);
            Destroy(gameObject);
        }
    }
}
