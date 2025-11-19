using UnityEngine;
using UnityEngine.Events;

public class SpawnEventManager : MonoBehaviour
{
    public UnityEvent<int> OnBuySkeleton = new();
    public UnityEvent OnCellSkeleton = new();
    public UnityEvent<BaseNpc, BaseNpc> OnSkeletonMerge = new();

    public static SpawnEventManager SpawnEventInstance;

    private void Awake()
    {
        if (SpawnEventManager.SpawnEventInstance == null)
        {
            SpawnEventManager.SpawnEventInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void BuySkeleton(NpsData skeletonData)
    {
        OnBuySkeleton?.Invoke(skeletonData.Level);
    }

    public void SellSkeleton()
    {
        OnCellSkeleton?.Invoke();
    }

    private void OnDestroy()
    {
        OnBuySkeleton = null;
        OnCellSkeleton = null;
        OnCellSkeleton = null;
    }
}