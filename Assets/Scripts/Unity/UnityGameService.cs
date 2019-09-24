using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnitGameEvents;

// This class joins unity runtime stuff to the game domain. It's coordinating
// input from the unity game controller with the domain model. 
// Probably can't instantiate this out of the runtime, but the majority of game
// logic should go in Game.cs and co.
// TODO: ObjectPool
public class UnityGameService
{

    private Game game = new Game();

    GameUpdate gameUpdate = new GameUpdate();

    public RectangleObject RectanglePrefab;
    public Shape CirclePrefab;
    public Shape TrianglePrefab;

    GameObject soldierPrefab;
    GameObject armyBasePrefab;

    TrailRenderer trailPrefab;
    PathRenderer pathRenderer;

    // TODO: Remove
    UnityTrailRendererPath trailRendererPath = new UnityTrailRendererPath();

    public void Initialize(RectangleObject rectanglePrefab, Shape circlePrefab, Shape trianglePrefab, TrailRenderer trailPrefab, GameObject soldierPrefab, GameObject armyBasePrefab)
    {
        this.RectanglePrefab = rectanglePrefab;
        this.TrianglePrefab = trianglePrefab;
        this.CirclePrefab = circlePrefab;
        this.trailPrefab = trailPrefab;
        this.soldierPrefab = soldierPrefab;
        this.armyBasePrefab = armyBasePrefab;

        pathRenderer = new PathRenderer(trailPrefab);
        pathRenderer.OnReadyEvent += PathRenderer_OnReadyEvent;

        game.Initialize();

        // TODO: This should just be another event eventually. 
        DrawMap();
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
        GameUpdateResult fu = game.Update(gameUpdate);
        HandleUnitEvents(fu.UnitEvents);

        gameUpdate = new GameUpdate();
    }

    void HandleUnitEvents(List<UnitEvent> events)
    {
        // TODO: Create this each time? And why is it a soldier renderer?
        SoldierRenderer soldierRenderer = new SoldierRenderer(soldierPrefab);
        foreach (dynamic ue in events)
        {
            if (ue is UnitCreatedEvent)
            {
                // TODO: Think about this special case.
                HandleUnitCreatedEvent(ue);

            }
            else
            {
                soldierRenderer.HandleEvent(ue);
            }
        }
    }

    public void GameTurnUpdate()
    {
        // FIXME:
        //RenderUnits(game.TurnUpdate());
    }

    private void HandleUnitCreatedEvent(UnitCreatedEvent unitCreatedEvent)
    {
        dynamic unit = unitCreatedEvent.Unit;
        unit.GameObject = RenderUnit(unit);
        game.OnUnitRenderedEvent(unit);
    }

    private void RenderUnits(List<UnitCreatedEvent> es)
    {
        // TODO: Is dynamic a smell here? It is nice...
        // Saves me messing around with interfaces.
        //foreach (dynamic unit in units)
        foreach(UnitCreatedEvent e in es)
        {
            dynamic unit = e.Unit;
            unit.GameObject = RenderUnit(unit);
            game.OnUnitRenderedEvent(unit);
        }
    }

    private void RenderUnits(List<Unit> units)
    {
        // TODO: Is dynamic a smell here? It is nice...
        // Saves me messing around with interfaces.
        foreach (dynamic unit in units)
        {
            unit.GameObject = RenderUnit(unit);
            game.OnUnitRenderedEvent(unit);
        }
    }




    private UnityGameServiceUpdate ParseInput(UnityGameServiceUpdate update)
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
            gameUpdate.UnityGameEvents.Add(new HomeBaseClickEvent(update.MousePos));
        }

        // TODO: Some cleanup with all these inputs
        bool clickAndDragging = ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved) || Input.GetMouseButton(0));

        if (!clickedInBase &&  clickAndDragging)
        {
            trailRendererPath = pathRenderer.Draw(update.MainBehaviour.transform.position);
        }

        gameUpdate.currentPath = trailRendererPath;

        // FIXME: Not sure about this update.
        return update;
    }


    public GameObject RenderUnit(Soldier soldier)
    {
        SoldierRenderer sr = new SoldierRenderer(soldierPrefab);
        GameObject soldierMono = sr.Draw(soldier.Position);
        BindSoldierEvents(sr, soldierMono, soldier);

        return soldierMono;
    }

    public GameObject RenderUnit(ArmyBase armyBase)
    {
        ArmyBaseRenderer abr = new ArmyBaseRenderer(this.armyBasePrefab);
        GameObject renderedBaseObject = abr.Draw(armyBase.Position, armyBase.Name);
        BindArmyBaseEvents(abr, renderedBaseObject, armyBase);
        return renderedBaseObject;
    }

    public void BindSoldierEvents(SoldierRenderer renderer, GameObject movingObject, Unit unit)
    {
        // These are unity game generated events.
        SoldierGameObject soldierGameObject = movingObject.GetComponent<SoldierGameObject>();
        soldierGameObject.OnCollisionEvent += HandleUnityGameEvent; 
        soldierGameObject.OnAnimationEvent += HandleUnityGameEvent;

        // These are game simulator events.
        unit.OnDamagedEvent += renderer.DrawDamage;
    }

    public void BindArmyBaseEvents(ArmyBaseRenderer renderer, GameObject movingObject, Unit unit)
    {
        unit.OnDamagedEvent += renderer.DrawDamage;

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
