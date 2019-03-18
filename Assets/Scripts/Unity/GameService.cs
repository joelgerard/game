using System.Collections;
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



    public void Initialize(RectangleObject rectanglePrefab, Shape circlePrefab, Shape trianglePrefab, TrailRenderer trailPrefab, RectangleObject playerBase)
    {
        Diagnostics.NotNull(playerBase, "PlayerBase");

        this.RectanglePrefab = rectanglePrefab;
        this.TrianglePrefab = trianglePrefab;
        this.CirclePrefab = circlePrefab;
        this.trailPrefab = trailPrefab;

        pathRenderer = new PathRenderer(trailPrefab);
        pathRenderer.OnReadyEvent += PathRenderer_OnReadyEvent;

        game.Initialize();

        DrawMap();
    }


    public void Update(GameServiceUpdate update)
    {
        update = ParseInput(update);

        // TODO: Need to call this once per frame?
        game.Update(update.GameUpdate);
    }

    public void GameTurnUpdate()
    {
        List<TurnUpdate> updates = game.TurnUpdate();
        foreach(TurnUpdate update in updates)
        {
            if (update.Unit is Soldier soldier)
            {
                // TODO: Cleanup. Duplicated in a weird way. 
                SoldierRenderer sr = new SoldierRenderer(RectanglePrefab);
                MoveableObject soldierMono = sr.Draw(soldier.Position,soldier.Name);
                BindUnitEvents(sr, soldierMono, soldier);

                // TODO: Move out of here. Part of AI. 
                soldier.TargetPosition = this.game.Player.ArmyBase.Position;
                soldier.StartMoving();
            }
        }
    }

    private GameServiceUpdate ParseInput(GameServiceUpdate update)
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
            PlayerBase_OnClick(update.MousePos);
        }

        // TODO: Some cleanup with all these inputs
        bool clickAndDragging = ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved) || Input.GetMouseButton(0));

        if (!clickedInBase && clickAndDragging)
        {
            trailRendererPath = pathRenderer.Draw(update.MainBehaviour.transform.position);
        }

        update.GameUpdate.currentPath = trailRendererPath;
        return update;
    }

    // TODO: Need to decide on how this goes. 
    // Maybe it should be: Input => Game => Renderer
    // right now it's Input => Renderer => Game
    // Or maybe, the AI should drive the clicks in the same way a PLayer does?
    // Seems awkward. 
    public void AddSoldier(Vector2 position)
    {
        // TODO: Be more dry?
        SoldierRenderer sr = new SoldierRenderer(RectanglePrefab);
        MoveableObject soldierMono = sr.Draw(position);

        Soldier soldier = game.OnAddSoldier(soldierMono.gameObject);

        BindUnitEvents(sr, soldierMono, soldier);
    }

    public void AddEnemyBase(Vector2 position)
    {
        ArmyBaseRenderer abr = new ArmyBaseRenderer(this.RectanglePrefab);
        MoveableObject enemyBase = abr.Draw(position, "EnemyBaseSquare");

        // TODO: Blerg. Shouldn't the game build itself?
        game.Enemy.ArmyBase.GameObject = enemyBase.gameObject;
        enemyBase.name = enemyBase.gameObject.name;

        BindUnitEvents(abr, enemyBase, game.Enemy.ArmyBase);
    }

    public void BindUnitEvents(IUnitRenderer renderer, MoveableObject movingObject, Unit unit)
    {
        movingObject.OnEnterEvent += Shape_OnEnterEvent;
        unit.OnDamagedEvent += renderer.DrawDamage;
        unit.OnDestroyedEvent += renderer.DrawDestroyed;
    }

    // TODO: Move out of here once Map is more complicated.
    protected void DrawMap()
    {
        Vector2 pos = new Vector2
        {
            x = 0.2f,
            y = 3f
        };
        AddEnemyBase(pos);

        // TODO: Dynamically create player base.
        

        // TODO: Clean up
        game.Path.target = pos;
    }

    void PlayerBase_OnClick(Vector2 pos)
    {
        AddSoldier(pos);
    }

    void Shape_OnEnterEvent(GameObject thisObject, GameObject otherObject)
    {
        game.OnUnitsCollide(thisObject.name, otherObject.name);
    }

    void PathRenderer_OnReadyEvent()
    {
        // TODO: Give it the path?
        game.OnPathReady();
    }

}
