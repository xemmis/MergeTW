using Unity.VisualScripting;
using UnityEngine;

public class SkeletonMergeService : MonoBehaviour, ISkeletonMergeHandler
{
    private SpawnManager _spawnManager;
    private SpawnEventManager _spawnEventManager;

    private void Start()
    {
        if (SpawnManager.SpawnManagerInstance != null)
        {
            _spawnManager = SpawnManager.SpawnManagerInstance;
        }

        if (SpawnEventManager.SpawnEventInstance != null)
        {
            _spawnEventManager = SpawnEventManager.SpawnEventInstance;
        }

        _spawnEventManager?.OnSkeletonMerge.AddListener(TryMergeWith);
    }

    private void OnDestroy()
    {
        _spawnEventManager?.OnSkeletonMerge.RemoveListener(TryMergeWith);
    }

    public void TryMergeWith(BaseNpc firstSkeleton, BaseNpc SecondSkeleton)
    {
        if (firstSkeleton.GetData().Level != SecondSkeleton.GetData().Level || firstSkeleton == SecondSkeleton)
        {
            return;
        }


        Spawner spawner = SecondSkeleton.GetSpawner();
        int requiredLvl = SecondSkeleton.GetData().Level + 1;
        if (_spawnManager.Factories.TryGetValue(typeof(BaseNpc), out object factoryObj))
        {
            var factory = factoryObj as INpsFabric;
            if (factory.CanCreateLevel(requiredLvl))
            {
                _spawnManager.ReturnToPool<BaseNpc>(firstSkeleton);
                _spawnManager.ReturnToPool<BaseNpc>(SecondSkeleton);
                _spawnManager.SpawnSkeleton(spawner, requiredLvl);
            }
            return;
        }
    }
}
