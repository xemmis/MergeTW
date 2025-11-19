using UnityEngine;
using UnityEngine.Events;

public class SpawnEventManager : MonoBehaviour
{
    public static UnityEvent OnBuySkeleton = new();
    public static UnityEvent OnCellSkeleton = new();
    public static UnityEvent<Skeleton, Skeleton, Spawner> OnSkeletonMerge = new();

    public void BuySkeleton()
    {
        OnBuySkeleton?.Invoke();
    }

    public void SellSkeleton()
    {
        OnCellSkeleton?.Invoke();
    }

    private void OnDestroy()
    {
        OnBuySkeleton = null;
        OnCellSkeleton = null;
    }
}