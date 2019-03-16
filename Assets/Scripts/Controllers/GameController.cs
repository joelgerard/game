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
    public GameService gameService = new GameService();
    public RectangleObject RectanglePrefab;
    public Shape CirclePrefab;
    public Shape TrianglePrefab;



    [SerializeField] TrailRenderer trailPrefab;



    private void Awake()
    {
    }

    void Start()
    {
        // TODO: Is this slow, canonical unity way of doing things? Use fixedUpdate?
        InvokeRepeating("OutputTime", 0.1f, 0.1f);  //1s delay, repeat every 1s
        gameService.Initialize(RectanglePrefab, CirclePrefab, TrianglePrefab, trailPrefab);
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

        GameUpdate update = new GameUpdate
        { 
            deltaTime = Time.deltaTime
        };
        GameServiceUpdate gsUpdate = new GameServiceUpdate
        {
            GameUpdate = update,
            MouseUp = mouseUp,
            MouseDown = mouseDown,
            MousePos = mousePos,
            Click = click,
            MainBehaviour = this
        };
        gameService.Update(gsUpdate);

        //Collider2D hitCollider = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        //bool clickedInBase = (hitCollider != null && hitCollider.CompareTag("LaunchPad"));

        //if (hitCollider != null)
        //{
        //    GameController.Log("y n ");
        //}
    }
}