using System;
using System.Collections;

using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Interactables.Items
{
	public class Sword : Equipment
	{
		private Transform playerHandSocket;
		private Transform swordTransform;
		private static readonly int IsArmed = Animator.StringToHash("IsArmed");
		private Coroutine tolo;

		private void Awake()
		{
			itemName = "Sword";
			InteractableType = InteractableType.Collect;
			playerHandSocket = FindObjectOfType<Player>().GetWeaponSocket();
			swordTransform = transform.GetChild(0);
		}
		
		public override void CollectItem()
		{
			base.CollectItem();
			StartCoroutine(DisableCollider());
		}

		protected override void UseItem(IItem _item)
		{
			GameManager.GetPlayer().GetComponent<PlayerInputHandler>().GetAnimator().SetBool(IsArmed, true);
			Debug.Log("THIS");
		}

		private void FixedUpdate()
		{
			if (transform.parent != playerHandSocket) return;
			if (transform.localPosition == Vector3.zero) return;
			if (tolo != null) return;

			tolo = StartCoroutine(LerpToPlayerHandSocket());
			/*transform.localPosition = Vector3.zero;
			transform.localRotation = Quaternion.identity;*/
		}

		private IEnumerator DisableCollider()
		{
			GetComponent<BoxCollider>().enabled = false;
			
			yield return new WaitForSeconds(0.5f);
			yield return LerpToPlayerHandSocket();
		}

		private IEnumerator LerpToPlayerHandSocket()
		{
			float time = 0f;
			transform.SetParent(playerHandSocket);
			while(time < 2)
			{
				time += Time.deltaTime;
				transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, .1f);
				transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.identity, .1f);
				swordTransform.localRotation = Quaternion.Slerp(swordTransform.localRotation, Quaternion.Euler(0, 90, 0), .1f);
				yield return null;
			}
			transform.localPosition = Vector3.zero;
			transform.localRotation = Quaternion.identity;

			tolo = null;
		}

		// POTENTIALLY HAVE SWORD ACTIVATE IT'S COLLIDER ON SWING TO REGISTER COLLISIONS WITH ENEMIES AND OTHER GAMEOBJECTS
		
	}
}