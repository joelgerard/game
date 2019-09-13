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
    private Game game = new Game();

    public RectangleObject RectanglePrefab;
    public Shape CirclePrefab;
    public Shape TrianglePrefab;

    private GameObject playerBaseObject;

    TrailRenderer trailPrefab;
    PathRenderer pathRenderer;

    // TODO: Remove
    UnityTrailRendererPath trailRendererPath = new UnityTrailRendererPath();

    public void Initialize(RectangleObject rectanglePrefab, Shape circlePrefab, Shape trianglePrefab, TrailRenderer trailPrefab, GameObject playerBase)
    {
        Diagnostics.NotNull(playerBase, "PlayerBase");

        this.RectanglePrefab = rectanglePrefab;
        this.TrianglePrefab = trianglePrefab;
        this.CirclePrefab = circlePrefab;
        this.trailPrefab = trailPrefab;

        // TODO: Remove
        this.playerBaseObject = playerBase;

        pathRenderer = new PathRenderer(trailPrefab);
        pathRenderer.OnReadyEvent += PathRenderer_OnReadyEvent;

        game.Initialize();

        DrawMap();
    }




    public void Update(GameServiceUpdate update)
    {
        update = ParseInput(update);

        // TODO: Need to call this once per frame?
        List<Unit> createdUnits = game.Update(update.GameUpdate);
        foreach(Unit unit in createdUnits)
        {
            if (unit is Soldier)
            {
                unit.GameObject = RenderSoldier(unit as Soldier);
            }

            game.OnUnitRenderedEvent(unit);
        }
    }

    public void GameTurnUpdate()
    {
        List<TurnUpdate> updates = game.TurnUpdate();
        //foreach(TurnUpdate update in updates)
        //{
        //    if (update.Unit is Soldier soldier)
        //    {
        //        // TODO: Cleanup. Duplicated in a weird way. 
        //        SoldierRenderer sr = new SoldierRenderer(RectanglePrefab);
        //        MoveableObject soldierMono = sr.Draw(soldier.Position,soldier.Name);
        //        BindUnitEvents(sr, soldierMono, soldier);

        //        // TODO: Move out of here. Part of AI. 
        //        soldier.TargetPosition = this.playerBaseObject.transform.position; //this.game.Player.ArmyBase.Position;
        //        soldier.StartMoving();
        //    }
        //}
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
            update.GameUpdate.GameEvents.Add(new HomeBaseClickEvent(update.MousePos));
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


    public GameObject RenderSoldier(Soldier soldier)
    {
        // TODO: Be more dry?
        SoldierRenderer sr = new SoldierRenderer(RectanglePrefab);
        MoveableObject soldierMono = sr.Draw(soldier.Position);
        BindUnitEvents(sr, soldierMono, soldier);

        // TODO: Remove. Temp
        if (soldier.Allegiance == Allegiance.ENEMY)
        {
            soldier.StartMoving();
        }
        return soldierMono.gameObject;
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

        // TODO: Remove add some soldiers.
        //AddSoldier(pos, Allegiance.ENEMY);
       

        // TODO: Dynamically create player base.


        // TODO: Clean up
        game.Path.target = this.playerBaseObject.transform.position;
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
