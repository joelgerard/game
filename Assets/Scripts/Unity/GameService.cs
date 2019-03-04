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

    public DrawShape RectanglePrefab;
    public DrawShape CirclePrefab;
    public DrawShape TrianglePrefab;
    TrailRenderer trailPrefab;
    PathRenderer pathRenderer;

    // TODO: Remove
    UnityTrailRendererPath trailRendererPath = new UnityTrailRendererPath();



    public void Initialize(DrawShape rectanglePrefab, DrawShape circlePrefab, DrawShape trianglePrefab, TrailRenderer trailPrefab)
    {
        this.RectanglePrefab = rectanglePrefab;
        this.TrianglePrefab = trianglePrefab;
        this.CirclePrefab = circlePrefab;
        this.trailPrefab = trailPrefab;

        pathRenderer = new PathRenderer(trailPrefab);
        pathRenderer.OnReadyEvent += PathRenderer_OnReadyEvent;


        game.Initialize();

    }

    void PathRenderer_OnReadyEvent()
    {
        // TODO: Hmmm... Weak. Should this directly control the game?
        game.Player.StartMoving();
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

        bool clickAndDragging = ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved) || Input.GetMouseButton(0));

        // TODO: Some cleanup with all these inputs. 
        if (!clickedInBase && clickAndDragging)
        {
            // Start drawing path.
            trailRendererPath = pathRenderer.Draw(update.MainBehaviour.transform.position);
        }

        update.GameUpdate.currentPath = trailRendererPath;

        // TODO: Need to call this once per frame?
        game.Update(update.GameUpdate);
    }

    public void AddSoldier(Vector2 position)
    {
        SoldierRenderer sr = new SoldierRenderer();
        DrawShape soldierMono = sr.Draw(RectanglePrefab, position);
        soldierMono.OnEnterEvent += UnitMono_OnEnterEvent;

        game.OnAddSoldier(soldierMono.gameObject);
    }

    void UnitMono_OnEnterEvent(GameObject thisObject, GameObject otherObject)
    {
        game.OnUnitsCollide(thisObject.name, otherObject.name);
    }


}
