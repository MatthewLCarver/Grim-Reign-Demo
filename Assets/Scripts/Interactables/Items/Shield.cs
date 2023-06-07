using System;
using System.Collections;

using UnityEngine;

namespace Interactables.Items
{
	public class Shield : Equipment
	{
		private Transform playerHandSocket;
		
		private void Awake()
		{
			itemName = "Shield";
			InteractableType = InteractableType.CollectLeft;
			playerHandSocket = FindObjectOfType<Player>().GetShieldSocket();
		}
		
		public override void CollectItem()
		{
			base.CollectItem();
			StartCoroutine(MoveToPlayerHandSocket());
		}

		protected override void UseItem(IItem _item)
		{
			//throw new NotImplementedException();
		}

		private void FixedUpdate()
		{
			if (transform.parent == playerHandSocket)
			{
				transform.localPosition = Vector3.zero;
				transform.localRotation = Quaternion.identity;
			}
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
	}
}