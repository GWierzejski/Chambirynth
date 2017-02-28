using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that makes the camera follow the player.
/// </summary>
public class CameraFollow : MonoBehaviour {

	public Transform player;

	bool canRotate = true;
	float smoothing = 5;
	float rotationCooldownDuration = 0.3f;
	int currentCameraSetup = 0;
	Vector3 [] offsets;

	void Start () {
		offsets  = new Vector3[4];
		transform.position = transform.position + player.position;
		//Calculates the initial camera offset.
		offsets[0] = transform.position - player.position;
		offsets [1] = new Vector3 (-offsets [0].x, offsets [0].y, offsets [0].z);
		offsets [2] = new Vector3 (-offsets [0].x, offsets [0].y, -offsets [0].z);
		offsets [3] = new Vector3 (offsets [0].x, offsets [0].y, -offsets [0].z);

		transform.LookAt (player);
	}

	void FixedUpdate () {
		// Create a postion the camera is aiming for, based on the offset from the target.
		Vector3 targetCamPos = player.position + offsets[currentCameraSetup];

		// Smoothly interpolate between the camera's current position and it's target position.
		transform.position = Vector3.Lerp (transform.position, targetCamPos, smoothing * Time.deltaTime);

		if (Input.GetKey (KeyCode.Z) && canRotate)
			RotateLeft ();
		if (Input.GetKey (KeyCode.C) && canRotate)
			RotateRight ();
	}

	void RotateLeft ()
	{
		StartCoroutine ("RotationCooldown");
		if (currentCameraSetup == offsets.Length - 1)
			currentCameraSetup = 0;
		else
			currentCameraSetup++;
		
		transform.position = player.position + offsets [currentCameraSetup];
		transform.LookAt (player);
	}

	void RotateRight ()
	{
		StartCoroutine ("RotationCooldown");
		if (currentCameraSetup == 0)
			currentCameraSetup = 3;
		else
			currentCameraSetup--;

		transform.position = player.position + offsets [currentCameraSetup];
		transform.LookAt (player);
	}

	IEnumerator RotationCooldown ()
	{
		canRotate = false;
		yield return new WaitForSeconds (rotationCooldownDuration);
		canRotate = true;
	}
}
