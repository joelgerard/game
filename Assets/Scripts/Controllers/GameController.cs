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
    public DrawMode Mode = DrawMode.Rectangle;
    public Game Game = new Game();
    public DrawShape RectanglePrefab;
    public DrawShape CirclePrefab;
    public DrawShape TrianglePrefab;
    public bool go = false;

    public bool okToDrawTrail = true;

    public List<Soldier> soldiers = new List<Soldier>();
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
        Game.Initialize();

        DrawRectangle enemyBaseSquare = GameObject.Find("EnemyBaseSquare").gameObject.GetComponent<DrawRectangle>();
        ArmyBase enemyBase = new ArmyBase
        {
            Allegiance = Allegiance.ENEMY
        };
        enemyBase.Init();
        unitMap.Add(enemyBaseSquare.name, enemyBase);

        DrawRectangle launchPad = GameObject.Find("PlayerBaseSquare").gameObject.GetComponent<DrawRectangle>();
        ArmyBase playerBase = new ArmyBase
        {
            Allegiance = Allegiance.ALLY
        };
        playerBase.Init();
        unitMap.Add(launchPad.name, playerBase);

    }

    void OutputTime()
    {

        this.navigator.MoveUnits(this.soldiers, trailRendererPath);

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
            Add(mousePos);
        } 

        if (!clickedInBase && ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved) || Input.GetMouseButton(0)))
        {
            Plane plane = new Plane(Camera.main.transform.forward * -1, this.transform.position);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float distance;
            if (plane.Raycast(ray, out distance))
            {
                if (okToDrawTrail)
                {
                    trailRendererPath.TrailRenderer = Instantiate(trailPrefab, ray.GetPoint(distance), Quaternion.identity);
                    okToDrawTrail = false;
                }
                else
                {
                    trailRendererPath.TrailRenderer.transform.position = ray.GetPoint(distance);
                }
                foreach (Soldier soldier in soldiers)
                {
                    soldier.StartMoving();
                }
            }
        }

        foreach (Unit unit in unitMap.Values)
        {
            unit.Update(Time.deltaTime);
        }

    }

    /// <summary>
    /// Adds a new vertex to the current shape at the given position, 
    /// or creates a new shape if it doesn't exist
    /// </summary>
    private void Add(Vector2 position)
    {
        SoldierRenderer sr = new SoldierRenderer();
        DrawShape soldierMono = sr.Draw(RectanglePrefab, position);

        // TODO: Is this needed?
        //_allShapes.Add(soldierMono);
        soldierMono.OnEnterEvent += UnitMono_OnEnterEvent;

        Soldier soldier = new Soldier(soldierMono.gameObject)
        {
            Allegiance = Allegiance.ALLY
        };
        soldier.Init();
        soldiers.Add(soldier);
        unitMap.Add(soldierMono.name, soldier);
    }

    void UnitMono_OnEnterEvent(GameObject thisObject, GameObject otherObject)
    {

        Unit unit = unitMap[thisObject.name];
        Unit otherUnit = unitMap[otherObject.name];
        unit.Attack(otherUnit);
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