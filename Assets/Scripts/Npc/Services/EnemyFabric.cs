using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class EnemyFabric : BaseNpcFabric<EnemyBehaviorLogic>
{
    public UnityEvent OnEnemyDied = new();

    public void Init(INpsConfigurator<EnemyBehaviorLogic> npsConfigurator)
    {
        _configurator = npsConfigurator;
    }

    public override int GetNpcLevel(EnemyBehaviorLogic npc)
    {
        return npc.GetNpcData().Level;
    }

    public override EnemyBehaviorLogic SpawnNpc(Spawner spawner, int level = 1)
    {
        NpcData data = _npcData.Find(npc => npc.Level == level);
        if (data == null)
        {
            Debug.LogWarning($"No NPC data found for level {level}");
            return null;
        }

        if (_poolsByLevel.TryGetValue(level, out ObjectPool<EnemyBehaviorLogic> pool))
        {
            EnemyBehaviorLogic npc = pool.Get();
            npc.gameObject.SetActive(true);
            _configurator.Configure(npc, data, spawner);
            _currentNpc.Add(npc);
            return npc;
        }

        Debug.LogError($"No pool found for level {level}");
        return null;
    }

    public override void ReturnNpc(EnemyBehaviorLogic component)
    {
        int level = GetNpcLevel(component);

        if (_poolsByLevel.TryGetValue(level, out ObjectPool<EnemyBehaviorLogic> pool))
        {
            _currentNpc.Remove(component);
            _configurator.Reset(component);
            component.gameObject.SetActive(false);
            pool.Return(component);
        }
        else
        {
            Debug.LogError($"No pool found for level {level}, destroying NPC");
            UnityEngine.Object.Destroy(component.gameObject);
        }

        OnEnemyDied?.Invoke();
        Wallet.EarnMoney(component.GetNpcData().Cost);
    }
}
