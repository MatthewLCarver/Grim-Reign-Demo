using System;

using UnityEngine;

namespace Interactables.Items
{
	public abstract class Equipment : MonoBehaviour, IItem, IInteractable
	{
		public string itemName { get; set; }
		public bool isCollected { get; set; }
		public GameObject usableObject { get; set; }

		private void OnEnable()
		{
			GameManager.GetPlayer().OnItemCollected += UseItem;
		}

		public virtual void CollectItem()
		{
			GameManager.GetPlayer().AddItemToInventory(this);
		}

		protected abstract void UseItem(IItem _item);

		public void UseItem()
		{
			//throw new System.NotImplementedException();
		}

		public InteractableType InteractableType { get; set; }
		public bool IsActivated { get; set; }
		public virtual void InteractOn(ref InteractableType _iType)
		{
			if(!IsActivated)
			{
				_iType = InteractableType;
				CollectItem();
			}
		}

		public void InteractOff()
		{
			//throw new System.NotImplementedException();
		}

		private void Awake()
		{
			IsActivated = false;
			isCollected = false;
		}
	}
}