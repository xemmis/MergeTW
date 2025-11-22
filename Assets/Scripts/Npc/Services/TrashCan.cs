using UnityEngine;

public class TrashCan : MonoBehaviour, ITrashCan
{
    public void AbortSkeleton(SkeletonBehaviorLogic skeleton)
    {
        SpawnManager.ReturnNpc(skeleton);
    }
}
