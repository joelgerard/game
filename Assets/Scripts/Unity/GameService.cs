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

    public bool okToDrawTrail = true;
    public DrawShape RectanglePrefab;
    public DrawShape CirclePrefab;
    public DrawShape TrianglePrefab;
    TrailRenderer trailPrefab;

    UnityTrailRendererPath trailRendererPath = new UnityTrailRendererPath();

    public void Initialize(DrawShape rectanglePrefab, DrawShape circlePrefab, DrawShape trianglePrefab, TrailRenderer trailPrefab)
    {
        this.RectanglePrefab = rectanglePrefab;
        this.TrianglePrefab = trianglePrefab;
        this.CirclePrefab = circlePrefab;
        this.trailPrefab = trailPrefab;
        game.Initialize();

    }

    public void Update(GameServiceUpdate update)
    {
        bool clickedInBase = false;

        Collider2D hitCollider = Physics2D.OverlapPoint(update.MousePos/*Input.mousePosition*/);
        clickedInBase = (hitCollider != null && hitCollider.CompareTag("LaunchPad"));

        if (update.MouseDown)
        {
            trailRendererPath.TrailRenderer = null;
        }

        okToDrawTrail |= update.MouseUp;

        if (update.Click && clickedInBase)
        {
            AddSoldier(update.MousePos);
        }

        if (!clickedInBase && ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved) || Input.GetMouseButton(0)))
        {
            // TODO: Should all this drawing stuff be in here?
            Plane plane = new Plane(Camera.main.transform.forward * -1, update.MainBehaviour.transform.position);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float distance;
            if (plane.Raycast(ray, out distance))
            {
                if (okToDrawTrail)
                {
                    trailRendererPath.TrailRenderer = Object.Instantiate(trailPrefab, ray.GetPoint(distance), Quaternion.identity);
                    okToDrawTrail = false;
                }
                else
                {
                    trailRendererPath.TrailRenderer.transform.position = ray.GetPoint(distance);
                }
                // TODO: Hmmm... Weak. 
                game.Player.StartMoving();
            }
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
