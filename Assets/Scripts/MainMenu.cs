using UnityEngine;
using UnityEngine.UI;


public class MainMenu : MonoBehaviour {
	
	public Player player;
	public Text scoreLabel;
	
	public void StartGame () {
		player.StartGame();
		gameObject.SetActive(false);
	}

	public void EndGame (float distanceTraveled) {
		scoreLabel.text = ((int)(distanceTraveled * 10f)).ToString();
		gameObject.SetActive(true);
	}
}