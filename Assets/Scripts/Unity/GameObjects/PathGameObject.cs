using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathGameObject : MonoBehaviour, IPath
{
    List<Vector3> points = new List<Vector3>();
    LineRenderer lineRenderer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DestroyList()
    {
        points.Clear();
        lineRenderer.positionCount = 0;
        lineRenderer.SetPositions(points.ToArray());
        Destroy(lineRenderer.gameObject);
    }

    public Vector2 GetPosition(int index)
    {
        return points[index];
    }

    public void AddPosition(Vector3 position)
    {
        points.Add(position);
        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPositions(points.ToArray());

    }

    public int Count()
    {
        return points.Count;
    }
}
