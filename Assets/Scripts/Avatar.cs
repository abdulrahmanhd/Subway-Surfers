using UnityEngine;

public class Avatar : MonoBehaviour {

	private Player player;

	private void Awake () {
		player = transform.root.GetComponent<Player>();
	}

	private void OnTriggerEnter (Collider collider) {
		Debug.Log("atata");
		//player.Die ();
	}
}