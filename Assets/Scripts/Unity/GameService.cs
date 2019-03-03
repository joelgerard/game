using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class joins unity runtime stuff to the game domain.
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

    public void Initialize(DrawShape rectanglePrefab, DrawShape circlePrefab, DrawShape trianglePrefab)
    {
        this.RectanglePrefab = rectanglePrefab;
        this.TrianglePrefab = trianglePrefab;
        this.CirclePrefab = circlePrefab;

        game.Initialize();

    }

    public void Update(GameUpdate update)
    {
        // TODO: Need to call this once per frame?
        game.Update(update, Time.deltaTime);
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
