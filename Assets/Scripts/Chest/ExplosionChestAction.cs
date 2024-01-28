using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionChestAction : IChestAction
{
    private Chest _chest;

    
    public ExplosionChestAction(Chest chest)
    {
        _chest = chest;
    }
    
    public void DoChestAction()
    {
        foreach(ParticleSystem ps in _chest.ExplosionParticalSystems)
        {
            ps.Emit(5);
            MapManager.player.HP.TakeDamage(_chest.ExplosionDamage);
            _chest.ExplosionTrace.SetActive(true);
        }
    }
}
