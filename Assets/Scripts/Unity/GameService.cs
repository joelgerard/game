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

    private Game game = new Game();

    public RectangleObject RectanglePrefab;
    public Shape CirclePrefab;
    public Shape TrianglePrefab;

    //private GameObject playerBaseObject;

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

        // TODO: This should just be another event eventually. 
        DrawMap();
    }




    public void Update(GameServiceUpdate update)
    {
        // TODO: Need to call this once per frame?

        // Get the input, which is compiled into a game update object.
        // This includes events that the game will need to loop through, e.g.
        // a player has added a solder to the base.
        update = ParseInput(update);

        // Once the game has updated itself, it will return new objects that
        // are in the game but have not been drawn or created on the Unity side.
        RenderUnits(game.Update(update.GameUpdate));
    }

    private void RenderUnits(List<Unit> units)
    {
        foreach (Unit unit in units)
        {
            // TODO: Replace with generics?
            if (unit is Soldier)
            {
                unit.GameObject = RenderSoldier(unit as Soldier);
            }
            if (unit is ArmyBase)
            {
                unit.GameObject = RenderBase(unit as ArmyBase);
            }
            game.OnUnitRenderedEvent(unit);
        }
    }

    // TODO: This isn't used yet.
    public void GameTurnUpdate()
    {
        List<TurnUpdate> updates = game.TurnUpdate();
    }

    private GameServiceUpdate ParseInput(GameServiceUpdate update)
    {
        bool clickedInBase = false;

        Collider2D hitCollider = Physics2D.OverlapPoint(update.MousePos);

        clickedInBase = (hitCollider != null && hitCollider.attachedRigidbody.gameObject.name == "PlayerBaseSquare");


        if (update.MouseDown)
        {
            trailRendererPath.TrailRenderer = null;
        }

        // FIXME: STartDrawing is initialized improperly. 
        // If you draw before clicking, you're fucked. 
        pathRenderer.StartDrawing |= update.MouseUp;

        if (update.Click && clickedInBase)
        {
            update.GameUpdate.GameEvents.Add(new HomeBaseClickEvent(update.MousePos));
        }

        // TODO: Some cleanup with all these inputs
        bool clickAndDragging = ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved) || Input.GetMouseButton(0));

        if (!clickedInBase &&  clickAndDragging)
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

    public GameObject RenderBase(ArmyBase armyBase)
    {
        ArmyBaseRenderer abr = new ArmyBaseRenderer(this.RectanglePrefab);
        MoveableObject renderedBaseObject = abr.Draw(armyBase.Position, armyBase.Name);
        BindUnitEvents(abr, renderedBaseObject, armyBase);
        return renderedBaseObject.gameObject;
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
        RenderUnits(game.DrawMap());

        //AddEnemyBase(pos);

        // TODO: Remove add some soldiers.
        //AddSoldier(pos, Allegiance.ENEMY);


        // TODO: Dynamically create player base.


        // TODO: Clean up
        //game.Path.target = new Vector2(0f, -3f); //this.playerBaseObject.transform.position;
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
