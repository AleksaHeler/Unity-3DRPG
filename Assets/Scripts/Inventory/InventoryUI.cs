using UnityEngine;

public class InventoryUI : MonoBehaviour {

	// Parent object of all item slots
	public Transform itemsParent;
	public GameObject inventoryUI;
	private InventorySlot[] slots;
	private Inventory inventory;

	private void Start() {
		// Get inventory reference, and subscribe to ItemChanged event
		inventory = Inventory.instance;
		inventory.onItemChangedCallback += UpdateUI;

		// Get slots array
		slots = itemsParent.GetComponentsInChildren<InventorySlot>();
	}


	private void Update() {
		if (Input.GetButtonDown("Inventory")) {
			inventoryUI.SetActive(!inventoryUI.activeSelf);
		}
	}

	private void UpdateUI() {
		// Go trough the slots and if there is an item in inventory add it
		for(int i = 0; i < slots.Length; i++) {
			if(i < inventory.items.Count) {
				slots[i].AddItem(inventory.items[i]);
			} else { // If there are no more items
				slots[i].ClearSlot();
			}
		}
	}
}
