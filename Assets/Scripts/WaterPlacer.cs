using UnityEngine;

public class WaterPlacer : PipeItemGenerator
{

    public PipeItem[] itemPrefabs;

	public override void GenerateItems(Pipe pipe, float rotation, Vector3[] a,Vector3[] b,Vector3[] c,Vector3[] d)
    {		
		float angleStep = pipe.CurveAngle / pipe.CurveSegmentCount;
        for (int i = 0; i < pipe.CurveSegmentCount; i++)
        {			
            PipeItem item = Instantiate<PipeItem>(
                itemPrefabs[Random.Range(0, itemPrefabs.Length)]);			
			item.Position(pipe, (i+0.5f)*angleStep, -rotation);
			item.Break(pipe, a[i],b[i],c[i],d[i]);
        }
    }
}