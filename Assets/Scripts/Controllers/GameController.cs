﻿using System;
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
    public UnityGameService gameService = new UnityGameService();
    public RectangleObject RectanglePrefab;
    public Shape CirclePrefab;
    public Shape TrianglePrefab;
    public GameObject SoldierPrefab;
    public GameObject ArmyBasePrefab;
    public GameObject GreenOrbPrefab;
    public GameObject MapImage;
    public TextAsset Map;



    [SerializeField] TrailRenderer trailPrefab;
    [SerializeField] public LineRenderer LinePrefab;
    //[SerializeField] RectangleObject PlayerBaseSquare;


    private void Awake()
    {
    }

    void Start()
    {
        // TODO: Is this slow, canonical unity way of doing things? Use fixedUpdate?
        InvokeRepeating("GameTurnUpdate", 0.1f, 0.1f);  //1s delay, repeat every 1s

        MapImage = GameObject.Find("MapImage");
        //RectanglePrefab, CirclePrefab, TrianglePrefab, trailPrefab, SoldierPrefab, ArmyBasePrefab, LinePrefab

        gameService.Initialize(this); //, baseObj);


    }

    void GameTurnUpdate()
    {
        gameService.GameTurnUpdate();
    }

    public static void Log(string msg)
    {
        Debug.Log(msg);
    }

    public static void LogWarning(string msg)
    {
        Debug.LogWarning(msg);
    }

    private void Update()
    {
        // FIXME: Doing this here??? Old code from demo. Remove and fix.
        var mousePos = (Vector2) Camera.main.ScreenToWorldPoint(Input.mousePosition);


        var click = Input.GetKeyUp(KeyCode.Mouse0) &&
                    !EventSystem.current.IsPointerOverGameObject();

        var mouseDown = Input.GetKeyDown(KeyCode.Mouse0);
        var mouseUp = Input.GetKeyUp(KeyCode.Mouse0);


        UnityGameServiceUpdate gsUpdate = new UnityGameServiceUpdate
        {
            DeltaTime = Time.deltaTime,
            MouseUp = mouseUp,
            MouseDown = mouseDown,
            MousePos = mousePos,
            Click = click,
            MainBehaviour = this
        };
        gameService.Update(gsUpdate);
    }
}