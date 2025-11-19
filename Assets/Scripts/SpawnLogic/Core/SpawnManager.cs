using System;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private List<Spawner> _spawners = new();
    [SerializeField] private BaseSkeleton _skeletonPrefab;
    [SerializeField] private GameObject _warriorPrefab;
    private INpsFabric _currentNpsFabric = null;
    private EmptyCellChecker _cellCheckerService = null;
    private Dictionary<Type, object> _factories = new();
    public static SpawnManager SpawnManagerInstance;

    private void Awake()
    {
        InitializeCore();
    }

    private void Start()
    {
        InitializeFabrics();
    }

    private void InitializeFabrics()
    {
        _factories[typeof(Skeleton)] = new SkeletonFabric(_skeletonPrefab);
    }

    private void InitializeListeners()
    {
        SpawnEventManager.OnBuySkeleton.AddListener(SpawnSkeleton);
    }

    private void InitializeCore()
    {
        _cellCheckerService = new();

        if (SpawnManager.SpawnManagerInstance == null)
        {
            SpawnManager.SpawnManagerInstance = this;
        }
        else
        {
            Destroy(gameObject);
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

    public void SpawnSkeleton()
    {
        if (_factories.TryGetValue(typeof(Skeleton), out object factoryObj))
        {
            var factory = factoryObj as INpsFabric;
            factory?.CreateNps(_cellCheckerService.Check(_spawners));
        }
    }

    public void SpawnMergedSkeleton(SkeletonData skeleton, Spawner spawner)
    {
        print(skeleton.Level + 1);
    }
}

public class SkeletonConfigurator
{

}