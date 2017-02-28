using UnityEngine;
using System.Collections;

/// <summary>
/// Base class for every movable object.
/// </summary>
public class Movable : MonoBehaviour {

	protected bool moving = false;
	protected float rayLength = 1;
	protected float step;
	protected Vector3 targetPosition;
	
	float movementSpeed = 2;

	void Start ()
	{
		step = movementSpeed * Time.deltaTime;
	}
	void FixedUpdate ()
	{
		//If object is moving it enters the Move function.
		if (moving)
			Move ();
	}

	/// <summary>
	/// Checks if the movable object can move towards given direction and if it can it sets moving to true.
	/// </summary>
	/// <returns><c>true</c>, if object can move towards the given direction, <c>false</c> otherwise.</returns>
	/// <param name="direction">Direction.</param>
	public virtual bool CheckForMobility (Vector3 direction)
	{
		//If nothing was hit by the ray.
		if (!Physics.Raycast (transform.position, direction, rayLength)) {
			//Set the target position as a transform position translated by direction.
			targetPosition = transform.position + direction;

			//Set moving true, and return true.
			moving = true;

			return true;
		} else
			return false;
	}

	/// <summary>
	/// Move this instance.
	/// </summary>
	public void Move ()
	{
		//Translates the transform position to the target position, by time defined in step.
		transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

		//If the target position has been reached stops moving.
		if (transform.position == targetPosition) {
			moving = false;
			transform.position = targetPosition;
		}
	}
}
