using System;
using UnityEngine;

public class UnityTrailRendererPath : IPath
{


    public UnityTrailRendererPath()
    {
    }

    public TrailRenderer TrailRenderer { get; set; }

    public Vector2 GetPosition(int index)
    {
        return this.TrailRenderer.GetPosition(index);
    }

    public int Count()
    {
        if (this.TrailRenderer == null)
        {
            return 0;
        }
        // TODO: WTF
        Vector3[] positions = new Vector3[10000];
        return this.TrailRenderer.GetPositions(positions);
    }
}
