public class SkeletonConfigurator : INpsConfigurator<SkeletonBehaviorLogic>
{
    public void Configure(SkeletonBehaviorLogic npc, NpcData data, Spawner spawner)
    {
        npc.GetNpc().Configure(data, spawner);
        npc.ChangeState(new IdleState());
    }

    public void Reset(SkeletonBehaviorLogic npc)
    {
        npc.GetNpc().SetSpawner(null);
    }
}
