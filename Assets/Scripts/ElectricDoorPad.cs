using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that checks if any object started or stopped colliding with the gameobject it is attached to.
/// If the object material color is the same as the color of the pad it interracts with the electric door it is attached to.
/// </summary>
public class ElectricDoorPad : MonoBehaviour {

	public ElectricDoor electricDoor;

	ParticleSystem ps;
	Renderer rend;

	void Awake () {
		ps = GetComponent<ParticleSystem> ();
		rend = GetComponent<Renderer> ();
	}

	void OnTriggerEnter (Collider other)
	{
		if (other.GetComponent<MeshRenderer> ().material.color == rend.material.color)
			electricDoor.AddAPad ();
	}

	void OnTriggerExit (Collider other)
	{
		if (other.GetComponent<MeshRenderer> ().material.color == rend.material.color)
			electricDoor.SubAPad ();
	}

	void OnMouseEnter ()
	{
		electricDoor.StartGlowing ();
	}

	void OnMouseExit ()
	{
		electricDoor.StopGlowing ();
	}

	/// <summary>
	/// ElectricDoorPad starts glowing.
	/// </summary>
	public void StartGlowing ()
	{
		ps.Play ();
	}

	/// <summary>
	/// ElectricDoorPad stops glowing.
	/// </summary>
	public void StopGlowing ()
	{
		ps.Stop ();
	}
}
