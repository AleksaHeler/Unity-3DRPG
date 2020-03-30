using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

	#region Singleton
	
	public static Inventory instance;

	private void Awake() {
		if (instance != null) { Debug.LogWarning("More than one instance of inventory found!"); return;  }
		instance = this;
	}
	#endregion

	// Delegate - an event you can subscribe different methods to
	public delegate void OnItemChanged();
	public OnItemChanged onItemChangedCallback;

	public int space = 20;
	public List<Item> items = new List<Item>();

	public bool Add(Item item) {
		if (!item.isDefaultItem) {
			if(items.Count >= space) {
				Debug.Log("Not enough space in inventory!");
				return false;
			}
			items.Add(item);

			// Call delegate functions, as an event happened
			if (onItemChangedCallback != null)
				onItemChangedCallback.Invoke();
			return true;
		}
		return false;
	}

	public void Remove(Item item) {
		items.Remove(item);

		// Call delegate functions, as an event happened
		if (onItemChangedCallback != null)
			onItemChangedCallback.Invoke();
	}
}
