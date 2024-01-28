using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureInsideAction : IChestAction
{
    private Chest _chest;
    
    public TreasureInsideAction(Chest chest)
    {
        _chest = chest;
        _chest.Coins.SetActive(true);
    }
    
    public void DoChestAction()
    {
        
    }

    
}
