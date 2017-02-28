using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Door class responsible for opening and closing way through gameobject it is attached to.
/// </summary>
public class Door : MonoBehaviour {

	BoxCollider boxCol;

	// Use this for initialization
	void Awake () {
		boxCol = GetComponent<BoxCollider> ();
	}

	/// <summary>
	/// Opens the door.
	/// </summary>
	public void Open ()
	{
		if(boxCol.enabled)
			boxCol.enabled = false;
	}

	/// <summary>
	/// Closes the door.
	/// </summary>
	public void Close ()
	{
		if(!boxCol.enabled)
			boxCol.enabled = true;		
	}
}
