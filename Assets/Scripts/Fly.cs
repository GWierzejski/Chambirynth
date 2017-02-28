using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fly : Movable {

	public float flyDuration;
	public List <Vector3> waypoints;

	int nextWaypoint = 0;
	Renderer rend;

	void Awake () {
		rend = GetComponent<Renderer> ();
		waypoints = new List<Vector3> ();
	}

	void Start ()
	{
		step = 0.02f;
		waypoints.Add (transform.position);
		waypoints.Add (new Vector3 (transform.position.x + 1, transform.position.y, transform.position.z));
		waypoints.Add (new Vector3 (transform.position.x + 1, transform.position.y, transform.position.z - 1));
		waypoints.Add (new Vector3 (transform.position.x, transform.position.y, transform.position.z - 1));
		waypoints.Add (transform.position);
	}

	void FixedUpdate () {
		if (moving)
			Move ();
		else
			CalculateTargetPosition ();
		}

	void CalculateTargetPosition ()
	{
		if (waypoints [nextWaypoint] == transform.position)
		{
			if (nextWaypoint == waypoints.Count - 1) {
				if (waypoints [nextWaypoint] == waypoints [0])
					nextWaypoint = 1;
				else
				{
					nextWaypoint = 0;
					waypoints.Reverse ();					
				}
			} else
				nextWaypoint++;
		}
		else
		{
			if (waypoints [nextWaypoint].x != transform.position.x)
			{
				if (waypoints [nextWaypoint].x > transform.position.x)
					moving = CheckForMobility (new Vector3 (1, 0, 0));
				else
					moving = CheckForMobility (new Vector3 (-1, 0, 0));
			}
			else
			{
				if (waypoints [nextWaypoint].z > transform.position.z)
					moving = CheckForMobility (new Vector3 (0, 0, 1));
				else
					moving = CheckForMobility (new Vector3 (0, 0, -1));
			}

			if (!moving) {
				nextWaypoint--;
				if (transform.position == waypoints [nextWaypoint] && nextWaypoint != 0)
					nextWaypoint--;
			}
		}
	}
}
