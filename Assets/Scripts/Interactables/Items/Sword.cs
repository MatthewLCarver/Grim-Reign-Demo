using System.Collections;

using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Interactables.Items
{
	public class Sword : Equipment
	{
		private Transform playerHandSocket;
		
		private void Awake()
		{
			itemName = "Sword";
			InteractableType = InteractableType.Collect;
			playerHandSocket = FindObjectOfType<Player>().GetWeaponSocket();
		}
		
		public override void CollectItem()
		{
			base.CollectItem();
			StartCoroutine(MoveToPlayerHandSocket());
		}

		private IEnumerator MoveToPlayerHandSocket()
		{
			GetComponent<BoxCollider>().enabled = false;

			float time = 0f;
			yield return new WaitForSeconds(0.5f);
			transform.SetParent(playerHandSocket);
			while(time < 2)
			{
				time += Time.deltaTime;
				transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, .1f);
				transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.identity, .1f);
				yield return null;
			}
			transform.localPosition = Vector3.zero;
			transform.localRotation = Quaternion.identity;
		}
		
		// POTENTIALLY HAVE SWORD ACTIVATE IT'S COLLIDER ON SWING TO REGISTER COLLISIONS WITH ENEMIES AND OTHER GAMEOBJECTS
		
	}
}