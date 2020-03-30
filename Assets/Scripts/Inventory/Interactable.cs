using UnityEngine;

public class Interactable : MonoBehaviour {

	/// <summary>
	/// How close the player needs to be in order to interact with the object
	/// </summary>
	public float radius = 3f;
	public Transform interactionTransform;
	
	bool isFocus = false;
	bool hasInteracted = false;
	Transform player;

	public virtual void Interact() {
		// This is supposed to be overwritten
		Debug.Log("Interacting with " + transform.name);
	}

	private void Update() {
		if (isFocus && !hasInteracted) {
			float dist = Vector3.Distance(player.position, interactionTransform.position);
			if(dist <= radius) {
				hasInteracted = true;
				Interact();
			}
		}
	}

	public void OnFocused(Transform playerTransform) {
		hasInteracted = false;
		isFocus = true;
		player = playerTransform;
	}
	public void OnDefocused() {
		hasInteracted = false;
		isFocus = false;
		player = null;
	}

	// Draw 'radius' as a sphere
	private void OnDrawGizmosSelected() {
		if (interactionTransform == null)
			interactionTransform = transform;
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(interactionTransform.position, radius);
	}

}
