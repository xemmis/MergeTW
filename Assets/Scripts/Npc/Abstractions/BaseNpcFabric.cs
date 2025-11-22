using System.Collections.Generic;
using UnityEngine;

public abstract class BaseNpcFabric<T> : MonoBehaviour, INpcFabric<T> where T : Component
{
    [SerializeField] protected Dictionary<int, ObjectPool<T>> _poolsByLevel = new();
    [SerializeField] protected List<NpcData> _npcData = null;
    [SerializeField] protected INpsConfigurator<T> _configurator = null;
    [SerializeField] protected List<T> _currentNpc = new();

    protected virtual void Awake()
    {
        InitializePools();
    }

    protected virtual void InitializePools()
    {
        foreach (NpcData npcData in _npcData)
        {
            if (npcData.Prefab != null && npcData.Prefab.GetComponent<T>() != null)
            {
                _poolsByLevel[npcData.Level] = new ObjectPool<T>(
                    npcData.Prefab.GetComponent<T>(),
                    initialSize: 7
                );
            }
        }
    }

    public virtual T BuyNpc(Spawner spawner, int level = 1)
    {
        NpcData data = _npcData.Find(npc => npc.Level == level);
        if (Wallet.CanAfford(data.Cost) && data.UseCostOnSpawn)
        {
            Wallet.SpendMoney(data.Cost);
            T npc = SpawnNpc(spawner, level);

            if (npc != null)
            {
                _currentNpc.Add(npc);
            }

            return npc;
        }

        return null;
    }

    public virtual T MergeNpc(Spawner spawner, int level = 1)
    {
        T npc = SpawnNpc(spawner, level);
        if (npc != null)
        {
            _currentNpc.Add(npc);
        }
        return npc;
    }

    public virtual T SpawnNpc(Spawner spawner, int level = 1)
    {
        NpcData data = _npcData.Find(npc => npc.Level == level);
        if (data == null)
        {
            Debug.LogWarning($"No NPC data found for level {level}");
            return null;
        }

        if (_poolsByLevel.TryGetValue(level, out ObjectPool<T> pool))
        {
            T npc = pool.Get();
            npc.gameObject.SetActive(true);
            _configurator.Configure(npc, data, spawner);
            return npc;
        }

        Debug.LogError($"No pool found for level {level}");
        return null;
    }

    public virtual void ReturnNpc(T component)
    {
        // Наследники должны реализовать получение уровня
        int level = GetNpcLevel(component);

        if (_poolsByLevel.TryGetValue(level, out ObjectPool<T> pool))
        {
            _configurator.Reset(component);
            component.gameObject.SetActive(false);
            pool.Return(component);
        }
        else
        {
            Debug.LogError($"No pool found for level {level}, destroying NPC");
            UnityEngine.Object.Destroy(component.gameObject);
        }
        _currentNpc.Remove(component);
    }

    // Абстрактный метод - наследники знают как получить уровень своего NPC
    public abstract int GetNpcLevel(T npc);

    // Дополнительные общие методы
    public virtual bool CanSpawnNpc(int level)
    {
        NpcData data = _npcData.Find(npc => npc.Level == level);
        return data != null && (!data.UseCostOnSpawn || Wallet.CanAfford(data.Cost));
    }

    public virtual NpcData GetNpcData(int level)
    {
        return _npcData.Find(npc => npc.Level == level);
    }

    public List<T> ReturnCurrentNpc()
    {
        return _currentNpc;
    }
}