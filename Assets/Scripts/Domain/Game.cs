using System.Collections;
using System.Collections.Generic;

public class Game
{
    public List<Army> Armies {get;set;} = new List<Army>();

    public void Initialize()
    {
        // Enemy
        AddArmy();
        

        // Player
        AddArmy();
    }

    public Army Player { get { return Armies[1]; } }
    public Army Enemy { get { return Armies[0]; } }

    public Army AddArmy()
    {
        Army army = new Army();
        Armies.Add(army);
        return army;
    }
}
