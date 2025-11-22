using System.Collections.Generic;
using UnityEngine;

public class SpawnEventManager : MonoBehaviour
{    
    private void Awake()
    {
        _mergeHandler = new MergeHandler();
    }
    #region SkeletonSpawn
    private MergeHandler _mergeHandler = new();
    [SerializeField] private List<Spawner> _spawners = new();

    private Spawner GetFreeSpawner()
    {
        foreach (Spawner spawner in _spawners)
        {
            if (!spawner.IsOccupied())
            {
                return spawner;
            }
        }
        return null;
    }

    public void SpawnSkeleton(int level = 1)
    {
        SpawnManager.BuyNpc<SkeletonBehaviorLogic>(GetFreeSpawner(), level);
    }

    public void MergeSkeleton(SkeletonBehaviorLogic firstNpc, SkeletonBehaviorLogic secondNpc)
    {
        _mergeHandler.HandleMerge(firstNpc, secondNpc);
    }

    public void SellSkeleton(SkeletonBehaviorLogic npcBehaviorLogic)
    {
        SpawnManager.ReturnNpc<SkeletonBehaviorLogic>(npcBehaviorLogic);
    }
    #endregion
}
