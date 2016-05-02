using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

	public PipeSystem pipeSystem;

	public float velocity;
	public float rotationAcceleration;
	public Text scoreLabel;

	private Pipe currentPipe;

	private float rotationVelocity;
	private float distanceTraveled;
	private float deltaToRotation;
	private float systemRotation;
	private float worldRotation, avatarRotation;
	private float gravity;

	public float lr;

	private Transform world, rotater;

	public void Die () {
		gameObject.SetActive(false);
	}

	public void EndGame (float distanceTraveled) {
		scoreLabel.text = ((int)(distanceTraveled * 10f)).ToString();
		gameObject.SetActive(true);
	}

	public void StartGame () {
		distanceTraveled = 0f;
		avatarRotation = 0f;
		systemRotation = 0f;
		worldRotation = 0f;	
		currentPipe = pipeSystem.SetupFirstPipe();
		SetupCurrentPipe();
		gameObject.SetActive(true);
	}

	private void Awake () {
		world = pipeSystem.transform.parent;
		rotater = transform.GetChild(0);
		gameObject.SetActive(false);
	}

	private void Update () {
		float delta = velocity * Time.deltaTime;
		distanceTraveled += delta;
		systemRotation += delta * deltaToRotation;

		if (systemRotation >= currentPipe.CurveAngle) {
			delta = (systemRotation - currentPipe.CurveAngle) / deltaToRotation;
			currentPipe = pipeSystem.SetupNextPipe();
			SetupCurrentPipe();
			systemRotation = delta * deltaToRotation;
		}

		pipeSystem.transform.localRotation =
			Quaternion.Euler(0f, 0f, systemRotation);

		UpdateAvatarRotation();
	}

	private void UpdateAvatarRotation () {
		if (avatarRotation < 180f) {
			gravity = avatarRotation;
		} else {
			gravity = (avatarRotation-360f);
		}
		rotationVelocity += (rotationAcceleration*Input.GetAxis("Horizontal")-gravity)* Time.deltaTime;
		rotationVelocity *= 0.99f;
		avatarRotation += rotationVelocity * Time.deltaTime + (rotationAcceleration*Input.GetAxis("Horizontal")-gravity)*Time.deltaTime*Time.deltaTime;
		if (avatarRotation < 0f) {
			avatarRotation += 360f;
		}
		else if (avatarRotation >= 360f) {
			avatarRotation -= 360f;
		}
		rotater.localRotation = Quaternion.Euler(avatarRotation, 0f, 0f);
		lr = avatarRotation;
	}

	private void SetupCurrentPipe () {
		deltaToRotation = 360f / (2f * Mathf.PI * currentPipe.CurveRadius);
		worldRotation += currentPipe.RelativeRotation;
		if (worldRotation < 0f) {
			worldRotation += 360f;
		}
		else if (worldRotation >= 360f) {
			worldRotation -= 360f;
		}
		world.localRotation = Quaternion.Euler(worldRotation, 0f, 0f);
	}
}