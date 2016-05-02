using UnityEngine;

public class BootPlacer : PipeItemGenerator {
	
	public PipeItem[] itemPrefabs;
	public float bootsPerPipe;
	
	public override void GenerateItems (Pipe pipe, float rotation,Vector3[] a, Vector3[] b, Vector3[] c, Vector3[] d) {
		if (Random.value < bootsPerPipe) {
			float angleStep = pipe.CurveAngle / pipe.CurveSegmentCount;
			PipeItem item = Instantiate<PipeItem> (
				itemPrefabs [Random.Range(0, itemPrefabs.Length)]);            
			item.Position (pipe, Mathf.FloorToInt(Random.Range(0, pipe.CurveSegmentCount-1)) * angleStep, -rotation);
		}
	}
}