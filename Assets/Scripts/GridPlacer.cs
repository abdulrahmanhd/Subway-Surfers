using UnityEngine;

public class GridPlacer : PipeItemGenerator {

	public PipeItem[] itemPrefabs;
	public float gridsPerPipe;

	public override void GenerateItems (Pipe pipe, float rotation,Vector3[] a, Vector3[] b, Vector3[] c, Vector3[] d) {
		if (Random.value < gridsPerPipe) {
			float angleStep = pipe.CurveAngle / pipe.CurveSegmentCount;
			PipeItem item = Instantiate<PipeItem> (
				itemPrefabs [Random.Range(0, itemPrefabs.Length)]);            
			item.Position (pipe, Mathf.FloorToInt(Random.Range(0, pipe.CurveSegmentCount-1)) * angleStep, -rotation);
		}
	}
}