using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <inheritdoc />
/// <summary>
/// Listens for input from the mouse, where shapes are created and updated by 
/// the current cursor position.
/// </summary>
public class GameController : MonoBehaviour
{
    public Game game = new Game();
    public DrawShape RectanglePrefab;
    public DrawShape CirclePrefab;
    public DrawShape TrianglePrefab;
    public bool go = false;

    public bool okToDrawTrail = true;

    Dictionary<string, Unit> unitMap = new Dictionary<string, Unit>();

    [SerializeField] TrailRenderer trailPrefab;

    Navigator navigator = new Navigator();
    UnityTrailRendererPath trailRendererPath = new UnityTrailRendererPath();



    // Associates a draw mode to the prefab to instantiate
    private Dictionary<DrawMode, DrawShape> _drawModeToPrefab;

    private readonly List<MonoBehaviour> _allShapes = new List<MonoBehaviour>();


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
        // TODO: Is this slow, canonical unity way of doing things?
        InvokeRepeating("OutputTime", 0.1f, 0.1f);  //1s delay, repeat every 1s
        game.Initialize(RectanglePrefab, CirclePrefab, TrianglePrefab);
    }

    void OutputTime()
    {
        // TODO: Maybe this should just go in update?
        // this.navigator.MoveUnits(game.soldiers, trailRendererPath);
    }

    public static void Log(string msg)
    {
        Debug.Log(msg);
    }

    private void Update()
    {

        var mousePos = (Vector2) Camera.main.ScreenToWorldPoint(Input.mousePosition);


        var click = Input.GetKeyUp(KeyCode.Mouse0) &&
                    !EventSystem.current.IsPointerOverGameObject();

        var mouseDown = Input.GetKeyDown(KeyCode.Mouse0);
        var mouseUp = Input.GetKeyUp(KeyCode.Mouse0);

        // TODO: Change to enum
        bool clickedInBase = false;

        Collider2D hitCollider = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        clickedInBase = (hitCollider != null && hitCollider.CompareTag("LaunchPad"));

        if (mouseDown)
        {
            trailRendererPath.TrailRenderer = null;
        }

        okToDrawTrail |= mouseUp;

        if (click && clickedInBase) {
            game.AddSoldier(mousePos);
        } 

        if (!clickedInBase && ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved) || Input.GetMouseButton(0)))
        {
            Debug.Log("Clicked out of base");
            Plane plane = new Plane(Camera.main.transform.forward * -1, this.transform.position);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float distance;
            if (plane.Raycast(ray, out distance))
            {
                Debug.Log("Drawing " + game.soldiers.Count.ToString());
                if (okToDrawTrail)
                {
                    trailRendererPath.TrailRenderer = Instantiate(trailPrefab, ray.GetPoint(distance), Quaternion.identity);
                    okToDrawTrail = false;
                }
                else
                {
                    trailRendererPath.TrailRenderer.transform.position = ray.GetPoint(distance);
                }
                foreach (Soldier soldier in game.soldiers)
                {

                    soldier.StartMoving();
                    Debug.Log("Move");
                }
            }
        }

        // TODO: This has kind of moved to the game object
        foreach (Unit unit in unitMap.Values)
        {
            unit.Update(Time.deltaTime);
        }

        // TODO: Move to game?
        foreach (Soldier soldier in game.soldiers)
        {
            soldier.StateMachine.ProcessStateTransitions();
        }
        navigator.MoveUnits(game.soldiers, trailRendererPath);

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