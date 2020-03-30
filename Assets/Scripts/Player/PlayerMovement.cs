using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	private Camera cam;
	private NavMeshAgent agent;
	private Interactable focus;
	private Transform target;
	private ThirdPersonCharacter character;
	public float defaultStoppingDistance = 0.2f;


	private void Start() {
		cam = Camera.main;
		agent = GetComponent<NavMeshAgent>();
		agent.updateRotation = false;
		character = GetComponent<ThirdPersonCharacter>();
	}

	private void Update() {
		// Move towards destination
		if (target) {
			agent.SetDestination(target.position);
			//FaceTarget();
		}

		// Where is the mouse pointing?
		if (cam && agent && !EventSystem.current.IsPointerOverGameObject()) {

			// On left mouse press set navmesh agent destination
			if (Input.GetMouseButton(0)) {
				// Find where the mouse is pointing
				Ray ray = cam.ScreenPointToRay(Input.mousePosition);
				RaycastHit hit;

				// Cast a ray :)
				if (Physics.Raycast(ray, out hit)) {
					agent.SetDestination(hit.point);
					RemoveFocus();
				}
			}

			// On right click, focus on an interactable
			if (Input.GetMouseButtonDown(1)) {
				// Find where the mouse is pointing
				Ray ray = cam.ScreenPointToRay(Input.mousePosition);
				RaycastHit hit;

				// Cast a ray :)
				if (Physics.Raycast(ray, out hit)) {
					Interactable interactable = hit.collider.GetComponent<Interactable>();
					if (interactable) {
						SetFocus(interactable);
					}
				}
			}
		}

		if (character != null) {
			// If we are not where we are supposed to be
			if (agent.remainingDistance > agent.stoppingDistance) {
				// Move character trough ThirdPersonCharacter
				character.Move(agent.desiredVelocity, false, false);
			} else {
				character.Move(Vector3.zero, false, false);
			}
		}
	}

	#region Set/remove focus interactable
	void SetFocus(Interactable newFocus) {
		if(focus != newFocus) {
			if (focus != null)
				focus.OnDefocused();
			focus = newFocus;
			FollowTarget(newFocus);
		}
		focus.OnFocused(transform);
	}
	void RemoveFocus() {
		if (focus != null)
			focus.OnDefocused();
		focus = null;
		StopFollowingTarget();
	}
	#endregion


	#region Agent following the focused interactable
	// 
	void FollowTarget(Interactable newTarget) {
		agent.stoppingDistance = newTarget.radius * 0.8f;
		//agent.updateRotation = false;
		target = newTarget.interactionTransform;
	}
	void StopFollowingTarget() {
		agent.stoppingDistance = defaultStoppingDistance;
		//agent.updateRotation = true;
		target = null;
	}
	void FaceTarget() {
		Vector3 dir = (target.position - transform.position).normalized;
		Quaternion lookRotation = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z));
		transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
	}
	#endregion
}
