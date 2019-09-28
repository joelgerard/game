using System;
using UnityEngine;

public class PathRenderer
{
    PathGameObject pathGameObject = null;

    public event OnReady OnReadyEvent;
    public delegate void OnReady();

    readonly LineRenderer lineRendererPrefab;

    public bool StartDrawing { get; set; }


    public PathRenderer(LineRenderer linePrefab)
    {
        this.lineRendererPrefab = linePrefab;
    }

    // TODO: rename/remove okToDrawTrail
    public PathGameObject Draw(Vector3 position )
    {
        Plane plane = new Plane(Camera.main.transform.forward * -1, position);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if ( plane.Raycast(ray, out float distance))
        {
            if (StartDrawing)
            {
                pathGameObject =
                 UnityEngine.Object.Instantiate(lineRendererPrefab, new Vector3(0, 0, 0), Quaternion.identity).GetComponent<PathGameObject>();
                StartDrawing = false;
            }
            else 
            {
                if (pathGameObject != null)
                {
                    // TODO: Check distance from points.
                    Vector3 hitpoint = ray.GetPoint(distance);
                    pathGameObject.AddPosition(hitpoint);
                }
            }
            OnReadyEvent?.Invoke();
        }
        return pathGameObject;
    }
}
