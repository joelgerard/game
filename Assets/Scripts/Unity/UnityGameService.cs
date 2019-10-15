﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static GameEvents;
using static UnitGameEvents;

// This class joins unity runtime stuff to the game domain. It's coordinating
// input from the unity game controller with the domain model. 
// Probably can't instantiate this out of the runtime, but the majority of game
// logic should go in Game.cs and co.
// TODO: ObjectPool
public class UnityGameService
{

    private Game game = new Game();

    const string GREEN_PATH_ORIGIN = "GreenPathOrigin";

    GameUpdate gameUpdate = new GameUpdate();

    GameController gameController;

    private GameObject gameOverPanel;

    SoldierRenderer soldierRenderer;
    PathRenderer pathRenderer;

    // Replace when IL2CPP introduces 'dynamic' keyword support.
    Dictionary<Type, Func<Unit, GameObject>> UnitRenderers;
    Dictionary<Type, Action<GameEvent>> RenderedEventHandlers;

    public UnityGameService()
    {
        UnitRenderers = new Dictionary<Type, Func<Unit, GameObject>>
        {
            {typeof (Soldier), (unit) =>
                this.RenderUnit((Soldier)unit) },
            {typeof (ArmyBase), (unit) =>
                this.RenderUnit((ArmyBase)unit) },
            {typeof (Thing), (unit) =>
                this.RenderUnit((Thing)unit) },

        };

        RenderedEventHandlers = new Dictionary<Type, Action<GameEvent>>
        {
            {typeof (UnitCreatedEvent), (createdEvent) =>
                this.HandleUnitCreatedEvent((UnitCreatedEvent)createdEvent) },
            {typeof (GameOverEvent), (gameOverEvent) =>
                this.HandleGameOverEvent((GameOverEvent)gameOverEvent) },
            {typeof (UnitDyingEvent), (unitDyingEvent) =>
                soldierRenderer.HandleEvent((UnitDyingEvent) unitDyingEvent) },
            {typeof (UnitDiedEvent), (unitDiedEvent) =>
                soldierRenderer.HandleEvent((UnitDiedEvent) unitDiedEvent) }
        };

    }

    public void Initialize(GameController gameController)
    {
        this.gameController = gameController;

        soldierRenderer = new SoldierRenderer(gameController.SoldierPrefab);

        pathRenderer = new PathRenderer(gameController.LinePrefab);
        pathRenderer.OnReadyEvent += PathRenderer_OnReadyEvent;

        game.Initialize();

        // TODO: This should just be another event eventually. 
        DrawMap();




        UnityEngine.UI.Button resetButton = GameObject.Find("ResetButton").GetComponent<UnityEngine.UI.Button>();
        resetButton.onClick.AddListener(ResetButton_ClickEvent);

        gameOverPanel = GameObject.Find("GameOverPanel");
        gameOverPanel.SetActive(false);
    }

    public void ResetButton_ClickEvent()
    {
        GameController.Log("Click");
    }

    public void Update(UnityGameServiceUpdate update)
    {
        // Ohhh. This is confusing. Basically,
        // the update object is used by other events,
        // called outside of the update loop by unity.
        // It gets reset every frame. 
        gameUpdate.deltaTime = update.DeltaTime;

        // Get the input, which is compiled into a game update object.
        // This includes events that the game will need to loop through, e.g.
        // a player has added a solder to the base.
        update = ParseInput(update);

        // Once the game has updated itself, it will return new objects that
        // are in the game but have not been drawn or created on the Unity side.
        GameUpdateResult fu;
        fu = game.Update(gameUpdate);
        HandleUnitEvents(fu.GameEvents);

        gameUpdate = new GameUpdate();
    }

    public void GameTurnUpdate()
    {
        GameUpdateResult fu = game.TurnUpdate();
        HandleUnitEvents(fu.GameEvents);
    }

    void HandleUnitEvents(List<GameEvent> events)
    {
        //foreach (dynamic ue in events)
        foreach(GameEvent ue in events)
        {
            RenderedEventHandlers[ue.GetType()](ue);
        }
    }

    private void HandleGameOverEvent(GameOverEvent gameOverEvent)
    {
        string text = "You lost!";
        if (gameOverEvent.isPlayerWinner)
        {
            text = "You won!";
        }

        gameOverPanel.SetActive(true);
        UnityEngine.UI.Text gameOverText = GameObject.Find("GameOverText").GetComponent<UnityEngine.UI.Text>();
        gameOverText.text = text;
    }

    private void HandleUnitCreatedEvent(UnitCreatedEvent unitCreatedEvent)
    {
        Unit unit = unitCreatedEvent.Unit;
        unit.GameObject = UnitRenderers[unit.GetType()](unit); //RenderUnit(unit);
        game.OnUnitRenderedEvent(unit);
    }

    private void RenderUnits(List<Unit> units)
    {
        foreach (Unit unit in units)
        {
            unit.GameObject = UnitRenderers[unit.GetType()](unit); 
            game.OnUnitRenderedEvent(unit);
        }
    }




    private UnityGameServiceUpdate ParseInput(UnityGameServiceUpdate update)
    {
        bool clickedInBase = false;

        Collider2D hitCollider = Physics2D.OverlapPoint(update.MousePos);

        clickedInBase = (hitCollider != null && hitCollider.attachedRigidbody.gameObject.name == "PlayerBaseSquare");

        bool clickedInOrigin = (hitCollider != null && hitCollider.attachedRigidbody.gameObject.name == GREEN_PATH_ORIGIN);


        if (Input.GetButtonDown("Fire1") && clickedInOrigin)
        {
            GameController.Log("Click on " + hitCollider?.attachedRigidbody.gameObject.name + " " + clickedInOrigin);
            // Click, so start drawing a new line.

            //pathRenderer.pathGameObject = null;
            pathRenderer.StartDrawing = true;
            pathRenderer.StopDrawing = false;
        }
        if (Input.GetButton("Fire1") && !pathRenderer.StopDrawing)
        {
            //pathRenderer.StartDrawing = false;
            // Mouse is still down and we are dragging, so keep drawing.
            gameUpdate.currentPath = pathRenderer.Draw(update.MousePos); //(update.MainBehaviour.transform.position);
        }

        // FIXME: OMFG. My brain is fried. This isn't needed. Can calcualte this if the other
        // vars are set corrrectly.
        pathRenderer.StopDrawing |= update.MouseUp;

        //if (!clickedInBase && update.MouseDown)
        //{
        //    pathGameObject = null;
        //}

        //// FIXME: STartDrawing is initialized improperly. 
        //// If you draw before clicking, you're fucked. 
        //if (!clickedInBase)
        //{
        //    pathRenderer.StartDrawing |= update.MouseUp;
        //}

        gameUpdate.currentPath = pathRenderer.PathGameObject;

        if (update.Click && clickedInBase)
        {
            gameUpdate.UnityGameEvents.Add(new ClickInPlayerBaseEvent(update.MousePos, Allegiance.ALLY));
        }


        // TODO: Some cleanup with all these inputs
        bool clickAndDragging = ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved) || Input.GetMouseButton(0));

        if (!clickedInBase && clickAndDragging)
        {
            //pathGameObject = pathRenderer.Draw(update.MousePos); //(update.MainBehaviour.transform.position);
        }


        // FIXME: Not sure about this update.
        return update;
    }


    public GameObject RenderUnit(Soldier soldier)
    {
        SoldierRenderer sr = new SoldierRenderer(gameController.SoldierPrefab);
        GameObject soldierMono = sr.Draw(soldier.Position);
        BindSoldierEvents(sr, soldierMono, soldier);

        return soldierMono;
    }

    public GameObject RenderUnit(ArmyBase armyBase)
    {
        ArmyBaseRenderer abr = new ArmyBaseRenderer(gameController.ArmyBasePrefab);
        GameObject renderedBaseObject = abr.Draw(armyBase.Position, armyBase.Name);
        BindArmyBaseEvents(abr, renderedBaseObject, armyBase);
        return renderedBaseObject;
    }

    public GameObject RenderUnit(Thing thing)
    {
        SpriteRenderer spriteRenderer = new SpriteRenderer(gameController.GreenOrbPrefab);
        return spriteRenderer.Draw(thing.Position, GREEN_PATH_ORIGIN);
    }

    public void BindSoldierEvents(SoldierRenderer renderer, GameObject movingObject, Unit unit)
    {
        // These are unity game generated events.
        SoldierGameObject soldierGameObject = movingObject.GetComponent<SoldierGameObject>();
        soldierGameObject.OnCollisionEvent += HandleUnityGameEvent; 
        soldierGameObject.OnAnimationEvent += HandleUnityGameEvent;
    }

    public void BindArmyBaseEvents(ArmyBaseRenderer renderer, GameObject movingObject, Unit unit)
    {
        ArmyBaseUnityGameObject armyBaseGameObject = movingObject.GetComponent<ArmyBaseUnityGameObject>();
        armyBaseGameObject.OnCollisionEvent += HandleUnityGameEvent;

    }

    // TODO: Move out of here once Map is more complicated.
    protected void DrawMap()
    {
        RenderUnits(game.DrawMap());
    }

    void HandleUnityGameEvent(UnityGameEvent gameEvent)
    {
        gameUpdate.UnityGameEvents.Add(gameEvent);
    }

    void PathRenderer_OnReadyEvent()
    {
        // TODO: Give it the path?
        game.OnPathReady();
    }

}
