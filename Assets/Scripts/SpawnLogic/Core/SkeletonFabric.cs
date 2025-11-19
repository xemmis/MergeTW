using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class SkeletonFabric : INpsFabric, IReturnToPool<BaseNpc>
{
    private Dictionary<int, ObjectPool<BaseNpc>> _poolsByLevel = new();
    private Dictionary<int, NpsData> _dataByLevel = new();
    private INpsConfigurator<BaseNpc> _configurator;

    public SkeletonFabric(List<NpsData> skeletonData, INpsConfigurator<BaseNpc> configurator)
    {
        foreach (var data in skeletonData)
        {
            _dataByLevel[data.Level] = data;
            _poolsByLevel[data.Level] = new ObjectPool<BaseNpc>(data.Prefab, 5);
        }
        if (configurator == null)
        {
            throw new System.ArgumentNullException(nameof(configurator), "Configuraton is null on Constructor");
        }
        _configurator = configurator;
    }

    public GameObject CreateNpc(Spawner spawner, int level = 1)
    {
        if (!_poolsByLevel.ContainsKey(level) || spawner == null)
        {
            return null;
        }

        BaseNpc skeleton = _poolsByLevel[level].Get();
        skeleton.gameObject.SetActive(true);
        skeleton.transform.position = spawner.transform.position;
        _configurator.Configure(skeleton, skeleton.GetData(), spawner);
        
        return skeleton.gameObject;
    }

    public bool CanCreateLevel(int level)
    {
        return _poolsByLevel.ContainsKey(level);
    }

    public void ReturnToPool(BaseNpc component)
    {
        int level = component.GetData().Level;
        if (_poolsByLevel.ContainsKey(level))
        {
            _configurator.Reset(component);
            _poolsByLevel[level].Return(component);
        }
    }
}