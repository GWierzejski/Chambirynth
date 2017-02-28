using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Color swapper class responsible for changing the color of the object that collides with the gameobject it is attached to.
/// </summary>
public class ColorSwapper : MonoBehaviour {

	Renderer rend;

	void Awake () {
		rend = GetComponent<Renderer> ();
	}

	void OnTriggerEnter (Collider other)
	{
		other.GetComponent<Renderer> ().material.color = rend.material.color;
	}
}
