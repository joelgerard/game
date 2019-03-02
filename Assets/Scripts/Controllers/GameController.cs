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


    public bool okToDrawTrail = true;
    [SerializeField] TrailRenderer trailPrefab;

    UnityTrailRendererPath trailRendererPath = new UnityTrailRendererPath();

    private void Awake()
    {
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
                foreach (Soldier soldier in game.soldiers)
                {
                    soldier.StartMoving();
                }
            }
        }

        GameUpdate update = new GameUpdate
        {
            currentPath = trailRendererPath,
            deltaTime = Time.deltaTime
        };
        game.Update(update);


    }
}