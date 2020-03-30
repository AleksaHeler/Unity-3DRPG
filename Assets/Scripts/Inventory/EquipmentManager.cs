using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour {

	#region Singleton
	public static EquipmentManager instance;
	private void Awake() {
		if (instance != null) { Debug.LogError("More than one instance of Equipment Manager!"); return; }
		instance = this;
	}
	#endregion

	public SkinnedMeshRenderer targetMesh;
	Equipment[] currentEquipment;
	SkinnedMeshRenderer[] currentMeshes;
	Inventory inventory;

	// Event
	public delegate void OnEquipmentChanged(Equipment newItem, Equipment oldItem);
	public OnEquipmentChanged onEquipmentChanged;

	private void Start() {
		// Initialise the current equpment array with length of how many types of equipment we have
		int numSlots = System.Enum.GetNames(typeof(EquipmentSlot)).Length;
		currentEquipment = new Equipment[numSlots];
		currentMeshes = new SkinnedMeshRenderer[numSlots];
		inventory = Inventory.instance;
		targetMesh = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<SkinnedMeshRenderer>();
	}

	public void Equip(Equipment newItem) {
		// Get the type of equipment
		int slotIndex = (int)newItem.equipSlot;

		Equipment oldItem = null;

		// If something is already equipped there
		if (currentEquipment[slotIndex] != null) {
			oldItem = currentEquipment[slotIndex];
			inventory.Add(oldItem);
		}

		// If anything is subscribed to this event
		if (onEquipmentChanged != null) {
			onEquipmentChanged.Invoke(newItem, oldItem);
		}

		// Make body below the item a bit thinner so it wont interfere
		SetEquipmentBlendShapes(newItem, 100);

		// Add it to the array
		currentEquipment[slotIndex] = newItem;

		// Instantiate meshes
		if (targetMesh != null) {
			SkinnedMeshRenderer newMesh = Instantiate<SkinnedMeshRenderer>(newItem.mesh);
			newMesh.transform.parent = targetMesh.transform;

			// New mesh should deform based on the bones of target mesh (player)
			newMesh.bones = targetMesh.bones;
			newMesh.rootBone = targetMesh.rootBone;
			currentMeshes[slotIndex] = newMesh;
		}
	}

	public void Unequip(int slotIndex) {
		if (currentEquipment[slotIndex] != null) {
			if (currentMeshes[slotIndex] != null) {
				Destroy(currentMeshes[slotIndex].gameObject);
			}

			// Add current item to inventory and remove it from array
			Equipment oldItem = currentEquipment[slotIndex];
			inventory.Add(oldItem);
			SetEquipmentBlendShapes(oldItem, 0);
			currentEquipment[slotIndex] = null;

			// If anything is subscribed to this event
			if (onEquipmentChanged != null) {
				onEquipmentChanged.Invoke(null, oldItem);
			}
		}
	}

	public void UnequipAll() {
		for (int i = 0; i < currentEquipment.Length; i++) {
			Unequip(i);
		}
	}

	private void SetEquipmentBlendShapes(Equipment item, int weight) {
		foreach (EquipmentMeshRegion blendShape in item.coveredMeshRegions) {
			// Currently no blend shapes on player skinned mesh, i dont know why
			//targetMesh.SetBlendShapeWeight((int)blendShape, weight);
		}
	}

	private void Update() {
		if (Input.GetKeyDown(KeyCode.U)) {
			UnequipAll();
		}
	}
}
