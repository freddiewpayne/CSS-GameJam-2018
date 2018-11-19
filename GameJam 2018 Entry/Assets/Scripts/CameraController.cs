using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public Vector3 offset;
    public Transform playerTransform;

	// Initialise camera position
	void Start () {

        gameObject.transform.position = playerTransform.position + offset;

	}
	
	// Update camera to follow player
	void FixedUpdate () {

        gameObject.transform.position = playerTransform.position + offset;

    }
}
