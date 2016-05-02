using UnityEngine;

public abstract class PipeItemGenerator : MonoBehaviour {

	public abstract void GenerateItems (Pipe pipe, float rotation, Vector3[] a, Vector3[] b, Vector3[] c, Vector3[] d);
}