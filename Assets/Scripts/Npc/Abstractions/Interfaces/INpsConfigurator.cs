using UnityEngine;

public interface INpsConfigurator<T> where T : Component
{
    public void Configure(T npc, NpcData data, Spawner spawner);
    public void Reset(T npc);
}
