public class EnemyConfigurator : INpsConfigurator<EnemyBehaviorLogic>
{
    public void Configure(EnemyBehaviorLogic npc, NpcData data, Spawner spawner)
    {
        npc.GetNpc().Configure(data, spawner);
    }

    public void Reset(EnemyBehaviorLogic npc)
    {
        Spawner spawner = npc.GetNpc().GetSpawner();
        spawner.SetStatus(false);
        npc.GetNpc().SetSpawner(null);
    }
}