using UnityEngine;

public class Pipe : MonoBehaviour {

	public float ringDistance;
	public float minCurveRadius, maxCurveRadius;
	public float pipeRadius;

	public int pipeSegmentCount;	
	public int minCurveSegmentCount, maxCurveSegmentCount;

	public PipeItemGenerator[] generators;

	private bool withItems;

	private int curveSegmentCount;

	private float curveRadius;
	private float curveAngle;
	private float relativeRotation;
		
	private Mesh mesh;

	private int[] triangles;

	private Vector2[] uv;

	private Vector3[] vertices;
	private Vector3[] a, b, c, d;

    static private float rotationSum;

	public float CurveAngle {
		get {
			return curveAngle;
		}
	}

	public float CurveRadius {
		get {
			return curveRadius;
		}
	}

	public float RelativeRotation {
		get {
			return relativeRotation;
		}
	}

	public int CurveSegmentCount {
		get {
			return curveSegmentCount;
		}
	}

	private void Awake () {
		GetComponent<MeshFilter>().mesh = mesh = new Mesh();
		mesh.name = "Pipe";
		MeshCollider meshc = gameObject.AddComponent(typeof(MeshCollider)) as MeshCollider;
	}

	public float Generate (bool WithItems = true, bool cascade = false) {        
		withItems = WithItems;
        if (cascade) {
			curveRadius = 10000;
			curveSegmentCount =	1;
		} else {
			curveRadius = Random.Range(minCurveRadius, maxCurveRadius);
			curveSegmentCount =
				Random.Range(minCurveSegmentCount, maxCurveSegmentCount + 1);
		}
		mesh.Clear();
		SetVertices();

		SetTriangles();
		mesh.RecalculateNormals();
        
		relativeRotation =
			Random.Range(0, curveSegmentCount) * 360f / pipeSegmentCount;
		//rotationSum = (rotationSum + relativeRotation) % 360;

        for (int i = 0; i < transform.childCount; i++) {
			Destroy(transform.GetChild(i).gameObject);
		}
		SetUV();		
		generators[1].GenerateItems(this, rotationSum,a,b,c,d);
		generators[2].GenerateItems(this, rotationSum,a,b,c,d);
		return relativeRotation;
	}

	private void SetVertices () {
		vertices = new Vector3[pipeSegmentCount * curveSegmentCount * 4];

		float uStep = ringDistance / curveRadius;
		curveAngle = uStep * curveSegmentCount * (360f / (2f * Mathf.PI));
		CreateFirstQuadRing(uStep);
		int iDelta = pipeSegmentCount * 4;
		for (int u = 2, i = iDelta; u <= curveSegmentCount; u++, i += iDelta) {
			CreateQuadRing(u * uStep, i);
		}
		mesh.vertices = vertices;
	}

	private void CreateFirstQuadRing (float u) {
		float vStep = (2f * Mathf.PI) / pipeSegmentCount;

		Vector3 vertexA = GetPointOnTorus(0f, 0f);
		Vector3 vertexB = GetPointOnTorus(u, 0f);
		for (int v = 1, i = 0; v <= pipeSegmentCount; v++, i += 4) {
			vertices[i] = vertexA;
			vertices[i + 1] = vertexA = GetPointOnTorus(0f, v * vStep);
			vertices[i + 2] = vertexB;
			vertices[i + 3] = vertexB = GetPointOnTorus(u, v * vStep);
		}
	}

	private void CreateQuadRing (float u, int i) {
		float vStep = (2f * Mathf.PI) / pipeSegmentCount;
		int ringOffset = pipeSegmentCount * 4;

		Vector3 vertex = GetPointOnTorus(u, 0f);
		for (int v = 1; v <= pipeSegmentCount; v++, i += 4) {
			vertices[i] = vertices[i - ringOffset + 2];
			vertices[i + 1] = vertices[i - ringOffset + 3];
			vertices[i + 2] = vertex;
			vertices[i + 3] = vertex = GetPointOnTorus(u, v * vStep);
		}
	}

	private void SetUV () {
		uv = new Vector2[vertices.Length];
		int firstVertice = 0;
		a = new Vector3[curveSegmentCount];
		b = new Vector3[curveSegmentCount];
		c = new Vector3[curveSegmentCount];
		d = new Vector3[curveSegmentCount];
		if (withItems) {
			rotationSum = (rotationSum + relativeRotation) % 360;
		}
		for (int i = 0; i< curveSegmentCount; i+=1) {
			int start = ((i*pipeSegmentCount+Mathf.FloorToInt(pipeSegmentCount - rotationSum/360f*pipeSegmentCount))*4)%64;
			/*if (start > ((i * pipeSegmentCount + 8 + Mathf.FloorToInt (pipeSegmentCount - rotationSum / 360f * pipeSegmentCount)) * 4) % 64) {
				a [i] = vertices [(((i - 1) * pipeSegmentCount + 8 + Mathf.FloorToInt (pipeSegmentCount - rotationSum / 360f * pipeSegmentCount)) * 4) % (pipeSegmentCount * curveSegmentCount * 4) + 3];
				b [i] = vertices [(((i - 1) * pipeSegmentCount + 8 + Mathf.FloorToInt (pipeSegmentCount - rotationSum / 360f * pipeSegmentCount)) * 4) % (pipeSegmentCount * curveSegmentCount * 4) + 1];
				c [i] = vertices [(((i - 1) * pipeSegmentCount + 7 + Mathf.FloorToInt (pipeSegmentCount - rotationSum / 360f * pipeSegmentCount)) * 4) % (pipeSegmentCount * curveSegmentCount * 4)];
				d [i] = vertices [(((i - 1) * pipeSegmentCount + 7 + Mathf.FloorToInt (pipeSegmentCount - rotationSum / 360f * pipeSegmentCount)) * 4) % (pipeSegmentCount * curveSegmentCount * 4) + 2];
			} else if (start > ((i * pipeSegmentCount + 7 + Mathf.FloorToInt (pipeSegmentCount - rotationSum / 360f * pipeSegmentCount)) * 4) % 64) {
				a [i] = vertices [((i * pipeSegmentCount + 8 + Mathf.FloorToInt (pipeSegmentCount - rotationSum / 360f * pipeSegmentCount)) * 4) % (pipeSegmentCount * curveSegmentCount * 4) + 3];
				b [i] = vertices [((i * pipeSegmentCount + 8 + Mathf.FloorToInt (pipeSegmentCount - rotationSum / 360f * pipeSegmentCount)) * 4) % (pipeSegmentCount * curveSegmentCount * 4) + 1];
				c [i] = vertices [(((i - 1) * pipeSegmentCount + 7 + Mathf.FloorToInt (pipeSegmentCount - rotationSum / 360f * pipeSegmentCount)) * 4) % (pipeSegmentCount * curveSegmentCount * 4)];
				d [i] = vertices [(((i - 1) * pipeSegmentCount + 7 + Mathf.FloorToInt (pipeSegmentCount - rotationSum / 360f * pipeSegmentCount)) * 4) % (pipeSegmentCount * curveSegmentCount * 4) + 2];
			} else {*/
			a [i] = vertices [((i * pipeSegmentCount + 8 + Mathf.FloorToInt (pipeSegmentCount - rotationSum / 360f * pipeSegmentCount)) * 4) % (pipeSegmentCount * curveSegmentCount * 4) + 3];
			b [i] = vertices [((i * pipeSegmentCount + 8 + Mathf.FloorToInt (pipeSegmentCount - rotationSum / 360f * pipeSegmentCount)) * 4) % (pipeSegmentCount * curveSegmentCount * 4) + 1];
			c [i] = vertices [((i * pipeSegmentCount + 7 + Mathf.FloorToInt (pipeSegmentCount - rotationSum / 360f * pipeSegmentCount)) * 4) % (pipeSegmentCount * curveSegmentCount * 4)];
			d [i] = vertices [((i * pipeSegmentCount + 7 + Mathf.FloorToInt (pipeSegmentCount - rotationSum / 360f * pipeSegmentCount)) * 4) % (pipeSegmentCount * curveSegmentCount * 4) + 2];
			/*}*/
			for (int j=pipeSegmentCount/2; j<pipeSegmentCount;j+=1){
				int current = ((i*pipeSegmentCount+j+Mathf.FloorToInt(pipeSegmentCount - rotationSum/360f*pipeSegmentCount))*4) % 64;
				if (start > current) {
					firstVertice = (((i-1)*pipeSegmentCount+j+Mathf.FloorToInt(pipeSegmentCount - rotationSum/360f*pipeSegmentCount))*4) % (pipeSegmentCount*curveSegmentCount*4);
				} else {
					firstVertice = ((i*pipeSegmentCount+j+Mathf.FloorToInt(pipeSegmentCount - rotationSum/360f*pipeSegmentCount))*4) % (pipeSegmentCount*curveSegmentCount*4);
				}
				uv[firstVertice] = new Vector2(0.972f*i/(curveSegmentCount*1.0f), 0.974f*(j)/(pipeSegmentCount*1.0f)*2);
				uv[firstVertice+2] = new Vector2(0.972f*(i+1)/(curveSegmentCount*1.0f), 0.974f*(j)/(pipeSegmentCount*1.0f)*2);
				uv[firstVertice+1] = new Vector2(0.972f*i/(curveSegmentCount*1.0f), 0.974f*(j+1)/(pipeSegmentCount*1.0f)*2);
				uv[firstVertice+3] = new Vector2(0.972f*(i+1)/(curveSegmentCount*1.0f), 0.974f*(j+1)/(pipeSegmentCount*1.0f)*2);		
			}
		}
		for (int i = 0; i< curveSegmentCount; i+=1) {
			int start = ((i*pipeSegmentCount+Mathf.FloorToInt(pipeSegmentCount - rotationSum/360f*pipeSegmentCount))*4)%64;
			for (int j=0; j<pipeSegmentCount/2;j+=1){
				int current = ((i*pipeSegmentCount+j+Mathf.FloorToInt(pipeSegmentCount - rotationSum/360f*pipeSegmentCount))*4) % 64;
				if (start > current) {
					firstVertice = (((i-1)*pipeSegmentCount+j+Mathf.FloorToInt(pipeSegmentCount - rotationSum/360f*pipeSegmentCount))*4) % (pipeSegmentCount*curveSegmentCount*4);
				} else {
					firstVertice = ((i*pipeSegmentCount+j+Mathf.FloorToInt(pipeSegmentCount - rotationSum/360f*pipeSegmentCount))*4) % (pipeSegmentCount*curveSegmentCount*4);
				}
				uv[firstVertice] = new Vector2(0.972f*i/(curveSegmentCount*1.0f), 0.974f*(pipeSegmentCount-j)/(pipeSegmentCount*1.0f)*2);
				uv[firstVertice+2] = new Vector2(0.972f*(i+1)/(curveSegmentCount*1.0f), 0.974f*(pipeSegmentCount-j)/(pipeSegmentCount*1.0f)*2);
				uv[firstVertice+1] = new Vector2(0.972f*i/(curveSegmentCount*1.0f), 0.974f*(pipeSegmentCount-j-1)/(pipeSegmentCount*1.0f)*2);
				uv[firstVertice+3] = new Vector2(0.972f*(i+1)/(curveSegmentCount*1.0f), 0.974f*(pipeSegmentCount-j-1)/(pipeSegmentCount*1.0f)*2);		
			}
		}
		/*ater water = Instantiate<Water> (waterPrefab);
		water.Generate (vertices [startFirst], vertices [startFirst + 1], vertices [startSecond], vertices [startSecond + 1]);*/
		//if (withItems){
		/*generators[Random.Range(0, generators.Length)].GenerateItems(this);*/				
		//}
		if (curveSegmentCount > 1) {
			if (b [0].x == d [0].x) {
				Debug.Log ("Problem");
				Vector3 tmpA = a [curveSegmentCount - 1];
				Vector3 tmpB = b [curveSegmentCount - 1];
				for (int i = curveSegmentCount-1; i > 0; i--) {
					a [i] = a [i-1];
					b [i] = b [i-1];
				}
				a [0] = tmpA;
				b [0] = tmpB;
			}
		}
		mesh.uv = uv;
	}

	private void SetTriangles () {
		triangles = new int[pipeSegmentCount * curveSegmentCount * 6];
		for (int t = 0, i = 0; t < triangles.Length; t += 6, i += 4) {
			triangles[t] = i;
			triangles[t + 1] = triangles[t + 4] = i + 2;
			triangles[t + 2] = triangles[t + 3] = i + 1;
			triangles[t + 5] = i + 3;
		}
		mesh.triangles = triangles;
	}

	private Vector3 GetPointOnTorus (float u, float v) {
		Vector3 p;
		float r = (curveRadius + pipeRadius * Mathf.Cos(v));
		p.x = r * Mathf.Sin(u);
		p.y = r * Mathf.Cos(u);
		p.z = pipeRadius * Mathf.Sin(v);
		return p;
	}

	public void AlignWith (Pipe pipe) {        
        transform.SetParent(pipe.transform, false);
		transform.localPosition = Vector3.zero;	
		transform.localRotation = Quaternion.Euler(0f, 0f, -pipe.curveAngle);
		transform.Translate(0f, pipe.curveRadius, 0f);
		transform.Rotate(relativeRotation, 0f, 0f);
		transform.Translate(0f, -curveRadius, 0f);
		transform.SetParent(pipe.transform.parent);
		transform.localScale = Vector3.one;
	}

	public void GenerateWater() {
		generators[0].GenerateItems(this, rotationSum,a,b,c,d);		
	}
}