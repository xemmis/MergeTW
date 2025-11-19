using UnityEngine;

public class SkeletonFabric : INpsFabric, IReturnToPool<Skeleton>
{
    private ObjectPool<Skeleton> _skeletonPool;

    public SkeletonFabric(Skeleton npsPrefab)
    {
        _skeletonPool = new ObjectPool<Skeleton>(npsPrefab, 10);
    }

    public GameObject CreateNps(Spawner spawner)
    {
        if (spawner == null) return null;

        Skeleton skeleton = _skeletonPool.Get();
        skeleton.gameObject.SetActive(true);
        skeleton.transform.position = spawner.transform.position;
        skeleton.SetSpawner(spawner);
        return skeleton.gameObject;
    }

    public void ReturnToPool(Skeleton component)
    {
        _skeletonPool?.Return(component);
    }
}
