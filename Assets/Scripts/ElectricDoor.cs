using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Electric door class that is responsible for opening and closing the way through the gameobject that it is attached to.
/// </summary>
public class ElectricDoor : MonoBehaviour {

	public GameObject [] attachedPads;

	BoxCollider boxCol;
	ElectricDoorPad [] electricDoorPads;
	int amountOfActivePads;
	int amountOfConnectedPads = 3;
	ParticleSystem ps;

	void Awake () {
		boxCol = GetComponent<BoxCollider> ();
		electricDoorPads = new ElectricDoorPad[attachedPads.Length];
		ps = GetComponent<ParticleSystem> ();
	}

	void Start () {
		for(int i = 0; i < attachedPads.Length; i++)
		{
			if (attachedPads [i] == null) {
				amountOfConnectedPads -= attachedPads.Length - i;
				break;
			}
		}

		for(int i = 0; i < amountOfConnectedPads; i++)
		{
			electricDoorPads [i] = attachedPads [i].GetComponent<ElectricDoorPad> ();
		}
	}

	/// <summary>
	/// Checks if doors should open.
	/// </summary>
	void CheckForOpening ()
	{
		if (amountOfActivePads == amountOfConnectedPads)
			Open ();
	}

	/// <summary>
	/// Checks if doors should close.
	/// </summary>
	void CheckForClosing ()
	{
		if (amountOfActivePads == amountOfConnectedPads - 1)
			Close ();
	}

	void OnMouseEnter ()
	{
		StartGlowing ();
	}

	void OnMouseExit ()
	{
		StopGlowing ();
	}
		
	/// <summary>
	/// Opens the door instace.
	/// </summary>
	void Open ()
	{
		if(boxCol.enabled)
			boxCol.enabled = false;
	}

	/// <summary>
	/// Closes the door instace.
	/// </summary>
	void Close ()
	{
		if(!boxCol.enabled)
			boxCol.enabled = true;
	}

	/// <summary>
	/// Electric door and connected pads starts glowing.
	/// </summary>
	public void StartGlowing ()
	{
		ps.Play ();

		for (int i = 0; i < amountOfConnectedPads; i++)
			electricDoorPads [i].StartGlowing ();
	}

	/// <summary>
	/// Electric door and connected pads stops glowing.
	/// </summary>
	public void StopGlowing ()
	{
		ps.Stop ();

		for (int i = 0; i < amountOfConnectedPads; i++)
			electricDoorPads [i].StopGlowing ();
	}

	/// <summary>
	/// Adds a pad to the amount of active pads then checks if doors should open.
	/// </summary>
	public void AddAPad ()
	{
		amountOfActivePads++;
		CheckForOpening ();
	}

	/// <summary>
	/// Substrats a pad from the amount of active pads then checks if doors should close.
	/// </summary>
	public void SubAPad ()
	{
		amountOfActivePads--;
		CheckForClosing ();
	}
}
