using UnityEngine;

public class TrashCan : MonoBehaviour, IInteractable
{
    public int Priority { get; private set; } = 100;

    public void AbortSkeleton(SkeletonBehaviorLogic skeleton)
    {
        SpawnManager.ReturnNpc(skeleton);
    }

    public bool TryInteract(SkeletonBehaviorLogic skeleton)
    {
        if (skeleton == null)
        {
            return false;
        }

        AbortSkeleton(skeleton);
        return true;
    }

}
