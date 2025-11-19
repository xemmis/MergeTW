using Unity.VisualScripting;
using UnityEngine;

public class SkeletonMergeService : MonoBehaviour, ISkeletonMergeHandler
{
    [field: SerializeField] public SkeletonData SkeletonData { get; private set; } = null;
    private SpawnManager _spawnManager;
    private ISkeletonConfigurator _skeletonConfigurator;

    private void Awake()
    {
        if (SpawnManager.SpawnManagerInstance != null)
        {
            _spawnManager = SpawnManager.SpawnManagerInstance;
        }
    }

    public bool TryMergeWith(Skeleton firstSkeleton, Skeleton SecondSkeleton)
    {
        if (firstSkeleton.GetData().Level != SecondSkeleton.GetData().Level || firstSkeleton == SecondSkeleton)
        {
            return false;
        }

        _spawnManager.SpawnMergedSkeleton(SecondSkeleton.GetData(), SecondSkeleton.GetSpawner());
        return true;
    }
}

public interface ISkeletonConfigurator
{
    void Configure(SkeletonData skeletonData);
}
