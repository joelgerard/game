using System;
using UnityEngine;

public class PathRenderer
{
    UnityTrailRendererPath trailRendererPath = new UnityTrailRendererPath();
    public event OnReady OnReadyEvent;
    public delegate void OnReady();
    TrailRenderer trailPrefab;
    public bool StartDrawing { get; set; }

    public PathRenderer(TrailRenderer trailPrefab)
    {
        this.trailPrefab = trailPrefab;
    }



    // TODO: rename/remove okToDrawTrail
    public UnityTrailRendererPath Draw(Vector3 position )
    {
        Plane plane = new Plane(Camera.main.transform.forward * -1, position);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float distance;
        if (plane.Raycast(ray, out distance))
        {
            if (StartDrawing)
            {
                trailRendererPath.TrailRenderer = UnityEngine.Object.Instantiate(trailPrefab, ray.GetPoint(distance), Quaternion.identity);
                StartDrawing = false;
            }
            else
            {
                trailRendererPath.TrailRenderer.transform.position = ray.GetPoint(distance);
            }
            OnReadyEvent?.Invoke();
        }
        return trailRendererPath;
    }
}
