using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Animator))]
public class Ragdoll : MonoBehaviour
{
	public Animator animator = null;
	public List<Rigidbody> rigidbodies = new List<Rigidbody>();

	/// <summary>
	/// Returns the inverse of the animator's active status and will set all the Rigidbodies kinematic value to the
	/// inverse of the value
	/// </summary>
	public bool ragdollOn
	{
		get => !animator.enabled;
		set
		{
			animator.enabled = !value;
			foreach (Rigidbody rb in rigidbodies)
			{
				rb.isKinematic = !value;
			}
		}
	}

	private void Awake()
	{
		animator = GetComponent<Animator>();
	}

	/// <summary>
	/// Set all the Rigidbodies to kinematic to true to allow the animator to move the kinematic parts
	/// </summary>
	void Start()
	{
		foreach (Rigidbody rb in rigidbodies)
		{
			rb.isKinematic = true;
		}
	}

	/// <summary>
	/// Applies force to the closest Rigidbody to the contact point
	/// </summary>
	/// <param name="_force"></param>
	/// <param name="_contactPoint"></param>
	public void ApplyForce(Vector3 _force, Vector3 _contactPoint)
	{
		// find the rigidbody that is closest to the contact point Vector3
		Rigidbody closestRb = null;
		float closestDistance = float.MaxValue;
		foreach (Rigidbody rb in rigidbodies)
		{
			float distance = Vector3.Distance(rb.position, _contactPoint);
			if (distance < closestDistance)
			{
				closestDistance = distance;
				closestRb = rb;
			}
		}
		
		if (closestRb != null) 
			closestRb.AddForce(_force, ForceMode.Impulse);
	}

	/// <summary>
	/// Returns the closest Transform to the contact point Vector3
	/// </summary>
	/// <param name="_contactPoint"></param>
	/// <returns></returns>
	public Transform GetClosestTransform(Vector3 _contactPoint)
	{
		Rigidbody closestRb = null;
		float closestDistance = float.MaxValue;
		foreach (Rigidbody rb in rigidbodies)
		{
			float distance = Vector3.Distance(rb.position, _contactPoint);
			if (distance < closestDistance)
			{
				closestDistance = distance;
				closestRb = rb;
			}
		}

		if(closestRb != null)
			return closestRb.transform;
		return null;
	}
}