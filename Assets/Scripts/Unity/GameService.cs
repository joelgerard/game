﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class joins unity runtime stuff to the game domain. It's coordinating
// input from the unity game controller with the domain model. 
// Probably can't instantiate this out of the runtime, but the majority of game
// logic should go in Game.cs and co.
// TODO: ObjectPool
public class GameService
{

    // TODO: Make private
    public Game game = new Game();

    public RectangleObject RectanglePrefab;
    public Shape CirclePrefab;
    public Shape TrianglePrefab;
    TrailRenderer trailPrefab;
    PathRenderer pathRenderer;

    // TODO: Remove
    UnityTrailRendererPath trailRendererPath = new UnityTrailRendererPath();



    public void Initialize(RectangleObject rectanglePrefab, Shape circlePrefab, Shape trianglePrefab, TrailRenderer trailPrefab)
    {
        this.RectanglePrefab = rectanglePrefab;
        this.TrianglePrefab = trianglePrefab;
        this.CirclePrefab = circlePrefab;
        this.trailPrefab = trailPrefab;

        pathRenderer = new PathRenderer(trailPrefab);
        pathRenderer.OnReadyEvent += PathRenderer_OnReadyEvent;

        game.Initialize();

        DrawMap();

        // TODO: Fix this. I guess we can bind all these things at once.
        // Must be a better way to do this.
        ArmyBaseRenderer abr = new ArmyBaseRenderer(this.RectanglePrefab);
        game.Enemy.ArmyBase.OnDamagedEvent += abr.DrawDamage;
        game.Enemy.ArmyBase.OnDestroyedEvent += abr.DrawDestroyed;
    }

    void PathRenderer_OnReadyEvent()
    {
        // TODO: Give it the path?
        game.OnPathReady();
    }


    public void Update(GameServiceUpdate update)
    {
        bool clickedInBase = false;

        Collider2D hitCollider = Physics2D.OverlapPoint(update.MousePos);
        clickedInBase = (hitCollider != null && hitCollider.CompareTag("LaunchPad"));

        if (update.MouseDown)
        {
            trailRendererPath.TrailRenderer = null;
        }

        pathRenderer.StartDrawing |= update.MouseUp;

        if (update.Click && clickedInBase)
        {
            AddSoldier(update.MousePos);
        }

        // TODO: Some cleanup with all these inputs
        bool clickAndDragging = ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved) || Input.GetMouseButton(0));

        if (!clickedInBase && clickAndDragging)
        {
            trailRendererPath = pathRenderer.Draw(update.MainBehaviour.transform.position);
        }

        update.GameUpdate.currentPath = trailRendererPath;

        // TODO: Need to call this once per frame?
        game.Update(update.GameUpdate);
    }

    public void AddSoldier(Vector2 position)
    {
        SoldierRenderer sr = new SoldierRenderer();
        Shape soldierMono = sr.Draw(RectanglePrefab, position);
        soldierMono.OnEnterEvent += UnitMono_OnEnterEvent;

        Soldier soldier = game.OnAddSoldier(soldierMono.gameObject);
        soldier.OnDamagedEvent+=sr.DrawDamage;
        soldier.OnDestroyedEvent += sr.DrawDestroyed;
    }

    // TODO: Move out of here.
    protected void DrawMap()
    {
        ArmyBaseRenderer abr = new ArmyBaseRenderer(this.RectanglePrefab);
        Vector2 pos = new Vector2
        {
            x = 0.2f,
            y = 3f
        };
        Shape enemyBase = abr.Draw(RectanglePrefab, pos, "EnemyBaseSquare");

        // TODO: Blerg. Shouldn't the game build itself?
        game.Enemy.ArmyBase.GameObject = enemyBase.gameObject;
        enemyBase.OnEnterEvent += UnitMono_OnEnterEvent;
        enemyBase.name = enemyBase.gameObject.name;
    }


    void UnitMono_OnEnterEvent(GameObject thisObject, GameObject otherObject)
    {
        game.OnUnitsCollide(thisObject.name, otherObject.name);
    }


}
