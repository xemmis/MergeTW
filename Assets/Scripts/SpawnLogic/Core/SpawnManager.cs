using System;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private List<Spawner> _spawners = new();
    [SerializeField] private List<NpsData> _skeletonData = new();
    [SerializeField] private GameObject _warriorPrefab;
    private INpsFabric _currentNpsFabric = null;
    private EmptyCellChecker _cellCheckerService = null;
    private Dictionary<Type, object> _factories = new();
    private SkeletonConfigurator _skeletonConfigurator = null;
    private SpawnEventManager _spawnEventManager = null;
    public static SpawnManager SpawnManagerInstance;
    public IReadOnlyDictionary<Type, object> Factories => _factories;

    private void Awake()
    {
        if (SpawnManager.SpawnManagerInstance == null)
        {
            SpawnManager.SpawnManagerInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        InitializeCore();
        InitializeFabrics();
        InitializeListeners();
    }

    private void InitializeFabrics()
    {
        _skeletonConfigurator = new SkeletonConfigurator();
        _factories[typeof(BaseNpc)] = new SkeletonFabric(_skeletonData, _skeletonConfigurator);

    }

    private void InitializeListeners()
    {
        _spawnEventManager.OnBuySkeleton.AddListener(SpawnSkeleton);
    }

    private void InitializeCore()
    {
        _cellCheckerService = new();
        if (SpawnEventManager.SpawnEventInstance != null)
        {
            _spawnEventManager = SpawnEventManager.SpawnEventInstance;
        }
    }

    public void ReturnToPool<T>(T component) where T : Component
    {
        Type type = typeof(T);

        if (_factories.TryGetValue(type, out object factory))
        {
            // Пытаемся кастовать к IReturnToPool<T>
            var returnable = factory as IReturnToPool<T>;
            returnable?.ReturnToPool(component);
        }
    }

    public void SpawnSkeleton(int level = 1)
    {
        if (_factories.TryGetValue(typeof(BaseNpc), out object factoryObj))
        {
            var factory = factoryObj as INpsFabric;
            Spawner spawner = _cellCheckerService.Check(_spawners);
            factory.CreateNpc(spawner, level);
        }
    }

    public void SpawnSkeleton(Spawner spawner, int level = 1)
    {
        if (_factories.TryGetValue(typeof(BaseNpc), out object factoryObj))
        {
            var factory = factoryObj as INpsFabric;
            if (factory == null)
            {
                throw new System.ArgumentNullException(nameof(factory), "Factory in SpawnManager/SpawnMergedSkeleton is not SkeletonFactory Or null");
            }

            if (factory.CanCreateLevel(level))
            {
                factory.CreateNpc(spawner, level);
            }
        }
    }
}

public interface INpsConfigurator<T> where T : Component
{
    void Configure(T npc, NpsData data, Spawner spawner);
    void Reset(T npc);
}

public class SkeletonConfigurator : INpsConfigurator<BaseNpc>
{
    public void Configure(BaseNpc skeleton, NpsData skeletonData, Spawner spawner)
    {
        skeleton.gameObject.SetActive(true);

        skeleton.SetSpawner(spawner);
        skeleton.SetData(skeletonData);
        skeleton.transform.position = spawner.transform.position;
        spawner.SetStatus(true);
    }

    public void Reset(BaseNpc npc)
    {
        npc.GetSpawner().SetStatus(false);
        npc.SetSpawner(null);
    }
}