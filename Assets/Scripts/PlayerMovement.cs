using UnityEngine;
using System.Collections;

public class PlayerMovement : Movable {

	float horizontalMovementRotation = 90;
	float verticalMovementRotation = 180;

	void FixedUpdate ()
	{
		//Checks if the player wants to move.
		float h = Input.GetAxisRaw ("Horizontal");
		float v = Input.GetAxisRaw ("Vertical");

		//If the player is not moving...
		if (!moving) {
			//... but he is holding horizontal axis key, he enters the CheckForMobility function with the button he helds as a parameter.
			if (h != 0) {
				moving = CheckForMobility (new Vector3 (h, 0, 0));

					if (h > 0)
						transform.eulerAngles = new Vector2 (0, -horizontalMovementRotation);
					else
						transform.eulerAngles = new Vector2 (0, horizontalMovementRotation);				}

			//... but he is holding vertical axis key, he enters the CheckForMobility function with the button he helds as a parameter.
			else if (v != 0)
			{
				moving = CheckForMobility (new Vector3 (0, 0, v));

				if (v > 0)
					transform.eulerAngles = new Vector2 (0, -verticalMovementRotation);
				else
					transform.eulerAngles = new Vector2 (0, 0);
			}
		}
		//Otherwise the player enters the Move function.
		else
			Move ();
	}

	/// <summary>
	/// Checks if the player can move towards given direction and returns true if he can.
	/// </summary>
	/// <returns><c>true</c>, if player can move towards the given direction, <c>false</c> otherwise.</returns>
	/// <param name="direction">Direction.</param>
	public override bool CheckForMobility (Vector3 direction)
	{
		//Initializing RaycastHit to receive informations about the collision.
		RaycastHit hit;

		//Initializing a ray from the player into the given direction.
		Ray playerToDirectionRay = new Ray (transform.position, direction);

		//If the ray didn't hit anything or if the tag of the collider is "Movable"
		if (!Physics.Raycast (playerToDirectionRay, out hit, rayLength) || hit.collider.CompareTag("Movable")) {
			//If something is on the way, but it can't be pushed, returns false.
			if (hit.collider != null && !hit.transform.GetComponent<Movable> ().CheckForMobility (direction))
					return false;
			
			//Otherwise set the target position to the actual transform position, translated by direction and return true.
			else
			{
				targetPosition = transform.position + direction;
				return true;				
			}

		//If the ray hit something, but it's nothing that can be moved.
		} else
			return false;
	}
}
