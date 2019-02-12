using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <inheritdoc />
/// <summary>
/// Listens for input from the mouse, where shapes are created and updated by 
/// the current cursor position.
/// </summary>
public class GameController : MonoBehaviour
{
    public DrawMode Mode = DrawMode.Rectangle;
    public Game Game = new Game();
    public DrawShape RectanglePrefab;
    public DrawShape CirclePrefab;
    public DrawShape TrianglePrefab;


    // Associates a draw mode to the prefab to instantiate
    private Dictionary<DrawMode, DrawShape> _drawModeToPrefab;

    private readonly List<DrawShape> _allShapes = new List<DrawShape>();

    private DrawShape CurrentShapeToDraw { get; set; }
    private bool IsDrawingShape { get; set; }
    
    private void Awake()
    {
        _drawModeToPrefab = new Dictionary<DrawMode, DrawShape> {
            {DrawMode.Rectangle, RectanglePrefab},
            {DrawMode.Circle, CirclePrefab},
            {DrawMode.Triangle, TrianglePrefab}
        };
    }

    void Start()
    {
        // TODO: Is this slow?
        InvokeRepeating("OutputTime", 0.1f, 0.1f);  //1s delay, repeat every 1s
    }

    void OutputTime()
    {
        //Debug.Log(Time.time);
        if (_allShapes.Count > 0)
        {
            Debug.Log(Time.time);

            foreach(DrawShape shape in _allShapes)
            {
                RaycastHit2D rh;
                //shape.Move(0, 1, out rh);
            }

            
        }
    }

    private void Update()
    {
        var mousePos = (Vector2) Camera.main.ScreenToWorldPoint(Input.mousePosition);

        var click = Input.GetKeyUp(KeyCode.Mouse0) &&
                    !EventSystem.current.IsPointerOverGameObject();
        var canUpdateShape = CurrentShapeToDraw != null && IsDrawingShape;

        if (click) {
            AddShapeVertex(mousePos);
        } else if (canUpdateShape) {
            UpdateShapeVertex(mousePos);
        }
    }

    /// <summary>
    /// Adds a new vertex to the current shape at the given position, 
    /// or creates a new shape if it doesn't exist
    /// </summary>
    private void AddShapeVertex(Vector2 position)
    {
        if (CurrentShapeToDraw == null) {
            // No current shape -> instantiate a new shape and add two vertices:
            // one for the initial position, and the other for the current cursor
            var prefab = _drawModeToPrefab[Mode];
            CurrentShapeToDraw = Instantiate(prefab);
            CurrentShapeToDraw.name = "Shape " + _allShapes.Count;

            CurrentShapeToDraw.AddVertex(position);
            CurrentShapeToDraw.AddVertex(position);

            position.x += 0.1f;
            position.y += 0.1f;
            CurrentShapeToDraw.AddVertex(position);

            IsDrawingShape = true;
            CurrentShapeToDraw.SimulatingPhysics = false;

            _allShapes.Add(CurrentShapeToDraw);
            CurrentShapeToDraw.UpdateShape(position);
            CurrentShapeToDraw = null;
            IsDrawingShape = false;
        } else {
            // Current shape exists -> add vertex if finished, 
            // otherwise start physics simulation and reset reference
            IsDrawingShape = !CurrentShapeToDraw.ShapeFinished;

            if (IsDrawingShape) {
                CurrentShapeToDraw.AddVertex(position);
            } else {
                CurrentShapeToDraw.SimulatingPhysics = false;
                CurrentShapeToDraw = null;
            }
        }
    }

    /// <summary>
    /// Updates the current shape's latest vertex position to allow
    /// a shape to be updated with the mouse cursor and redrawn
    /// </summary>
    private void UpdateShapeVertex(Vector2 position)
    {
        if (CurrentShapeToDraw == null) {
            return;
        }

        CurrentShapeToDraw.UpdateShape(position);
    }

    /// <summary>
    /// Controlled via Unity GUI button
    /// </summary>
    public void SetDrawMode(string mode)
    {
        Mode = (DrawMode) Enum.Parse(typeof(DrawMode), mode);
    }

    /// <summary>
    /// The types of shapes that can be drawn, useful for
    /// selecting shapes to draw
    /// </summary>
    public enum DrawMode
    {
        Rectangle,
        Circle,
        Triangle
    }
}