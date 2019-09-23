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

    GameUpdate gameUpdate = new GameUpdate();

    public RectangleObject RectanglePrefab;
    public Shape CirclePrefab;
    public Shape TrianglePrefab;

    GameObject allyPrefab;
    TrailRenderer trailPrefab;
    PathRenderer pathRenderer;

    // TODO: Remove
    UnityTrailRendererPath trailRendererPath = new UnityTrailRendererPath();

    public void Initialize(RectangleObject rectanglePrefab, Shape circlePrefab, Shape trianglePrefab, TrailRenderer trailPrefab, GameObject allyPrefab)
    {
        this.RectanglePrefab = rectanglePrefab;
        this.TrianglePrefab = trianglePrefab;
        this.CirclePrefab = circlePrefab;
        this.trailPrefab = trailPrefab;
        this.allyPrefab = allyPrefab;

        pathRenderer = new PathRenderer(trailPrefab);
        pathRenderer.OnReadyEvent += PathRenderer_OnReadyEvent;

        game.Initialize();

        // TODO: This should just be another event eventually. 
        DrawMap();
    }

    public void Update(GameServiceUpdate update)
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
        FrameUpdate fu = game.Update(gameUpdate);

        RenderUnits(fu.CreatedEvents);
        HandleUnitEvents(fu.UnitEvents);

        gameUpdate = new GameUpdate();
    }

    void HandleUnitEvents(List<UnitEvent> events)
    {
        // TODO: Create this each time?
        SoldierRenderer sr = new SoldierRenderer(allyPrefab);
        foreach (dynamic ue in events)
        {
            sr.HandleEvent(ue);
        }
    }

    public void GameTurnUpdate()
    {
        // FIXME:
        //RenderUnits(game.TurnUpdate());
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
            gameUpdate.GameEvents.Add(new HomeBaseClickEvent(update.MousePos));
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
        // TODO: This renders a square. I'm figuring out unity. Fix.
        //SoldierRenderer sr = new SoldierRenderer(RectanglePrefab);

        SoldierRenderer sr = new SoldierRenderer(allyPrefab);


        MoveableObjectWrapper soldierMono = sr.Draw(soldier.Position);
        BindUnitEvents(sr, soldierMono, soldier);

        // TODO: Remove. Temp
        if (soldier.Allegiance == Allegiance.ENEMY)
        {
            soldier.StartMoving();
        }
        return soldierMono.GameObject;
    }

    public GameObject RenderUnit(ArmyBase armyBase)
    {
        ArmyBaseRenderer abr = new ArmyBaseRenderer(this.RectanglePrefab);
        MoveableObject renderedBaseObject = abr.Draw(armyBase.Position, armyBase.Name);
        BindUnitEvents(abr, renderedBaseObject, armyBase);
        return renderedBaseObject.gameObject;
    }

    public void BindUnitEvents(IUnitRenderer renderer, MoveableObjectWrapper movingObject, Unit unit)
    {
        // TODO: Need bindings on sprite. 
        if (movingObject.MoveableObject != null)
        {
            movingObject.MoveableObject.OnEnterEvent += Shape_OnEnterEvent;
        }
        else
        {
            // TODO: How to detect the collider has collided?
            SoldierGameObject soldierController = movingObject.GameObject.GetComponent<SoldierGameObject>();
            soldierController.OnCollisionEvent += Shape_OnEnterEvent;
            soldierController.OnAnimationEvent += SoldierController_OnAnimationEvent;
        }
        unit.OnDamagedEvent += renderer.DrawDamage;
        unit.OnDestroyedEvent += renderer.DrawDestroyed;
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
    }

    // TODO: Not a very good name for this event. 
    // TODO: Really, you could generalize this to an Event Object, same as the others.
    void Shape_OnEnterEvent(GameObject thisObject, GameObject otherObject)
    {
        // TODO: This first line isn't neccessary. Just give the game objects
        // in the event, and the lookup can be done later. Even better
        // this whole event function can go away and become something general.
        // It will mean that the UnitsCollideEvent will be renamed
        // and the properties inside will be gameobjects.
        //GameEvent gameEvent = game.OnUnitsCollide(thisObject.name, otherObject.name);

        UnitsCollideEvent gameEvent = new UnitsCollideEvent(thisObject.name, otherObject.name);
        gameUpdate.GameEvents.Add(gameEvent);
    }

    void SoldierController_OnAnimationEvent(GameObject gameObject, int animationId)
    {
        // TODO: Replace with Enum/CONST
        if (animationId == 1)
        {
            UnitExplosionComplete gameEvent = new UnitExplosionComplete
            {
                UnitName = gameObject.name
            };
            gameUpdate.GameEvents.Add(gameEvent);
    }
    }

    void PathRenderer_OnReadyEvent()
    {
        // TODO: Give it the path?
        game.OnPathReady();
    }

}
