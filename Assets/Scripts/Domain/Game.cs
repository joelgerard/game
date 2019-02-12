using System.Collections;
using System.Collections.Generic;

public class Game
{
    public List<Soldier> Soldiers {get;set;} = new List<Soldier>();

    public Soldier AddSoldier()
    {
        Soldier soldier = new Soldier();
        Soldiers.Add(soldier);
        return soldier;
    }
}
