using UnityEngine;

public class PipeItem : MonoBehaviour {

	private Transform rotater;
	private Mesh mesh;
	public Vector3[] verts;

	private void Awake () {
		rotater = transform.GetChild(0);
	}

	public void Position (Pipe pipe, float curveRotation, float ringRotation) {
		mesh = rotater.GetChild (0).gameObject.GetComponent<MeshFilter>().mesh;
		transform.SetParent(pipe.transform, false);
		transform.localRotation = Quaternion.Euler(0f, 0f, -curveRotation);
		rotater.localPosition = new Vector3(0f, pipe.CurveRadius);
		rotater.localRotation = Quaternion.Euler(ringRotation, 0f, 0f);
		verts = mesh.vertices;
	}

	public void Break(Pipe pipe, Vector3 a,Vector3 b,Vector3 c,Vector3 d) {
		mesh = rotater.GetChild (0).gameObject.GetComponent<MeshFilter>().mesh;
		Vector3[] vertices = mesh.vertices;
		a = rotater.InverseTransformPoint(pipe.transform.TransformPoint(a));
		b = rotater.InverseTransformPoint(pipe.transform.TransformPoint(b));
		c = rotater.InverseTransformPoint(pipe.transform.TransformPoint(c));
		d = rotater.InverseTransformPoint(pipe.transform.TransformPoint(d));
		vertices [0] = a;
		vertices [1] = b;
		vertices [2] = c;
		vertices [3] = d;
//		int i = 0;
//		while (i < vertices.Length) {
//			vertices[i] -= new Vector3(0f,Random.Range (0,50)*0.1f,0f);
//			i++;
//		}
		mesh.vertices = vertices;
		verts = mesh.vertices;
	}
}