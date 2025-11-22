public class MergeHandler
{
    public void HandleMerge(SkeletonBehaviorLogic first, SkeletonBehaviorLogic second)
    {
        if (first.GetNpcData().Level == second.GetNpcData().Level)
        {
            Spawner spawner = second.GetNpc().GetSpawner();
            int requiredLevel = second.GetNpcData().Level + 1;

            SkeletonBehaviorLogic skeleton = SpawnManager.MergeNpc<SkeletonBehaviorLogic>(spawner, requiredLevel);
            if (skeleton == null)
            {
                return;
            }

            first.GetNpc().GetSpawner().SetStatus(false);

            SpawnManager.ReturnNpc<SkeletonBehaviorLogic>(first);
            SpawnManager.ReturnNpc<SkeletonBehaviorLogic>(second);
        }
    }
}