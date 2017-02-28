using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// DoorPad checks the collider color and tells atteched Door to close or open.
/// </summary>
public class DoorPad : MonoBehaviour {

	Door door;
	Material doorMat;

	// Use this for initialization
	void Awake () {
		door = transform.parent.GetComponent<Door> ();
		doorMat = transform.parent.GetComponent<Renderer>().material;
	}
	
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.GetComponent<Renderer> ().material.color == doorMat.color)
			door.Open ();
	}

	void OnTriggerExit (Collider other)
	{
		door.Close ();
	}
}
