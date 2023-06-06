using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
	bool isAttached = false;
	private Collision collision;
	private Vector3 contactPoint;
	
	/// <summary>
	/// Saves the collision and contact point when colliding with an object
	/// </summary>
	/// <param name="_collision"></param>
	private void OnCollisionEnter(Collision _collision)
	{
		isAttached = true;
		collision = _collision;
		contactPoint = collision.GetContact(0).point;
	}

	/// <summary>
	/// Will set the collision object as the Arrows parent
	/// </summary>
	private void LateUpdate()
	{
		if(isAttached)
		{
			Ragdoll ragdoll = collision.collider.GetComponentInParent<Ragdoll>();
			if (ragdoll != null)
			{
				transform.SetParent(ragdoll.GetClosestTransform(contactPoint));
				GetComponent<Rigidbody>().isKinematic = true;
				GetComponents<Collider>()[0].enabled = false;
				GetComponents<Collider>()[1].enabled = false;
				return;
			}

			transform.SetParent(collision.transform);
			GetComponent<Rigidbody>().isKinematic = true;
			GetComponents<Collider>()[0].enabled = false;
			GetComponents<Collider>()[1].enabled = false;
		}
	}
}
